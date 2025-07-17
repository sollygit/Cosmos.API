using Bogus;
using Cosmos.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cosmos.Common
{
    public static class BogusUtil
    {
        public static List<Movie> Movies(int count)
        {
            var movies = new List<Movie>();

            for (int i = 0; i < count; i++)
            {
                movies.Add(GetMovie());
            }

            return movies;
        }

        private static Movie GetMovie(bool isActive = true)
        {
            return new Faker<Movie>()
                .RuleFor(c => c.Id, f => Guid.NewGuid().ToString("N"))
                .RuleFor(o => o.MovieID, f => f.Random.AlphaNumeric(9).ToUpper())
                .RuleFor(o => o.Title, f => f.Lorem.Sentence(3))
                .RuleFor(c => c.ReleaseDate, f => f.Date.Past(30))
                .RuleFor(o => o.Poster, f => f.Image.PicsumUrl(640, 480, true))
                .RuleFor(o => o.Price, f => decimal.Parse(f.Random.Decimal(1000).ToString("0.##")))
                .RuleFor(o => o.Genre, GetGenre(3))
                .RuleFor(o => o.IsActive, isActive)
                .Generate(1)
                .FirstOrDefault();
        }

        private static string[] GetGenre(int count)
        {
            var genres = new Faker<Genre>()
                .RuleFor(o => o.Name, f => f.Rant.Random.CollectionItem(Constants.Genres))
                .Generate(count)
                .Select(o => o.Name)
                .Distinct()
                .ToArray();
            return genres;
        }

        class Genre
        {
            public string Name { get; set; }
        }
    }
}
