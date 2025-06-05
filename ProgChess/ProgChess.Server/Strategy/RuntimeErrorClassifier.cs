using System.Text.RegularExpressions;
using ProChess.Server.Response;

namespace ProChess.Server.Strategy;

public class RuntimeErrorClassifier: ITestErrorClassifier
{
    public TestError execute(List<string> outputLines, string test)
    {
        var errorLine = outputLines.FirstOrDefault(l => Regex.IsMatch(l, @"\b(ReferenceError|TypeError|SyntaxError|RangeError|AssertionError)\b"));
        if (errorLine != null)
            outputLines.Remove(errorLine);
        return new TestError(test, errorLine, "");
    }
}