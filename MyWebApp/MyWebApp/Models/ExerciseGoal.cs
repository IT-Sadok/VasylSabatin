using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApp.Models;

public class ExerciseGoal
{
    public int Id { get; set; }
    
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public User User { get; set; }
    
    [ForeignKey(nameof(Exercise))]
    public int ExerciseId { get; set; }
    public Exercise Exercise { get; set; }
    
    public int TargetReps { get; set; }
    
    public float TargetWeight { get; set; }
    
    public DateTime Deadline { get; set; }
    
    public bool IsAchieved { get; set; }
}