using ProChess.Server.Response;

namespace ProChess.Server.Strategy;

public interface ITestErrorClassifier
{
    public TestError execute(List<string> outputLines, string test);
}