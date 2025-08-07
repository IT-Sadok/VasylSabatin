using System.Security.Claims;
using MyWebApp.DTO.Exceptions;
using MyWebApp.Services.Interfaces;

namespace MyWebApp.Services;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId {
        get
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? throw new InvalidTokenException();
        
            if (!int.TryParse(userIdClaim, out var userId))
                throw new InvalidTokenException();
            
            return userId;
        }
    }

}    