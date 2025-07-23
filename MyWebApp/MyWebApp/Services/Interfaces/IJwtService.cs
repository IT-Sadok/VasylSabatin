namespace MyWebApp.Interfaces;

public interface IJwtService
{
    string GenerateJwtToken(string userName, int userId);
}