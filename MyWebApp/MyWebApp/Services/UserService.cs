using Microsoft.AspNetCore.Identity;
using MyWebApp.DTO;
using MyWebApp.DTO.Exceptions;
using MyWebApp.Interfaces;
using MyWebApp.Mapper;
using MyWebApp.Models;
using MyWebApp.Services.Interfaces;

namespace MyWebApp;

public class UserService : IUserService
{
    
    private readonly UserManager<User> _userManager;
    private readonly IUserContext _userContext;

    public UserService(UserManager<User> userManager, IUserContext userContext)
    {
        _userManager = userManager;
        _userContext = userContext;   
        
    }

    public async Task<UserProfileModel> GetUserProfileAsync()
    {
        var userId = _userContext.UserId.ToString();
        var user = await _userManager.FindByIdAsync(userId);
        
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        return user.ToModel();
    }

    public async Task UpdateUserProfileAsync(UserUpdateModel model)
    {
        var userId = _userContext.UserId.ToString();
        
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            throw new UserNotFoundException();
        }
        
        model.ApplyTo(user);

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
            throw new UserUpdateFailedException(errors);
        }
    }

    public async Task DeleteUserProfileAsync()
    {
        var userId = _userContext.UserId.ToString();
        var user = await _userManager.FindByIdAsync(userId);
        
        if (user == null)
        {
            throw new UserNotFoundException();
        }
        
        var result = await _userManager.DeleteAsync(user);
        
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
            throw new UserDeleteFailedException(errors);
        }
    }
}