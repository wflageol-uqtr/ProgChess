using ProChess.Server.Enums;
using ProChess.Server.Record;
using ProChess.Server.Response;
using ProgChess.Server.Dto;

namespace ProChess.Server.ExecuteBuilder;

public class CodeExecutor
{
    public LanguageType Type;
    public VMExecuteDto vmExecuteDto;
    public NodeErrorLineMapper mapper;
}

public interface ISpecifyLanguage
{
    ICodeGenerator OfType(LanguageType type);
}

public interface ICodeGenerator
{
    IExecuteCode BuildCode(string code, string unitTest);
}

public interface IExecuteCode
{
    Task<IGenerateResult> Execute(HttpClient httClient);
}

public interface IGenerateResult
{
    ExecuteResult<List<TestResult>> GenerateExecuteResult();
}