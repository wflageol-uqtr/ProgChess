namespace ProChess.Server.Utils;

public static class Formatter
{
    public static List<string> FormatCodeString(string codeString)
    {
        return codeString.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    public static List<string> SplitByLine(string s)
    {
        return s.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();
    }
    
    public static IEnumerable<string> GetLines(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            yield return line;
        }
    }
}