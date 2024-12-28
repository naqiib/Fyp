using BlazorServerAuthenticationAndAuthorization.Authentication;

namespace BlazorServerAuthenticationAndAuthorization.Model
{
    public class Book
    {
        public int Id { get; set; }
        public string? BookName { get; set; }
        public string? ImagePath { get; set; }
        public string? ISBN { get; set; }
        public string? Author { get; set; }
        public string? Publisher { get; set; }
        public string? Edition { get; set; }
        public string? Exchange { get; set; }
        public string? Condition { get; set; }
        public string? Category { get; set; }
        public decimal? Price { get; set; }
        public string? CellNo { get; set; }
        public string? City { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? ExchangeBookName { get; set; }
        public bool IsExchangeable { get; set; }
        public int UserId { get; set; }
        public UserInfo? UserInfo { get; set; } // Navigation property for the related user
    }
}
