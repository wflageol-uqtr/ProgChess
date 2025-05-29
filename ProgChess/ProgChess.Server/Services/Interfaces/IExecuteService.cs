using ProChess.Server.Response;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public interface IExecuteService
{
    public Task<List<TestResult>> RunExerciseTestAsync(ExecuteDto request);
}