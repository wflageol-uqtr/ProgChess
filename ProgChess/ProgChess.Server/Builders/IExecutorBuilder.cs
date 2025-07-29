using System.Text;
using System.Text.Json;
using ProChess.Server.Enums;
using ProChess.Server.Record;
using ProChess.Server.Response;
using ProChess.Server.Utils;
using ProgChess.Server.Dto;

namespace ProChess.Server.ExecuteBuilder;

public class IExecutorBuilder
{
    private class Impl: ISpecifyLanguage, ICodeGenerator, IExecuteCode, IGenerateResult
    {
        private CodeExecutor codeExecutor = new();
        
        public ICodeGenerator OfType(LanguageType type)
        {
            codeExecutor.Type = type;
            return this;
        }

        public IExecuteCode BuildCode(string code, string unitTest)
        {
            switch (codeExecutor.Type)
            {
                case LanguageType.Javascript:
                    var importSection = "import assert from 'node:assert/strict';\nimport { it } from 'node:test';\n";
                    
                    int importLines = importSection.Count(c => c == '\n');
                    int codeLines = code.Count(c => c == '\n');
                    int testStartLine = unitTest.Count(c => c == '\n');
                    
                    var fullCode = importSection + code + "\n" + unitTest + "\n";
                    codeExecutor.mapper = new ErrorLineMapper(fullCode, importLines, testStartLine + 1, importLines + codeLines + 1);
                 break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return this;
        }
        
        public async Task<IGenerateResult> Execute(HttpClient httpClient)
        {
            var json = JsonSerializer.Serialize(codeExecutor.mapper.FullCode);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("/vm/execute", content);
        
            var result = await response.Content.ReadFromJsonAsync<VMExecuteDto>();
            codeExecutor.vmExecuteDto = result;
            return this;
        }
        
        public ExecuteResult<List<TestResult>> GenerateExecuteResult()
        {
            if (!codeExecutor.vmExecuteDto.IsSuccess)
            {
                var line = codeExecutor.mapper.GetExecutionErrorLine(codeExecutor.vmExecuteDto.Error);
                return ExecuteResult<List<TestResult>>.Failure(codeExecutor.mapper.CleanError, line.ToString());
            }
            return GenerateResult(Formatter.SplitByLine(codeExecutor.vmExecuteDto.Output));
        }
        
        private ExecuteResult<List<TestResult>> GenerateResult(List<string> output)
        {
            var result = new List<TestResult>();
            result.AddRange(new SuccessCreator(output).createTestResults());
            result.AddRange(new ErrorCreator(output).createTestResults());
            return ExecuteResult<List<TestResult>>.Success(result);
        }
    }
    
    public static ISpecifyLanguage Create()
    {
        return new Impl();
    }
    
}