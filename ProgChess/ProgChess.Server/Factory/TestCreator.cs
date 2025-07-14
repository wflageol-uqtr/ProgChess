using ProChess.Server.Enums;
using ProChess.Server.ExecuteBuilder;
using ProChess.Server.Response;

namespace ProChess.Server.Utils;

public abstract class TestCreator
{
     protected LanguageType Language { get; set; } = LanguageType.Javascript;
     public abstract List<TestResult> createTestResults();
}