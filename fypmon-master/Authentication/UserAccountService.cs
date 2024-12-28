using BlazorServerAuthenticationAndAuthorization.Data;
using BlazorServerAuthenticationAndAuthorization.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlazorServerAuthenticationAndAuthorization.Authentication
{
    public class UserAccountService
    {
        private readonly AppDbContext _context;

        public UserAccountService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserInfo?> GetByUserNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return null;
            }

            return await _context.UserInfo
                .Include(ua => ua.Books)
                .FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task<UserInfo?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            return await _context.UserInfo
                .Include(ua => ua.Books)
                .FirstOrDefaultAsync(x => x.Email == email);
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

        public async Task<List<UserInfo>> GetAllUsersAsync()
        {

            return await _context.UserInfo.
                Where(u => u.Role != "Administrator")
                .ToListAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _context.UserInfo.FindAsync(userId);
            if (user != null)
            {
                _context.UserInfo.Remove(user);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books
            .Include(b => b.UserId)
            .FirstOrDefaultAsync(b => b.Id == id);// Include the User information .FirstOrDefaultAsync(b => b.Id ==
        }

        public async Task<int> GetTotalBooksUploadedAsync() {
            return await _context.Books.CountAsync(); 
        }
    }
}
