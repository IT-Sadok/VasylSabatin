using Microsoft.AspNetCore.Identity;
using MyWebApp.Data;
using MyWebApp.DTO;
using MyWebApp.DTO.Exceptions;
using MyWebApp.Interfaces;
using MyWebApp.Models;

namespace MyWebApp;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    
    private readonly ApplicationContext _dbContext;
    private readonly IJwtService _jwtService;

    public AuthenticationService(UserManager<User> userManager, 
        ApplicationContext dbContext, 
        IJwtService jwtService)
    {
        _userManager = userManager;
        _dbContext = dbContext;
        _jwtService = jwtService;
    }
    
    public async Task<AuthModel> RegisterUserAsync(SignUpModel model)
    {
        var user = new User
        {
            UserName = model.Username,
            Email = model.Email,
            CreatedAt = DateTime.UtcNow,
            Description = model.AccountDescription
        };

        _dbContext.Users.Add(user);

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            throw new SignUpFailedException(
                string.Join("; ", result.Errors.Select(e => $"{e.Code}: {e.Description}"))
            );
        }

        return new AuthModel
        {
            AccessToken = _jwtService.GenerateJwtToken(user.UserName, user.Id)
        };
    }
    
    public async Task<AuthModel> LoginUserAsync(SignInModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null)
        {
            throw new SignInFailedException("User not found. Try again.");
        }
        var isPasswordValid = await _userManager.CheckPasswordAsync(user!, model.Password);
        
        if (!isPasswordValid)
        {
            throw new SignInFailedException("Invalid password. Try again.");
        }
        return new AuthModel
        {
            AccessToken = _jwtService.GenerateJwtToken(user.UserName, user.Id)
        };
    }
}