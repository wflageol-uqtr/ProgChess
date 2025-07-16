using System.Diagnostics;

namespace ProgChess.VM.Simulation;

public class ExecuteResponse()
{
    public bool IsSuccess { get; set; }
    
    public string? Error { get; set; }
    
    public string? Output { get; set; }    
}

public class VmService
{
    public async Task<ExecuteResponse> ExecuteAsync(string code)
    {
        var filename = Guid.NewGuid().ToString();
        var path = Path.Combine("Script", $"{filename}.js");

        try
        {
            await File.WriteAllTextAsync(path, code);
            var result = await RunNodeCommandLineAsync(filename);
            return result;
        }
        finally
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }

    private async Task<ExecuteResponse> RunNodeCommandLineAsync(string filename)
    {
        var psi = new ProcessStartInfo
        {
            WorkingDirectory = "Script",
            FileName = "node",
            Arguments = $"{filename}.js",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        using var process = new Process { StartInfo = psi };
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        if (!string.IsNullOrWhiteSpace(error))
        {
            return new ExecuteResponse
            {
                IsSuccess = false,
                Error = error
            };
        }
        return new ExecuteResponse
        {
            IsSuccess = true,
            Output = output
        };
    }
}
