﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kyoo.Models;

namespace Kyoo.Controllers
{
	public class LibraryManager : ILibraryManager
	{
		private readonly ILibraryRepository _libraries;
		private readonly ICollectionRepository _collections;
		private readonly IShowRepository _shows;
		private readonly ISeasonRepository _seasons;
		private readonly IEpisodeRepository _episodes;
		private readonly ITrackRepository _tracks;
		private readonly IGenreRepository _genres;
		private readonly IStudioRepository _studios;
		private readonly IPeopleRepository _people;
		private readonly IProviderRepository _providers;
		
		public LibraryManager(ILibraryRepository libraries, 
			ICollectionRepository collections, 
			IShowRepository shows, 
			ISeasonRepository seasons, 
			IEpisodeRepository episodes,
			ITrackRepository tracks, 
			IGenreRepository genres, 
			IStudioRepository studios,
			IProviderRepository providers, 
			IPeopleRepository people)
		{
			_libraries = libraries;
			_collections = collections;
			_shows = shows;
			_seasons = seasons;
			_episodes = episodes;
			_tracks = tracks;
			_genres = genres;
			_studios = studios;
			_providers = providers;
			_people = people;
		}
		
		public void Dispose()
		{
			_libraries.Dispose();
			_collections.Dispose();
			_shows.Dispose();
			_seasons.Dispose();
			_episodes.Dispose();
			_tracks.Dispose();
			_genres.Dispose();
			_studios.Dispose();
			_people.Dispose();
			_providers.Dispose();
		}
		
		public async ValueTask DisposeAsync()
		{
			await Task.WhenAll(
				_libraries.DisposeAsync().AsTask(),
				_collections.DisposeAsync().AsTask(),
				_shows.DisposeAsync().AsTask(),
				_seasons.DisposeAsync().AsTask(),
				_episodes.DisposeAsync().AsTask(),
				_tracks.DisposeAsync().AsTask(),
				_genres.DisposeAsync().AsTask(),
				_studios.DisposeAsync().AsTask(),
				_people.DisposeAsync().AsTask(),
				_providers.DisposeAsync().AsTask()
			);
		}

		public Task<Library> GetLibrary(string slug)
		{
			return _libraries.Get(slug);
		}

		public Task<Collection> GetCollection(string slug)
		{
			return _collections.Get(slug);
		}

		public Task<Show> GetShow(string slug)
		{
			return _shows.Get(slug);
		}

		public Task<Season> GetSeason(string showSlug, int seasonNumber)
		{
			return _seasons.Get(showSlug, seasonNumber);
		}

		public Task<Episode> GetEpisode(string showSlug, int seasonNumber, int episodeNumber)
		{
			return _episodes.Get(showSlug, seasonNumber, episodeNumber);
		}

		public Task<Episode> GetMovieEpisode(string movieSlug)
		{
			return _episodes.Get(movieSlug);
		}

		public Task<Track> GetTrack(int id)
		{
			return _tracks.Get(id);
		}
		
		public Task<Track> GetTrack(int episodeID, string language, bool isForced)
		{
			return _tracks.Get(episodeID, language, isForced);
		}

		public Task<Genre> GetGenre(string slug)
		{
			return _genres.Get(slug);
		}

		public Task<Studio> GetStudio(string slug)
		{
			return _studios.Get(slug);
		}

		public Task<People> GetPeople(string slug)
		{
			return _people.Get(slug);
		}

		public Task<ICollection<Library>> GetLibraries()
		{
			return _libraries.GetAll();
		}

		public Task<ICollection<Collection>> GetCollections()
		{
			return _collections.GetAll();
		}

		public Task<ICollection<Show>> GetShows()
		{
			return _shows.GetAll();
		}

		public Task<ICollection<Season>> GetSeasons()
		{
			return _seasons.GetAll();
		}

		public Task<ICollection<Episode>> GetEpisodes()
		{
			return _episodes.GetAll();
		}

		public Task<ICollection<Track>> GetTracks()
		{
			return _tracks.GetAll();
		}

		public Task<ICollection<Studio>> GetStudios()
		{
			return _studios.GetAll();
		}

		public Task<ICollection<People>> GetPeoples()
		{
			return _people.GetAll();
		}

		public Task<ICollection<Genre>> GetGenres()
		{
			return _genres.GetAll();
		}

		public Task<ICollection<ProviderID>> GetProviders()
		{
			return _providers.GetAll();
		}

		public Task<ICollection<Season>> GetSeasons(int showID)
		{
			return _seasons.GetSeasons(showID);
		}

		public Task<ICollection<Season>> GetSeasons(string showSlug)
		{
			return _seasons.GetSeasons(showSlug);
		}

