using MyWebApp.DTO;
using MyWebApp.DTO.Exceptions;
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

    public WorkoutExerciseService(IUserContext userContext, IWorkoutRepository workoutRepository, IExerciseRepository exerciseRepository, IWorkoutExerciseRepository workoutExerciseRepository)
    {
        _userContext = userContext;
        _workoutRepository = workoutRepository;
        _exerciseRepository = exerciseRepository;
        _workoutExerciseRepository = workoutExerciseRepository;
    }

    public async Task<WorkoutExerciseModel> AddExerciseToWorkoutAsync(WorkoutExerciseModel model, CancellationToken token)
    {
        if (model.Sets < 1) throw new SetsValidationException();
        if (model.Reps < 1) throw new RepsValidationException();
        if (model.Weight is < 0) throw new WeightValidationException();
        
        var workout = await _workoutRepository.GetWorkoutByIdAsync(model.WorkoutId, _userContext.UserId, token);
        if (workout is null)
            throw new WorkoutNotFoundException(model.WorkoutId);
        
        var exercise = await _exerciseRepository.GetExerciseByIdAsync(model.ExerciseId, token);
        if (exercise is null)
            throw new ExerciseNotFoundException(model.ExerciseId);
        
        var exists = await _workoutExerciseRepository.ExistsAsync(model.WorkoutId, model.ExerciseId, token);
        
        if (exists)
            throw new WorkoutExerciseConflictException(model.WorkoutId, model.ExerciseId);

        var link = new WorkoutExercise
        {
            WorkoutId = model.WorkoutId,
            ExerciseId = model.ExerciseId,
            Sets = model.Sets,
            Reps = model.Reps,
            Weight = model.Weight
        };
        
        await _workoutExerciseRepository.CreateAsync(link, token);
        await _workoutExerciseRepository.SaveChangesAsync(token);

        return new WorkoutExerciseModel
        {
            WorkoutId = link.WorkoutId,
            ExerciseId = link.ExerciseId,
            ExerciseName = exercise.Name,
            Sets = link.Sets,
            Reps = link.Reps,
            Weight = link.Weight
        };
    }

    public async Task<List<WorkoutExerciseModel>> GetExercisesForWorkoutAsync(int workoutId, CancellationToken token)
    {
        var userId = _userContext.UserId;
        var workout = await _workoutRepository.GetWorkoutByIdAsync(workoutId, userId, token);
        if (workout is null)
            throw new WorkoutNotFoundException(workoutId);
        
        var items = await _workoutExerciseRepository.GetByWorkoutIdAsync(workoutId, token);
        
        return items.Select(x => new WorkoutExerciseModel
        {
            ExerciseId = x.ExerciseId,
            ExerciseName = x.Exercise?.Name ?? string.Empty,
            Sets = x.Sets,
            Reps = x.Reps,
            Weight = x.Weight
        })
        .ToList();
    }

    public async Task<WorkoutExerciseModel> UpdateWorkoutExerciseAsync(WorkoutExerciseModel model, CancellationToken token)
    {
        var link = await _workoutExerciseRepository.GetAsync(model.WorkoutId, model.ExerciseId, token);
        if (link is null)
            throw new WorkoutExerciseNotFoundException(model.WorkoutId, model.ExerciseId);
        
        var workout = await _workoutRepository.GetWorkoutByIdAsync(model.WorkoutId, _userContext.UserId, token);
        if (workout is null)
            throw new WorkoutNotFoundException(model.WorkoutId);
        
        if (model.Sets < 1) throw new SetsValidationException();
        if (model.Reps < 1) throw new RepsValidationException();
        if (model.Weight is < 0) throw new WeightValidationException();
        
        link.Sets   = model.Sets;
        link.Reps   = model.Reps;
        link.Weight = model.Weight;
        
        _workoutExerciseRepository.Update(link);
        await _workoutExerciseRepository.SaveChangesAsync(token);
        
        return new WorkoutExerciseModel
        {
            WorkoutId = link.WorkoutId,
            ExerciseId = link.ExerciseId,
            Sets = link.Sets,
            Reps = link.Reps,
            Weight = link.Weight
        };
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