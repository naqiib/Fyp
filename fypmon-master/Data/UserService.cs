using BlazorServerAuthenticationAndAuthorization.Data;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using BlazorServerAuthenticationAndAuthorization.Authentication;

namespace BlazorServerAuthenticationAndAuthorization.Services
{
    public class UserService
    {
        private readonly string _connectionString;

        public UserService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<UserInfo?> GetUserByEmailAsync(string email)
        {
            Console.WriteLine($"Fetching user with email: {email}");

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("SELECT * FROM UserInfo WHERE Email = @Email", connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                return new UserInfo
                                {
                                    Id = reader.GetInt32(0),
                                    UserName = reader.GetString(1),
                                    Email = reader.GetString(2),
                                    Password = reader.GetString(3),
                                    Role = reader.GetString(4),
                                    Name = reader.GetString(5),
                                    Phone = reader.GetString(6),
                                    Location = reader.GetString(7),
                                    Bio = reader.GetString(8),
                                    ProfilePicture = reader.GetString(9)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user: {ex.Message}");
            }

            Console.WriteLine("User not found.");
            return null;
        }

        public async Task UpdateUserAsync(UserInfo userInfo)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("UpdateUserInfo", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", userInfo.Id);
                        command.Parameters.AddWithValue("@UserName", userInfo.UserName);
                        command.Parameters.AddWithValue("@Email", userInfo.Email);
                        command.Parameters.AddWithValue("@Password", userInfo.Password);
                        command.Parameters.AddWithValue("@Role", userInfo.Role);
                        command.Parameters.AddWithValue("@Name", userInfo.Name);
                        command.Parameters.AddWithValue("@Phone", userInfo.Phone);
                        command.Parameters.AddWithValue("@Location", userInfo.Location);
                        command.Parameters.AddWithValue("@Bio", userInfo.Bio);
                        command.Parameters.AddWithValue("@ProfilePicture", userInfo.ProfilePicture);
                        await command.ExecuteNonQueryAsync();
                    }
                }

                Console.WriteLine($"User {userInfo.UserName} updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
            }
        }
    }
}
