using MyWebApp.DTO;
using MyWebApp.DTO.Exceptions;
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

    public async Task CreateWorkoutAsync(WorkoutModel model, CancellationToken token)
    {
        var userId = _userContext.UserId;

        var workout = new Workout
        {
            Title = model.Title,
            DateOfTraining = model.DateOfTraining,
            DurationMinutes = model.DurationMinutes,
            Notes = model.Notes,
            UserId = userId
        };

        await _workoutRepository.CreateWorkoutAsync(workout, token);
        await _workoutRepository.SaveChangesAsync(token);
    }

    public async Task<IEnumerable<WorkoutModel>> GetAllWorkoutsAsync(CancellationToken token)
    {
        var userId = _userContext.UserId;
        var workouts = await _workoutRepository.GetAllWorkoutsAsync(userId, token);

        return workouts.Select(w => new WorkoutModel
        {
            Id = w.Id,
            Title = w.Title,
            DateOfTraining = w.DateOfTraining,
            DurationMinutes = w.DurationMinutes,
            Notes = w.Notes
        }).ToList();
    }

    public async Task UpdateWorkoutAsync(int id, WorkoutModel model, CancellationToken token)
    {
        var userId = _userContext.UserId;
        var workout = await _workoutRepository.GetWorkoutByIdAsync(id, userId, token);
        if (workout == null)
            throw new WorkoutNotFoundException(id);

        workout.Title = model.Title;
        workout.DateOfTraining = model.DateOfTraining;
        workout.DurationMinutes = model.DurationMinutes;
        workout.Notes = model.Notes;
        
        await _workoutRepository.UpdateWorkoutAsync(workout, token);
        await _workoutRepository.SaveChangesAsync(token);
    }
    
    public async Task DeleteWorkoutAsync(int id, CancellationToken token)
    {
        var userId = _userContext.UserId;
        var workout = await _workoutRepository.GetWorkoutByIdAsync(id, userId, token);
        if (workout == null)
            throw new WorkoutNotFoundException(id);
        
        await _workoutRepository.DeleteWorkoutAsync(id, userId, token);
        await _workoutRepository.SaveChangesAsync(token);
    }
    
    public async Task<IEnumerable<WorkoutModel>> SearchWorkoutsByKeywordAsync(string keyword, CancellationToken token)
    {
        var userId = _userContext.UserId;
        var workouts = await _workoutRepository.SearchWorkoutsByKeywordAsync(keyword, userId, token);
        
        return workouts.Select(w => new WorkoutModel
        {
            Id = w.Id,
            Title = w.Title,
            DateOfTraining = w.DateOfTraining,
            DurationMinutes = w.DurationMinutes,
            Notes = w.Notes
        }).ToList();
    }

    public async Task<IEnumerable<WorkoutModel>> SortWorkoutsByDateAsync(WorkoutSortByDateModel model, CancellationToken token)
    {
        var userId = _userContext.UserId;
        var workouts = await _workoutRepository.SortWorkoutsByDateAsync(userId, model.IsDescending, token);
        
        return workouts.Select(w => new WorkoutModel
        {
            Id = w.Id,
            Title = w.Title,
            DateOfTraining = w.DateOfTraining,
            DurationMinutes = w.DurationMinutes,
            Notes = w.Notes
        }).ToList();
    }
}    