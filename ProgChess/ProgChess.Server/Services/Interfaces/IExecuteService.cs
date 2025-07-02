using ProChess.Server.Response;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public interface IExecuteService
{
    public Task<ExecuteResult<List<TestResult>>> ExecuteOnVm(string solution, string unitTest);
    public Task<ExecuteResult<List<TestResult>>> RunHiddenExerciseTestAsync(string solution, int exerciseId);
}