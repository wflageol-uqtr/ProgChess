using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProChess.Server.Entities;
using ProChess.Server.Response;
using ProChess.Server.Utils;
using ProgChess.Server.Dto;
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

    public List<TestResult>? RunVisibleExerciseTestAsync(string solution, string unitTest, int exerciseId)
    {
        try
        {
            var code = BuildVisibleTestCode(solution, unitTest);
            return RunTestsWithNode(code);
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task<List<TestResult>?> RunHiddenExerciseTestAsync(string solution, int exerciseId)
    {
        try
        {
            var exercise = await _exerciseService.GetById(exerciseId);
            var code = BuildFullTestCode(solution, exercise.UnitTests);
            return RunTestsWithNode(code);
        }
        catch (Exception e)
        {
            return null;
        }
    }

    private List<TestResult>? RunTestsWithNode(string code)
    {
        try
        { 
            File.WriteAllText($"Script/{filename}.js", code);
            var result = RunNodeCommandLine();
            File.Delete($"Script/{filename}.js");
            return result;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    private List<TestResult>? RunNodeCommandLine()
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
        process.WaitForExit();
        return GenerateResult(Formatter.SplitByLine(output));
    }
    
    private string BuildFullTestCode(string userCode, IEnumerable<UnitTest> unitTests)
    {
        return
            "import assert from 'node:assert/strict';\nimport { it } from 'node:test';\n"
            + userCode + "\n"
            + string.Join("\n", unitTests.Select(e => e.Code));
    }

    private string BuildVisibleTestCode(string userCode, string userTest)
    {
        return
            "import assert from 'node:assert/strict';\nimport { it } from 'node:test';\n"
            + userCode + "\n"
            + userTest + "\n";
        
    }

    private List<TestResult>? GenerateResult(List<string> output)
    {
        var result = new List<TestResult>();
        result.AddRange(new SuccessCreator(output).createTestResults());
        result.AddRange(new ErrorCreator(output).createTestResults());
        return result;
    }
}