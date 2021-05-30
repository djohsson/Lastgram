using Lastgram.Commands;
using LastgramTest.Commands.Mocks;
using LastgramTest.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace LastgramTest.Commands
{
    [TestFixture]
    public class CommandHandlerTest
    {
        private CommandHandler commandHandler;

        private IServiceCollection services;
        private ServiceProvider serviceProvider;
        private Mock<IAvailableCommandsService> availableCommandsServiceMock;

        [SetUp]
        public void Setup()
        {
            services = new ServiceCollection();
            serviceProvider = services.BuildServiceProvider();
            availableCommandsServiceMock = new();

            commandHandler = new(serviceProvider, availableCommandsServiceMock.Object);
        }

        [Test]
        public async Task ShouldNotExecuteAnythingOnGeneralMessage()
        {
            Type type;
            string respondedMessage = null;

            availableCommandsServiceMock
                .Setup(m => m.TryParseCommandType(It.IsAny<string>(), out type))
                .Returns(value: false);

            await commandHandler.ExecuteCommandAsync(
                MessageHelper.CreateMessage(text: "general message"),
                (chat, message) =>
                {
                    respondedMessage = message;
                    return Task.CompletedTask;
                });

            Assert.Null(respondedMessage);
        }

        [Test]
        public async Task ExecuteCorrectCommand()
        {
            string respondedMessage = null;
            const string response = "response";

            Type type = typeof(CommandMock);
            availableCommandsServiceMock
                .Setup(m => m.TryParseCommandType(It.IsAny<string>(), out type))
                .Returns(value: true);

            CommandMock commandMock = new(null, response);
            services.AddTransient<CommandMock>(p => commandMock);

            // Must create a new CommandHandler
            commandHandler = new(services.BuildServiceProvider(), availableCommandsServiceMock.Object);

            await commandHandler.ExecuteCommandAsync(
                MessageHelper.CreateCommandMessage(commandMock),
                (chat, message) =>
                {
                    respondedMessage = message;
                    return Task.CompletedTask;
                });

            Assert.AreEqual(response, respondedMessage);
        }
    }
}
