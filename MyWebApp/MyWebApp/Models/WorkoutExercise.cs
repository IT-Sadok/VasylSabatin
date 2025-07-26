using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApp.Models;

[Table(nameof(WorkoutExercise))]

public class WorkoutExercise
{
    public int Id { get; set; }
    
    [ForeignKey(nameof(Workout))]
    public int WorkoutId { get; set; }
    public Workout Workout { get; set; }
    
    [ForeignKey(nameof(Exercise))]
    public int ExerciseId { get; set; }
    public Exercise Exercise { get; set; }
    
    public int Sets { get; set; }
    
    public int Reps { get; set; }
    
    public float Weight { get; set; }
}