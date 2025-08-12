using MyWebApp.DTO;

namespace MyWebApp.Services.Interfaces;

public interface IExerciseService
{
    Task CreateExerciseAsync(ExerciseModel model, CancellationToken token);

    Task<List<ExerciseModel>> GetAllExercisesAsync(CancellationToken token);

    Task<ExerciseModel> GetExerciseByIdAsync(int exerciseId, CancellationToken token);

    Task<List<ExerciseModel>> GetExercisesByCategoryAsync(string category, CancellationToken token);

    Task UpdateExerciseAsync(int exerciseId, ExerciseModel model, CancellationToken token);
    
    Task DeleteExerciseAsync(int exerciseId, CancellationToken token);
}