using MyWebApp.DTO;
using MyWebApp.Models;

namespace MyWebApp.Services.Interfaces;

public interface IWorkoutService
{
    Task CreateWorkoutAsync(WorkoutModel model);

    Task<List<WorkoutModel>> GetAllWorkoutsAsync();

    Task UpdateWorkoutAsync(int id, WorkoutModel model);

    Task DeleteWorkoutAsync(int id);

    Task<List<WorkoutModel>> SearchWorkoutsByKeywordAsync(string keyword);

    Task<List<WorkoutModel>> SortWorkoutsByDateAsync(WorkoutSortByDateModel model);
}