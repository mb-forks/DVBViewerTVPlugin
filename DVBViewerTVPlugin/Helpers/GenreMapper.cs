﻿using System;
using System.Collections.Generic;
using System.Linq;

using MediaBrowser.Controller.LiveTv;
using MediaBrowser.Plugins.DVBViewer.Configuration;

namespace MediaBrowser.Plugins.DVBViewer.Helpers
{
    /// <summary>
    /// Provides methods to map configure genres to MB programs
    /// </summary>
    public class GenreMapper
    {
        public const string GENRE_MOVIE = "GENREMOVIE";
        public const string GENRE_SERIES = "GENRESERIES";
        public const string GENRE_SPORT = "GENRESPORT";
        public const string GENRE_NEWS = "GENRENEWS";
        public const string GENRE_KIDS = "GENREKIDS";
        public const string GENRE_LIVE = "GENRELIVE";

        private readonly PluginConfiguration _configuration;
        private readonly List<String> _movieGenres;
        private readonly List<String> _seriesGenres;
        private readonly List<String> _sportGenres;
        private readonly List<String> _newsGenres;
        private readonly List<String> _kidsGenres;
        private readonly List<String> _liveGenres;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenreMapper"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public GenreMapper(PluginConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            _configuration = configuration;

            _movieGenres = new List<string>();
            _seriesGenres = new List<string>();
            _sportGenres = new List<string>();
            _newsGenres = new List<string>();
            _kidsGenres = new List<string>();
            _liveGenres = new List<string>();

            LoadInternalLists(_configuration.GenreMappings);
        }

        private void LoadInternalLists(Dictionary<string, List<string>> genreMappings)
        {
            if (genreMappings != null)
            {
                if (_configuration.GenreMappings.ContainsKey(GENRE_MOVIE) && _configuration.GenreMappings[GENRE_MOVIE] != null)
                {
                    _movieGenres.AddRange(_configuration.GenreMappings[GENRE_MOVIE]);
                }

                if (_configuration.GenreMappings.ContainsKey(GENRE_SERIES) && _configuration.GenreMappings[GENRE_SERIES] != null)
                {
                    _seriesGenres.AddRange(_configuration.GenreMappings[GENRE_SERIES]);
                }

                if (_configuration.GenreMappings.ContainsKey(GENRE_SPORT) && _configuration.GenreMappings[GENRE_SPORT] != null)
                {
                    _sportGenres.AddRange(_configuration.GenreMappings[GENRE_SPORT]);
                }

                if (_configuration.GenreMappings.ContainsKey(GENRE_NEWS) && _configuration.GenreMappings[GENRE_NEWS] != null)
                {
                    _newsGenres.AddRange(_configuration.GenreMappings[GENRE_NEWS]);
                }

                if (_configuration.GenreMappings.ContainsKey(GENRE_KIDS) && _configuration.GenreMappings[GENRE_KIDS] != null)
                {
                    _kidsGenres.AddRange(_configuration.GenreMappings[GENRE_KIDS]);
                }

                if (_configuration.GenreMappings.ContainsKey(GENRE_LIVE) && _configuration.GenreMappings[GENRE_LIVE] != null)
                {
                    _liveGenres.AddRange(_configuration.GenreMappings[GENRE_LIVE]);
                }
            }
        }

        /// <summary>
        /// Populates the program genres.
        /// </summary>
        /// <param name="program">The program.</param>
        public void PopulateProgramGenres(ProgramInfo program)
        {
            // Check there is a program and genres to map
            if (program != null && program.Overview != null)
            {
                program.Genres = new List<String>();

                if (_movieGenres.All(g => !string.IsNullOrWhiteSpace(g)))
                {
                    program.Genres.Add(_movieGenres.FirstOrDefault(g => program.Overview.IndexOf(g, StringComparison.InvariantCultureIgnoreCase) != -1));
                    program.IsMovie = _movieGenres.Any(g => program.Overview.IndexOf(g, StringComparison.InvariantCultureIgnoreCase) != -1);
                }

                if (_seriesGenres.All(g => !string.IsNullOrWhiteSpace(g)))
                {
                    program.Genres.Add(_seriesGenres.FirstOrDefault(g => program.Overview.IndexOf(g, StringComparison.InvariantCultureIgnoreCase) != -1));
                    program.IsSeries = _seriesGenres.Any(g => program.Overview.IndexOf(g, StringComparison.InvariantCultureIgnoreCase) != -1);
                }
                //else
                //{
                //    program.IsSeries = true;
                //    program.IsPremiere = false;
                //    program.IsRepeat = true;
                //}

                if (_sportGenres.All(g => !string.IsNullOrWhiteSpace(g)))
                {
                    program.Genres.Add(_sportGenres.FirstOrDefault(g => program.Overview.IndexOf(g, StringComparison.InvariantCultureIgnoreCase) != -1));
                    program.IsSports = _sportGenres.Any(g => program.Overview.IndexOf(g, StringComparison.InvariantCultureIgnoreCase) != -1);
                }

                if (_newsGenres.All(g => !string.IsNullOrWhiteSpace(g)))
                {
                    program.Genres.Add(_newsGenres.FirstOrDefault(g => program.Overview.IndexOf(g, StringComparison.InvariantCultureIgnoreCase) != -1));
                    program.IsNews = _newsGenres.Any(g => program.Overview.IndexOf(g, StringComparison.InvariantCultureIgnoreCase) != -1);
                }

                if (_kidsGenres.All(g => !string.IsNullOrWhiteSpace(g)))
                {
                    program.Genres.Add(_kidsGenres.FirstOrDefault(g => program.Overview.IndexOf(g, StringComparison.InvariantCultureIgnoreCase) != -1));
                    program.IsKids = _kidsGenres.Any(g => program.Overview.IndexOf(g, StringComparison.InvariantCultureIgnoreCase) != -1);
                }

                if (_liveGenres.All(g => !string.IsNullOrWhiteSpace(g)))
                {
                    program.Genres.Add(_liveGenres.FirstOrDefault(g => program.Overview.IndexOf(g, StringComparison.InvariantCultureIgnoreCase) != -1));
                    program.IsLive = _liveGenres.Any(g => program.Overview.IndexOf(g, StringComparison.InvariantCultureIgnoreCase) != -1);
                }

                if (!program.IsMovie && !program.IsSeries && !program.IsSports && !program.IsNews && !program.IsKids && !program.IsLive)
                {
                    program.IsSeries = true;
                    program.IsPremiere = false;
                    program.IsRepeat = true;
                }
            }
        }
    }
}