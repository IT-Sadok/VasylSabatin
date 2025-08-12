using System.Runtime.CompilerServices;
using MyWebApp.DTO;
using MyWebApp.DTO.Exceptions;
using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;
using MyWebApp.Services.Interfaces;

namespace MyWebApp.Services;

public class ExerciseService : IExerciseService
{
    private readonly IUserContext _userContext;
    private readonly IExerciseRepository _exerciseRepository;

    public ExerciseService(IUserContext userContext, IExerciseRepository exerciseRepository)
    {
        _userContext = userContext;
        _exerciseRepository = exerciseRepository;
    }

    public async Task CreateExerciseAsync(ExerciseModel model, CancellationToken token)
    {
        var exercise = new Exercise
        {
            Id = model.Id,
            Name = model.Name,
            Category = model.Category,
            Description = model.Description
        };
        
        await _exerciseRepository.CreateExerciseAsync(exercise, token);
        await _exerciseRepository.SaveChangesAsync(token);
    }
    
    public async Task<List<ExerciseModel>> GetAllExercisesAsync(CancellationToken token)
    {
        var exercises = await _exerciseRepository.GetAllExercisesAsync(token);
        
        return exercises.Select(e => new ExerciseModel
        {
            Id = e.Id,
            Name = e.Name,
            Category = e.Category,
            Description = e.Description
        }).ToList();
    }

    public async Task<ExerciseModel> GetExerciseByIdAsync(int exerciseId,CancellationToken token)
    {
        var exercise = await _exerciseRepository.GetExerciseByIdAsync(exerciseId, token);

        if (exercise == null)
            throw new ExerciseNotFoundException(exerciseId);

        return new ExerciseModel
        {
            Id = exercise.Id,
            Name = exercise.Name,
            Category = exercise.Category,
            Description = exercise.Description
        };
    }

    public async Task<List<ExerciseModel>> GetExercisesByCategoryAsync(string category, CancellationToken token)
    {
        var exercises = await _exerciseRepository.GetExercicesByCategoryAsync(category, token);
        
        return exercises.Select(e => new ExerciseModel
        {
            Id = e.Id,
            Name = e.Name,
            Category = e.Category,
            Description = e.Description
        }).ToList();
    }

    public async Task UpdateExerciseAsync(int exerciseId, ExerciseModel model, CancellationToken token)
    {
        var exercise = await _exerciseRepository.GetExerciseByIdAsync(exerciseId, token);
        
        if (exercise == null)
            throw new ExerciseNotFoundException(exerciseId);
        
        exercise.Name = model.Name;
        exercise.Category = model.Category;
        exercise.Description = model.Description;
        
        await _exerciseRepository.UpdateExerciseAsync(exercise, token);
        await _exerciseRepository.SaveChangesAsync(token);
    }

    public async Task DeleteExerciseAsync(int exerciseId, CancellationToken token)
    {
        var exercise = await _exerciseRepository.GetExerciseByIdAsync(exerciseId, token);
        
        if (exercise == null)
            throw new ExerciseNotFoundException(exerciseId);
        
        await _exerciseRepository.DeleteExerciseAsync(exercise, token);
        await _exerciseRepository.SaveChangesAsync(token);
    }
}
