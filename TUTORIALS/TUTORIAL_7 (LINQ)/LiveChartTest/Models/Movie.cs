using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

class Movie {
    [Name("movieId")]
    public int MovieId { get; set; }
    
    [Name("title")]
    public string Title { get; set; } = String.Empty;
    
    [Name("genres")]
    public string Genres { get; set; } = String.Empty;

    public List<string> GenresList => Genres.Split('|').ToList();

    public override string ToString() {
        return $"MovieId: {MovieId}, Title: {Title}, Genres: {Genres}";
    }


}