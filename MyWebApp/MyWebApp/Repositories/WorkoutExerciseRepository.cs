using Microsoft.EntityFrameworkCore;
using MyWebApp.Data;
using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;

namespace MyWebApp.Repositories;

public sealed class WorkoutExerciseRepository : IWorkoutExerciseRepository
{
    private readonly ApplicationContext _context;

    public WorkoutExerciseRepository(ApplicationContext context)
    {
        _context = context;
    }

    public Task SaveChangesAsync(CancellationToken token)
    {
        return _context.SaveChangesAsync(token);
    }

    public async Task<bool> ExistsAsync(int workoutId, int exerciseId, CancellationToken token)
    {
        return await _context.WorkoutExercises
            .AsNoTracking()
            .AnyAsync(we => we.WorkoutId == workoutId && we.ExerciseId == exerciseId, token);
    }

    public async Task<WorkoutExercise?> GetAsync(int workoutId, int exerciseId, CancellationToken token)
    {
        return await _context.WorkoutExercises
            .AsNoTracking()
            .Include(we => we.Exercise)
            .FirstOrDefaultAsync(we => we.WorkoutId == workoutId && we.ExerciseId == exerciseId, token);
    }

    public async Task<IEnumerable<WorkoutExercise>> GetByWorkoutIdAsync(int workoutId, CancellationToken token)
    {
        return await _context.WorkoutExercises
            .AsNoTracking()
            .Include(e => e.Exercise)
            .Where(we => we.WorkoutId == workoutId)
            .OrderBy(w => w.ExerciseId)
            .ToListAsync(token);
    }
    
    public async Task CreateAsync(WorkoutExercise workoutExercise, CancellationToken token)
    {
        await _context.WorkoutExercises.AddAsync(workoutExercise, token);
    }

    public void Update(WorkoutExercise workoutExercise)
    {
        _context.WorkoutExercises.Update(workoutExercise);
    }
    
    public void Delete(WorkoutExercise workoutExercise)
    {
        _context.WorkoutExercises.Remove(workoutExercise);
    }
}