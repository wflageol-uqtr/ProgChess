using System.Text.RegularExpressions;
using ProChess.Server.Enums;
using ProChess.Server.Exceptions;
using ProChess.Server.ExecuteBuilder;
using ProChess.Server.Response;
using ProChess.Server.Utils.Interface;

namespace ProChess.Server.Utils;

public class SuccessCreator : TestCreator
{
    private List<string> outputLines;

    public SuccessCreator(List<string> outputLines)
    {
        this.outputLines = outputLines;
    }
    
    public override List<TestResult> createTestResults()
    {
        return Language switch
        {
            LanguageType.Javascript => new JsTestResult().CreateSuccessTest(outputLines),
            _ => throw new ExecutionErrorException($"Language not supported")

        };
    }
}