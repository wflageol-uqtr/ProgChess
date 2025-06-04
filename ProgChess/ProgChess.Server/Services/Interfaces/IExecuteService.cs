using ProChess.Server.Response;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public interface IExecuteService
{
    public List<TestResult>? RunVisibleExerciseTestAsync(string solution, string unitTest, int exerciseId);
    public Task<List<TestResult>?> RunHiddenExerciseTestAsync(string solution, int exerciseId);
}