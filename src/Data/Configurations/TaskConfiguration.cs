using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;

class TaskConfiguration : IEntityTypeConfiguration<MyTask>
{
	public void Configure(EntityTypeBuilder<MyTask> builder)
	{
		builder.HasKey(t => t.Id);

		builder.HasIndex(t => t.Name)
			.IsUnique();
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

		builder.Navigation(t => t.Users)
			.HasField("_users")
			.UsePropertyAccessMode(PropertyAccessMode.Field);

		builder.HasMany(t => t.Users)
			.WithMany(u => u.Tasks)
			.UsingEntity(j => j.ToTable("TaskUsers"));

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