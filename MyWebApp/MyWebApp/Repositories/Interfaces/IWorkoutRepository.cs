using MyWebApp.Models;

namespace MyWebApp.Repositories.Interfaces;

public interface IWorkoutRepository
{
    Task CreateWorkoutAsync(Workout workout, CancellationToken token);

    Task<List<Workout>> GetAllWorkoutsAsync(int userId, CancellationToken token);

    Task<Workout?> GetWorkoutByIdAsync(int workoutId, int userId, CancellationToken token);

    Task UpdateWorkoutAsync(Workout workout, CancellationToken token);

    Task DeleteWorkoutAsync(int workoutId, int userId, CancellationToken token);

    Task<List<Workout>> SearchWorkoutsByKeywordAsync(string keyword, int userId, CancellationToken token);

    Task<List<Workout>> SortWorkoutsByDateAsync(int userId, bool descending, CancellationToken token);
}