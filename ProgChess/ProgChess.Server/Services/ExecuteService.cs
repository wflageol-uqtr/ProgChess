using System.Diagnostics;
using ProChess.Server.Entities;
using ProChess.Server.Exceptions;
using ProChess.Server.Response;
using ProChess.Server.Utils;
using TestResult = ProChess.Server.Response.TestResult;

namespace ProgChess.Server.Services;

public class ExecuteService: IExecuteService
{
    private readonly IExerciseService _exerciseService;
    private Guid filename;
    
    public ExecuteService(IExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
        filename = Guid.NewGuid();
    }

    public ExecuteResult<List<TestResult>> RunExerciseTest(string solution, string unitTest)
    {
        var code = BuildVisibleTestCode(solution, unitTest);
        var result = RunTestsWithNode(code);
        if (result.IsFailure)
            throw new BadRequestException(result.Error);
        return result;
    }

    public async Task<ExecuteResult<List<TestResult>>> RunHiddenExerciseTestAsync(string solution, int exerciseId)
    {
        var exercise = await _exerciseService.GetByIdWithHiddenTest(exerciseId);
        if (exercise == null)
            throw new NotFoundException("Aucun exercice trouvé");
        var code = BuildFullTestCode(solution, exercise.UnitTests);
        var result = RunTestsWithNode(code);
        if (result.IsFailure)
            throw new BadRequestException(result.Error);
        return result;
    }

    private ExecuteResult<List<TestResult>> RunTestsWithNode(string code)
    {
        File.WriteAllText($"Script/{filename}.js", code);
        var result = RunNodeCommandLine();
        File.Delete($"Script/{filename}.js");
        return result;
    }

    private ExecuteResult<List<TestResult>> RunNodeCommandLine()
    {
        var psi = new ProcessStartInfo
        {
            WorkingDirectory = "Script",
            FileName = "node",
            Arguments = $"{filename}.js",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = false
        };
        
        using var process = Process.Start(psi);
        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd(); 
        process.WaitForExit();
        if (!string.IsNullOrEmpty(error))
        {
            return ExecuteResult<List<TestResult>>.Failure(error);
        }
        return GenerateResult(Formatter.SplitByLine(output));
    }
    
    private string BuildFullTestCode(string userCode, IEnumerable<UnitTest> unitTests)
    {
        return
            "import assert from 'node:assert/strict';\nimport { it } from 'node:test';\n"
            + userCode + "\n"
            + string.Join("\n", unitTests.Where(e => !e.IsActive).Select(e => e.Code));
    }

    private string BuildVisibleTestCode(string userCode, string userTest)
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
}