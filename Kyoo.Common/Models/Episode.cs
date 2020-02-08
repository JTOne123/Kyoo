﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kyoo.Models
{
    public class Episode : IMergable<Episode>
    {
        [JsonIgnore] public long ID { get; set; }
        [JsonIgnore] public long ShowID { get; set; }
        [JsonIgnore] public virtual Show Show { get; set; }
        [JsonIgnore] public long SeasonID { get; set; }
        [JsonIgnore] public virtual Season Season { get; set; }

        public long SeasonNumber { get; set; }
        public long EpisodeNumber { get; set; }
        public long AbsoluteNumber { get; set; }
        [JsonIgnore] public string Path { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public long Runtime { get; set; } //This runtime variable should be in minutes

        [JsonIgnore] public string ImgPrimary { get; set; }
        public string ExternalIDs { get; set; }

        public IEnumerable<Track> Tracks { get; set; }

        public string ShowTitle; //Used in the API response only
        public string Link; //Used in the API response only
        public string Thumb; //Used in the API response only


        public Episode()
        {
	        SeasonNumber = -1;
            EpisodeNumber = -1;
            AbsoluteNumber = -1;
        }

        public Episode(long seasonNumber, long episodeNumber, long absoluteNumber, string title, string overview, DateTime? releaseDate, long runtime, string imgPrimary, string externalIDs)
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

        public Episode(long showID, long seasonID, long seasonNumber, long episodeNumber, long absoluteNumber, string path, string title, string overview, DateTime? releaseDate, long runtime, string imgPrimary, string externalIDs)
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

        public static string GetSlug(string showSlug, long seasonNumber, long episodeNumber)
        {
            return showSlug + "-s" + seasonNumber + "e" + episodeNumber;
        }

        public Episode Merge(Episode other)
        {
            if (other == null)
                return this;
            if (ID == -1)
                ID = other.ID;
            if (ShowID == -1)
                ShowID = other.ShowID;
            if (SeasonID == -1)
                SeasonID = other.SeasonID;
            if (SeasonNumber == -1)
                SeasonNumber = other.SeasonNumber;
            if (EpisodeNumber == -1)
                EpisodeNumber = other.EpisodeNumber;
            if (AbsoluteNumber == -1)
                AbsoluteNumber = other.AbsoluteNumber;
            if (Path == null)
                Path = other.Path;
            if (Title == null)
                Title = other.Title;
            if (Overview == null)
                Overview = other.Overview;
            if (ReleaseDate == null)
                ReleaseDate = other.ReleaseDate;
            if (Runtime == -1)
                Runtime = other.Runtime;
            if (ImgPrimary == null)
                ImgPrimary = other.ImgPrimary;
            ExternalIDs += '|' + other.ExternalIDs;
            return this;
        }
    }
}
