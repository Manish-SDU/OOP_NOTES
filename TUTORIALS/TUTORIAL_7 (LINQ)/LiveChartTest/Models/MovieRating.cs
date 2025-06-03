using CsvHelper.Configuration.Attributes;

class MovieRating {
    [Name("userId")]
    public int UserId { get; set; }

    [Name("movieId")]
    public int MovieId { get; set; }

    [Name("rating")]
    public double Rating { get; set;} 

    [Name("timestamp")]
    public long Timestamp { get; set; }
}