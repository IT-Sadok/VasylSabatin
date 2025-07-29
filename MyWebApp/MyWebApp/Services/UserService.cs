using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MyWebApp.Data;
using MyWebApp.DTO;
using MyWebApp.DTO.Exceptions;
using MyWebApp.Interfaces;
using MyWebApp.Models;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Services;
using MyWebApp.Services.Interfaces;

namespace MyWebApp;

public class UserService : IUserService
{
    
    private readonly UserManager<User> _userManager;
    private readonly IRequesterContext _requesterContext;

    public UserService(UserManager<User> userManager, IRequesterContext requesterContext)
    {
        _userManager = userManager;
        _requesterContext = requesterContext;   
        
    }

    public async Task<UserProfileModel> GetUserProfileAsync()
    {
        var userId = _requesterContext.GetRequesterContext();

        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (userId == null)
        {
            throw new InvalidTokenException();
        }

        if (user == null)
        {
            throw new UserNotFoundException();
        }

        return new UserProfileModel
        {
            FullName = user.FullName,
            Age = user.Age,
            Weight = user.Weight,
            Email = user.Email,
            AccountDescription = user.AccountDescription
        };
    }

    public async Task UpdateUserProfileAsync(UserUpdateModel dto)
    {
        var userId = _requesterContext.GetRequesterContext();

        if (userId == null)
        {
            throw new InvalidTokenException();
        }
        
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            throw new UserNotFoundException();
        }
        
        user.FullName = dto.FullName;
        user.Age = dto.Age;
        user.Weight = dto.Weight;
        user.AccountDescription = dto.AccountDescription;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
            throw new UserUpdateFailedException(errors);
        }
    }

    public async Task DeleteUserProfileAsync()
    {
        var userId = _requesterContext.GetRequesterContext();
        var user = await _userManager.FindByIdAsync(userId.ToString());
        
        if (user == null)
        {
            throw new UserNotFoundException();
        }
        
        var result = await _userManager.DeleteAsync(user);
        
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
            throw new UserUpdateFailedException(errors);
        }
    }
}