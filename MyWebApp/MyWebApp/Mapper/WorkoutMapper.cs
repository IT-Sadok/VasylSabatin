using MyWebApp.DTO;
using MyWebApp.Models;

namespace MyWebApp.Mapper;

public static class WorkoutMapper
{
    public static WorkoutModel ToModel(this Workout entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        return new WorkoutModel()
        {
            Id = entity.Id,
            Title = entity.Title,
            DateOfTraining = entity.DateOfTraining,
            DurationMinutes = entity.DurationMinutes,
            Notes = entity.Notes

        };
    }
    
    public static IEnumerable<WorkoutModel> ToModels(this IEnumerable<Workout> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));
        
        return entities.Select(ToModel);
    }
    
    public static Workout ToEntity(this WorkoutModel model, int userId)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));

        return new Workout()
        {
            Title = model.Title,
            DateOfTraining = model.DateOfTraining,
            DurationMinutes = model.DurationMinutes,
            Notes = model.Notes,
            UserId = userId
        };
    }
    
    public static void ApplyTo(this WorkoutModel model, Workout entity)
    {
        entity.Title = model.Title;
        entity.DateOfTraining = model.DateOfTraining;
        entity.DurationMinutes = model.DurationMinutes;
        entity.Notes = model.Notes;
    }
}
