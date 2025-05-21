namespace ProChess.Server.Entities;

public class Exercice
{
    public int Id { get; set; }
    
    public string Situation { get; set; } = string.Empty;
    
    public string? Code { get; set; } = string.Empty;
    
    public ICollection<UnitTest> UnitTests { get; set; } = new List<UnitTest>();
}