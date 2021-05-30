using Lastgram.Commands;
using LastgramTest.Commands.Mocks;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LastgramTest.Commands
{
    [TestFixture]
    public class AvailableCommandsServiceTest
    {
        private AvailableCommandsService availableCommandsService;

        private List<ICommand> commands;

        [SetUp]
        public void Setup()
        {
            commands = new();

            availableCommandsService = new(commands);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public void ShouldReturnAllCommands(int count)
        {
            for(int i = 0; i < count; i++)
            {
                commands.Add(new CommandMock());
            }

            int availableCommandsCount = availableCommandsService.GetBotCommands().Count;

            Assert.AreEqual(count, availableCommandsCount);
        }

        [TestCase("/np@bot", typeof(NowPlayingCommand))]
        [TestCase("/np", typeof(NowPlayingCommand))]
        [TestCase("/NP", typeof(NowPlayingCommand))]
        [TestCase("/NP@bot", typeof(NowPlayingCommand))]
        [TestCase("/forgetme", typeof(ForgetMeCommand))]
        [TestCase("/fOrgEtMe", typeof(ForgetMeCommand))]
        [TestCase("/setusername", typeof(SetLastfmUsernameCommand))]
        [TestCase("/toptracks", typeof(TopTracksCommand))]
        [TestCase("/nonexisting", null, false)]
        [TestCase("/nonexisting@bot", null, false)]
        [TestCase("/", null, false)]
        [TestCase("", null, false)]
        [TestCase(null, null, false)]
        public void ShouldParseCorrectCommandType(string text, Type correctType, bool isValid = true)
        {
            commands.Add(new NowPlayingCommand(null, null, null));
            commands.Add(new ForgetMeCommand(null));
            commands.Add(new SetLastfmUsernameCommand(null));
            commands.Add(new TopTracksCommand(null, null, null));

            if (availableCommandsService.TryParseCommandType(text, out Type type) != isValid)
            {
                Assert.Fail();
                return;
            }

            Assert.AreEqual(correctType, type);
        }
    }
}
