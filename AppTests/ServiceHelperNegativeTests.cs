using App;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Moq;

namespace AppTests;

public class ServiceHelperNegativeTests
{
	[Fact]
	public async Task EnsureAccessAsync_Throws_WhenRoleInvalid()
	{
		var user = TestData.Employee();

		var repo = new Mock<IUserRepository>();
		repo.Setup(x => x.GetByIdAsync(user.Id))
			.ReturnsAsync(user);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(repo.Object);

		var helper = new ServiceHelper(uow.Object);

		await Assert.ThrowsAsync<AccessException>(() =>
			helper.EnsureAccessAsync(user.Id, UserRole.Admin));
	}

	[Fact]
	public async Task EnsureUserExistAsync_Throws_WhenUserNotFound()
	{
		var repo = new Mock<IUserRepository>();
		repo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
			.ReturnsAsync((User?)null);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(repo.Object);

		var helper = new ServiceHelper(uow.Object);

		await Assert.ThrowsAsync<NotFoundException>(() =>
			helper.EnsureUserExistAsync(Guid.NewGuid()));
	}

	[Fact]
	public async Task EnsureTaskExistAsync_Throws_WhenTaskNotFound()
	{
		var repo = new Mock<IWorkTaskRepository>();
		repo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
			.ReturnsAsync((WorkTask?)null);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.WorkTaskRepository).Returns(repo.Object);

		var helper = new ServiceHelper(uow.Object);

		await Assert.ThrowsAsync<NotFoundException>(() =>
			helper.EnsureTaskExistAsync(Guid.NewGuid()));
	}
}