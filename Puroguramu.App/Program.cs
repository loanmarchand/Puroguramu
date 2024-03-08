using Puroguramu.Domains;
using Puroguramu.Infrastructures.Dummies;
using Puroguramu.Infrastructures.Roslyn;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ILogger, Logger<object>>();
builder.Services.AddScoped<IExercisesRepository, DummyExercisesRepository>();
builder.Services.AddScoped<IAssessExercise, RoslynAssessor>();

builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
