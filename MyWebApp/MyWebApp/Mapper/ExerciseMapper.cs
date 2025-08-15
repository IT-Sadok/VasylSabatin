using MyWebApp.DTO;
using MyWebApp.Models;

namespace MyWebApp.Mapper;

public static class ExerciseMapper
{
    public static ExerciseModel ToModel(this Exercise entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        
        return new ExerciseModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Category = entity.Category,
            Description = entity.Description
        };
    }

    public static IEnumerable<ExerciseModel> ToModels(this IEnumerable<Exercise> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));
        
        return entities.Select(ToModel);
    }
    
    public static Exercise ToEntity(this ExerciseModel model)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));

        return new Exercise
        {
            Id = model.Id,
            Name = model.Name,
            Category = model.Category,
            Description = model.Description
        };
    }
    
    public static void ApplyTo(this ExerciseModel model, Exercise entity)
    {
        entity.Name = model.Name;
        entity.Category = model.Category;
        entity.Description = model.Description;
    }
}