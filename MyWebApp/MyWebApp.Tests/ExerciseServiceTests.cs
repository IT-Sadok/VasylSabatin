using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using MyWebApp.DTO;
using MyWebApp.DTO.Exceptions;
using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;
using MyWebApp.Services;
using NSubstitute;

namespace MyWebApp.Tests;

public class ExerciseServiceTests
{
    private const int ExerciseId1 = 1;
    private const int ExerciseId2 = 2;
    private const int NonExistingExerciseId = 999;

    [Fact]
    public async Task CreateExerciseAsync_WhenSuccess_ReturnExercise()
    {
        // Arrange
        var repo = CreateExerciseRepositorySubstitute();
        var service = new ExerciseService(repo);
        CancellationToken token = default;

        var model = new ExerciseModel
        {
            Name = "Bench Press",
            Category = "Push",
            Description = "Chest exercise"
        };
        
        //Act
        await service.CreateExerciseAsync(model, token);
        
        //Assert
        await repo.Received(1).CreateExerciseAsync(
            Arg.Is<Exercise>(e =>
                e.Name == "Bench Press" &&
                e.Category == "Push" &&
                e.Description == "Chest exercise"),
            token);
        
        await repo.Received(1).SaveChangesAsync(token);
    }

    [Fact]
    public async Task GetAllExercisesAsync_WhenSuccess_ReturnListOfExerciseModels()
    {
        // Arrange
        var repo = CreateExerciseRepositorySubstitute();
        var service = new ExerciseService(repo);
        CancellationToken token = default;

        var entities = new List<Exercise>
        {
            new() { Id = ExerciseId1, Name = "Bench Press", Category = "Push", Description = "Chest exercise"}, 
            new() { Id = ExerciseId2, Name = "Squat", Category = "Leg", Description = "Leg exercise" }
        };
        
        repo.GetAllExercisesAsync(token)
            .Returns(Task.FromResult<IEnumerable<Exercise>>(entities));
        
        //Act
        var result = (await service.GetAllExercisesAsync(token)).ToList();
        
        //Assert
        await repo.Received(1).GetAllExercisesAsync(token);
        await repo.DidNotReceive().SaveChangesAsync(token);
        
        Assert.Equal(2, result.Count);
        Assert.Equal("Bench Press", result[0].Name);
        Assert.Equal("Push", result[0].Category);
        Assert.Equal("Chest exercise", result[0].Description);
        
        Assert.Equal("Squat", result[1].Name);
        Assert.Equal("Leg", result[1].Category);
        Assert.Equal("Leg exercise", result[1].Description);
    }

    [Fact]
    public async Task UpdateExerciseAsync_WhenSuccess_ReturnExercise()
    { 
        //Arrange
        var repo = CreateExerciseRepositorySubstitute();
        var service = new ExerciseService(repo);
        CancellationToken token = default;

        var existingExercise = new Exercise
        {
            Id = ExerciseId1,
            Name = "Bench Press",
            Category = "Push",
            Description = "Chest exercise"
        };
        
        repo.GetExerciseByIdAsync(ExerciseId1, token)
            .Returns(existingExercise);
        
        var updateExerciseModel = new ExerciseModel
        {
            Name = "Squat",
            Category = "Leg",
            Description = "Leg exercise"
        };
        
        //Act
        await service.UpdateExerciseAsync(ExerciseId1, updateExerciseModel, token);
        
        //Assert
        await repo.Received(1).GetExerciseByIdAsync(ExerciseId1, token);
        
        await repo.Received(1).UpdateExerciseAsync(
            Arg.Is<Exercise>(e =>
                ReferenceEquals(e, existingExercise) &&
                e.Name == "Squat" &&
                e.Category == "Leg" &&
                e.Description == "Leg exercise"),
            token);
        
        await repo.Received(1).SaveChangesAsync(token);
    }
    
    [Fact]
    public async Task UpdateExerciseAsync_WhenFailure_ThrowsExerciseNotFoundException()
    {
        //Arrange
        var repo = CreateExerciseRepositorySubstitute();
        var service = new ExerciseService(repo);
        CancellationToken token = default;
        
        repo.GetExerciseByIdAsync(NonExistingExerciseId, token)
            .Returns((Exercise?)null);
        
        var updateExerciseModel = new ExerciseModel
        {
            Name = "Squat",
            Category = "Leg",
            Description = "Leg exercise"
        };
        
        //Act && Assert
        await Assert.ThrowsAsync<ExerciseNotFoundException>(() =>
            service.UpdateExerciseAsync(NonExistingExerciseId, updateExerciseModel, token));

        await repo.Received(1).GetExerciseByIdAsync(NonExistingExerciseId, token);
        await repo.DidNotReceive().UpdateExerciseAsync(Arg.Any<Exercise>(), token);
        await repo.DidNotReceive().SaveChangesAsync(token);
    }

