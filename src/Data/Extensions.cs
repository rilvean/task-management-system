using Domain.Models;
using Domain.Models.Submodels;
using Microsoft.EntityFrameworkCore;

namespace Data;

static class Extensions
{
	public static IQueryable<WorkTask> IncludeAssignments(this IQueryable<WorkTask> query)
		=> query
			.Include(x => EF.Property<IEnumerable<Assignment>>(x, "_assignments"))
			.ThenInclude(a => a.User);

	public static IQueryable<User> IncludeAssignments(this IQueryable<User> query)
		=> query
			.Include(x => EF.Property<IEnumerable<Assignment>>(x, "_assignments"))
			.ThenInclude(a => a.Task);
}