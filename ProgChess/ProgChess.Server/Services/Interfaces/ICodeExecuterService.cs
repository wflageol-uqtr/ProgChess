using ProChess.Server.ExecuteBuilder;
using ProChess.Server.Response;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public interface ICodeExecuterService
{
    public Task<ExecuteResult<List<TestResult>>> ExecuteOnVm(string solution, string unitTest);
    public Task<ExecuteResult<List<TestResult>>> ExecuteOnDocker(string solution, string unitTest);

    public Task<ExecuteResult<List<TestResult>>> ExecuteHiddenTestOnVm(string solution, int exerciseId);
}