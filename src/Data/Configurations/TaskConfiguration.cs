using Domain.Models;
using Domain.Models.Submodels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;

class TaskConfiguration : IEntityTypeConfiguration<WorkTask>
{
	public void Configure(EntityTypeBuilder<WorkTask> builder)
	{
		builder.HasKey(t => t.Id);

		builder.HasIndex(t => t.Name).IsUnique();
		builder.Property(t => t.Name)
			.HasField("_name")
			.UsePropertyAccessMode(PropertyAccessMode.Property)
			.IsRequired()
			.HasMaxLength(400);

		builder.Property(t => t.Description)
			.HasMaxLength(2000);

		builder.HasIndex(t => t.Deadline);
		builder.Property(t => t.Deadline)
			.HasColumnType("datetimeoffset")
			.HasPrecision(0);

		builder.HasIndex(t => t.Priority);
		builder.Property(t => t.Priority)
			.HasConversion<string>()
			.IsRequired()
			.HasMaxLength(20);

		builder.HasIndex(t => t.Status);
		builder.Property(t => t.Status)
			.HasConversion<string>()
			.IsRequired()
			.HasMaxLength(20);

		builder.HasMany<Assignment>("_assignments")
			.WithOne(a => a.Task)
			.HasForeignKey(a => a.TaskId);

		builder.Navigation("_assignments")
			.UsePropertyAccessMode(PropertyAccessMode.Field);

		builder.Ignore(t => t.Users);

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