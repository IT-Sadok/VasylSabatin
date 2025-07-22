namespace MyWebApp.DTO;

public class SignUpModel
{
    public string Username { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public string? AccountDescription { get; set; }
}