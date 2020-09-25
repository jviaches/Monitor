
namespace Monitor.Core.Validations
{
    public class Result
    {
        public bool Success { get; set; }
        public string Error { get; set; }

        public Result(bool success, string errorMessage)
        {
            Success = success;
            Error = errorMessage;
        }

        public static Result Ok()
        {
            return new Result(true, string.Empty);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, string.Empty);
        }

        public static Result Fail()
        {
            return new Result(false, string.Empty);
        }

        public static Result<T> Fail<T>(T value)
        {
            return new Result<T>(value, false, string.Empty);
        }
    }

    public class Result<T>: Result
    {
        public T Value { get; set; }

        protected internal Result(T value, bool success, string error): base(success, error)
        {
            Value = value;
        }
    }
}
