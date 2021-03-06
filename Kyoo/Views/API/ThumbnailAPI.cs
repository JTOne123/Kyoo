﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using Kyoo.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace Kyoo.Api
{
	public class ThumbnailController : ControllerBase
	{
		private readonly ILibraryManager _libraryManager;
		private readonly string _peoplePath;


		public ThumbnailController(ILibraryManager libraryManager, IConfiguration config)
		{
			_libraryManager = libraryManager;
			_peoplePath = config.GetValue<string>("peoplePath");
		}

		[HttpGet("poster/{showSlug}")]
		[Authorize(Policy="Read")]
		public async Task<IActionResult> GetShowThumb(string showSlug)
		{
			string path = (await _libraryManager.GetShow(showSlug))?.Path;
			if (path == null)
				return NotFound();

			string thumb = Path.Combine(path, "poster.jpg");

			if (System.IO.File.Exists(thumb))
				return new PhysicalFileResult(Path.GetFullPath(thumb), "image/jpg");
			return NotFound();
		}

		[HttpGet("logo/{showSlug}")]
		[Authorize(Policy="Read")]
		public async Task<IActionResult> GetShowLogo(string showSlug)
		{
			string path = (await _libraryManager.GetShow(showSlug))?.Path;
			if (path == null)
				return NotFound();

			string thumb = Path.Combine(path, "logo.png");

			if (System.IO.File.Exists(thumb))
				return new PhysicalFileResult(Path.GetFullPath(thumb), "image/jpg");
			return NotFound();
		}

		[HttpGet("backdrop/{showSlug}")]
		[Authorize(Policy="Read")]
		public async Task<IActionResult> GetShowBackdrop(string showSlug)
		{
			string path = (await _libraryManager.GetShow(showSlug))?.Path;
			if (path == null)
				return NotFound();

			string thumb = Path.Combine(path, "backdrop.jpg");

			if (System.IO.File.Exists(thumb))
				return new PhysicalFileResult(Path.GetFullPath(thumb), "image/jpg");
			return NotFound();
		}

		[HttpGet("peopleimg/{peopleSlug}")]
		[Authorize(Policy="Read")]
		public IActionResult GetPeopleIcon(string peopleSlug)
		{
			string thumbPath = Path.Combine(_peoplePath, peopleSlug + ".jpg");
			if (!System.IO.File.Exists(thumbPath))
				return NotFound();

			return new PhysicalFileResult(Path.GetFullPath(thumbPath), "image/jpg");
		}

		[HttpGet("thumb/{showSlug}-s{seasonNumber}e{episodeNumber}")]
		[Authorize(Policy="Read")]
		public async Task<IActionResult> GetEpisodeThumb(string showSlug, int seasonNumber, int episodeNumber)
		{
			string path = (await _libraryManager.GetEpisode(showSlug, seasonNumber, episodeNumber))?.Path;
			if (path == null)
				return NotFound();

			string thumb = Path.ChangeExtension(path, "jpg");

			if (System.IO.File.Exists(thumb))
				return new PhysicalFileResult(Path.GetFullPath(thumb), "image/jpg");
			return NotFound();
		}
	}
}
