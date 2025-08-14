using MyWebApp.DTO;
using MyWebApp.DTO.Exceptions;
using MyWebApp.Mapper;
using MyWebApp.Repositories.Interfaces;
using MyWebApp.Services.Interfaces;

namespace MyWebApp.Services;

public class ExerciseService : IExerciseService
{
    private readonly IExerciseRepository _exerciseRepository;

    public ExerciseService(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    public async Task CreateExerciseAsync(ExerciseModel model, CancellationToken token)
    {
        var exercise = model.ToEntity();
        
        await _exerciseRepository.CreateExerciseAsync(exercise, token);
        await _exerciseRepository.SaveChangesAsync(token);
    }
    
    public async Task<IEnumerable<ExerciseModel>> GetAllExercisesAsync(CancellationToken token)
    {
        var exercises = await _exerciseRepository.GetAllExercisesAsync(token);

        return exercises.ToModels();
    }

    public async Task<ExerciseModel> GetExerciseByIdAsync(int exerciseId,CancellationToken token)
    {
        var exercise = await _exerciseRepository.GetExerciseByIdAsync(exerciseId, token);

        if (exercise == null)
            throw new ExerciseNotFoundException(exerciseId);

        return exercise.ToModel();   
    }

    public async Task<IEnumerable<ExerciseModel>> GetExercisesByCategoryAsync(string category, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(category))
            return [];

        var normalized = category.Trim();
        var exercises = await _exerciseRepository.GetExercisesByCategoryAsync(normalized, token);

        return exercises.ToModels();
    }

    public async Task UpdateExerciseAsync(int exerciseId, ExerciseModel model, CancellationToken token)
    {
        var exercise = await _exerciseRepository.GetExerciseByIdAsync(exerciseId, token);
        
        if (exercise == null)
            throw new ExerciseNotFoundException(exerciseId);
        
        model.ApplyTo(exercise);
        
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
