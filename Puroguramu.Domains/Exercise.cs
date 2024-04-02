namespace Puroguramu.Domains;

public class Exercise
{
    public string Titre { get; set; }

    public string Enonce { get; set; }

    public string Modele { get; set; }

    public string Solution { get; set; }

    public bool EstVisible { get; set; }

    public string Difficulte { get; set; }
    private readonly string _template = @"// code-insertion-point

public class Test
{
    public static TestResult Ensure(float b, int exponent, float expected)
    {
      TestStatus status = TestStatus.Passed;
      float actual = float.NaN;
      try
      {
         actual = Exercice.Power(b, exponent);
         if(Math.Abs(actual - expected) > 0.00001f)
         {
             status = TestStatus.Failed;
         }
      }
      catch(Exception ex)
      {
         status = TestStatus.Inconclusive;
      }

      return new TestResult(
        string.Format(""Power of {0} by {1} should be {2}"", b, exponent, expected),
        status,
        status == TestStatus.Passed ? string.Empty : string.Format(""Expected {0}. Got {1}."", expected, actual)
      );
    }
}

return new TestResult[] {
  Test.Ensure(2, 4, 16.0f),
  Test.Ensure(2, -4, 1.0f/16.0f)
};
";

    public string Stub => @"public class Exercice
{
  // Tapez votre code ici
}
";

    public string InjectIntoTemplate(string code)
        => _template.Replace("// code-insertion-point", code);
}
