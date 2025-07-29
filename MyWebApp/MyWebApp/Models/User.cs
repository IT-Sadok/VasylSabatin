using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MyWebApp.Models;

public class User : IdentityUser<int>
{
    public string FullName { get; set; }
    
    public int Age { get; set; }
    
    public double Weight { get; set; }
    
    public string? AccountDescription { get; set; }

    public List<Workout> Workouts { get; set; } = [];

    public List<ExerciseGoal> ExerciseGoals { get; set; } = [];

    public List<BodyGoal> BodyGoals { get; set; } = [];
}