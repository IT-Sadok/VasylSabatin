using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApp.Models;

public class ExerciseGoal
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
    
    public int ExerciseId { get; set; }
    public Exercise Exercise { get; set; }
    
    public int TargetReps { get; set; }
    
    public double TargetWeight { get; set; }
    
    public DateTime? Deadline { get; set; }
    
    public bool IsAchieved { get; set; }

    public List<ExerciseGoal> ExerciseGoals { get; set; } = [];
}