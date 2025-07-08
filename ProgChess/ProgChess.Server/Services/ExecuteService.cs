using System.Text;
using System.Text.Json;
using ProChess.Server.Entities;
using ProChess.Server.Enums;
using ProChess.Server.Exceptions;
using ProChess.Server.ExecuteBuilder;
using ProChess.Server.Response;
using ProChess.Server.Utils;
using ProgChess.Server.Dto;
using TestResult = ProChess.Server.Response.TestResult;

namespace ProgChess.Server.Services;

public class ExecuteService: ICodeExecuterService
{
    private readonly IExerciseService _exerciseService;
    private readonly HttpClient _httpClient;
    
    public ExecuteService(IExerciseService exerciseService, IHttpClientFactory factory)
    {
        _exerciseService = exerciseService;
        _httpClient = factory.CreateClient("VmApi");
    }

    public async Task<ExecuteResult<List<TestResult>>> ExecuteOnVm(string solution, string unitTest)
    {
        var code = BuildCode(solution, unitTest);
        var result = await ExecuteHttpClient(code);
        if (result.IsFailure)
            throw new BadRequestException(result.Error);
        return result;
    }

    public async Task<ExecuteResult<List<TestResult>>> ExecuteTest(string solution, string unitTest)
    {
        var result = (await IExecutorBuilder.Create()
            .OfType(LanguageType.Javascript)
            .BuildCode(solution, unitTest)
            .Execute(_httpClient))
            .GenerateExecuteResult();
        
        if (result.IsFailure) {
            throw new ExecutionErrorException(result.Error);
        }
        return result;
    }

    public async Task<ExecuteResult<List<TestResult>>> ExecuteHiddenTestOnVm(string solution, int exerciseId)
    {
        var exercise = await _exerciseService.GetByIdWithHiddenTest(exerciseId);
        if (exercise == null)
            throw new NotFoundException("Aucun exercice trouvé");
        var code = BuildCode(solution, string.Join("\n", exercise.UnitTests.Where(e => !e.IsActive).Select(e => e.Code)));
        var result = await ExecuteHttpClient(code);
        
        if (result.IsFailure)
            throw new BadRequestException(result.Error);
        return result;
    }
    private string BuildCode(string userCode, string userTest)
    {
        return
            "import assert from 'node:assert/strict';\nimport { it } from 'node:test';\n"
            + userCode + "\n"
            + userTest + "\n";
        
    }

    private ExecuteResult<List<TestResult>> GenerateResult(List<string> output)
    {
        var result = new List<TestResult>();
        result.AddRange(new SuccessCreator(output).createTestResults());
        result.AddRange(new ErrorCreator(output).createTestResults());
        return ExecuteResult<List<TestResult>>.Success(result);
    }

    private async Task<ExecuteResult<List<TestResult>>> ExecuteHttpClient(string code)
    {
        string json = JsonSerializer.Serialize(code);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/vm/execute", content);
        
        var result = await response.Content.ReadFromJsonAsync<VMExecuteDto>();
        if (result == null || !result.IsSuccess)
        {
            return ExecuteResult<List<TestResult>>.Failure(result.Error);
        }
        return GenerateResult(Formatter.SplitByLine(result.Output));
    }
}