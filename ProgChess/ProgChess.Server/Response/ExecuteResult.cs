namespace ProChess.Server.Response;

public class ExecuteResult<T>
{
    public bool IsSuccess { get; set; }
    public bool IsFailure => !IsSuccess;
    public T Value { get; set; }
    public string Error { get; set; }

    private ExecuteResult(bool isSuccess, T value, string error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static ExecuteResult<T> Success(T value) => new ExecuteResult<T>(true, value, null);
    public static ExecuteResult<T> Failure(string error) => new ExecuteResult<T>(false, default, error);
}