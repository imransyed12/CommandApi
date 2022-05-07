using System;
using Xunit;
using CommandAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Moq;
using AutoMapper;
using CommandAPI.Models;
using CommandAPI.Dtos;
using CommandAPI.Data;
using CommandAPI.Profiles;

namespace CommandAPI.Tests
{
    public class CommandsControllerTests : IDisposable
    {

        Mock<ICommandAPIRepo> mockRepo;
        CommandsProfile realProfile;
        MapperConfiguration configuration;
        IMapper mapper;

        public CommandsControllerTests()
        {
            mockRepo = new Mock<ICommandAPIRepo>();
            realProfile = new CommandsProfile();
            configuration = new MapperConfiguration(cfg => cfg.
            AddProfile(realProfile));
            mapper = new Mapper(configuration);
        }

        [Fact]
        public void GetCommandItems_Returns200OK_WhenDBIsEmpty()
        {
        mockRepo.Setup(repo =>
        repo.GetAllCommands()).Returns(GetCommands(0));
        
        var controller = new CommandsController(mockRepo.Object, mapper);

        // Act
        var result = controller.GetAllCommands();

        //Assert 

        Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetAllCommands_Returns200OK_WhenDBHasOneResource()
        {
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));

            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act

            var result = controller.GetAllCommands();

            var okResult = result.Result as OkObjectResult;
            var commands = okResult.Value as List<CommandReadDto>;

            Assert.Single(commands);

        }

        [Fact]
        public void GetAllCommands_ReturnCorrectTypeWhenDBHasOneResource()
        {
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));

            var controller = new CommandsController(mockRepo.Object, mapper);

            var result = controller.GetAllCommands();

            Assert.IsType<ActionResult<IEnumerable<CommandReadDto>>>(result);
        }

        [Fact]
        public void GetCommandByID_Returns404NotFound_WhenNonExistentIDProvided()
        {
            // Arrange
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(()=>null);

            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act 
            var result = controller.GetCommandById(1);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);

        }

        [Fact]
        public void GetCommandByID_Returns200OK_WhenValidIdProvided()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command{ Id = 1, HowTo="MockTest", Platform="Mock Platform", CommandLine="Mock line" });

            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.GetCommandById(1);

            //Assert

            Assert.IsType<OkObjectResult>(result.Result);

        }

        [Fact]
        public void GetCommandByID_ReturnsCorrectResultType_WhenValidIdProvided()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command{ Id = 1, HowTo="MockTest", Platform="Mock Platform", CommandLine="Mock line" });

            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.GetCommandById(1);

            //Assert
            Assert.IsType<ActionResult<CommandReadDto>>(result);
        }

//         [Fact]
// public void CreateCommand_ReturnsCorrectResourceType_WhenValidObjectSubmitted()
// {
//         //Arrange
//         mockRepo.Setup(repo =>
//         repo.GetCommandById(1)).Returns(new Command { Id = 1,
//         HowTo = "mock",
//         Platform = "Mock",
//         CommandLine = "Mock" });
//         var controller = new CommandsController(mockRepo.Object, mapper);
//         //Act
//         var result = controller.CreateCommand(new CommandCreateDto { });
//         //Assert
//         Assert.IsType<ActionResult<CommandReadDto>>(result);
// }

// [Fact]
// public void CreateCommand_Returns201Created_WhenValidObjectSubmitted()
// {
//     //Arrange
//     mockRepo.Setup(repo => repo.CreateCommand()).Returns(new Command { Id = 1,
//     HowTo = "mock",
//     Platform = "Mock",
//     CommandLine = "Mock" });
//     var controller = new CommandsController(mockRepo.Object, mapper);
//     //Act
//     var result = controller.CreateCommand(new CommandCreateDto { });
//     //Assert
//     Assert.IsType<CreatedAtRouteResult>(result);
// }

[Fact]
public void UpdateCommand_Returns204NoContent_WhenValidObjectSubmitted()
{
    //Arrange
    mockRepo.Setup(repo =>
    repo.GetCommandById(1)).Returns(new Command { Id = 1,
    HowTo = "mock",
    Platform = "Mock",
    CommandLine = "Mock" });
    var controller = new CommandsController(mockRepo.Object, mapper);
    //Act
    var result = controller.UpdateCommand(1, new CommandUpdateDto { });
    //Assert
    Assert.IsType<NoContentResult>(result);
}

[Fact]
public void UpdateCommand_Returns404NotFound_WhenNonExistentResourceIDSubmitted()
{
    //Arrange
    mockRepo.Setup(repo =>
    repo.GetCommandById(0)).Returns(() => null);
    var controller = new CommandsController(mockRepo.Object, mapper);
    //Act
    var result = controller.UpdateCommand(0, new CommandUpdateDto { });
    //Assert
    Assert.IsType<NotFoundResult>(result);
}

public void Dispose()
{
    mockRepo = null;
    mapper = null;
    configuration = null;
    realProfile = null;
}

        private List<Command> GetCommands(int num)
        {
            var commands = new List<Command>();
            if (num > 0){
            commands.Add(new Command
              {
            Id = 0,
            HowTo = "How to generate a migration",
            CommandLine = "dotnet ef migrations add <Name of Migration>",
            Platform = ".Net Core EF"
            });
            }
            return commands;
        }
    }
}