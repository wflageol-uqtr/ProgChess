namespace ProChess.Server.Entities;

public class UnitTest
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int? ExerciceId { get; set; }
    public Exercice? Exercice { get; set; }
}