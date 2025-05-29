using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace ProChess.Server.Response;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(TestSuccess), "success")]
[JsonDerivedType(typeof(TestError), "error")]
public class TestResult
{
    public string TestName { get; set; }
    
    public bool Success { get; set; }

    protected TestResult(string testName, bool success)
    {
        TestName = testName;
        Success = success;
    }
}