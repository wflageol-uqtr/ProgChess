using System.Text.RegularExpressions;
using ProChess.Server.Response;

namespace ProChess.Server.Utils;

public class ErrorCreator : TestCreator
{
    private List<string> outputLines;

    public ErrorCreator(List<string> outputLines)
    {
        this.outputLines = outputLines;
    }
    
    public override List<TestResult> createTestResults()
    {
        var result = new List<TestResult>();
        var failedTest = outputLines.Where(l => l.StartsWith("✖") 
                                                && Regex.IsMatch(l, @"\(.+?ms\)")).Distinct().ToList();

        foreach (var test in failedTest)
        {
            var actualLine = outputLines.FirstOrDefault(l => l.Contains("actual:"));
            if (actualLine != null)
                outputLines.Remove(actualLine);
            var expectedLine = outputLines.FirstOrDefault(l => l.Contains("expected:"));
            if (expectedLine != null)
                outputLines.Remove(expectedLine);
            
            result.Add(new TestError(test, actualLine.Trim(), expectedLine.Trim()));
        }
        return result;
    }
}