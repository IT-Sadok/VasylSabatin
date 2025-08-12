using MyWebApp.DTO;

namespace MyWebApp.Services.Interfaces;

public interface IWorkoutExerciseService
{
    Task<WorkoutExerciseModel> CreateWorkoutExerciseAsync(WorkoutExerciseModel model, CancellationToken token);

    Task<IEnumerable<WorkoutExerciseModel>> GetExercisesForWorkoutAsync(int workoutId, CancellationToken token);

    Task<WorkoutExerciseModel> UpdateWorkoutExerciseAsync(WorkoutExerciseModel model, CancellationToken token);
    
    Task DeleteWorkoutExerciseAsync(int workoutId, int exerciseId, CancellationToken token);
}