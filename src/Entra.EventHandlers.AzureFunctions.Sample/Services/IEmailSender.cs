namespace Entra.EventHandlers.AzureFunctions.Sample.Services;

public interface IEmailSender
{
    Task SendOtpAsync(string email, string code, CancellationToken cancellationToken = default);
}