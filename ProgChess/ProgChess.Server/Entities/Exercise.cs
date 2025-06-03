namespace ProChess.Server.Entities;

public class Exercise
{
    public int Id { get; set; }
    
    public string Situation { get; set; } = string.Empty;
    
    public string? BaseCode { get; set; } = string.Empty;

    public List<string> StudentCodes { get; set; } = new();
    public ICollection<UnitTest> UnitTests { get; set; } = new List<UnitTest>();
}