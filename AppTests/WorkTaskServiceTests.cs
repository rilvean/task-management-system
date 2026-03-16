using App.Services;
using Domain.Interfaces;
using Domain.Models;
using Moq;

namespace AppTests;

public class WorkTaskServiceTests
{
	[Fact]
	public async Task CreateAsync_AddsTask()
	{
		var manager = TestData.Manager();

		var userRepo = new Mock<IUserRepository>();
		userRepo.Setup(x => x.GetByIdAsync(manager.Id))
			.ReturnsAsync(manager);

		var taskRepo = new Mock<IWorkTaskRepository>();

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(userRepo.Object);
		uow.SetupGet(x => x.WorkTaskRepository).Returns(taskRepo.Object);

		var service = new WorkTaskService(uow.Object);

		await service.CreateAsync(manager.Id, "task", null);

		taskRepo.Verify(x => x.AddAsync(It.IsAny<WorkTask>()), Times.Once);
	}
}