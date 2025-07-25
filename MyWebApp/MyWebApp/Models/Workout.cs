using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;

namespace MyWebApp.Models;

public class Workout
{
    public int Id { get; set; }
    
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    
    public DateTime DateOfTraining { get; set; }
    
    public string? Notes { get; set; }
    
    public ICollection<WorkoutExercise> WorkoutExercises { get; set; }
}