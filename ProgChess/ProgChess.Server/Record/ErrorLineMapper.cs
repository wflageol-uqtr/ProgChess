using ProChess.Server.Utils;

namespace ProChess.Server.Record;

public class ErrorLineMapper
{
    public string FullCode { get; set; }
    public int ImportLength { get; set; }
    public int CodeSection { get; set; }
    public int TestLength { get; set; }
    public string CleanError { get; set; }

    public ErrorLineMapper(string fullCode, int importLength, int testLength, int codeSection)
    {
        FullCode = fullCode;
        ImportLength = importLength;
        TestLength = testLength;
        CodeSection = codeSection;
    }

    public int GetExecutionErrorLine(string errorMessage)
    {
        var errorLine = CleanExecutionErrorMessage(errorMessage);
        var lineNumber = 0;
        Console.WriteLine(errorLine);
        if (errorLine <= CodeSection)
        {
            Console.WriteLine(ImportLength);
            lineNumber = errorLine - ImportLength;
        }
        else
        {
            lineNumber = errorLine - ImportLength - TestLength;
            Console.WriteLine(lineNumber);
            if (lineNumber >= 0)
            {
                lineNumber = 1;
            }
        }
        return lineNumber;
    }

    private int CleanExecutionErrorMessage(string errorMessage)
    {
        var lines = Formatter.SplitByLine(errorMessage);
        int lineNumber = 1;

        if (lines.Count > 0 && lines[0].StartsWith("file:///"))
        {
            var marker = ".js:";
            var index = lines[0].IndexOf(marker, StringComparison.Ordinal);

            if (index != -1 && index + marker.Length < lines[0].Length)
            {
                lineNumber = int.Parse(lines[0].Substring(index + marker.Length));
                lines = lines.Skip(1).ToList();
            }
        }

        CleanError = string.Join('\n', lines);
        return lineNumber;
    }
}