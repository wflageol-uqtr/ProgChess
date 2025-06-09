using ProChess.Server.Response;

namespace ProChess.Server.ExecutionError;

public class RuntimeErrorClassifier: ITestErrorClassifier
{
    public TestError execute(List<string> outputLines, string test)
    {
        return new TestError(test, outputLines[2].Trim(), "");
    }
}