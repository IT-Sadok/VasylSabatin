namespace MyWebApp.Models;

public class Workout
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

    public string Title { get; set; } = null!;
    public DateTime DateOfTraining { get; set; }
    public TimeSpan DurationMinutes { get; set; }
    public string? Notes { get; set; }


    public List<WorkoutExercise> WorkoutExercises { get; set; } = [];
}