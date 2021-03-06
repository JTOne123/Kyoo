﻿using Kyoo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kyoo.Controllers
{
	public interface IMetadataProvider
	{
		ProviderID Provider { get; }

		Task<Collection> GetCollectionFromName(string name);

		Task<Show> GetShowByID(Show show);
		Task<IEnumerable<Show>> SearchShows(string showName, bool isMovie);
		Task<IEnumerable<PeopleLink>> GetPeople(Show show);

		Task<Season> GetSeason(Show show, int seasonNumber);

		Task<Episode> GetEpisode(Show show, int seasonNumber, int episodeNumber, int absoluteNumber);
	}
}
