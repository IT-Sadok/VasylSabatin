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
    
    public async Task CreateWorkoutAsync(Workout workout)
    {
        await _dbContext.Workouts.AddAsync(workout);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Workout>> GetAllWorkoutsAsync(int userId)
    {
        return await _dbContext.Workouts
            .Where(w => w.UserId == userId)
            .Include(w => w.WorkoutExercises)
            .ToListAsync();
    }

    public async Task<Workout?> GetWorkoutByIdAsync(int workoutId, int userId)
    {
        return await  _dbContext.Workouts
            .Include(w => w.WorkoutExercises)
            .FirstOrDefaultAsync(w => w.Id == workoutId && w.UserId == userId);
    }

    public async Task UpdateWorkoutAsync(Workout workout)
    {
        _dbContext.Workouts.Update(workout);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteWorkoutAsync(int workoutId, int userId)
    {
        var workout = await _dbContext.Workouts
            .FirstOrDefaultAsync(w => w.Id == workoutId && w.UserId == userId);

        if (workout == null)
        {
            throw new WorkoutNotFoundException(workoutId);
        }
        
        _dbContext.Workouts.Remove(workout);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Workout>> SearchWorkoutsByKeywordAsync(string keyword, int userId)
    {
        return await  _dbContext.Workouts
            .Where(w => w.UserId == userId && w.Title.ToLower().Contains(keyword.ToLower()))
            .Include(w => w.WorkoutExercises)
            .ToListAsync();
    }

    public async Task<List<Workout>> SortWorkoutsByDateAsync(int userId, bool descending = false)
    {
        var query = _dbContext.Workouts
            .Where(w => w.UserId == userId)
            .Include(w => w.WorkoutExercises);
        
        var sortedQuery = descending
            ? query.OrderByDescending(w => w.DateOfTraining)
            : query.OrderBy(w => w.DateOfTraining);
        
        return await sortedQuery.ToListAsync();
    }
}