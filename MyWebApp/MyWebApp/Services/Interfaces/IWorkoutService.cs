using MyWebApp.DTO;
using MyWebApp.Models;

namespace MyWebApp.Services.Interfaces;

public interface IWorkoutService
{
    Task CreateWorkoutAsync(WorkoutModel model, CancellationToken token);

    Task<List<WorkoutModel>> GetAllWorkoutsAsync(CancellationToken token);

    Task UpdateWorkoutAsync(int id, WorkoutModel model, CancellationToken token);

    Task DeleteWorkoutAsync(int id, CancellationToken token);

    Task<List<WorkoutModel>> SearchWorkoutsByKeywordAsync(string keyword, CancellationToken token);

    Task<List<WorkoutModel>> SortWorkoutsByDateAsync(WorkoutSortByDateModel model, CancellationToken token);
}