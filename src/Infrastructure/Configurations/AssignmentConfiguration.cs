using Domain.Models.Submodels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;


class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
{
	public void Configure(EntityTypeBuilder<Assignment> builder)
	{
		builder.HasKey(x => new { x.TaskId, x.UserId });

		builder.Navigation(x => x.Task)
			.UsePropertyAccessMode(PropertyAccessMode.Property);

		builder.Navigation(x => x.User)
			.UsePropertyAccessMode(PropertyAccessMode.Property);
	}
}