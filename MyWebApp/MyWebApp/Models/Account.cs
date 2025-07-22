using System.ComponentModel.DataAnnotations.Schema;
namespace MyWebApp.Models;

[Table(nameof(Account))]

public class Account
{
    public int Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public string? Description { get; set; }
}