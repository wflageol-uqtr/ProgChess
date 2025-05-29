using System.Text.RegularExpressions;
using ProChess.Server.Response;

namespace ProChess.Server.Utils;

public class SuccessCreator : TestCreator
{
    private List<string> outputLines;

    public SuccessCreator(List<string> outputLines)
    {
        this.outputLines = outputLines;
    }
    
    public override List<TestResult> createTestResults()
    {
        var result = new List<TestResult>();

        var passedTest = outputLines.Where(l => l.StartsWith("✔") 
                                                && Regex.IsMatch(l, @"\(.+?ms\)")).Distinct().ToList();
        foreach (var test in passedTest)
        {
            result.Add(new TestSuccess(test));
        }
        return result;
    }
}