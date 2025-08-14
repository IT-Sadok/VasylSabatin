namespace MyWebApp.Constants;

public static class ExerciseRoutes
{
    public const string Root = "";
    
    public const string Base = $"{ApiRoutes.Prefix}/exercises";
    
    public const string ById = $"/{RouteParams.Id}";
}