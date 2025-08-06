namespace MyWebApp.DTO;

public class WorkoutModel
{
    public string Title { get; set; } = null!;
    
    public DateTime DateOfTraining { get; set; }
    
    public TimeSpan DurationMinutes { get; set; }

    public string? Notes { get; set; }
}