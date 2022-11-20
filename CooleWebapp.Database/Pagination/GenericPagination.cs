using CooleWebapp.Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace CooleWebapp.Database.Pagination;

public static class GenericPaging
{
  public static IQueryable<T> Page<T>(this IQueryable<T> query, Page page)
  {
    if (page.PageSize == 0)
      throw new ArgumentOutOfRangeException
          (nameof(page.PageSize), "PageSize cannot be zero.");

    if (page.PageIndex != 0)
      query = query.Skip((int)(page.PageIndex * page.PageSize));

    return query.Take((int)page.PageSize);
  }

  public static async Task<Paginated<T>> Paginated<T>(
    this IQueryable<T> query, 
    Page page, 
    CancellationToken ct)
  {
    var totalItemCount = await query.CountAsync(ct);
    return new Paginated<T>(
      await Page(query, page).ToListAsync(ct),
      new Core.Utilities.Pagination((uint)totalItemCount, page));
  }
}
