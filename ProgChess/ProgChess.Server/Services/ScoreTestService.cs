using Microsoft.EntityFrameworkCore;
using ProChess.Server.Entities;
using ProChess.Server.Exceptions;
using ProChess.Server.Response;
using ProgChess.Server.Database;
using ProgChess.Server.Dto;

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

    public async Task CreateFromAdmin(int scoreId, ICollection<ScoreTestDto> scoreTests)
    {
        foreach (var item in scoreTests)
        {

            var scoreTest = new ScoreTest
            {
                ScoreId = scoreId,
                Name = item.Name,
                IsSuccess = item.IsSuccess,
                Expected = item.Expected,
                Actual = item.Actual
            };
            dbContext.Add(scoreTest);
        }
        await dbContext.SaveChangesAsync();
    }

    public async Task Edit(Score score, ICollection<ScoreTestDto> scoreTests)
    {
        dbContext.ScoreTest.RemoveRange(score.ScoreTests);
        var newScoreTest = new List<ScoreTest>();
        foreach (var unitTest in scoreTests)
        {
            newScoreTest.Add(new ScoreTest()
            {
                Name = unitTest.Name,
                IsSuccess = unitTest.IsSuccess,
                Expected = unitTest.Expected,
                Actual = unitTest.Actual
            });
        }
            
        score.ScoreTests = newScoreTest;
        await dbContext.SaveChangesAsync();
    }
}