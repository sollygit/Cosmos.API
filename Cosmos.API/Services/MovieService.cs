using Cosmos.Common;
using Cosmos.Model;
using Microsoft.Azure.CosmosRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cosmos.Api.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAsync();
        Task<Movie> GetAsync(string id);
        Task<Movie> CreateAsync(Movie movie);
        Task<IEnumerable<Movie>> CreateAsync(IEnumerable<Movie> movies);
        Task<IEnumerable<Movie>> CreateAsync(int count, bool saveToDatabase = false);
        Task<Movie> CreateDummyAsync();
        Task<Movie> UpdateAsync(string id, Movie movie);
        Task<Movie> DeleteAsync(string id);
    }

    public class MovieService : IMovieService
    {
        readonly IRepository<Movie> _movieRepository;

        public MovieService(IRepository<Movie> movieRepository) =>
            (_movieRepository) = (movieRepository);

        public async Task<IEnumerable<Movie>> GetAsync()
        {
            return await _movieRepository.GetAsync(o => o.Price > 0);
        }

        public async Task<Movie> GetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("Movie ID cannot be empty");
            }

            return await _movieRepository.GetAsync(id);
        }

        public async Task<Movie> CreateAsync(Movie movie)
        {
            if (movie == null)
            {
                throw new Exception("Movie cannot be null");
            }

            return await _movieRepository.CreateAsync(movie);
        }

        public async Task<IEnumerable<Movie>> CreateAsync(IEnumerable<Movie> movies)
        {
            if (movies == null)
            {
                throw new Exception("Movies cannot be null");
            }
            return await _movieRepository.CreateAsync(movies);
        }

        public async Task<IEnumerable<Movie>> CreateAsync(int count, bool saveToDB)
        {
            var items = BogusUtil.Movies(count);
            if (saveToDB)
            {
                await _movieRepository.CreateAsync(items);
            }
            return items;
        }

        public async Task<Movie> CreateDummyAsync()
        {
            var items = BogusUtil.Movies(1);
            var dummy = items.FirstOrDefault();
            dummy.Genre = [];
            dummy.MovieRatings = [];
            dummy.IsActive = false;
            return await _movieRepository.CreateAsync(dummy);
        }

        public async Task<Movie> UpdateAsync(string id, Movie movie)
        {
            var item = await _movieRepository.GetAsync(id);

            item.MovieID = movie.MovieID;
            item.Title = movie.Title;
            item.ReleaseDate = movie.ReleaseDate;
            item.Poster = movie.Poster;
            item.Price = movie.Price;
            item.Genre = movie.Genre;
            item.MovieRatings = movie.MovieRatings;
            item.IsActive = movie.IsActive;

            return await _movieRepository.UpdateAsync(item);
        }

        public async Task<Movie> DeleteAsync(string id)
        {
            var item = await _movieRepository.GetAsync(id);
            await _movieRepository.DeleteAsync(id);
            return item;
        }
    }
}
