using App.Excepions;
using App.Interfaces;
using App.Services;
using Moq;

namespace AppTests;

public class WorkTaskServiceNegativeTests
{
	[Fact]
	public async Task CreateAsync_Throws_WhenNotManager()
	{
		var employee = TestData.Employee();

		var repo = new Mock<IUserRepository>();
		repo.Setup(x => x.GetByIdAsync(employee.Id))
			.ReturnsAsync(employee);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(repo.Object);

		var service = new WorkTaskService(uow.Object);

		await Assert.ThrowsAsync<AccessException>(() =>
			service.CreateAsync(employee.Id, "task", null));
	}
}