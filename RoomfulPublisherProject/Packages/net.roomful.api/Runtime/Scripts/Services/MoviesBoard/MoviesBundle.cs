using System;
using System.Collections.Generic;
using System.Linq;

namespace net.roomful.api.movies
{
    public class Language
    {
        public string Locale { get; }

        public Language(string locale) {
            Locale = locale;
        }

        internal static Language Unknown => new Language("Unknown");
    }

    public class Rating
    {
        public string Alias { get; }
        public string Source { get; }

        private Rating() { }

        private Rating(string alias, string source) {
            Alias = alias;
            Source = source;
        }

        public static Rating FromJsonBundle(object jsonBundle) {
            if (!(jsonBundle is Dictionary<string, object> bundle)) {
                return new Rating();
            }

            return new Rating((string)bundle["rating"], (string)bundle["ratingSource"]);
        }
        
        internal static Rating Unknown => new Rating("Unknown", "Unknown");
    }

    public sealed class Video
    {
        public string Id { get; }
        public string Type { get; }
        public string Url { get; }
        public string ThumbnailUrl { get; }
        public string Quality { get; }

        private Video() {
            Id = Guid.NewGuid().ToString();
        }

        private Video(string type, string url, string quality, string thumbnailUrl) {
            Id = url;
            Type = type;
            Url = url;
            ThumbnailUrl = thumbnailUrl;
            Quality = quality;
        }

        public static Video FromJsonBundle(object jsonBundle, string thumbnailUrl) {
            if (!(jsonBundle is Dictionary<string, object> bundle)) {
                return new Video();
            }

            return new Video((string)bundle["videoType"], (string)bundle["url"], (string)bundle["quality"], thumbnailUrl);
        }
    }

    public sealed class Content
    {
        public float Duration { get; }

        public Language Language { get; }
        public Rating Rating { get; }
        
        public DateTime DateAdded { get; }
        
        public IEnumerable<Video> Videos => m_videos;
        
        private readonly List<Video> m_videos = new List<Video>();
        
        private Content() { }

        private Content(float duration, Language language, Rating rating, DateTime dateAdded) {
            Duration = duration;
            Language = language;
            Rating = rating;
            DateAdded = dateAdded;
        }
        
        private void AddVideo(Video video) {
            m_videos.Add(video);
        }

        public static Content FromJsonBundle(object jsonBundle, string thumbnailUrl) {
            if (!(jsonBundle is Dictionary<string, object> bundle)) {
                return new Content();
            }

            var content = new Content(float.Parse((string)bundle["duration"]),
                bundle.ContainsKey("language") ? new Language((string)bundle["language"]) : Language.Unknown,
                bundle.ContainsKey("rating") ? Rating.FromJsonBundle(bundle["rating"]) : Rating.Unknown,
                DateTime.Parse((string)bundle["dateAdded"]));
            if (bundle["videos"] is List<object> videos) {
                foreach (var v in videos) {
                    content.AddVideo(Video.FromJsonBundle(v, thumbnailUrl));
                }
            }
            
            return content;
        }

        internal static Content Empty => new Content();
    }

    public sealed class Movie
    {
        public string Id { get; }
        public string Title { get; }
        public string ThumbnailUrl { get; }

        public string ShortDescription { get; private set; }
        public string LongDescription { get; private set; }
        
        public Content Content { get; private set; }

        public IEnumerable<string> Tags { get; private set; }

        public DateTime ReleaseDate { get; private set; }

        private Movie() {
            Content = Content.Empty;
        }

        private Movie(string id, string title, string thumbnailUrl) {
            Id = id;
            Title = title;
            ThumbnailUrl = thumbnailUrl;
        }

        public static bool TryParseFromJsonBundle(object jsonBundle, out Movie movie) {
            movie = null;
            if (!(jsonBundle is Dictionary<string, object> bundle)) {
                return false;
            }

            var id = (string)bundle["id"];
            if (string.IsNullOrEmpty(id)) {
                return false;
            }
            
            var thumbnailUrl = (string)bundle["thumbnail"];
            movie = new Movie(id, (string)bundle["title"], thumbnailUrl) {
                ShortDescription = bundle.ContainsKey("shortDescription") ? (string)bundle["shortDescription"] : string.Empty,
                LongDescription = bundle.ContainsKey("longDescription") ? (string)bundle["longDescription"] : string.Empty,
                ReleaseDate = DateTime.Parse((string)bundle["releaseDate"]),
                Tags = !(bundle["tags"] is List<object> tags) ? new List<string>() : tags.Select(t => (string)t),
                Content = Content.FromJsonBundle(bundle["content"], thumbnailUrl)
            };

            return true;
        }
    }

    public sealed class MoviesBundle
    {
        public string Provider { get; }
        public Language Language { get; }
        public Rating Rating { get; }
        public DateTime LastUpdated { get; }

        public IEnumerable<Movie> Movies => m_movies;

        private readonly List<Movie> m_movies = new List<Movie>();

        private MoviesBundle() { }

        private MoviesBundle(string provider, Language language, Rating rating, DateTime lastUpdated) {
            Provider = provider;
            Language = language;
            Rating = rating;
            LastUpdated = lastUpdated;
        }

        private void AddMovie(Movie movie) {
            m_movies.Add(movie);
        }

        public bool TryGetVideoById(string id, out Video video) {
            video = null;
            foreach (var movie in m_movies) {
                foreach (var v in movie.Content.Videos) {
                    if (v.Id.Equals(id)) {
                        video = v;
                        return true;
                    }
                }
            }
            
            return false;
        }

        public static MoviesBundle FromJson(string json) {
            if (!(Json.Deserialize(json) is Dictionary<string, object> bundle)) {
                return new MoviesBundle();
            }

            var provider = (string)bundle["providerName"];
            var locale = (string)bundle["language"];

            var moviesBundle = new MoviesBundle(provider, new Language(locale), Rating.FromJsonBundle(bundle["rating"]), DateTime.Parse((string)bundle["lastUpdated"]));
            if (bundle["shortFormVideos"] is List<object> shortFormVideos) {
                foreach (var videoBundle in shortFormVideos) {
                    if (Movie.TryParseFromJsonBundle(videoBundle, out var movie)) {
                        moviesBundle.AddMovie(movie);
                    }
                }
            }
            
            if (bundle["movies"] is List<object> movies) {
                foreach (var movieBundle in movies) {
                    if (Movie.TryParseFromJsonBundle(movieBundle, out var movie)) {
                        moviesBundle.AddMovie(movie);
                    }
                }
            }

            return moviesBundle;
        }
    }
}