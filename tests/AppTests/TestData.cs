using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Domain.Services;
using Domain.ValueObjects;
using Moq;

namespace AppTests;

public static class TestData
{
	public static User Admin()
		=> new("admin", Email.From("admin@mail.com"), PasswordHasher.Hash("password"), UserRole.Admin);

	public static User Manager()
		=> new("manager", Email.From("manager@mail.com"), PasswordHasher.Hash("password"), UserRole.Manager);

	public static User Employee()
		=> new("employee", Email.From("employee@mail.com"), PasswordHasher.Hash("password"), UserRole.Employee);

	public static WorkTask Task(string name = "task")
		=> new(name, null);
}