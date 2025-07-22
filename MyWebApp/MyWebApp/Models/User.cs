using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MyWebApp.Models;

[Table(nameof(User))]

public class User : IdentityUser<int>
{
    public int AccountId { get; set; }
    
    [ForeignKey(nameof(AccountId))]
    
    public Account? Account { get; set; }
}