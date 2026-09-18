using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Protocol;
using Entra.EventHandlers.Abstractions.Protocol.Authentication;
using Entra.EventHandlers.Abstractions.Protocol.Otp;
using Entra.EventHandlers.AzureFunctions.Sample.Handlers;
using Entra.EventHandlers.AzureFunctions.Sample.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Entra.EventHandlers.AzureFunctions.Sample.UnitTests.Handlers;

public sealed class EmailOtpSendHandlerTests
{
    private readonly EmailOtpSendHandler _sut;

    private readonly IEmailSender _emailSender;
    private readonly ILogger<EmailOtpSendHandler> _logger;

    public EmailOtpSendHandlerTests()
    {
        _logger = Substitute.For<ILogger<EmailOtpSendHandler>>();
        _emailSender = Substitute.For<IEmailSender>();

        _sut = new EmailOtpSendHandler(_logger, _emailSender);
    }

    [Fact]
    public async Task HandleAsync_SendsOtp_AndContinuesDefaultBehavior()
    {
        // Arrange
        var token = new CancellationTokenSource().Token;

        var email = "user@example.com";
        var otp = "123456";

        var evt = new EmailOtpSendEvent
        {
            Source = "/source",
            Data = new EmailOtpSendEventPayload
            {
                RawOdataType = EntraOdataTypes.EmailOtpSend.CalloutData,
                AuthenticationContext = new AuthenticationContext(),
                OtpContext = new OtpContext
                {
                    Identifier = email,
                    OneTimeCode = otp
                }
            }
        };

        // Act
        var result = await _sut.HandleAsync(evt, token);

        // Assert: email sender was called
        await _emailSender
            .Received(1)
            .SendOtpAsync(email, otp, token);

        // Assert: no exception
        result.HasException.Should().BeFalse();
        result.Exception.Should().BeNull();

        // Assert: response metadata
        var response = result.Response;

        response.Data.OdataType
            .Should().Be(EntraOdataTypes.EmailOtpSend.ResponseData);

        response.Data.Actions.Should().ContainSingle();

        response.Data.Actions.Single().OdataType
            .Should().Be(EntraOdataTypes.EmailOtpSend.ContinueWithDefaultBehavior);
    }

    [Fact]
    public async Task HandleAsync_WhenEmailSenderThrows_SetsExceptionFlag()
    {
        // Arrange
        var token = new CancellationTokenSource().Token;

        var evt = new EmailOtpSendEvent
        {
            Source = "/source",
            Data = new EmailOtpSendEventPayload
            {
                RawOdataType = EntraOdataTypes.EmailOtpSend.CalloutData,
                AuthenticationContext = new AuthenticationContext(),
                OtpContext = new OtpContext
                {
                    Identifier = "user@example.com",
                    OneTimeCode = "123456"
                }
            }
        };

        _emailSender
            .SendOtpAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Throws(new InvalidOperationException("boom"));

        // Act
        var result = await _sut.HandleAsync(evt, token);

        // Assert
        result.HasException.Should().BeTrue();
        result.Exception.Should().BeOfType<InvalidOperationException>();
    }

}
