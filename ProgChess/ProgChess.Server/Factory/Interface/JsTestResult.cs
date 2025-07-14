using System.Text.RegularExpressions;
using ProChess.Server.ExecutionError;
using ProChess.Server.Response;

namespace ProChess.Server.Utils.Interface;

public class JsTestResult: ILanguageTestResult
{    
    private ITestErrorClassifier testErrorClassifier;
    
    public List<TestResult> CreateSuccessTest(List<string> outputLines)
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

    public List<TestResult> CreateFailureTest(List<string> outputLines)
    {
        var index = outputLines.FindIndex(l =>  l.StartsWith("✖") && Regex.IsMatch(l, "failing tests"));
        if (index < 0)
        {
            return new List<TestResult>();
        }
        return GenerateTestErrorList(outputLines.Skip(index + 1));
    }
    
    private List<TestResult> GenerateTestErrorList(IEnumerable<string> errorLines)
    {
        var results = new List<TestResult>();
        return ProcessTestError(errorLines, results);
    }


    private List<TestResult> ProcessTestError(IEnumerable<string> errorLines, List<TestResult> results)
    {
        var lines = Formatter.GetLines(errorLines).Take(3).ToList();
        var testName = lines.FirstOrDefault(l => l.StartsWith("✖"));
        
        if (Regex.IsMatch(lines[2], "AssertionError", RegexOptions.IgnoreCase))
        {
            var assertionError = errorLines.TakeWhile(e => !e.Contains("operator:"));
            setCommonTestError(new AssertionErrorClassifier());
            results.AddRange(executeCommonTestError(assertionError.ToList(), testName));
        }

        if (Regex.IsMatch(lines[2], @"\b(ReferenceError|TypeError|SyntaxError|RangeError|Error)\b",
                RegexOptions.IgnoreCase))
        { 
            setCommonTestError(new RuntimeErrorClassifier());
            results.AddRange(executeCommonTestError(lines, testName));
        }
        
        var index = errorLines.Skip(1).ToList().FindIndex(l => l.Contains("test at"));
        if (index <= 0)
        {
            return results;
        }
        return ProcessTestError(errorLines.Skip(index + 1), results);
    }
    
    private TestError executeCommonTestError(List<string> assertionError, string test)
    {
        return testErrorClassifier.execute(assertionError, test);
    }

    private void setCommonTestError(ITestErrorClassifier testErrorClassifier)
    {
        this.testErrorClassifier = testErrorClassifier;
    }
}