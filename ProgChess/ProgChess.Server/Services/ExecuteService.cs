using ProChess.Server.Enums;
using ProChess.Server.Exceptions;
using ProChess.Server.ExecuteBuilder;
using ProChess.Server.Response;
using TestResult = ProChess.Server.Response.TestResult;

namespace ProgChess.Server.Services;

public class ExecuteService: ICodeExecuterService
{
    private readonly IExerciseService _exerciseService;
    private readonly HttpClient _dockerClient;
    
    public ExecuteService(IExerciseService exerciseService, IHttpClientFactory factory)
    {
        _exerciseService = exerciseService;
        _dockerClient = factory.CreateClient("dockerApi");
    }

    public async Task<ExecuteResult<List<TestResult>>> ExecuteOnVm(string solution, string unitTest)
    {
        var firstExecution = (await IExecutorBuilder.Create()
                .OfType(LanguageType.Javascript)
                .BuildCodeSample(solution)
                .Execute(_dockerClient))
            .SampleResult();

        if (firstExecution.IsFailure)
        {
            throw new ExecutionErrorException( $"Error at line:{firstExecution.LineError}" + firstExecution.Error);
        }
        
        var secondExecution = (await IExecutorBuilder.Create()
                .OfType(LanguageType.Javascript)
                .BuildFullCode(solution, unitTest)
                .Execute(_dockerClient))
            .GenerateExecuteResult();
       
        if (secondExecution.IsFailure) {
            throw new ExecutionErrorException( $"Error at line:{secondExecution.LineError}" + secondExecution.Error);
        }
        return secondExecution;
    }

    public async Task<ExecuteResult<List<TestResult>>> ExecuteHiddenTestOnVm(string solution, int exerciseId)
    {
        var exercise = await _exerciseService.GetByIdWithHiddenTest(exerciseId);
        if (exercise == null)
            throw new NotFoundException("Aucun exercice trouvé");
        
        var result = (await IExecutorBuilder.Create()
                .OfType(LanguageType.Javascript)
                .BuildFullCode(solution,  string.Join("\n", exercise.UnitTests.Where(e => !e.IsActive).Select(e => e.Code)))
                .Execute(_dockerClient))
            .GenerateExecuteResult();

        if (result.IsFailure) {
            throw new ExecutionErrorException(result.Error);
        }
        return result;
    }
    
}