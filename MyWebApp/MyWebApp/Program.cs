using MyWebApp.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProjectServices(builder.Configuration);
builder.Services.AddSwaggerwithJwt();
builder.Services.AddSwaggerGen(c =>
{
    c.DocumentFilter<TagOrderDocumentFilter>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
app.MapUserEndpoints();
app.MapWorkoutEndpoints();
app.MapExerciseEndpoints();
app.MapExerciseWorkoutEndpoints();

app.Run();