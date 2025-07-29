using System.Security.Claims;
using MyWebApp.DTO;
using MyWebApp.Models;

namespace MyWebApp.Interfaces;

public interface IUserService
{ 
    Task<UserProfileModel> GetUserProfileAsync(ClaimsPrincipal userClaims);
    
    Task UpdateUserProfileAsync(UserUpdateModel dto, ClaimsPrincipal userClaims);

    Task DeleteUserProfileAsync(int userId);
}