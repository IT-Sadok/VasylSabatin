using MyWebApp.Models;

namespace MyWebApp.Repositories.Interfaces;

public interface IExerciseRepository
{
    Task SaveChangesAsync(CancellationToken token);
    
    Task CreateExerciseAsync(Exercise exercise, CancellationToken token);
    Task<Exercise?> GetExerciseByIdAsync(int id, CancellationToken token);
    Task<IEnumerable<Exercise>> GetAllExercisesAsync(CancellationToken token);
    Task<IEnumerable<Exercise>> GetExercisesByCategoryAsync(string category, CancellationToken token);
    Task UpdateExerciseAsync(Exercise exercise, CancellationToken token);
    Task DeleteExerciseAsync(Exercise exercise, CancellationToken token);
}