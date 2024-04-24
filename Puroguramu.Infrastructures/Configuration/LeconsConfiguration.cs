using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Puroguramu.Infrastructures.data;
using Puroguramu.Infrastructures.dto;

namespace Puroguramu.Infrastructures.Configuration;

public class LeconsConfiguration : IEntityTypeConfiguration<Lecons>
{
    public void Configure(EntityTypeBuilder<Lecons> builder)
    {
        builder.HasKey(lecons => lecons.IdLecons);
        builder.HasIndex(lecons => lecons.Titre)
            .IsUnique();

        builder.HasMany(lecon => lecon.ExercicesList)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }

    public static async Task EnsureDefaultLeconCreated(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PurogumaruContext>();

        var defaultLeconTitle = "Premiere lecon de test";
        var lecon = await context.Lecons.FirstOrDefaultAsync(l => l.Titre == defaultLeconTitle);

        if (lecon == null)
        {
            var exercice = new Exercices
            {
                IdExercice = Guid.NewGuid().ToString(),
                Titre = "Math.Pow",
                Enonce = "Créez une fonction Power C# prenant en paramètre \n            une base b de type float et un exposant e de type int. \n            Power(b, e) retourne le float b e.",
                Modele =
                    "public class Test\n{\n    public static TestResult Ensure(float b, int exponent, float expected)\n    {\n      TestStatus status = TestStatus.Passed;\n      float actual = float.NaN;\n      try\n      {\n         actual = Exercice.Power(b, exponent);\n         if(Math.Abs(actual - expected) > 0.00001f)\n         {\n             status = TestStatus.Failed;\n         }\n      }\n      catch(Exception ex)\n      {\n         status = TestStatus.Inconclusive;\n      }\n\n      return new TestResult(\n        string.Format(\"Power of {0} by {1} should be {2}\", b, exponent, expected),\n        status,\n        status == TestStatus.Passed ? string.Empty : string.Format(\"Expected {0}. Got {1}.\", expected, actual)\n      );\n    }\n}\n\nreturn new TestResult[] {\n  Test.Ensure(2, 4, 16.0f),\n  Test.Ensure(2, -4, 1.0f/16.0f)\n};",
                Solution = "public class Exercice\n{\n  public static float Power(float b,int a) => (float)Math.Pow(b,a);\n}",
                EstVisible = true,
                Difficulte = "Facile"
            };
            var exerciceList = new List<Exercices>();
            exerciceList.Add(exercice);
            lecon = new Lecons
            {
                IdLecons = Guid.NewGuid().ToString(),
                Titre = defaultLeconTitle,
                Description = "Description de la premiere lecon de test",
                estVisible = true,
                ExercicesList = exerciceList
            };
            context.Lecons.Add(lecon);
            await context.SaveChangesAsync();
        }
    }
}
