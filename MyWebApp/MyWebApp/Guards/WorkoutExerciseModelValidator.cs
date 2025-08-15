using FluentValidation;
using MyWebApp.DTO;

namespace MyWebApp.Guards;

public sealed class WorkoutExerciseModelValidator : AbstractValidator<WorkoutExerciseModel>
{
    public WorkoutExerciseModelValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(x => x.WorkoutId)
            .GreaterThan(0)
            .WithMessage("Workout ID must be > 0. Provided: {PropertyValue}");
        
        RuleFor(x => x.ExerciseId)
            .GreaterThan(0)
            .WithMessage("Exercise ID must be > 0. Provided: {PropertyValue}");
        
        RuleFor(x => x.Sets)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Sets must be >= 1. Provided: {PropertyValue}");
        
        RuleFor(x => x.Reps)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Reps must be >= 1. Provided: {PropertyValue}");
        
        RuleFor(x => x.Weight)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Weight must be >= 0. Provided: {PropertyValue}");
    }
}