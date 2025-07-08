using ProChess.Server.Response;

namespace ProChess.Server.Utils.Interface;

public interface ILanguageTestResult
{
    List<TestResult> CreateSuccessTest(List<string> outputLines);
    List<TestResult> CreateFailureTest(List<string> outputLines);
}