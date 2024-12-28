using BlazorServerAuthenticationAndAuthorization.Authentication;
using BlazorServerAuthenticationAndAuthorization.Data;
using BlazorServerAuthenticationAndAuthorization.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerAuthenticationAndAuthorization.Services
{
    public class BookService
    {
        private readonly AppDbContext _context;

        public BookService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddBookAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _context.Books
                .Include(b => b.UserInfo)
                .ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books
                .Include(b => b.UserInfo)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<Book>> GetBooksByCategoryAsync(string category)
        {
            return await _context.Books
                .Include(b => b.UserInfo)
                .Where(b => b.Category == category)
                .ToListAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CreateUserAsync(UserInfo userInfo)
        {
            // Ensure all required fields are set
            if (string.IsNullOrEmpty(userInfo.Role))
            {
                userInfo.Role = "User"; // Default role if not selected
            }

            // Add the UserInfo
            _context.UserInfo.Add(userInfo);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<UserInfo?> GetUserInfoByEmailAsync(string email)
        {
            return await _context.UserInfo
                .Include(u => u.Books)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<Book>> GetBooksByUserIdAsync(int userId)
        {
            return await _context.Books
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Book>> GetNearbyBooksAsync(double userLatitude, double userLongitude, double maxDistanceKm)
        {
            var earthRadiusKm = 6371;
            var books = await _context.Books
                .Include(b => b.UserInfo)
                .ToListAsync();
            var nearbyBooks = new List<Book>();

            foreach (var book in books)
            {
                if (book.Latitude.HasValue && book.Longitude.HasValue)
                {
                    var distanceKm = CalculateDistance(userLatitude, userLongitude, (double)book.Latitude.Value, (double)book.Longitude.Value, earthRadiusKm);
                    if (distanceKm <= maxDistanceKm)
                    {
                        nearbyBooks.Add(book);
                    }
                }
            }

            return nearbyBooks;
        }

        public async Task<List<Book>> GetBooksByCityAsync(string city)
        {
            var books = await _context.Books
                .Include(b => b.UserInfo)
                .Where(b => b.City == city)
                .ToListAsync();

            // Debugging statement
            Console.WriteLine($"Filtered books by city '{city}': {books.Count} books found.");

            return books;
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2, double radius)
        {
            var dLat = DegreesToRadians(lat2 - lat1);
            var dLon = DegreesToRadians(lon2 - lon1);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                  Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                  Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = radius * c;
            return distance;
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}
