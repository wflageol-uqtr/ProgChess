using System.Text.RegularExpressions;
using ProChess.Server.Response;
using ProChess.Server.Strategy;

namespace ProChess.Server.Utils;

public class ErrorCreator : TestCreator
{
    private List<string> outputLines;
    private ITestErrorClassifier testErrorClassifier;

    public ErrorCreator(List<string> outputLines)
    {
        this.outputLines = outputLines;
    }
    
    public override List<TestResult> createTestResults()
    {
        var result = new List<TestResult>();
        var failedTest = outputLines.Where(l => l.StartsWith("✖") 
                                                && Regex.IsMatch(l, @"\(.+?ms\)")).Distinct().ToList();
        var errorType = ErrorChecker();
        var counter = 0;
        foreach (var test in failedTest)
        { 
            setCommonTestError((ITestErrorClassifier)Activator.CreateInstance(errorType[counter])!);
            result.Add(executeCommonTestError(test));
            counter++;
        }
        return result;
    }

    private List<Type> ErrorChecker()
    {
        var type = new List<Type>();
        foreach (var line in outputLines)
        {
            if (Regex.IsMatch(line, @"AssertionError|\[ERR_ASSERTION\]", RegexOptions.IgnoreCase))
            {
                type.Add(typeof(AssertionErrorClassifier));
            }
            if (Regex.IsMatch(line, @"\b(ReferenceError|TypeError|SyntaxError|RangeError)\b", RegexOptions.IgnoreCase))
            {
                type.Add(typeof(RuntimeErrorClassifier));

            }
        }
        return type;
    }

    private TestError executeCommonTestError(string test)
    {
        return testErrorClassifier.execute(outputLines, test);
    }

    private void setCommonTestError(ITestErrorClassifier testErrorClassifier)
    {
        this.testErrorClassifier = testErrorClassifier;
    }
}