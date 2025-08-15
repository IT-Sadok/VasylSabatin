namespace MyWebApp.Constants;

public static class WorkoutRoutes
{
    public const string Base = $"{ApiRoutes.Prefix}/workouts";
    
    public const string ById = $"/{RouteParams.Id}";
    
    public const string Sorted = "/sorted";
}