﻿using System;
using Kyoo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Kyoo.Controllers;
using Kyoo.Models.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace Kyoo.Api
{
	[Route("api/shows")]
	[Route("api/show")]
	[ApiController]
	public class ShowsController : ControllerBase
	{
		private readonly ILibraryManager _libraryManager;

		public ShowsController(ILibraryManager libraryManager)
		{
			_libraryManager = libraryManager;
		}

		[HttpGet]
		[Authorize(Policy="Read")]
		public IEnumerable<Show> GetShows()
		{
			return _libraryManager.GetShows();
		}

		[HttpGet("{slug}")]
		[Authorize(Policy="Read")]
		[JsonDetailed]
		public ActionResult<Show> GetShow(string slug)
		{
			Show show = _libraryManager.GetShowBySlug(slug);

			if (show == null)
				return NotFound();
			
			return show;
		}
		
		[HttpPost("edit/{slug}")]
		[Authorize(Policy="Write")]
		public IActionResult EditShow(string slug, [FromBody] Show show)
		{ 
			if (!ModelState.IsValid) 
				return BadRequest(show); 
			show.ID = 0;
			show.Slug = slug;
			try
			{
				_libraryManager.EditShow(show);
			}
			catch (ItemNotFound)
			{
				return NotFound();
			}
			return Ok();
		}
	}
}
