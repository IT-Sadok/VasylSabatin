using MyWebApp.Models;

namespace MyWebApp.Repositories.Interfaces;

public interface IWorkoutRepository
{
    Task CreateWorkoutAsync(Workout workout);

    Task<List<Workout>> GetAllWorkoutsAsync(int userId);

    Task<Workout?> GetWorkoutByIdAsync(int workoutId, int userId);

    Task UpdateWorkoutAsync(Workout workout);

    Task DeleteWorkoutAsync(int workoutId, int userId);

    Task<List<Workout>> SearchWorkoutsByKeywordAsync(string keyword, int userId);

    Task<List<Workout>> SortWorkoutsByDateAsync(int userId, bool descending = false);
}