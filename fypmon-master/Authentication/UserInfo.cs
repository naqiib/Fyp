using BlazorServerAuthenticationAndAuthorization.Model;

namespace BlazorServerAuthenticationAndAuthorization.Authentication
{
    public class UserInfo
    {
      


            public int Id { get; set; }
            public string? UserName { get; set; }
            public string? Email { get; set; }
            public string? Password { get; set; }
            public string? Role { get; set; }
            public string? Name { get; set; }
            public string? Phone { get; set; }
            public string? Location { get; set; }
            public string? Bio { get; set; }
            public string? ProfilePicture { get; set; }
            public ICollection<Book>? Books { get; set; } // Navigation property for related books
        }
    }


