using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;


public sealed class TagOrderDocumentFilter : IDocumentFilter
{
    private static readonly string[] OrderedTags =
        { "Authentication", "Users", "Workouts", "Exercises", "WorkoutExercises" };

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.Tags = OrderedTags
            .Select(t => new OpenApiTag { Name = t })
            .ToList();
    }
}