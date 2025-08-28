using Microsoft.AspNetCore.Identity;
using MyWebApp;
using MyWebApp.Constants;
using MyWebApp.DTO;
using MyWebApp.DTO.Exceptions;
using MyWebApp.Interfaces;
using MyWebApp.Models;
using NSubstitute;
using Xunit.Sdk;

public class AuthenticationServiceTests
{
    [Fact]
    public async Task RegisterUserAsync_WhenSuccess_ReturnsAuthModelWithToken()
    {
        // Arrange
        var userManager = CreateUserManagerSubstitute();
        var jwtService = Substitute.For<IJwtService>();

        var model = new SignUpModel
        {
            FullName = "Test",
            Age = 20,
            Weight = 78,
            Email = "example@gmail.com",
            Password = "StrongP@ssw0rd"
        };
        
        userManager.CreateAsync(Arg.Any<User>(), model.Password)
            .Returns(Task.FromResult(IdentityResult.Success));
        
        jwtService.GenerateJwtToken(Arg.Any<string>(), Arg.Any<int>())
            .Returns("token");
        
        var service = new AuthenticationService(userManager, null!, jwtService);
        
        //Act
        var result = await service.RegisterUserAsync(model);
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal("token", result.AccessToken);
        
        await userManager.Received(1)
            .CreateAsync(Arg.Is<User>(u => 
                u.FullName == model.FullName &&
                u.Age == model.Age &&
                u.Email == model.Email &&
                u.Weight == model.Weight &&
                u.UserName == model.Email),
            model.Password);
        
        jwtService.Received(1).GenerateJwtToken(model.Email, Arg.Any<int>());
    }
    
    [Fact]
    public async Task RegisterUserAsync_WhenFailure_ThrowsSignUpFailedException()
    {
        // Arrange
        var userManager = CreateUserManagerSubstitute();
        var jwtService = Substitute.For<IJwtService>();

        var identityErrors = new List<IdentityError>
        {
            new IdentityError { Code = "DublicateEmail", Description = ErrorMessages.DuplicateEmail }
        };

        userManager.CreateAsync(Arg.Any<User>(), Arg.Any<string>())
            .Returns(Task.FromResult(IdentityResult.Failed(identityErrors.ToArray())));
        
        var service = new AuthenticationService(userManager, null!, jwtService);

        var model = new SignUpModel
        {
            FullName = "Test",
            Age = 20,
            Weight = 78,
            Email = "test@example.com",
            Password = "Password123!"
        };
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<SignUpFailedException>(() => service.RegisterUserAsync(model));
        Assert.Contains(ErrorMessages.DuplicateEmail, exception.Message);
        
        jwtService.DidNotReceive()
            .GenerateJwtToken(Arg.Any<string>(), Arg.Any<int>());
        
        await userManager.Received(1)
            .CreateAsync(Arg.Is<User>(u => 
                    u.Email == model.Email &&
                    u.UserName == model.Email &&
                    u.FullName == model.FullName &&
                    u.Age == model.Age &&
                    u.Weight == model.Weight),
                model.Password);
    }
    
    [Fact]
    public async Task LoginUserAsync_WhenSuccess_ReturnsAuthModelWithToken()
    {
        // Arrange
        var userManager = CreateUserManagerSubstitute();
        var jwtService = Substitute.For<IJwtService>();

        var model = new SignInModel {Email = "test@example.com", Password = "Password123!"};
        var user = new User {Id = 1, Email = model.Email, UserName = model.Email};
        
        userManager.FindByNameAsync(model.Email)
            .Returns(Task.FromResult(user));
        
        userManager.CheckPasswordAsync(user, model.Password)
            .Returns(Task.FromResult(true));
        
        jwtService.GenerateJwtToken(Arg.Any<string>(), Arg.Any<int>())
            .Returns("token");
        
        var service = new AuthenticationService(userManager, null!, jwtService);
        
        //Act
        var result = await service.LoginUserAsync(model);
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal("token", result.AccessToken);
        
        await userManager.Received(1).FindByNameAsync(model.Email);
        await userManager.Received(1).CheckPasswordAsync(user, model.Password);
        jwtService.Received(1).GenerateJwtToken(model.Email, user.Id);
        
    }
    
    [Fact]
    public async Task LoginUserAsync_WhenUserNotFound_ThrowsSignInFailedException()
    {
        // Arrange
        var userManager = CreateUserManagerSubstitute();
        var jwtService = Substitute.For<IJwtService>();

        var model = new SignInModel { Email = "test@example.com", Password = "Password123!" };
        
        userManager.FindByNameAsync(model.Email).Returns((User?)null);
        
        var service = new AuthenticationService(userManager, null!, jwtService);
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<SignInFailedException>(() => service.LoginUserAsync(model));
        Assert.Equal("User not found. Try again.", exception.Message);
        
        await userManager.Received(1).FindByNameAsync(model.Email);
        await userManager.DidNotReceive().CheckPasswordAsync(Arg.Any<User>(), Arg.Any<string>());
        jwtService.DidNotReceive().GenerateJwtToken(Arg.Any<string>(), Arg.Any<int>());
    }
    
    [Fact]
    public async Task LoginUserAsync_WhenPasswordInvalid_ThrowsSignInFailedException()
    {
        // Arrange
        var userManager = CreateUserManagerSubstitute();
        var jwtService = Substitute.For<IJwtService>();
        
        var model = new SignInModel { Email = "test@example.com", Password = "Password123!" };
        var user = new User {Id = 1, Email = model.Email, UserName = model.Email};
        
        userManager.FindByNameAsync(model.Email)
            .Returns(Task.FromResult(user));
        
        userManager.CheckPasswordAsync(user, model.Password)
            .Returns(Task.FromResult(false));
        
        var service = new AuthenticationService(userManager, null!, jwtService);
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<SignInFailedException>(() => service.LoginUserAsync(model));
        Assert.Equal("Invalid password. Try again.", exception.Message);
        
        await userManager.Received(1).FindByNameAsync(model.Email);
        await userManager.Received(1).CheckPasswordAsync(user, model.Password);
        jwtService.DidNotReceive().GenerateJwtToken(Arg.Any<string>(), Arg.Any<int>());
    }
    
    private static UserManager<User> CreateUserManagerSubstitute()
    {
        var store = Substitute.For<IUserStore<User>>();
        return Substitute.For<UserManager<User>>(
            store, null, null, null, null, null, null, null, null);
    }
} 