    [Fact]
    public async Task DeleteExerciseAsync_WhenSuccess_DeletesExercise()
    {
        //Arrange
        var repo = CreateExerciseRepositorySubstitute();
        var service = new ExerciseService(repo);
        CancellationToken token = default;

        var exercise = new Exercise
        {
            Id = ExerciseId1,
            Name = "Bench Press",
            Category = "Push",
            Description = "Chest exercise"
        };

        repo.GetExerciseByIdAsync(ExerciseId1, token)
            .Returns(exercise);
        
        //Act
        await service.DeleteExerciseAsync(ExerciseId1, token);
        
        //Assert
        await repo.Received(1).GetExerciseByIdAsync(ExerciseId1, token);
        await repo.Received(1).DeleteExerciseAsync(
            Arg.Is<Exercise>(e => ReferenceEquals(e, exercise)), token);
        await repo.Received(1).SaveChangesAsync(token);
    }
    
    [Fact]
    public async Task DeleteExerciseAsync_WhenFailure_ThrowsExerciseNotFoundException()
    {
        //Arrange
        var repo = CreateExerciseRepositorySubstitute();
        var service = new ExerciseService(repo);
        CancellationToken token = default;
        
        repo.GetExerciseByIdAsync(NonExistingExerciseId, token)
            .Returns((Exercise?)null);
        
        //Act && Assert
        await Assert.ThrowsAsync<ExerciseNotFoundException>(() =>
            service.DeleteExerciseAsync(NonExistingExerciseId, token));
        
        await repo.Received(1).GetExerciseByIdAsync(NonExistingExerciseId, token);
        await repo.DidNotReceive().DeleteExerciseAsync(Arg.Any<Exercise>(), token);
        await repo.DidNotReceive().SaveChangesAsync(token);
    }
    
    [Fact]
    public async Task SearchExercisesByKeywordAsync_WhenSuccess_ReturnsListOfExerciseModels()
    {
        //Arrange
        var repo = CreateExerciseRepositorySubstitute();
        var service = new ExerciseService(repo);
        CancellationToken token = default;

        var entities = new Exercise[]
        {
            new() { Id = ExerciseId1, Name = "Bench Press", Description = "Chest", Category = "Push" },
            new() { Id = ExerciseId2, Name = "Overhead Press", Description = "Shoulder", Category = "Push" }
        };

        repo.GetExercisesByCategoryAsync("Push", token)
            .Returns(Task.FromResult<IEnumerable<Exercise>>(entities));
        
        //Act
        var models = (await service.GetExercisesByCategoryAsync("Push", token)).ToList();
        
        //Assert
        await repo.Received(1).GetExercisesByCategoryAsync("Push", token);
        await repo.DidNotReceive().SaveChangesAsync(token);
        
        Assert.Equal(2, models.Count);
        Assert.Equal("Bench Press", models[0].Name);
        Assert.Equal("Push", models[0].Category);
        Assert.Equal("Chest", models[0].Description);
        
        Assert.Equal("Overhead Press", models[1].Name);
        Assert.Equal("Push", models[1].Category);
        Assert.Equal("Shoulder", models[1].Description);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SearchExercisesByKeywordAsync_WhenFailure_ReturnsEmptyList(string category)
    {
        //Arrange
        var repo = CreateExerciseRepositorySubstitute();
        var service = new ExerciseService(repo);
        CancellationToken token = default;
        
        //Act
        var result = (await service.GetExercisesByCategoryAsync(category, token)).ToList();
        
        //Assert
        Assert.Empty(result);
        
        await repo.DidNotReceive().GetExercisesByCategoryAsync(Arg.Any<string>(), token);
        await repo.DidNotReceive().SaveChangesAsync(token);
    }

    private static IExerciseRepository CreateExerciseRepositorySubstitute()
    {
        return Substitute.For<IExerciseRepository>();
    }
}