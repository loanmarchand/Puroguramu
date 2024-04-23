using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Puroguramu.App.Middlewares;
using Puroguramu.Domains;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.data;
using Puroguramu.Infrastructures.dto;
using Puroguramu.Infrastructures.Repository;
using Puroguramu.Infrastructures.Roslyn;
using Role = Puroguramu.Infrastructures.dto.Role;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<PurogumaruContext>(options =>
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
}
else
{
    builder.Services.AddDbContext<PurogumaruContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
}

builder.Services.AddDefaultIdentity<Utilisateurs>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PurogumaruContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsTeacher", policy => policy.RequireRole("Teacher"));
    options.AddPolicy("IsStudent", policy => policy.RequireRole("Student"));
});

builder.Services.AddScoped<ILogger, Logger<object>>();
builder.Services.AddScoped<ReverseProxyLinksMiddleware>();
builder.Services.AddScoped<IAssessExercise, RoslynAssessor>();
builder.Services.AddScoped<ILeconsRepository, LeconsRepository>();
builder.Services.AddScoped<IExercisesRepository, ExercicesRepository>();
builder.Services.AddScoped<IStatutExerciceRepository, StatutExerciceRepository>();
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
app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();
app.MapBlazorHub();

EnsureRolesCreated(app.Services).GetAwaiter().GetResult();

app.Run();

async Task EnsureRolesCreated(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Utilisateurs>>();


    await EnsureRoleAsync(roleManager, "Teacher");
    await EnsureRoleAsync(roleManager, "Student");

    await EnsureDefaultTeacherAsync(userManager);

    async Task EnsureDefaultTeacherAsync(UserManager<Utilisateurs> userManager1)
    {
        var defaultTeacher = "teacher@student.helmo.be";
        var user = await userManager1.FindByEmailAsync(defaultTeacher);

        if (user == null)
        {
            var teacher = new Utilisateurs
            {
                UserName = defaultTeacher,
                Email = defaultTeacher,
                Role = Role.Teacher,
                Matricule = "P200000",
                Nom = "teacher",
                Prenom = "teacher",
                Groupe = "2i1"
            };
            await userManager1.CreateAsync(teacher, "Romain1*");
            await userManager1.AddToRoleAsync(teacher, "Teacher");
        }
    }
}

async Task EnsureRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
{
    if (!await roleManager.RoleExistsAsync(roleName))
    {
        await roleManager.CreateAsync(new IdentityRole(roleName));
    }
}
