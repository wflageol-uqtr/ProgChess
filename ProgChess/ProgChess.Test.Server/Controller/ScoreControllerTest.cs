// using AutoFixture;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Moq;
// using ProChess.Server.Controllers;
// using ProChess.Server.Entities;
// using ProChess.Server.Exceptions;
// using ProgChess.Server.Dto;
// using ProgChess.Server.Services;
// using Xunit;
// using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
// using Exception = System.Exception;
//
// namespace ProgChess.Test.Server;
//
// public class ScoreControllerTest
// {
//     private Mock<IScoreService> _scoreServiceMock;
//     private Fixture _fixture;
//     private ScoreController _scoreController;
//
//     public ScoreControllerTest()
//     {
//         _scoreServiceMock = new Mock<IScoreService>();
//         _fixture = new Fixture();
//         _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
//             .ForEach(b => _fixture.Behaviors.Remove(b));
//         _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
//     }
//
//     // Create Section
//     [Fact]
//     public async Task Post_Score_ReturnsOk()
//     {
//         var scoreDto = _fixture.Create<ScoreDto>();
//         _scoreServiceMock.Setup(service => service.Create(scoreDto)).ReturnsAsync(new Score());
//         _scoreController = new ScoreController(_scoreServiceMock.Object);
//     
//         var result = await _scoreController.Create(scoreDto);
//         var okResult = result.Result as OkObjectResult;
//     
//         Assert.AreEqual(200, okResult.StatusCode);
//         Assert.IsNotNull(okResult.Value);
//         Assert.IsInstanceOfType<Score>(okResult.Value);
//     }
//
//     [Fact]
//     public async Task Post_Score_ReturnsError()
//     {
//         var scoreDto = _fixture.Create<ScoreDto>();
//
//         _scoreServiceMock.Setup(service => service.Create(scoreDto)).Throws(new Exception());
//         _scoreController = new ScoreController(_scoreServiceMock.Object);
//
//         await Assert.ThrowsExceptionAsync<Exception>(async () => 
//         {
//             await _scoreController.Create(scoreDto);
//         });
//     }
//     
//     // Read test section
//     [Fact]
//     public async Task Get_Scores_ReturnsOk()
//     {
//         var scores = _fixture.CreateMany<Score>(3).ToList();
//         _scoreServiceMock.Setup(service => service.GetAllScores()).ReturnsAsync(scores);
//         _scoreController = new ScoreController(_scoreServiceMock.Object);
//
//         var result = await _scoreController.GetScores();
//         var okResult = result.Result as OkObjectResult;
//         var obj = okResult.Value as List<Score>;
//         
//         Assert.IsNotNull(scores);
//         Assert.AreEqual(scores.Count, obj.Count);
//     }
//
//     [Fact]
//     public async Task Get_Scores_ReturnsException()
//     {
//         _scoreServiceMock.Setup(service => service.GetAllScores()).Throws(new Exception());
//         _scoreController = new ScoreController(_scoreServiceMock.Object);
//         await Assert.ThrowsExceptionAsync<Exception>(async () => 
//         {
//             await _scoreController.GetScores();
//         });
//     }
//
//     [Fact]
//     public async Task Get_ScoreById_ReturnsOk()
//     {
//         var score = _fixture.Create<Score>();
//         _scoreServiceMock.Setup(service => service.GetScoreByIdAsync(score.Id)).ReturnsAsync(score);
//         _scoreController = new ScoreController(_scoreServiceMock.Object);
//         
//         var result = await _scoreController.GetScoreById(score.Id);
//         var okResult = result as OkObjectResult;
//         var obj = okResult.Value as Score;
//         
//         Assert.IsNotNull(obj);
//         Assert.AreEqual(score.Id, obj.Id);
//     }
//
//     [Fact]
//     public async Task Get_ScoreById_ReturnsNotFound()
//     {
//         var score = _fixture.Create<Score>();
//         _scoreServiceMock.Setup(service => service.GetScoreByIdAsync(score.Id)).Throws(new NotFoundException("Pas trouvé"));
//         _scoreController = new ScoreController(_scoreServiceMock.Object);
//         var test = await Assert.ThrowsExceptionAsync<NotFoundException>(async () => 
//         {
//             await _scoreController.GetScoreById(score.Id);
//         });
//         Assert.AreEqual(test.Message, "Pas trouvé");
//     }
//
//     [Fact]
//     public async Task Get_ScoreByIdAndStudent_ReturnsOk()
//     {
//         var student = _fixture.Create<Student>();
//         var exercise = _fixture.Create<Exercise>();
//         var score = _fixture.Build<Score>()
//             .With(s => s.StudentId, student.Id)
//             .With(s => s.ExerciseId, exercise.Id)
//             .Create();
//
//         var context = new DefaultHttpContext
//         {
//             Items =
//             {
//                 ["studentCookie"] = student.PermanentCode
//             }
//         };
//         _scoreServiceMock.Setup(service => service.GetScoreByExerciseIdAndStudent(exercise.Id, student.PermanentCode)).ReturnsAsync(score);
//         _scoreController = new ScoreController(_scoreServiceMock.Object);
//         _scoreController.ControllerContext.HttpContext = context;
//         
//         var result = await _scoreController.GetScoreByExerciseAndStudent(exercise.Id);
//         var okResult = result as OkObjectResult;
//         
//         Assert.IsNotNull(okResult.Value);
//         Assert.AreEqual(score, okResult.Value);
//     }
//
//     [Fact]
//     public async Task Get_ScoreByIdAndStudent_ReturnsError_MissingCookie()
//     {
//         var student = _fixture.Create<Student>();
//         var exercise = _fixture.Create<Exercise>();
//         var score = _fixture.Build<Score>()
//             .With(s => s.StudentId, student.Id)
//             .With(s => s.ExerciseId, exercise.Id)
//             .Create();
//         
//         _scoreServiceMock.Setup(service => service.GetScoreByExerciseIdAndStudent(exercise.Id, student.PermanentCode)).ReturnsAsync(score);
//         _scoreController = new ScoreController(_scoreServiceMock.Object);
//         
//         var result = await Assert.ThrowsExceptionAsync<NullReferenceException>(async () => 
//         {
//             await _scoreController.GetScoreByExerciseAndStudent(exercise.Id);
//         });
//         Assert.IsInstanceOfType<NullReferenceException>(result);
//     }
//     
//     [Fact]
//     public async Task Get_ScoreByIdAndStudent_ReturnsNotFound()
//     {
//         var student = _fixture.Create<Student>();
//         var exercise = _fixture.Create<Exercise>();
//         var score = _fixture.Build<Score>()
//             .With(s => s.StudentId, student.Id)
//             .With(s => s.ExerciseId, exercise.Id)
//             .Create();
//         
//         _scoreServiceMock
//             .Setup(service => service.GetScoreByExerciseIdAndStudent(exercise.Id, student.PermanentCode))
//             .Throws(new NotFoundException("Pas trouvé"));
//         
//         var context = new DefaultHttpContext
//         {
//             Items =
//             {
//                 ["studentCookie"] = student.PermanentCode
//             }
//         };
//         _scoreController = new ScoreController(_scoreServiceMock.Object);
//         _scoreController.ControllerContext.HttpContext = context;
//         _scoreController.ControllerContext.HttpContext = context;
//         
//         var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
//         {
//             await _scoreController.GetScoreByExerciseAndStudent(exercise.Id);
//         });
//         Assert.IsInstanceOfType<NotFoundException>(ex);
//     }
//     
//     // Update test section
//     [Fact]
//     public async Task Put_Score_ReturnsOk()
//     {
//         var score = _fixture.Create<Score>();
//         var scoreDto = _fixture.Create<ScoreDto>();
//         _scoreServiceMock.Setup(service => service.Edit(score.Id, scoreDto)).ReturnsAsync(score);
//         _scoreController = new ScoreController(_scoreServiceMock.Object);
//         
//         var result = await _scoreController.Edit(score.Id, scoreDto);
//         var obj = result as ObjectResult;
//         
//         Assert.AreEqual(200, obj.StatusCode);
//     }
//     
//     [Fact]
//     public async Task Put_Score_ReturnsException()
//     {
//         var score = _fixture.Create<Score>();
//         var scoreDto = _fixture.Create<ScoreDto>();
//         _scoreServiceMock.Setup(service => service.Edit(score.Id, scoreDto)).Throws(new Exception());
//         _scoreController = new ScoreController(_scoreServiceMock.Object);
//         var ex = await Assert.ThrowsExceptionAsync<Exception>(async () =>
//         {
//             await _scoreController.Edit(score.Id, scoreDto);
//         });
//         Assert.IsInstanceOfType<Exception>(ex);
//     }
//     
//     [Fact]
//     public async Task Put_Score_ReturnsNotFound()
//     {
//         var score = _fixture.Create<Score>();
//         var scoreDto = _fixture.Create<ScoreDto>();
//         _scoreServiceMock.Setup(service => service.Edit(score.Id, scoreDto)).Throws(new NotFoundException("Score pas trouvé"));
//         _scoreController = new ScoreController(_scoreServiceMock.Object);
//         var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
//         {
//             await _scoreController.Edit(score.Id, scoreDto);
//         });
//         Assert.IsInstanceOfType<NotFoundException>(ex);
//     }
//
//
//     // Delete test section
//     [Fact]
//     public async Task Delete_Score_ReturnsOk()
//     {
//         var score = _fixture.Create<Score>();
//         _scoreServiceMock.Setup(service => service.Delete(score.Id)).Returns(Task.CompletedTask);
//         _scoreController = new ScoreController(_scoreServiceMock.Object);
//         
//         var result = await _scoreController.Delete(score.Id);
//         var obj = result as ObjectResult;
//         
//         Assert.AreEqual(200, obj.StatusCode);
//     }
//
//     [Fact]
//     public async Task Delete_Score_ReturnsNotFound()
//     {
//         var score = _fixture.Create<Score>();
//         _scoreServiceMock.Setup(service => service.Delete(score.Id)).Throws(new NotFoundException("Score pas trouvé"));
//         _scoreController = new ScoreController(_scoreServiceMock.Object);
//         var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
//         {
//             await _scoreController.Delete(score.Id);
//         });
//         Assert.IsInstanceOfType<NotFoundException>(ex);
//     }
// }