using MyWebApp.DTO;
using MyWebApp.DTO.Exceptions;
using MyWebApp.Mapper;
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
        var entity= model.ToEntity(userId);
        
        await _workoutRepository.CreateWorkoutAsync(entity, token);
        await _workoutRepository.SaveChangesAsync(token);
    }

    public async Task<IEnumerable<WorkoutModel>> GetAllWorkoutsAsync(CancellationToken token)
    {
        var userId = _userContext.UserId;
        var workouts = await _workoutRepository.GetAllWorkoutsAsync(userId, token);

        return workouts.ToModels();
    }

    public async Task UpdateWorkoutAsync(int id, WorkoutModel model, CancellationToken token)
    {
        var userId = _userContext.UserId;
        var workout = await _workoutRepository.GetWorkoutByIdAsync(id, userId, token);
        if (workout == null)
            throw new WorkoutNotFoundException(id);

        model.ApplyTo(workout);
        
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
        
        return workouts.ToModels();
    }

    public async Task<IEnumerable<WorkoutModel>> SortWorkoutsByDateAsync(WorkoutSortByDateModel model, CancellationToken token)
    {
        var userId = _userContext.UserId;
        var workouts = await _workoutRepository.SortWorkoutsByDateAsync(userId, model.IsDescending, token);
        
        return workouts.ToModels();
    }
}    