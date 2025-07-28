namespace MyWebApp.DTO;

public class SignUpModel
{
    public string FullName { get; set; }
    
    public int Age { get; set; }
    
    public double Weight { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public string? AccountDescription { get; set; }
}