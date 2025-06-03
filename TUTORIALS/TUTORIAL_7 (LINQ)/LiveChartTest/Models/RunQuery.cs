using System;
using System.Collections.Generic;
using System.Linq;

public class TopMovieGenresQueryRunner 
{
    public List<double> counts = [];
    public List<string> names = [];

    public void Run(int numGenres) 
    {
        DataLoader<Movie> movieLoader = new DataLoader<Movie>();
        movieLoader.LoadData("./Assets/movies.csv");

        DataLoader<MovieRating> ratingLoader = new DataLoader<MovieRating>();
        ratingLoader.LoadData("./Assets/ratings.csv");

        var movies = movieLoader.data;
        var ratings = ratingLoader.data;
    
        var movie_ratings = movies.Join(ratings,
                                    m => m.MovieId,
                                    r => r.MovieId,
                                    (movie, rating) => new { movie.MovieId, movie.Title, rating.Rating })
                                .ToList();

        var average_rating = movie_ratings.GroupBy(mr => mr.MovieId)
                                        .Select(mr => new
                                        {
                                            MovieId = mr.Key,
                                            MovieTitle = mr.First().Title,
                                            Count = mr.Count(),
                                            AverageRating = mr.Average(r => r.Rating),
                                            GenresList = movies.First(m => m.MovieId == mr.Key).GenresList
                                        })
                                        .Where(r => r.Count > 100)
                                        .OrderByDescending(r => r.AverageRating)
                                        .ToList();

        var genre_counts = average_rating.Take(1000).SelectMany(m => m.GenresList)
                            .GroupBy(g => g)
                            .Select(g => new
                            {
                                Genre = g.Key,
                                Count = g.Count() 
                            })
                            .OrderByDescending(g => g.Count);

        counts = genre_counts.Select(g => (double)g.Count).Take(numGenres).ToList();
        names = genre_counts.Select(g => g.Genre).Take(numGenres).ToList();
    }
}