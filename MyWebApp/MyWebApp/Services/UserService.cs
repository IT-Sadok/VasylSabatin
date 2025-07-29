using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MyWebApp.Data;
using MyWebApp.DTO;
using MyWebApp.DTO.Exceptions;
using MyWebApp.Interfaces;
using MyWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MyWebApp;

public class UserService : IUserService
{
    
    private readonly UserManager<User> _userManager;
    private readonly ApplicationContext _dbContext;
    
    public UserService(UserManager<User> userManager, ApplicationContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public async Task<UserProfileModel> GetUserProfileAsync(ClaimsPrincipal userClaims)
    {
        var userId = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? userClaims.FindFirst("sub")?.Value;

        var user = await _userManager.FindByIdAsync(userId);

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

    public async Task UpdateUserProfileAsync(UserUpdateModel dto, ClaimsPrincipal userClaims)
    {
        var userId = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? userClaims.FindFirst("sub")?.Value;

        var user = await _userManager.FindByIdAsync(userId);

        if (userId == null)
        {
            throw new InvalidTokenException();
        }

        if (user == null)
        {
            throw new UserNotFoundException();
        }

        if (dto.FullName != null)
            user.FullName = dto.FullName;


        if (dto.Age.HasValue)
            user.Age = dto.Age.Value;

        if (dto.Weight.HasValue)
            user.Weight = dto.Weight.Value;

        if (dto.AccountDescription != null)
            user.AccountDescription = dto.AccountDescription;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
            throw new UserUpdateFailedException(errors);
        }
    }

    public async Task DeleteUserProfileAsync(int userId)
    {
        var user = await _dbContext.Users.FindAsync(userId);
        if (user == null)
        {
            throw new UserNotFoundException();
        }
        
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }
}