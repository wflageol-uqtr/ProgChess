namespace ProChess.Server.Entities;

public class Image
{
    public int Id { get; set; }
    
    public string UserId { get; set; }
    public User User { get; set; }
    
    public string Name { get; set; }
    
    public string Path { get; set; }
}