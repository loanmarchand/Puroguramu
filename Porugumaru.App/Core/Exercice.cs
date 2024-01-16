namespace Porugumaru.App.Core;

public class Exercice
{
    private const string Template = @"
public class Test {
    public static string AssertEquals(float expected, float actual, string message) 
    {
      return string.Format(message, expected, actual)+  (Math.Abs(expected - actual) <= 0.0001 ? ""-> PASSED"" : ""-> FAILED"");
    }

  // code-insertion-point
}

var result = new string[] {
  Test.AssertEquals(16.0f, Test.Power(2, 4), ""Expected {0}. Got {1}"")
};
";

    public string MergeTemplateWith(string code)
        => Template.Replace("// code-insertion-point", code);
}