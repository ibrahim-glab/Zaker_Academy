using Zaker_Academy.Service.ErrorHandling;

namespace Zaker_Academy.Service.Interfaces
{
    public interface IEmailService
    {
        Task<ServiceResult> sendAsync(string Subject, string body, string Sender, string reciver);
    }
}