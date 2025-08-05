namespace ProChess.Server.Response;

public class ExecuteResult<T>
{
    public bool IsSuccess { get; set; }
    public bool IsFailure => !IsSuccess;
    public T Value { get; set; }
    public string Error { get; set; }
    
    public string LineError { get; set; }

    private ExecuteResult(bool isSuccess, T value, string error, string lineError)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        LineError = lineError;
    }

    public static ExecuteResult<T> Success(T value) => new ExecuteResult<T>(true, value, null, null);
    public static ExecuteResult<T> Failure(string error, string lineError) => new ExecuteResult<T>(false, default, error, lineError);
}