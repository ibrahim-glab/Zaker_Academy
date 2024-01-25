namespace Zaker_Academy.Service.ErrorHandling
{
    public class ServiceResult<T>
    {
        public bool succeeded { get; set; }

        public string? Message { get; set; } = "";
        public string? Error { get; set; } = "";

        public T Data { get; set; }
    }
}