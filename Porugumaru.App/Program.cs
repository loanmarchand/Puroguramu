using Porugumaru.App.Core;
using Porugumaru.App.Core.Dummies;
using Porugumaru.App.Core.Roslyn;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ILogger, Logger<object>>();
builder.Services.AddScoped<IExercicesRepository, DummyExercicesRepository>();
builder.Services.AddScoped<IAssessCode, RoslynCodeAssessment>();

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
