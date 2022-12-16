using System.Globalization;
using FirebotProxy.Domain.Adapters;
using FirebotProxy.Domain.PrimaryPorts.GetChatWordCloud;
using FirebotProxy.Domain.Representations;
using FirebotProxy.Domain.Tests.FakeHandlers;
using FirebotProxy.SecondaryPorts.GenerateWordCloud;
using FirebotProxy.TestBase;
using FluentAssertions;
using FluentValidation;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;

namespace FirebotProxy.Domain.Tests;

public class GetChatWordCloudRequestHandlerTestsBase
{
    protected GetChatWordCloudRequest BuildRequest()
    {
        return new GetChatWordCloudRequest
        {
            WordCloudSettings = new WordCloudSettings
            {
                Width = 100,
                Height = 100,
                BackgroundHexColour = "#ffffff",
                FontFamily = "Arial",
                WordHexColours = Array.Empty<string>()
            }
        };
    }

    protected readonly ILogger<GetChatWordCloudRequestHandler> Logger = new NullLogger<GetChatWordCloudRequestHandler>();

    protected IValidator<GetChatWordCloudRequest> ConstructRequestValidator(bool shouldValidateRequest)
    {
        return new FakeValidator<GetChatWordCloudRequest>(shouldValidateRequest);
    }

    protected IMediator ConstructMediator(bool getChatTextShouldFail, bool generateWordCloudShouldFail, bool shortenUrlShouldFail)
    {
        return new MediatRFactory(typeof(GetChatWordCloudRequestHandlerTestsBase).Assembly)
            .AddSingletonHandler(new FakeGetChatTextQueryHandler(getChatTextShouldFail))
            .AddSingletonHandler(new FakeGenerateWordCloudCommandHandler(generateWordCloudShouldFail))
            .AddSingletonHandler(new FakeShortenUrlCommandHandler(shortenUrlShouldFail))
            .Build();
    }

    protected IMapper ConstructMapper(bool mapperShouldFail)
    {
        var mapper = new Mock<Mapper>();

        if (mapperShouldFail)
        {
            mapper.Setup(x => x.Map<WordCloudOptions>(It.IsAny<WordCloudSettings>()))
                .Throws(new Exception("test exception message from Mapster"));
        }
        else
        {
            mapper.CallBase = true;
            mapper.As<IMapper>();
        }

        return mapper.Object;
    }
}

[TestFixture]
public class A_GetChatWordCloud_Request_Handler
{
    public class Generates_A_Word_Cloud : GetChatWordCloudRequestHandlerTestsBase
    {
        [Test]
        public async Task Normally()
        {
            var request = BuildRequest();
            request.ViewerUsername = "blackb0x3";
            request.StreamDate = DateTime.UtcNow.ToString("s", CultureInfo.InvariantCulture);
            var validator = ConstructRequestValidator(true);
            var mediator = ConstructMediator(false, false, false);
            var mapper = ConstructMapper(false);

            var handler = new GetChatWordCloudRequestHandler(Logger, validator, mediator, mapper);

            var result = await handler.Handle(request, CancellationToken.None);

            result.Value.Should().BeOfType<GetChatWordCloudResponse>();
            result.AsT0.WordCloudUrl.Should().Be("https://shortened.url/abcdef");
        }

        [Test]
        public async Task When_There_Is_No_Viewer_Username_In_The_Request()
        {
            var request = BuildRequest();
            request.StreamDate = DateTime.UtcNow.ToString("s", CultureInfo.InvariantCulture);
            var validator = ConstructRequestValidator(true);
            var mediator = ConstructMediator(false, false, false);
            var mapper = ConstructMapper(false);

            var handler = new GetChatWordCloudRequestHandler(Logger, validator, mediator, mapper);

            var result = await handler.Handle(request, CancellationToken.None);

            result.Value.Should().BeOfType<GetChatWordCloudResponse>();
            result.AsT0.WordCloudUrl.Should().Be("https://shortened.url/abcdef");
        }

        [Test]
        public async Task When_There_Is_No_Stream_Date_In_The_Request()
        {
            var request = BuildRequest();
            request.ViewerUsername = "blackb0x3";
            var validator = ConstructRequestValidator(true);
            var mediator = ConstructMediator(false, false, false);
            var mapper = ConstructMapper(false);

            var handler = new GetChatWordCloudRequestHandler(Logger, validator, mediator, mapper);

            var result = await handler.Handle(request, CancellationToken.None);

            result.Value.Should().BeOfType<GetChatWordCloudResponse>();
            result.AsT0.WordCloudUrl.Should().Be("https://shortened.url/abcdef");
        }

        [Test]
        public async Task When_There_Is_No_Viewer_Username_Or_Stream_Date_In_The_Request()
        {
            var request = BuildRequest();
            var validator = ConstructRequestValidator(true);
            var mediator = ConstructMediator(false, false, false);
            var mapper = ConstructMapper(false);

            var handler = new GetChatWordCloudRequestHandler(Logger, validator, mediator, mapper);

            var result = await handler.Handle(request, CancellationToken.None);

            result.Value.Should().BeOfType<GetChatWordCloudResponse>();
            result.AsT0.WordCloudUrl.Should().Be("https://shortened.url/abcdef");
        }
    }

    public class Does_Not_Generate_A_Word_Cloud : GetChatWordCloudRequestHandlerTestsBase
    {
        [Test]
        public async Task When_The_WordCloudRequest_Is_Invalid()
        {
            var request = BuildRequest();
            var validator = ConstructRequestValidator(false);
            var mediator = ConstructMediator(false, false, false);
            var mapper = ConstructMapper(false);

            var handler = new GetChatWordCloudRequestHandler(Logger, validator, mediator, mapper);

            var result = await handler.Handle(request, CancellationToken.None);

            result.Value.Should().BeOfType<ValidationRepresentation>();
        }

        [Test]
        public async Task When_The_Chat_Text_Cannot_Be_Retrieved()
        {
            var request = BuildRequest();
            var validator = ConstructRequestValidator(true);
            var mediator = ConstructMediator(true, false, false);
            var mapper = ConstructMapper(false);

            var handler = new GetChatWordCloudRequestHandler(Logger, validator, mediator, mapper);

            var result = await handler.Handle(request, CancellationToken.None);

            result.Value.Should().BeOfType<ErrorRepresentation>();
        }

        [Test]
        public async Task When_The_Word_Cloud_Cannot_Be_Generated()
        {
            var request = BuildRequest();
            var validator = ConstructRequestValidator(true);
            var mediator = ConstructMediator(false, true, false);
            var mapper = ConstructMapper(false);

            var handler = new GetChatWordCloudRequestHandler(Logger, validator, mediator, mapper);

            var result = await handler.Handle(request, CancellationToken.None);

            result.Value.Should().BeOfType<ErrorRepresentation>();
        }

        [Test]
        public async Task When_The_Link_To_The_Word_Cloud_Cannot_Be_Shortened()
        {
            var request = BuildRequest();
            var validator = ConstructRequestValidator(true);
            var mediator = ConstructMediator(false, false, true);
            var mapper = ConstructMapper(false);

            var handler = new GetChatWordCloudRequestHandler(Logger, validator, mediator, mapper);

            var result = await handler.Handle(request, CancellationToken.None);

            result.Value.Should().BeOfType<ErrorRepresentation>();
        }
    }
}