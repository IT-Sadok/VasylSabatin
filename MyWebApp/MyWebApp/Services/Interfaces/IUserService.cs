using System.Security.Claims;
using MyWebApp.DTO;
using MyWebApp.Models;

namespace MyWebApp.Interfaces;

public interface IUserService
{ 
    Task<UserProfileModel> GetUserProfileAsync();
    
    Task UpdateUserProfileAsync(UserUpdateModel dto);

    Task DeleteUserProfileAsync();
}