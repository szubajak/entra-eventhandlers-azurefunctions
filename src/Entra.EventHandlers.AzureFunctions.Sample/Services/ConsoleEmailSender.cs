namespace Entra.EventHandlers.AzureFunctions.Sample.Services;

public sealed class ConsoleEmailSender : IEmailSender
{
    public Task SendOtpAsync(string email, string code, CancellationToken cancellationToken = default)
    {
        // In a real project, call SendGrid / ACS / SMTP here
        Console.WriteLine($"Sending OTP {code} to {email}");
        return Task.CompletedTask;
    }
}