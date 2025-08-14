using System.Security.Cryptography.X509Certificates;
using MyWebApp.DTO;
using MyWebApp.Models;

namespace MyWebApp.Mapper;

public static class WorkoutExerciseMapper
{
    public static WorkoutExerciseModel ToModel(this WorkoutExercise entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        
        return new WorkoutExerciseModel
        {
            WorkoutId = entity.WorkoutId,
            ExerciseId = entity.ExerciseId,
            ExerciseName = entity.Exercise.Name,
            Sets = entity.Sets,
            Reps = entity.Reps,
            Weight = entity.Weight
        };
    }
    
    public static IEnumerable<WorkoutExerciseModel> ToModels(this IEnumerable<WorkoutExercise> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));
        
        return entities.Select(ToModel);
    }

    public static WorkoutExercise ToEntity(this WorkoutExerciseModel model) => new WorkoutExercise
    {
        WorkoutId = model.WorkoutId,
        ExerciseId = model.ExerciseId,
        Sets = model.Sets,
        Reps = model.Reps,
        Weight = model.Weight
    };

    public static void ApplyTo(this WorkoutExerciseModel model, WorkoutExercise entity)
    {
        entity.Sets = model.Sets;
        entity.Reps = model.Reps;
        entity.Weight = model.Weight;
    }
}