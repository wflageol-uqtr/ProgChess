using ProChess.Server.Response;

namespace ProChess.Server.ExecutionError;

public interface ITestErrorClassifier
{
    public TestError execute(List<string> outputLines, string test);
}