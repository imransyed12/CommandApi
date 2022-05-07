using Xunit;
using CommandAPI.Models;
using System;

namespace CommandAPI.Tests
{
    public class CommandTests : IDisposable
    {
        Command testCommand;

        public CommandTests()
        {
            testCommand = new Command
            {
            HowTo = "Do something",
            Platform = "Some platform",
            CommandLine = "Some commandline"
            };
            
        }

        [Fact]
        public void CanChangeHowTo()
        {
            //Arrange
            var testCommand = new Command
            {
            HowTo = "Do something awesome",
            Platform = "xUnit",
            CommandLine = "dotnet test"
            };
            //Act
            testCommand.HowTo = "Execute Unit Tests";

            //Assert
            Assert.Equal("Execute Unit Tests", testCommand.HowTo);
        }

        [Fact]
        public void CanChangePlatform()
        {
        //Arrange
        //Act
        testCommand.Platform = "xUnit";
        //Assert
        Assert.Equal("xUnit", testCommand.Platform);
        }

        [Fact]
        public void CanChangeCommandLine()
        {
        //Arrange
        //Act
        testCommand.CommandLine = "dotnet test";
        //Assert
        Assert.Equal("dotnet test", testCommand.CommandLine);
        }

        public void Dispose()
        {
            testCommand = null;
        }
    }
}