using MyWebApp.Models;

namespace MyWebApp.Repositories.Interfaces;

public interface IExerciseRepository
{
    Task SaveChangesAsync(CancellationToken token);
    
    Task CreateExerciseAsync(Exercise exercise, CancellationToken token);
    Task<Exercise?> GetExerciseByIdAsync(int id, CancellationToken token);
    Task<List<Exercise>> GetAllExercisesAsync(CancellationToken token);
    Task<List<Exercise>> GetExercicesByCategoryAsync(string category, CancellationToken token);
    Task UpdateExerciseAsync(Exercise exercise, CancellationToken token);
    Task DeleteExerciseAsync(Exercise exercise, CancellationToken token);
}