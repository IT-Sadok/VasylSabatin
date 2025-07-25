using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApp.Models;

public class BodyGoal
{
    public int Id { get; set; }
    
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public User User { get; set; }
    
    public string GoalType { get; set; }
    
    public string TargetValue { get; set; }
    
    public string Unit { get; set; }
    
    public DateTime Deadline { get; set; }
    
    public bool IsAchieved { get; set; }
}