using Microsoft.EntityFrameworkCore;
using MyWebApp.Data;
using MyWebApp.DTO.Exceptions;
using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;

namespace MyWebApp.Repositories;

public class WorkoutRepository : IWorkoutRepository
{
    private readonly ApplicationContext _dbContext;

    public WorkoutRepository(ApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SaveChangesAsync(CancellationToken token)
    {
        return _dbContext.SaveChangesAsync(token);
    }

    public async Task CreateWorkoutAsync(Workout workout, CancellationToken token)
    {
        await _dbContext.Workouts.AddAsync(workout, token);
    }

    public async Task<IEnumerable<Workout>> GetAllWorkoutsAsync(int userId, CancellationToken token)
    {
        return await _dbContext.Workouts
            .Where(w => w.UserId == userId)
            .Include(w => w.WorkoutExercises)
            .ToListAsync(token);
    }

    public async Task<Workout?> GetWorkoutByIdAsync(int workoutId, int userId, CancellationToken token)
    {
        return await  _dbContext.Workouts
            .Include(w => w.WorkoutExercises)
            .FirstOrDefaultAsync(w => w.Id == workoutId && w.UserId == userId, token);
    }

    public Task UpdateWorkoutAsync(Workout workout, CancellationToken token)
    {
        _dbContext.Workouts.Update(workout);
        return Task.CompletedTask;
    }

    public async Task DeleteWorkoutAsync(int workoutId, int userId, CancellationToken token)
    {
        var workout = await _dbContext.Workouts
            .FirstOrDefaultAsync(w => w.Id == workoutId && w.UserId == userId, token);

        if (workout == null)
        {
            throw new WorkoutNotFoundException(workoutId);
        }
        
        _dbContext.Workouts.Remove(workout);
    }

    public async Task<IEnumerable<Workout>> SearchWorkoutsByKeywordAsync(string keyword, int userId, CancellationToken token)
    {
        return await  _dbContext.Workouts
            .Where(w => w.UserId == userId && w.Title.ToLower().Contains(keyword.ToLower()))
            .Include(w => w.WorkoutExercises)
            .ToListAsync(token);
    }

    public async Task<IEnumerable<Workout>> SortWorkoutsByDateAsync(int userId, bool descending,
        CancellationToken token)
    {
        var query = _dbContext.Workouts
            .Where(w => w.UserId == userId)
            .Include(w => w.WorkoutExercises);
        
        var sortedQuery = descending
            ? query.OrderByDescending(w => w.DateOfTraining)
            : query.OrderBy(w => w.DateOfTraining);
        
        return await sortedQuery.ToListAsync(token);
    }
}