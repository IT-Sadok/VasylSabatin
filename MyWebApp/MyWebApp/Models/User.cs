using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MyWebApp.Models;

[Table(nameof(User))]

public class User : IdentityUser<int>
{
    public DateTime CreatedAt { get; set; }
    
    public string? Description { get; set; }
}