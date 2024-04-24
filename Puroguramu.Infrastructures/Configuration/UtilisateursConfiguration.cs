using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Puroguramu.Infrastructures.dto;

namespace Puroguramu.Infrastructures.Configuration;

public class UtilisateursConfiguration : IEntityTypeConfiguration<Utilisateurs>
{
    public void Configure(EntityTypeBuilder<Utilisateurs> builder) => builder.HasIndex(utilisateurs => utilisateurs.Matricule).IsUnique();

    public static async Task EnsureRolesCreated(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Utilisateurs>>();


        await EnsureRoleAsync(roleManager, "Teacher");
        await EnsureRoleAsync(roleManager, "Student");

        await EnsureDefaultTeacherAsync(userManager);
        await EnsureDefaultStudentAsync(userManager);
        return;

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

        async Task EnsureDefaultStudentAsync(UserManager<Utilisateurs> userManager1)
        {
            var defaultStudent = "romaincoibion@gmail.com";
            var user = await userManager1.FindByEmailAsync(defaultStudent);

            if (user == null)
            {
                var student = new Utilisateurs
                {
                    UserName = defaultStudent,
                    Email = defaultStudent,
                    Role = Role.Student,
                    Matricule = "Q210027",
                    Nom = "Coibion",
                    Prenom = "Romain",
                    Groupe = "2i1",
                };
                await userManager1.CreateAsync(student, "Romain1*");
                await userManager1.AddToRoleAsync(student, "Student");
            }
        }
    }

    static async Task EnsureRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}
