using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.AzureFunctions.Sample.Services;
using Entra.EventHandlers.Builders;
using Entra.EventHandlers.Handlers.Base;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.AzureFunctions.Sample.Handlers;

public sealed class EmailOtpSendHandler(
    ILogger<EmailOtpSendHandler> logger,
    IEmailSender emailSender)
    : EmailOtpSendHandlerBase(logger)
{
    protected override async Task<EmailOtpSendResponse> HandleCoreAsync(
        EmailOtpSendEvent request,
        CancellationToken cancellationToken = default)
    {
        var otpContext = request.Data.OtpContext;

        var email = otpContext.Identifier;
        var code = otpContext.OneTimeCode;

        await emailSender.SendOtpAsync(
            email,
            code,
            cancellationToken);

        return EntraEventResponses.EmailOtpSend()
            .ContinueWithDefaultBehavior()
            .Build();
    }
}