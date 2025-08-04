using MyWebApp.DTO;
using MyWebApp.DTO.Exceptions;
using MyWebApp.Interfaces;
using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;
using MyWebApp.Services.Interfaces;

namespace MyWebApp.Services;

public class WorkoutService : IWorkoutService
{
    private readonly IUserContext _userContext;
    private readonly IWorkoutRepository _workoutRepository;

    public WorkoutService(IUserContext userContext, IWorkoutRepository workoutRepository)
    {
        _userContext = userContext;
        _workoutRepository = workoutRepository; 
    }

    public async Task CreateWorkoutAsync(WorkoutModel model)
    {
        var userId = _userContext.GetUserContext().UserId;

        var workout = new Workout
        {
            Title = model.Title,
            DateOfTraining = model.DateOfTraining,
            DurationMinutes = model.DurationMinutes,
            Notes = model.Notes,
            UserId = userId
        };

        await _workoutRepository.CreateWorkoutAsync(workout);
    }

    public async Task<List<WorkoutModel>> GetAllWorkoutsAsync()
    {
        var userId = _userContext.GetUserContext().UserId;
        var workouts = await _workoutRepository.GetAllWorkoutsAsync(userId);

        return workouts.Select(w => new WorkoutModel
        {
            Title = w.Title,
            DateOfTraining = w.DateOfTraining,
            DurationMinutes = w.DurationMinutes,
            Notes = w.Notes
        }).ToList();
    }

    public async Task UpdateWorkoutAsync(int id, WorkoutModel model)
    {
        var userId = _userContext.GetUserContext().UserId;
        var workout = await _workoutRepository.GetWorkoutByIdAsync(id, userId);
        if (workout == null)
            throw new WorkoutNotFoundException(id);

        workout.Title = model.Title;
        workout.DateOfTraining = model.DateOfTraining;
        workout.DurationMinutes = model.DurationMinutes;
        workout.Notes = model.Notes;
        
        await _workoutRepository.UpdateWorkoutAsync(workout);
    }
    
    public async Task DeleteWorkoutAsync(int id)
    {
        var userId = _userContext.GetUserContext().UserId;
        var workout = await _workoutRepository.GetWorkoutByIdAsync(id, userId);
        if (workout == null)
            throw new WorkoutNotFoundException(id);
        
        await _workoutRepository.DeleteWorkoutAsync(id, userId);
    }
    
    public async Task<List<WorkoutModel>> SearchWorkoutsByKeywordAsync(string keyword)
    {
        var userId = _userContext.GetUserContext().UserId;
        var workouts = await _workoutRepository.SearchWorkoutsByKeywordAsync(keyword, userId);
        
        return workouts.Select(w => new WorkoutModel
        {
            Title = w.Title,
            DateOfTraining = w.DateOfTraining,
            DurationMinutes = w.DurationMinutes,
            Notes = w.Notes
        }).ToList();
    }

    public async Task<List<WorkoutModel>> SortWorkoutsByDateAsync(WorkoutSortByDateModel model)
    {
        var userId = _userContext.GetUserContext().UserId;
        var workouts = await _workoutRepository.SortWorkoutsByDateAsync(userId, model.Descending);
        
        return workouts.Select(w => new WorkoutModel
        {
            Title = w.Title,
            DateOfTraining = w.DateOfTraining,
            DurationMinutes = w.DurationMinutes,
            Notes = w.Notes
        }).ToList();
    }
}    