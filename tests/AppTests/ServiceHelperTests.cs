using App;
using App.Interfaces;
using Domain.Enums;
using Moq;

namespace AppTests;

public class ServiceHelperTests
{
	[Fact]
	public async Task EnsureUserExistAsync_ReturnsUser()
	{
		var user = TestData.Admin();

		var repo = new Mock<IUserRepository>();
		repo.Setup(x => x.GetByIdAsync(user.Id))
			.ReturnsAsync(user);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(repo.Object);

		var helper = new ServiceHelper(uow.Object);

		var result = await helper.EnsureUserExistAsync(user.Id);

		Assert.Equal(user, result);
	}

	[Fact]
	public async Task EnsureTaskExistAsync_ReturnsTask()
	{
		var task = TestData.Task();

		var repo = new Mock<IWorkTaskRepository>();
		repo.Setup(x => x.GetByIdAsync(task.Id))
			.ReturnsAsync(task);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.WorkTaskRepository).Returns(repo.Object);

		var helper = new ServiceHelper(uow.Object);

		var result = await helper.EnsureTaskExistAsync(task.Id);

		Assert.Equal(task, result);
	}
}