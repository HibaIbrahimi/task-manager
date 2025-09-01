using FluentValidation;
using System.ComponentModel.DataAnnotations;
using Moq;
using TaskManager.Application.Services;
using TaskManager.Domaine.Abstractions;
using TaskManager.Domaine.Entities;
using TaskManager.Domaine.ValueObjects;

namespace TaskManagerTests.ApplicationTests
{
    [TestFixture]
    public class TaskServiceTests
    {
        private Mock<ITaskRepository> _repositoryMock;
        private Mock<IValidator<TaskItem>> _validator;
        private TaskService _service;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<ITaskRepository>();
            _validator = new Mock<IValidator<TaskItem>>();
            _service = new TaskService(_repositoryMock.Object, _validator.Object);
        }

        [Test]
        public async Task GetAllAsync_Should_Return_All_Tasks()
        {
            // Arrange
            var tasks = new List<TaskItem>
            {
                new TaskItem(new TaskTitle("Task 1")),
                new TaskItem(new TaskTitle("Task 2"))
            };
            _repositoryMock.Setup(r => r.ListAsync(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(tasks);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Title, Is.EqualTo("Task 1"));
        }

        [Test]
        public async Task CreateAsync_Should_Add_Task_And_Return_Id()
        {
            // Arrange
            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<TaskItem>(), default))
                           .Returns(Task.CompletedTask);
            _repositoryMock.Setup(r => r.SaveChangesAsync(default))
                           .ReturnsAsync(1);
            _validator.Setup(r => r.Validate(It.IsAny<TaskItem>()))
                           .Returns(new FluentValidation.Results.ValidationResult { 
                               Errors = new List<FluentValidation.Results.ValidationFailure>()
                           });

            var taskCommand = new TaskManager.Application.DTOs.CreateTaskCommand
            {
                Title = "New Task"
            };

            // Act
            var id = await _service.CreateAsync(taskCommand);

            // Assert
            Assert.That(id.ToString(), Is.Not.EqualTo(Guid.Empty.ToString()));
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<TaskItem>(), default), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
        }

        [Test]
        public async Task CompleteAsync_Should_Mark_Task_As_Completed()
        {
            // Arrange
            var task = new TaskItem(new TaskTitle("Task"));
            _repositoryMock.Setup(r => r.GetByIdAsync(task.Id, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(task);
            _repositoryMock.Setup(r => r.SaveChangesAsync(default))
                           .ReturnsAsync(1);

            // Act
            await _service.CompleteAsync(task.Id);

            // Assert
            Assert.That(task.IsCompleted, Is.True);
            _repositoryMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
        }
    }
}
