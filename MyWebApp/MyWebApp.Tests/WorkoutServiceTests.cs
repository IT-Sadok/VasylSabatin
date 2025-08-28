using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MyWebApp.Constants;
using MyWebApp.DTO;
using MyWebApp.DTO.Exceptions;
using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;
using MyWebApp.Services;
using MyWebApp.Services.Interfaces;
using Npgsql.Replication.PgOutput.Messages;
using NSubstitute;

namespace MyWebApp.Tests;

public class WorkoutServiceTests
{
    [Fact]
    public async Task CreateWorkoutAsync_WhenSuccess_ReturnsWorkoutModel()
    {
        //Arrange
        var userContext = CreateUserContextSubstitute(TestConstants.TestUserId);
        var repo = CreateWorkoutRepositorySubstitute();
        var service = new WorkoutService(userContext, repo);
        var token = CancellationToken.None;

        var model = new WorkoutModel
        {
            Title = "Leg Day",
            DateOfTraining = new DateTime(2025, 8, 28),
            DurationMinutes = TimeSpan.FromMinutes(90),
            Notes = "Some notes",
        };

        //Act
        await service.CreateWorkoutAsync(model, token);
        
        //Assert
        await repo.Received(1).CreateWorkoutAsync(
            Arg.Is<Workout>(w =>
                w.Title == "Leg Day" &&
                w.DateOfTraining == new DateTime(2025, 8, 28) &&
                w.DurationMinutes == TimeSpan.FromMinutes(90) &&
                w.Notes == "Some notes"),
            token);
        
        await repo.Received(1).SaveChangesAsync(token);
    }

    [Fact]
    public async Task GetAllWorkoutsAsync_WhenSuccess_ReturnsListOfWorkoutModels()
    {
        //Arrange
        var userContext = CreateUserContextSubstitute(TestConstants.TestUserId);
        var repo = CreateWorkoutRepositorySubstitute();
        var service = new WorkoutService(userContext, repo);
        
        var token = CancellationToken.None;
        var userId = TestConstants.TestUserId;

        var entities = new List<Workout>
        {
            new ()
            {
                Id = TestConstants.WorkoutId1, UserId = userId,
                Title = "Leg Day",
                DateOfTraining = new DateTime(2025, 8, 28),
                DurationMinutes = TimeSpan.FromMinutes(90),
                Notes = "Some notes"
            },
            new() {
                Id = TestConstants.WorkoutId2, UserId = userId,
                Title = "Push Day",
                DateOfTraining = new DateTime(2025, 8, 29),
                DurationMinutes = TimeSpan.FromMinutes(60),
                Notes = null 
            }
        };

        repo.GetAllWorkoutsAsync(userId, token)
            .Returns(Task.FromResult<IEnumerable<Workout>>(entities));
        
        //Act
        var result = (await service.GetAllWorkoutsAsync(token)).ToList();
        
        //Assert
        await repo.Received(1).GetAllWorkoutsAsync(userId, token);
        await repo.DidNotReceive().SaveChangesAsync(token);
        
        Assert.Equal(2, result.Count);

        Assert.Equal("Leg Day", result[0].Title);
        Assert.Equal(new DateTime(2025, 8, 28), result[0].DateOfTraining);
        Assert.Equal(TimeSpan.FromMinutes(90), result[0].DurationMinutes);
        Assert.Equal("Some notes", result[0].Notes);
        
        Assert.Equal("Push Day", result[1].Title);
        Assert.Equal(new DateTime(2025, 8, 29), result[1].DateOfTraining);
        Assert.Equal(TimeSpan.FromMinutes(60), result[1].DurationMinutes);
        Assert.Null(result[1].Notes);
    }

    [Fact]
    public async Task UpdateWorkoutAsync_WhenSuccess_ReturnsWorkoutModel()
    {
        //Arrange
        var userContext = CreateUserContextSubstitute(TestConstants.TestUserId);
        var repo = CreateWorkoutRepositorySubstitute();
        var service = new WorkoutService(userContext, repo);
        var token = CancellationToken.None;
        
        var existingWorkout = new Workout
        {
            Id = TestConstants.WorkoutId1,
            UserId = TestConstants.TestUserId,
            Title = "Old title",
            DateOfTraining = new DateTime(2025, 8, 28),
            DurationMinutes = TimeSpan.FromMinutes(90),
            Notes = "Old notes"
        };

        repo.GetWorkoutByIdAsync(TestConstants.WorkoutId1, TestConstants.TestUserId, token)
            .Returns(existingWorkout);
        
        var updateModel = new WorkoutModel
        {
            Title = "New title",
            DateOfTraining = new DateTime(2025, 8, 29),
            DurationMinutes = TimeSpan.FromMinutes(60),
            Notes = "New notes"
        };
        
        //Act
        await service.UpdateWorkoutAsync(TestConstants.WorkoutId1, updateModel, token);
        
        //Assert
        await repo.Received(1).UpdateWorkoutAsync(
            Arg.Is<Workout>(w =>
                ReferenceEquals(w, existingWorkout) &&
                w.Title == "New title" &&
                w.DateOfTraining == new DateTime(2025, 8, 29) &&
                w.DurationMinutes == TimeSpan.FromMinutes(60) &&
                w.Notes == "New notes"),
            token);

        await repo.Received(1).SaveChangesAsync(token);
    }

