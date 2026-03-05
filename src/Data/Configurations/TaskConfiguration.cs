using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;

class TaskConfiguration : IEntityTypeConfiguration<MyTask>
{
	public void Configure(EntityTypeBuilder<MyTask> builder)
	{
		builder.HasKey(t => t.Id);

		builder.HasIndex(t => t.Name);
		builder.Property(t => t.Name)
			.HasField("_name")
			.UsePropertyAccessMode(PropertyAccessMode.Property)
			.IsRequired()
			.HasMaxLength(999);

		builder.Property(t => t.Description)
			.HasMaxLength(999);

		builder.HasIndex(t => t.Deadline);
		builder.Property(t => t.Deadline)
			.HasColumnType("datetime2");

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

		builder.Property<DateTime>("CreatedAt")
			.IsRequired();
		builder.Property<DateTime>("UpdatedAt")
			.IsRequired();
	}
}