using Microsoft.AspNetCore.Identity;
using MyWebApp.Constants;
using MyWebApp.DTO;
using MyWebApp.DTO.Exceptions;
using MyWebApp.Models;
using MyWebApp.Services.Interfaces;
using NSubstitute;

namespace MyWebApp.Tests;

public class UserServiceTests
{
    [Fact]
    public async Task GetUserProfileAsync_WhenSuccess_ReturnsUserProfile()
    {
        // Arrange
        var userManager = CreateUserManagerSubstitute();
        var userContext = CreateUserContextSubstitute(userId: TestConstants.TestUserId);

        var user = new User
        {
            Id = TestConstants.TestUserId,
            Email = "test@example.com",
            UserName = "test@example.com",
            FullName = "Tester",
            Age = 20,
            Weight = 78
        };

        userManager.FindByIdAsync(TestConstants.TestUserIdString).Returns(user);
        
        var service = new UserService(userManager,  userContext);
        
        //Act
        var result = await service.GetUserProfileAsync();
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.FullName, result.FullName);
        Assert.Equal(user.Age, result.Age);
        Assert.Equal(user.Weight, result.Weight);
        
        await userManager.Received(1).FindByIdAsync(TestConstants.TestUserIdString);
    }

    [Fact]
    public async Task GetUserProfileAsync_WhenFailure_ThrowsUserNotFoundException()
    {
        var userManager = CreateUserManagerSubstitute();
        var userContext = CreateUserContextSubstitute(userId: TestConstants.TestUserId);
        
        userManager.FindByIdAsync(TestConstants.TestUserIdString).Returns((User)null);
        
        var service = new UserService(userManager,  userContext);
        
        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(()  => service.GetUserProfileAsync());
        await userManager.Received(1).FindByIdAsync(TestConstants.TestUserIdString);
    }

    [Fact]
    public async Task UpdateUserProfileAsync_WhenSuccess_UpdatesUser()
    {
        //Arrange
        var userManager = CreateUserManagerSubstitute();
        var userContext = CreateUserContextSubstitute(userId: TestConstants.TestUserId);

        var user = new User
        {
            Id = TestConstants.TestUserId,
            FullName = "Old Name",
            Age = 18,
            Weight = 70,
            Email = "old@example.com",
            UserName = "old@example.com"
        };

        var model = new UserUpdateModel
        {
            FullName = "New Name",
            Age = 21,
            Weight = 80
        };
        
        userManager.FindByIdAsync(TestConstants.TestUserIdString).Returns(user);
        userManager.UpdateAsync(Arg.Any<User>()).Returns(IdentityResult.Success);

        var service = new UserService(userManager, userContext);
        
        //Act 
        await service.UpdateUserProfileAsync(model);
        
        //Assert
        await userManager.Received(1).FindByIdAsync(TestConstants.TestUserIdString);
        await userManager.Received(1).UpdateAsync(Arg.Is<User>(u =>
            u.Id == TestConstants.TestUserId &&
            u.FullName == model.FullName &&
            u.Age == model.Age &&
            u.Weight == model.Weight));
    }

    [Fact]
    public async Task UpdateUserProfileAsync_WhenFailure_ThrowsUserNotFoundException()
    {
        //Arrange
        var userManager = CreateUserManagerSubstitute();
        var userContext = CreateUserContextSubstitute(userId: TestConstants.TestUserId);
        
        userManager.FindByIdAsync(TestConstants.TestUserIdString).Returns((User)null);
        
        var service = new UserService(userManager, userContext);
        
        //Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(()  => service.UpdateUserProfileAsync(new UserUpdateModel()));
        await userManager.Received(1).FindByIdAsync(TestConstants.TestUserIdString);
        await userManager.DidNotReceive().UpdateAsync(Arg.Any<User>());
    }

    [Fact]
    public async Task UpdateUserProfileAsync_WhenFailure_ThrowsUserUpdateFailedException()
    {
        //Arrange
        var userManager = CreateUserManagerSubstitute();
        var userContext = CreateUserContextSubstitute(userId: TestConstants.TestUserId);

        var user = new User { Id = TestConstants.TestUserId, FullName = "Old Name" };
        var model = new UserUpdateModel {FullName = "New Name"};

        userManager.FindByIdAsync(TestConstants.TestUserIdString).Returns(user);
        
        var errors = new []
            {
                new IdentityError { Code = "DuplicateEmail", Description = ErrorMessages.DuplicateEmail},
                new IdentityError { Code = "DuplicateUserName", Description = ErrorMessages.DuplicateUserName}
            };
        
        userManager.UpdateAsync(Arg.Any<User>())
            .Returns(IdentityResult.Failed(errors));
        
        var service = new UserService(userManager, userContext);
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<UserUpdateFailedException>(() => service.UpdateUserProfileAsync(model));
        Assert.Contains(ErrorMessages.DuplicateEmail, exception.Message);
        Assert.Contains(ErrorMessages.DuplicateUserName, exception.Message);
        
        await userManager.Received(1).FindByIdAsync(TestConstants.TestUserIdString);
        await userManager.Received(1).UpdateAsync(Arg.Is<User>(u =>
            u.Id == TestConstants.TestUserId &&
            u.FullName == model.FullName));
    }
    
    [Fact]
    public async Task DeleteUserProfileAsync_WhenSuccess_DeletesUser()
    {
        //Arrange
        var userManager = CreateUserManagerSubstitute();
        var userContext = CreateUserContextSubstitute(userId: TestConstants.TestUserId);

        var user = new User { Id = TestConstants.TestUserId, Email = "test@example.com", UserName = "Test" };
        
        userManager.FindByIdAsync(TestConstants.TestUserIdString).Returns(user);
        userManager.DeleteAsync(Arg.Any<User>()).Returns(IdentityResult.Success);
        
        var service = new UserService(userManager, userContext);
        
        //Act
        await service.DeleteUserProfileAsync();
        
        //Assert
        await userManager.Received(1).FindByIdAsync(TestConstants.TestUserIdString);
        await userManager.Received(1).DeleteAsync(Arg.Is<User>(u => u.Id == TestConstants.TestUserId));
    }
    
    [Fact]
    public async Task DeleteUserProfileAsync_WhenFailure_ThrowsUserNotFoundException()
    {
        //Arrange
        var userManager = CreateUserManagerSubstitute();
        var userContext = CreateUserContextSubstitute(userId: TestConstants.TestUserId);
        
        userManager.FindByIdAsync(TestConstants.TestUserIdString).Returns((User)null);
        
        var service = new UserService(userManager, userContext);
        
        //Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(()  => service.DeleteUserProfileAsync());
        await userManager.Received(1).FindByIdAsync(TestConstants.TestUserIdString);
        await userManager.DidNotReceive().DeleteAsync(Arg.Any<User>());
    }
    
    [Fact]
    public async Task DeleteUserProfileAsync_WhenFailure_ThrowsUserDeleteFailedException()
    {
        //Arrange
        var userManager = CreateUserManagerSubstitute();
        var userContext = CreateUserContextSubstitute(userId: TestConstants.TestUserId);

        var user = new User { Id = TestConstants.TestUserId };
        
        userManager.FindByIdAsync(TestConstants.TestUserIdString).Returns(user);
        
        var errors = new []
            {
                new IdentityError { Code = "DataBaseError", Description = ErrorMessages.DeleteDbError},
            };
        
        userManager.DeleteAsync(Arg.Any<User>())
            .Returns(IdentityResult.Failed(errors));
        
        var service = new UserService(userManager, userContext);
        
        //Act && Assert
        var exception = await Assert.ThrowsAsync<UserDeleteFailedException>(() => service.DeleteUserProfileAsync());
        Assert.Contains(ErrorMessages.DeleteDbError, exception.Message);
        
        await userManager.Received(1).FindByIdAsync(TestConstants.TestUserIdString);
        await userManager.Received(1)
            .DeleteAsync(Arg.Is<User>(u => u.Id == TestConstants.TestUserId));
    }

    private static UserManager<User> CreateUserManagerSubstitute()
    {
        var store = Substitute.For<IUserStore<User>>();
        return Substitute.For<UserManager<User>>(
            store, null, null, null, null, null, null,  null, null);
    }

    private static IUserContext CreateUserContextSubstitute(int userId)
    {
        var userContext = Substitute.For<IUserContext>();
        userContext.UserId.Returns(userId);
        return userContext;
    }
}