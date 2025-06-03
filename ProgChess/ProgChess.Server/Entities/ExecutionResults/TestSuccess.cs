namespace ProChess.Server.Response;

public class TestSuccess : TestResult
{
    public TestSuccess(string testName) : base(testName, true)
    {
    }
}