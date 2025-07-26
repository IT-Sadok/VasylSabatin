using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MyWebApp.Models;

[Table(nameof(User))]

public class User : IdentityUser<int>
{
    public string FullName { get; set; }
    
    public int Age { get; set; }
    
    public float Weight { get; set; }
    
    public ICollection<Workout> Workouts { get; set; }
    
    public ICollection<ExerciseGoal> ExerciseGoals { get; set; }
    
    public ICollection<BodyGoal> BodyGoals { get; set; }
}