using Microsoft.EntityFrameworkCore;
using MyWebApp.Data;
using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;

namespace MyWebApp.Repositories;

public class ExerciseRepository : IExerciseRepository
{
    private readonly ApplicationContext _dbContext;

    public ExerciseRepository(ApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SaveChangesAsync(CancellationToken token)
    {
        return _dbContext.SaveChangesAsync(token);
    }

    public async Task CreateExerciseAsync(Exercise exercise, CancellationToken token)
    {
        await _dbContext.Exercises.AddAsync(exercise, token);
    }

    public async Task<Exercise?> GetExerciseByIdAsync(int id, CancellationToken token)
    {
        return await _dbContext.Exercises
            .Include(e => e.WorkoutExercises)
            .FirstOrDefaultAsync(e => e.Id == id, token);
    }

    public async Task<List<Exercise>> GetAllExercisesAsync(CancellationToken token)
    {
        return await _dbContext.Exercises.ToListAsync(token);
    }

    public async Task<List<Exercise>> GetExercicesByCategoryAsync(string category, CancellationToken token)
    {
        return await _dbContext.Exercises
            .Where(e => e.Category.ToLower() == category.ToLower())
            .Include(e => e.WorkoutExercises)
            .ToListAsync(token);
    }

    public Task UpdateExerciseAsync(Exercise exercise, CancellationToken token)
    {
        _dbContext.Exercises.Update(exercise);
        return Task.CompletedTask;
    }

    public Task DeleteExerciseAsync(Exercise exercise, CancellationToken token)
    {
        _dbContext.Exercises.Remove(exercise);
        return Task.CompletedTask;
    }
}