using ProChess.Server.Response;

namespace ProChess.Server.Strategy;

public class AssertionErrorClassifier : ITestErrorClassifier
{
    public TestError execute(List<string> outputLines, string test)
    {
        var actualLine = outputLines.FirstOrDefault(l => l.Contains("actual:"));
        if (actualLine != null)
            outputLines.Remove(actualLine);
        var expectedLine = outputLines.FirstOrDefault(l => l.Contains("expected:"));
        if (expectedLine != null)
            outputLines.Remove(expectedLine);
            
        return new TestError(test, actualLine.Trim(), expectedLine.Trim());
    }
}