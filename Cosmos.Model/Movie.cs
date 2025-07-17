using Microsoft.Azure.CosmosRepository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cosmos.Model
{
    public class Movie : Item
    {
        [Required]
        public string MovieID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        public string Year => $"{ReleaseDate.Year}";
        public string Poster { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string[] Genre { get; set; } = Array.Empty<string>();
        public IEnumerable<MovieRating> MovieRatings { get; set; } = new List<MovieRating>();
        public bool IsActive { get; set; } = true;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class MovieRating : Item
    {
        [Required]
        public string Rated { get; set; }
        [Required]
        public string Language { get; set; }
        [Required]
        public int Metascore { get; set; }
        [Required]
        public decimal Rating { get; set; }
        public string Votes { get; set; }
    }
}
