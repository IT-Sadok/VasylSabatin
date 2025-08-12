namespace MyWebApp.DTO;

public class WorkoutExerciseModel
{
    public int WorkoutId { get; set; }
    
    public int ExerciseId { get; set; }
    
    public string ExerciseName { get; set; }
    
    public int Sets { get; set; }
    
    public int Reps { get; set; }
    
    public decimal Weight { get; set; }
}