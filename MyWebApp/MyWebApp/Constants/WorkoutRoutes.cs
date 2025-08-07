namespace MyWebApp.Constants;

public class WorkoutRoutes
{
    public const string Base = "api/workouts";
    
    public const string ById = Base + "/{id:int}";
    
    public const string Sort = Base + "/sorted";
}