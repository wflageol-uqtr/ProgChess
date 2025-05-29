namespace ProChess.Server.Response;

public class TestError: TestResult
{
    public string Actual { get; set; }
    public string Expected { get; set; }

    public TestError(string testname,string actual, string expected) : base(testname, false)
    {
        Actual = actual;
        Expected = expected;
    }
}