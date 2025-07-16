using AutoFixture;
using ProChess.Server.Entities;
using ProChess.Server.Exceptions;
using ProgChess.Server.Database;
using ProgChess.Server.Dto;
using ProgChess.Server.Services;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace ProgChess.Test.Server.Service;


public class ScoreServiceTest
{
    private readonly AppDbContext dbContext;
    private Fixture _fixture;
    private ScoreService _scoreService;

    public ScoreServiceTest()
    {
        dbContext = TestHelper.ContextGenerator();
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));        
    }
    
    // Create test section
    [Fact]
    public async Task Create_Score_Success()
    {
        var scoreDto = _fixture.Create<ScoreDto>();
        _scoreService = new ScoreService(dbContext, new StudentService(dbContext), new StudentExerciseService(dbContext, new StudentService(dbContext)), new ScoreTestService(dbContext)); 
        var score = await _scoreService.Create(scoreDto);
        
        Assert.IsNotNull(score);
        Assert.IsInstanceOfType<Score>(score);
        await dbContext.Database.EnsureDeletedAsync();
    }
    
    // Read test section
    [Fact]
    public async Task Read_ScoreById_Success()
    {
        var student = _fixture.Create<Student>();
        var exercise = _fixture.Create<Exercise>();
        await dbContext.Students.AddAsync(student);
        await dbContext.Exercises.AddAsync(exercise);
        await dbContext.SaveChangesAsync();

        var scoreDto = _fixture.Build<ScoreDto>()
            .With(x => x.StudentId, student.Id)
            .With(x => x.ExerciseId, exercise.Id)
            .Create();
        
        _scoreService = new ScoreService(dbContext, new StudentService(dbContext), new StudentExerciseService(dbContext, new StudentService(dbContext)), new ScoreTestService(dbContext)); 
        var createdScore = await _scoreService.Create(scoreDto);
        
        var score = await _scoreService.GetScoreByIdAsync(createdScore.Id);
        Assert.IsNotNull(score);
        Assert.IsInstanceOfType<Score>(score);
        await dbContext.Database.EnsureDeletedAsync();
    }
    
    [Fact]
    public async Task Read_ScoreById_NotFound()
    {
        var student = _fixture.Create<Student>();
        var exercise = _fixture.Create<Exercise>();
        await dbContext.Students.AddAsync(student);
        await dbContext.Exercises.AddAsync(exercise);
        await dbContext.SaveChangesAsync();

        var scoreDto = _fixture.Build<ScoreDto>()
            .With(x => x.StudentId, student.Id)
            .With(x => x.ExerciseId, exercise.Id)
            .Create();
        
        _scoreService = new ScoreService(dbContext, new StudentService(dbContext), new StudentExerciseService(dbContext, new StudentService(dbContext)), new ScoreTestService(dbContext)); 
        var score = await _scoreService.Create(scoreDto);
        
        await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
        {
            await _scoreService.GetScoreByIdAsync(score .Id+ 1);
        });
        await dbContext.Database.EnsureDeletedAsync();

    }

    // Update test section
    [Fact]
    public async Task Update_Score_Success()
    {
        var student = _fixture.Create<Student>();
        var exercise = _fixture.Create<Exercise>();
        await dbContext.Students.AddAsync(student);
        await dbContext.Exercises.AddAsync(exercise);
        await dbContext.SaveChangesAsync();

        var scoreDto = _fixture.Build<ScoreDto>()
            .With(x => x.StudentId, student.Id)
            .With(x => x.ExerciseId, exercise.Id)
            .Create();
        
        _scoreService = new ScoreService(dbContext, new StudentService(dbContext), new StudentExerciseService(dbContext, new StudentService(dbContext)), new ScoreTestService(dbContext)); 
        var createdScore = await _scoreService.Create(scoreDto);
        
        scoreDto.Answer = "Updated Answer";
        
        var updatedScore = await _scoreService.Edit(createdScore.Id, scoreDto);
        Assert.IsNotNull(updatedScore);
        Assert.AreEqual("Updated Answer", updatedScore.Answer);
    }

    [Fact]
    public async Task Update_Score_NotFound()
    {
        var student = _fixture.Create<Student>();
        var exercise = _fixture.Create<Exercise>();
        await dbContext.Students.AddAsync(student);
        await dbContext.Exercises.AddAsync(exercise);
        await dbContext.SaveChangesAsync();

        var scoreDto = _fixture.Build<ScoreDto>()
            .With(x => x.StudentId, student.Id)
            .With(x => x.ExerciseId, exercise.Id)
            .Create();
        
        _scoreService = new ScoreService(dbContext, new StudentService(dbContext), new StudentExerciseService(dbContext, new StudentService(dbContext)), new ScoreTestService(dbContext)); 
        var createdScore = await _scoreService.Create(scoreDto);
        
        scoreDto.Answer = "Updated Answer";
        await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
        {
            await _scoreService.Edit(createdScore.Id + 1, scoreDto);
        });
    }

    // Delete test section
    [Fact]
    public async Task Delete_Score_Success()
    {
        var scoreDto = _fixture.Create<ScoreDto>();
        _scoreService = new ScoreService(dbContext, new StudentService(dbContext), new StudentExerciseService(dbContext, new StudentService(dbContext)), new ScoreTestService(dbContext)); 
        var score = await _scoreService.Create(scoreDto);
        await _scoreService.Delete(score.Id);
        
        await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
        {
            await _scoreService.GetScoreByIdAsync(score.Id);
        });
        await dbContext.Database.EnsureDeletedAsync();
    }
    
    [Fact]
    public async Task Delete_Score_NotFound()
    {
        var scoreDto = _fixture.Create<ScoreDto>();
        _scoreService = new ScoreService(dbContext, new StudentService(dbContext), new StudentExerciseService(dbContext, new StudentService(dbContext)), new ScoreTestService(dbContext)); 
        var result = await _scoreService.Create(scoreDto);
        
        await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
        {
            await _scoreService.Delete(result.Id + 1);
        });
        await dbContext.Database.EnsureDeletedAsync();
    }
}