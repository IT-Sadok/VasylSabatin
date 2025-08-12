using MyWebApp.DTO;
using MyWebApp.DTO.Exceptions;

namespace MyWebApp.Guards;

public class WorkoutExerciseValidator
{
    public static void Validate(WorkoutExerciseModel model)
    {
        if (model.Sets < 1)
            throw new SetsValidationException(model.Sets);
    
       
        if (model.Reps < 1) 
            throw new RepsValidationException(model.Reps);
        
        if (model.Weight is < 0)
            throw new WeightValidationException(model.Weight);
    }
}