    [Fact]
    public async Task UpdateWorkoutAsync_WhenFailure_ThrowsWorkoutNotFoundException()
    {
        //Arrange
        var userContext = CreateUserContextSubstitute(TestConstants.TestUserId);
        var repo = CreateWorkoutRepositorySubstitute();
        var service = new WorkoutService(userContext, repo);
        var token = CancellationToken.None;
        
        repo.GetWorkoutByIdAsync(TestConstants.NonExistingWorkoutId, TestConstants.TestUserId, token)
            .Returns((Workout?)null);

        var model = new WorkoutModel
        {
            Title = "New title",
            DateOfTraining = new DateTime(2025, 8, 29),
            DurationMinutes = TimeSpan.FromMinutes(60),
            Notes = "New notes"
        };
        
        //Act && Assert
        await Assert.ThrowsAsync<WorkoutNotFoundException>(() => 
            service.UpdateWorkoutAsync(TestConstants.NonExistingWorkoutId, model, token));

        await repo.Received(1).GetWorkoutByIdAsync(TestConstants.NonExistingWorkoutId, TestConstants.TestUserId, token);
        await repo.DidNotReceive().UpdateWorkoutAsync(Arg.Any<Workout>(), token);
        await repo.DidNotReceive().SaveChangesAsync(token);
    }

    [Fact]
    public async Task DeleteWorkoutAsync_WhenSuccess_ReturnsWorkoutModel()
    {
        //Arrange
        var userContext = CreateUserContextSubstitute(TestConstants.TestUserId);
        var repo = CreateWorkoutRepositorySubstitute();
        var service = new WorkoutService(userContext, repo);
        var token = CancellationToken.None;
        
        repo.GetWorkoutByIdAsync(TestConstants.WorkoutId1, TestConstants.TestUserId, token)
            .Returns(new Workout
            {
                Id = TestConstants.WorkoutId1,
                UserId = TestConstants.TestUserId,
                Title = "Test title",
                DateOfTraining = new DateTime(2025, 8, 28),
                DurationMinutes = TimeSpan.FromMinutes(90),
                Notes = "Some notes"
            });
        
        //Act
        await service.DeleteWorkoutAsync(TestConstants.WorkoutId1, token);
        
        //Assert
        await repo.Received(1).GetWorkoutByIdAsync(TestConstants.WorkoutId1, TestConstants.TestUserId, token);
        await repo.Received(1).DeleteWorkoutAsync(TestConstants.WorkoutId1, TestConstants.TestUserId, token);
        await repo.Received(1).SaveChangesAsync(token);
    }
    
    [Fact]
    public async Task DeleteWorkoutAsync_WhenFailure_ThrowsWorkoutNotFoundException()
    {
        //Arrange
        var userContext = CreateUserContextSubstitute(TestConstants.TestUserId);
        var repo = CreateWorkoutRepositorySubstitute();
        var service = new WorkoutService(userContext, repo);
        var token = CancellationToken.None;

        repo.GetWorkoutByIdAsync(TestConstants.NonExistingWorkoutId, TestConstants.TestUserId, token)
            .Returns((Workout?)null);
        
        //Act && Assert
        await Assert.ThrowsAsync<WorkoutNotFoundException>(() => 
            service.DeleteWorkoutAsync(TestConstants.NonExistingWorkoutId, token));
        
        await repo.Received(1).GetWorkoutByIdAsync(TestConstants.NonExistingWorkoutId, TestConstants.TestUserId, token);
        await repo.DidNotReceive().DeleteWorkoutAsync(Arg.Any<int>(), Arg.Any<int>(), token);
        await repo.DidNotReceive().SaveChangesAsync(token);
    }

