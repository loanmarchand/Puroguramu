using Microsoft.EntityFrameworkCore;
using Puroguramu.App.Middlewares;
using Puroguramu.Domains;
using Puroguramu.Infrastructures.data;
using Puroguramu.Infrastructures.dto;
using Puroguramu.Infrastructures.Roslyn;
using Microsoft.AspNetCore.Identity;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.Repository;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();

builder.Services.AddDbContext<PurogumaruContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddDefaultIdentity<Utilisateurs>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<PurogumaruContext>();

builder.Services.AddScoped<ILogger, Logger<object>>();
builder.Services.AddScoped<ReverseProxyLinksMiddleware>();
builder.Services.AddScoped<IExercisesRepository, DummyExercisesRepository>();
builder.Services.AddScoped<IAssessExercise, RoslynAssessor>();
builder.Services.AddScoped<ILeconsRepository, LeconsRepository>();
builder.Services.AddScoped<IExercisesRepository, ExercicesRepository>();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions());
app.UseReverseProxyLinks();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();

app.Run();