		public Task<ICollection<Episode>> GetEpisodes(int showID, int seasonNumber)
		{
			return _episodes.GetEpisodes(showID, seasonNumber);
		}

		public Task<ICollection<Episode>> GetEpisodes(string showSlug, int seasonNumber)
		{
			return _episodes.GetEpisodes(showSlug, seasonNumber);
		}

		public Task<ICollection<Episode>> GetEpisodes(int seasonID)
		{
			return _episodes.GetEpisodes(seasonID);
		}

		public Task<Show> GetShowByPath(string path)
		{
			return _shows.GetByPath(path);
		}

		public Task AddShowLink(int showID, int? libraryID, int? collectionID)
		{
			return _shows.AddShowLink(showID, libraryID, collectionID);
		}

		public Task AddShowLink(Show show, Library library, Collection collection)
		{
			if (show == null)
				throw new ArgumentNullException(nameof(show));
			return AddShowLink(show.ID, library?.ID, collection?.ID);
		}
		
		public Task<ICollection<Library>> SearchLibraries(string searchQuery)
		{
			return _libraries.Search(searchQuery);
		}

		public Task<ICollection<Collection>> SearchCollections(string searchQuery)
		{
			return _collections.Search(searchQuery);
		}

		public Task<ICollection<Show>> SearchShows(string searchQuery)
		{
			return _shows.Search(searchQuery);
		}

		public Task<ICollection<Season>> SearchSeasons(string searchQuery)
		{
			return _seasons.Search(searchQuery);
		}

		public Task<ICollection<Episode>> SearchEpisodes(string searchQuery)
		{
			return _episodes.Search(searchQuery);
		}

		public Task<ICollection<Genre>> SearchGenres(string searchQuery)
		{
			return _genres.Search(searchQuery);
		}

		public Task<ICollection<Studio>> SearchStudios(string searchQuery)
		{
			return _studios.Search(searchQuery);
		}

		public Task<ICollection<People>> SearchPeople(string searchQuery)
		{
			return _people.Search(searchQuery);
		}
		
		public Task RegisterLibrary(Library library)
		{
			return _libraries.Create(library);
		}

		public Task RegisterCollection(Collection collection)
		{
			return _collections.Create(collection);
		}

		public Task RegisterShow(Show show)
		{
			return _shows.Create(show);
		}

		public Task RegisterSeason(Season season)
		{
			return _seasons.Create(season);
		}

		public Task RegisterEpisode(Episode episode)
		{
			return _episodes.Create(episode);
		}

		public Task RegisterTrack(Track track)
		{
			return _tracks.Create(track);
		}

		public Task RegisterGenre(Genre genre)
		{
			return _genres.Create(genre);
		}

		public Task RegisterStudio(Studio studio)
		{
			return _studios.Create(studio);
		}

		public Task RegisterPeople(People people)
		{
			return _people.Create(people);
		}

		public Task EditLibrary(Library library, bool resetOld)
		{
			return _libraries.Edit(library, resetOld);
		}

		public Task EditCollection(Collection collection, bool resetOld)
		{
			return _collections.Edit(collection, resetOld);
		}

		public Task EditShow(Show show, bool resetOld)
		{
			return _shows.Edit(show, resetOld);
		}

		public Task EditSeason(Season season, bool resetOld)
		{
			return _seasons.Edit(season, resetOld);
		}

		public Task EditEpisode(Episode episode, bool resetOld)
		{
			return _episodes.Edit(episode, resetOld);
		}

		public Task EditTrack(Track track, bool resetOld)
		{
			return _tracks.Edit(track, resetOld);
		}

		public Task EditGenre(Genre genre, bool resetOld)
		{
			return _genres.Edit(genre, resetOld);
		}

		public Task EditStudio(Studio studio, bool resetOld)
		{
			return _studios.Edit(studio, resetOld);
		}

		public Task EditPeople(People people, bool resetOld)
		{
			return _people.Edit(people, resetOld);
		}

		public Task DelteLibrary(Library library)
		{
			return _libraries.Delete(library);
		}

		public Task DeleteCollection(Collection collection)
		{
			return _collections.Delete(collection);
		}

		public Task DeleteShow(Show show)
		{
			return _shows.Delete(show);
		}

		public Task DeleteSeason(Season season)
		{
			return _seasons.Delete(season);
		}

		public Task DeleteEpisode(Episode episode)
		{
			return _episodes.Delete(episode);
		}

		public Task DeleteTrack(Track track)
		{
			return _tracks.Delete(track);
		}

		public Task DeleteGenre(Genre genre)
		{
			return _genres.Delete(genre);
		}

		public Task DeleteStudio(Studio studio)
		{
			return _studios.Delete(studio);
		}

		public Task DeletePeople(People people)
		{
			return _people.Delete(people);
		}
	}
}
