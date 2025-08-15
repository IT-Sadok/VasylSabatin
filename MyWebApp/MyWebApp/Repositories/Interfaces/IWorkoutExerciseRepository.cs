using MyWebApp.Models;

namespace MyWebApp.Repositories.Interfaces;

public interface IWorkoutExerciseRepository
{
    Task SaveChangesAsync(CancellationToken token);

    Task<bool> ExistsAsync(int workoutId, int exerciseId, CancellationToken token);

    Task<WorkoutExercise?> GetAsync(int workoutId, int exerciseId, CancellationToken token);

    Task<IEnumerable<WorkoutExercise>> GetByWorkoutIdAsync(int workoutId, CancellationToken token);
    
    Task CreateAsync(WorkoutExercise workoutExercise, CancellationToken token);

    void Update(WorkoutExercise workoutExercise);

    void Delete(WorkoutExercise workoutExercise);
}