    [Fact]
    public async Task SearchWorkoutsByKeywordAsync_WhenSuccess_ReturnsListOfWorkoutModels()
    {
        //Arrange
        var userContext = CreateUserContextSubstitute(TestConstants.TestUserId);
        var repo = CreateWorkoutRepositorySubstitute();
        var service = new WorkoutService(userContext, repo);
        var token = CancellationToken.None;

        var entities = new[]
        {
            new Workout 
            { 
                Id = TestConstants.WorkoutId1, 
                UserId = TestConstants.TestUserId, 
                Title = "Chest day", 
                DateOfTraining = new DateTime(2025,8,1), 
                DurationMinutes = TimeSpan.FromMinutes(50)
            },
            
            new Workout
            {
                Id = TestConstants.WorkoutId2, 
                UserId = TestConstants.TestUserId, 
                Title = "Legs day", 
                DateOfTraining = new DateTime(2025,6,2), 
                DurationMinutes = TimeSpan.FromMinutes(80)
            }
        };
        
        repo.SearchWorkoutsByKeywordAsync("leg", TestConstants.TestUserId, token)
            .Returns(Task.FromResult(entities.Where(e => e.Title.Contains("leg", StringComparison.OrdinalIgnoreCase))
            ));
        
        //Act
        var models = (await service.SearchWorkoutsByKeywordAsync("leg", token)).ToList();
        
        //Assert
        await repo.Received(1).SearchWorkoutsByKeywordAsync("leg", TestConstants.TestUserId, token);
        await repo.DidNotReceive().SaveChangesAsync(token);

        Assert.Single(models);
        Assert.Equal("Legs day", models[0].Title);
        Assert.Equal(new DateTime(2025, 6, 2), models[0].DateOfTraining);
        Assert.Equal(TimeSpan.FromMinutes(80), models[0].DurationMinutes);
        
    }

    [Fact]
    public async Task SortWorkoutsByDateAsync_WhenDescending_ReturnsListOfWorkoutModels()
    {
        //Arrange
        var userContext = CreateUserContextSubstitute(TestConstants.TestUserId);
        var repo = CreateWorkoutRepositorySubstitute();
        var service = new WorkoutService(userContext, repo);
        var token = CancellationToken.None;

        var entities = new[]
        {
            new Workout { Id = 1, UserId = TestConstants.TestUserId, Title = "A", DateOfTraining = new DateTime(2025, 8, 1), DurationMinutes = TimeSpan.FromMinutes(30) },
            new Workout { Id = 2, UserId = TestConstants.TestUserId, Title = "B", DateOfTraining = new DateTime(2025, 8, 3), DurationMinutes = TimeSpan.FromMinutes(40) },
            new Workout { Id = 3, UserId = TestConstants.TestUserId, Title = "C", DateOfTraining = new DateTime(2025, 8, 2), DurationMinutes = TimeSpan.FromMinutes(50) }
        };
        
        repo.SortWorkoutsByDateAsync(TestConstants.TestUserId, true, token)
            .Returns(Task.FromResult<IEnumerable<Workout>>(entities.OrderByDescending(e => e.DateOfTraining)));
        
        //Act
        var result = (await service.SortWorkoutsByDateAsync(new WorkoutSortByDateModel { IsDescending = true }, token)).ToList();
        
        //Assert
        await repo.Received(1).SortWorkoutsByDateAsync(TestConstants.TestUserId, true, token);
        await repo.DidNotReceive().SaveChangesAsync(token);
        Assert.Equal(["B", "C", "A"], result.Select(w => w.Title));
    }

    [Fact]
    public async Task SortWorkoutsByDateAsync_WhenAscending_ReturnsListOfWorkoutModels()
    {
        //Arrange
        var userContext = CreateUserContextSubstitute(TestConstants.TestUserId);
        var repo = CreateWorkoutRepositorySubstitute();
        var service = new WorkoutService(userContext, repo);
        var token = CancellationToken.None;

        var entities = new[]
        {
            new Workout { Id = 1, UserId = TestConstants.TestUserId, Title = "A", DateOfTraining = new DateTime(2025, 8, 1), DurationMinutes = TimeSpan.FromMinutes(30) },
            new Workout { Id = 2, UserId = TestConstants.TestUserId, Title = "B", DateOfTraining = new DateTime(2025, 8, 3), DurationMinutes = TimeSpan.FromMinutes(40) },
            new Workout { Id = 3, UserId = TestConstants.TestUserId, Title = "C", DateOfTraining = new DateTime(2025, 8, 2), DurationMinutes = TimeSpan.FromMinutes(50) }
        };
        
        repo.SortWorkoutsByDateAsync(TestConstants.TestUserId, false, token)
            .Returns(Task.FromResult<IEnumerable<Workout>>(entities.OrderBy(e => e.DateOfTraining)));
        
        //Act
        var result = (await service.SortWorkoutsByDateAsync(new WorkoutSortByDateModel { IsDescending = false }, token)).ToList();
        
        //Assert
        await repo.Received(1).SortWorkoutsByDateAsync(TestConstants.TestUserId, false, token);
        await repo.DidNotReceive().SaveChangesAsync(token);
        Assert.Equal(["A", "C", "B"], result.Select(w => w.Title));
    }

    private static IUserContext CreateUserContextSubstitute(int  userId)
    {
        var userContext = Substitute.For<IUserContext>();
        userContext.UserId.Returns(userId);
        return userContext;
    }

    private static IWorkoutRepository CreateWorkoutRepositorySubstitute()
    {
        return Substitute.For<IWorkoutRepository>();
    }
}