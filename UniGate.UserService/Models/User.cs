using Microsoft.AspNetCore.Identity;
using UniGate.UserService.Enums;

namespace UniGate.UserService.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Patronymic { get; set; } = string.Empty;
    public Role Role { get; set; }
}