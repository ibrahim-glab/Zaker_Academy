namespace Zaker_Academy.Service.ErrorHandling
{
    public class ServiceResult
    {
        public bool succeeded { get; set; }

        public string? Message { get; set; } = "";

        public object? Details { get; set; } = "";
    }
}