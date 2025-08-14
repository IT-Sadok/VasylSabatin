using FluentValidation;
using MyWebApp.DTO;
using MyWebApp.DTO.Exceptions;
using MyWebApp.Guards;
using MyWebApp.Mapper;
using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;
using MyWebApp.Services.Interfaces;

namespace MyWebApp.Services;

public class WorkoutExerciseService : IWorkoutExerciseService
{
    private readonly IUserContext _userContext;
    private readonly IWorkoutRepository _workoutRepository;
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IWorkoutExerciseRepository _workoutExerciseRepository;
    private readonly IValidator<WorkoutExerciseModel> _validator;

    public WorkoutExerciseService(IUserContext userContext,
        IWorkoutRepository workoutRepository,
        IExerciseRepository exerciseRepository,
        IWorkoutExerciseRepository workoutExerciseRepository,
        IValidator<WorkoutExerciseModel> validator)
    {
        _userContext = userContext;
        _workoutRepository = workoutRepository;
        _exerciseRepository = exerciseRepository;
        _workoutExerciseRepository = workoutExerciseRepository;
        _validator = validator;
    }

    public async Task<WorkoutExerciseModel> CreateWorkoutExerciseAsync(WorkoutExerciseModel model, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(model, token);
        
        var workout = await _workoutRepository.GetWorkoutByIdAsync(model.WorkoutId, _userContext.UserId, token);
        if (workout is null)
            throw new WorkoutNotFoundException(model.WorkoutId);
        
        var exercise = await _exerciseRepository.GetExerciseByIdAsync(model.ExerciseId, token);
        if (exercise is null)
            throw new ExerciseNotFoundException(model.ExerciseId);
        
        var exists = await _workoutExerciseRepository.ExistsAsync(model.WorkoutId, model.ExerciseId, token);
        
        if (exists)
            throw new WorkoutExerciseConflictException(model.WorkoutId, model.ExerciseId);

        var link = model.ToEntity();
        
        await _workoutExerciseRepository.CreateAsync(link, token);
        await _workoutExerciseRepository.SaveChangesAsync(token);

        return link.ToModel();
    }

    public async Task<IEnumerable<WorkoutExerciseModel>> GetExercisesForWorkoutAsync(int workoutId, CancellationToken token)
    {
        var userId = _userContext.UserId;
        var workout = await _workoutRepository.GetWorkoutByIdAsync(workoutId, userId, token);
        if (workout is null)
            throw new WorkoutNotFoundException(workoutId);
        
        var items = await _workoutExerciseRepository.GetByWorkoutIdAsync(workoutId, token);
        
        return items.ToModels();
    }

    public async Task<WorkoutExerciseModel> UpdateWorkoutExerciseAsync(WorkoutExerciseModel model, CancellationToken token)
    {
        var link = await _workoutExerciseRepository.GetAsync(model.WorkoutId, model.ExerciseId, token);
        if (link is null)
            throw new WorkoutExerciseNotFoundException(model.WorkoutId, model.ExerciseId);
        
        var workout = await _workoutRepository.GetWorkoutByIdAsync(model.WorkoutId, _userContext.UserId, token);
        if (workout is null)
            throw new WorkoutNotFoundException(model.WorkoutId);
        
        await _validator.ValidateAndThrowAsync(model, token);
        
        model.ApplyTo(link);
        
        _workoutExerciseRepository.Update(link);
        await _workoutExerciseRepository.SaveChangesAsync(token);
        
        return link.ToModel();
    }
    
    public async Task DeleteWorkoutExerciseAsync(int workoutId, int exerciseId, CancellationToken token)
    {
        var link = await _workoutExerciseRepository.GetAsync(workoutId, exerciseId, token);
        if (link is null)
            throw new WorkoutExerciseNotFoundException(workoutId, exerciseId);
        
        var workout = await _workoutRepository.GetWorkoutByIdAsync(workoutId, _userContext.UserId, token);
        if (workout is null)
            throw new WorkoutNotFoundException(workoutId);
        
        _workoutExerciseRepository.Delete(link);
        await _workoutExerciseRepository.SaveChangesAsync(token);
    }
}