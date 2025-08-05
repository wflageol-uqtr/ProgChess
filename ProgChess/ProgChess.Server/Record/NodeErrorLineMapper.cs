using ProChess.Server.Utils;

namespace ProChess.Server.Record;

// Rename to Node js
public class NodeErrorLineMapper
{
    public string FullCode { get; set; }
    public int ImportLength { get; set; }
    public int CodeSection { get; set; }
    public int TestLength { get; set; }
    public string CleanError { get; set; }

    public NodeErrorLineMapper(string fullCode, int importLength, int testLength, int codeSection)
    {
        FullCode = fullCode;
        ImportLength = importLength;
        TestLength = testLength;
        CodeSection = codeSection;
    }

    public int GetExecutionErrorLine(string errorMessage)
    {
        var lines = Formatter.SplitByLine(errorMessage);
        var lineNumber = 1;
        if (lines.Count > 0 && lines[0].StartsWith("file:///"))
        {
            var marker = ".mjs:";
            var index = lines[0].IndexOf(marker, StringComparison.Ordinal);
            if (index != -1 && index + marker.Length < lines[0].Length)
            {
                lineNumber = int.Parse(lines[0].Substring(index + marker.Length));
                lines = lines.Skip(1).ToList();
            }
        }

        CleanError = string.Join('\n', lines);
        return lineNumber - ImportLength;
    }
}