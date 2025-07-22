using Microsoft.AspNetCore.Identity;
using MyWebApp.Data;
using MyWebApp.DTO;
using MyWebApp.DTO.Exceptions;
using MyWebApp.Models;

namespace MyWebApp;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    
    private readonly ApplicationContext _dbContext;
    private readonly IJwtService _jwtService;

    public UserService(UserManager<User> userManager, 
        ApplicationContext dbContext, 
        IJwtService jwtService, 
        IHttpContextAccessor contextAccessor)
    {
        _userManager = userManager;
        _dbContext = dbContext;
        _jwtService = jwtService;
    }
    
    public async Task<AuthModel> RegisterUserAsync(SignUpModel model)
    {
        var account = new Account
        {
            CreatedAt = DateTime.UtcNow,
            Description = model.AccountDescription
        };

        _dbContext.Accounts.Add(account);
        await _dbContext.SaveChangesAsync();

        var user = new User
        {
            AccountId = account.Id,
            UserName = model.Username,
            Email = model.Email
        };

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

    public interface IUserService
{
    Task<AuthModel> RegisterUserAsync(SignUpModel model);
    
    Task<AuthModel> LoginUserAsync(SignInModel model);
}