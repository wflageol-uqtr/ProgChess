using System.Text;
using System.Text.Json;
using ProChess.Server.Enums;
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
                    codeExecutor.code = "import assert from 'node:assert/strict';\nimport { it } from 'node:test';\n"
                    + code + "\n"
                    + unitTest + "\n"; 
                 break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return this;
        }
        
        public async Task<IGenerateResult> Execute(HttpClient httpClient)
        {
            var json = JsonSerializer.Serialize(codeExecutor.code);
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
                return ExecuteResult<List<TestResult>>.Failure(codeExecutor.vmExecuteDto.Error);
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