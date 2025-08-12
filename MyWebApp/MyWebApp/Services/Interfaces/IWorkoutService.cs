using MyWebApp.DTO;
using MyWebApp.Models;

namespace MyWebApp.Services.Interfaces;

public interface IWorkoutService
{
    Task CreateWorkoutAsync(WorkoutModel model, CancellationToken token);

    Task<IEnumerable<WorkoutModel>> GetAllWorkoutsAsync(CancellationToken token);

    Task UpdateWorkoutAsync(int id, WorkoutModel model, CancellationToken token);

    Task DeleteWorkoutAsync(int id, CancellationToken token);

    Task<IEnumerable<WorkoutModel>> SearchWorkoutsByKeywordAsync(string keyword, CancellationToken token);

    Task<IEnumerable<WorkoutModel>> SortWorkoutsByDateAsync(WorkoutSortByDateModel model, CancellationToken token);
}