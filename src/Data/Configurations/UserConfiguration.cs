using Domain.Models;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;

class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.HasKey(u => u.Id);

		builder.HasIndex(u => u.Name);
		builder.Property(u => u.Name)
			.HasField("_name")
			.UsePropertyAccessMode(PropertyAccessMode.Property)
			.IsRequired()
			.HasMaxLength(50);

		builder.HasIndex(u => u.Email)
			.IsUnique();
		builder.Property(u => u.Email)
			.HasField("_email")
			.UsePropertyAccessMode(PropertyAccessMode.Property)
			.HasConversion(
				e => e.Value,
				v => Email.From(v)
			)
			.IsRequired()
			.HasMaxLength(50);

		builder.Property(u => u.PasswordHash)
			.HasField("_password")
			.UsePropertyAccessMode(PropertyAccessMode.Property)
			.HasConversion(
				p => p.Value,
				v => PasswordHash.From(v)
			)
			.IsRequired()
			.HasMaxLength(100);

		builder.HasIndex(u => u.Role);
		builder.Property(u => u.Role)
			.HasConversion<string>()
			.IsRequired()
			.HasMaxLength(20);

		builder.Navigation(u => u.Tasks)
			.HasField("_tasks")
			.UsePropertyAccessMode(PropertyAccessMode.Field);

		builder.Property<DateTimeOffset>("CreatedAt")
			.IsRequired()
			.HasColumnType("datetimeoffset")
			.HasPrecision(0);
		builder.Property<DateTimeOffset>("UpdatedAt")
			.IsRequired()
			.HasColumnType("datetimeoffset")
			.HasPrecision(0);
	}
}