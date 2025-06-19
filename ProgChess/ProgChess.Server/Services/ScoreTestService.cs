using ProChess.Server.Entities;
using ProChess.Server.Response;
using ProgChess.Server.Database;

namespace ProgChess.Server.Services;

public class ScoreTestService(AppDbContext dbContext): IScoreTestService
{
    public async Task Create(int scoreId, List<TestResult> testResults)
    {
        foreach (var testResult in testResults)
        {
            string expected = null;
            string actual = null;

            if (testResult is TestError error)
            {
                expected = error.Expected;
                actual = error.Actual;
            }

            var scoreTest = new ScoreTest
            {
                ScoreId = scoreId,
                Name = testResult.TestName,
                IsSuccess = testResult.Success,
                Expected = expected,
                Actual = actual
            };
            dbContext.Add(scoreTest);
        }
        await dbContext.SaveChangesAsync();
    }
}