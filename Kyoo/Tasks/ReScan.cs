using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kyoo.Controllers;
using Kyoo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Kyoo.Tasks
{
	public class ReScan: ITask
	{
		public string Slug => "re-scan";
		public string Name => "ReScan";
		public string Description => "Re download metadata of an item using it's external ids.";
		public string HelpMessage => null;
		public bool RunOnStartup => false;
		public int Priority => 0;
		
		
		private ILibraryManager _libraryManager;
		private IThumbnailsManager _thumbnailsManager;
		private IProviderManager _providerManager;
		private DatabaseContext _database;

		public async Task Run(IServiceProvider serviceProvider, CancellationToken cancellationToken, string arguments = null)
		{
			using IServiceScope serviceScope = serviceProvider.CreateScope();
			_libraryManager = serviceScope.ServiceProvider.GetService<ILibraryManager>();
			_thumbnailsManager = serviceScope.ServiceProvider.GetService<IThumbnailsManager>();
			_providerManager = serviceScope.ServiceProvider.GetService<IProviderManager>();
			_database = serviceScope.ServiceProvider.GetService<DatabaseContext>();
		
			if (arguments == null || !arguments.Contains('/'))
				return;

			string slug = arguments.Substring(arguments.IndexOf('/') + 1);
			switch (arguments.Substring(0, arguments.IndexOf('/')))
			{
				case "show":
					await ReScanShow(slug);
					break;
				case "season":
					await ReScanSeason(slug);
					break;
			}
		}

		private async Task ReScanShow(string slug)
		{
			Show old = _database.Shows.FirstOrDefault(x => x.Slug == slug);
			if (old == null)
				return;
			Library library = _libraryManager.GetLibraryForShow(slug);
			Show edited = await _providerManager.CompleteShow(old, library);
			edited.ID = old.ID;
			edited.Slug = old.Slug;
			edited.Path = old.Path;
			_libraryManager.EditShow(edited);
			await _thumbnailsManager.Validate(edited, true);
			if (old.Seasons != null)
				await Task.WhenAll(old.Seasons.Select(x => ReScanSeason(old, x)));
			IEnumerable<Episode> orphans = old.Episodes.Where(x => x.Season == null).ToList();
			if (orphans.Any())
				await Task.WhenAll(orphans.Select(x => ReScanEpisode(old, x)));
		}
		
		private async Task ReScanSeason(string seasonSlug)
		{
			string[] infos = seasonSlug.Split('-');
			if (infos.Length != 2 || int.TryParse(infos[1], out int seasonNumber))
				return;
			string slug = infos[0];
			Show show = _database.Shows.FirstOrDefault(x => x.Slug == slug);
			if (show == null)
				return;
			Season old = _database.Seasons.FirstOrDefault(x => x.SeasonNumber == seasonNumber && x.Show.ID == show.ID);
			if (old == null)
				return;
			await ReScanSeason(show, old);
		}

		private async Task ReScanSeason(Show show, Season old)
		{
			Library library = _libraryManager.GetLibraryForShow(show.Slug);
			Season edited = await _providerManager.GetSeason(show, old.SeasonNumber, library);
			edited.ID = old.ID;
			_libraryManager.EditSeason(edited);
			await _thumbnailsManager.Validate(edited, true);
			if (old.Episodes != null)
				await Task.WhenAll(old.Episodes.Select(x => ReScanEpisode(show, x)));
		}

		private async Task ReScanEpisode(Show show, Episode old)
		{
			Library library = _libraryManager.GetLibraryForShow(show.Slug);
			Episode edited = await _providerManager.GetEpisode(show, old.Path, old.SeasonNumber, old.EpisodeNumber, old.AbsoluteNumber, library);
			edited.ID = old.ID;
			_libraryManager.EditEpisode(edited);
			await _thumbnailsManager.Validate(edited, true);
		}

		public IEnumerable<string> GetPossibleParameters()
		{
			return default;
		}

		public int? Progress()
		{
			return null;
		}
	}
}