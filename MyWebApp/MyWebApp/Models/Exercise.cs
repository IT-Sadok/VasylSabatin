using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApp.Models;

[Table(nameof(Exercise))]

public class Exercise
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Category { get; set; }
    
    public string? Description { get; set; }
    
    public ICollection<WorkoutExercise> WorkoutExercises { get; set; }
    
    public ICollection<ExerciseGoal> ExerciseGoals { get; set; }
}