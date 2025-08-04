namespace MyWebApp.Constants;

public class WorkoutRoutes
{
    public const string Base = "api/workouts";

    public const string Create = Base;
    
    public const string GetAll = Base;
    
    public const string Update = Base + "/{id:int}";
    
    public const string Delete = Base + "/{id:int}";
    
    public const string Search = Base + "/search";
    
    public const string Sort = Base + "/sort";
}