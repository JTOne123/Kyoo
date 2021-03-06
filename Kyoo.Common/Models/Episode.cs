﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Kyoo.Models.Attributes;

namespace Kyoo.Models
{
	public class Episode : IOnMerge
	{
		[JsonIgnore] public int ID { get; set; }
		[JsonIgnore] public int ShowID { get; set; }
		[JsonIgnore] public virtual Show Show { get; set; }
		[JsonIgnore] public int? SeasonID { get; set; }
		[JsonIgnore] public virtual Season Season { get; set; }

		public int SeasonNumber { get; set; } = -1;
		public int EpisodeNumber { get; set; } = -1;
		public int AbsoluteNumber { get; set; } = -1;
		[JsonIgnore] public string Path { get; set; }
		public string Title { get; set; }
		public string Overview { get; set; }
		public DateTime? ReleaseDate { get; set; }

		public int Runtime { get; set; } //This runtime variable should be in minutes

		[JsonIgnore] public string ImgPrimary { get; set; }
		public virtual IEnumerable<MetadataID> ExternalIDs { get; set; }

		[JsonIgnore] public virtual IEnumerable<Track> Tracks { get; set; }

		public string ShowTitle => Show.Title; // Used in the API response only
		public string Slug => GetSlug(Show.Slug, SeasonNumber, EpisodeNumber);
		public string Thumb
		{
			get
			{
				if (Show != null)
					return "thumb/" + Slug;
				return ImgPrimary;
			}
		}


		public Episode() { }

		public Episode(int seasonNumber, 
			int episodeNumber,
			int absoluteNumber,
			string title,
			string overview,
			DateTime? releaseDate,
			int runtime,
			string imgPrimary,
			IEnumerable<MetadataID> externalIDs)
		{
			SeasonNumber = seasonNumber;
			EpisodeNumber = episodeNumber;
			AbsoluteNumber = absoluteNumber;
			Title = title;
			Overview = overview;
			ReleaseDate = releaseDate;
			Runtime = runtime;
			ImgPrimary = imgPrimary;
			ExternalIDs = externalIDs;
		}

		public Episode(int showID, 
			int seasonID,
			int seasonNumber, 
			int episodeNumber, 
			int absoluteNumber, 
			string path,
			string title, 
			string overview, 
			DateTime? releaseDate, 
			int runtime, 
			string imgPrimary,
			IEnumerable<MetadataID> externalIDs)
		{
			ShowID = showID;
			SeasonID = seasonID;
			SeasonNumber = seasonNumber;
			EpisodeNumber = episodeNumber;
			AbsoluteNumber = absoluteNumber;
			Path = path;
			Title = title;
			Overview = overview;
			ReleaseDate = releaseDate;
			Runtime = runtime;
			ImgPrimary = imgPrimary;
			ExternalIDs = externalIDs;
		}

		public static string GetSlug(string showSlug, int seasonNumber, int episodeNumber)
		{
			return showSlug + "-s" + seasonNumber + "e" + episodeNumber;
		}

		public void OnMerge(object merged)
		{
			Episode other = (Episode)merged;
			if (SeasonNumber == -1 && other.SeasonNumber != -1)
				SeasonNumber = other.SeasonNumber;
			if (EpisodeNumber == -1 && other.EpisodeNumber != -1)
				EpisodeNumber = other.EpisodeNumber;
			if (AbsoluteNumber == -1 && other.AbsoluteNumber != -1)
				AbsoluteNumber = other.AbsoluteNumber;
		}
	}
}
