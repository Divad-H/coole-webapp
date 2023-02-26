using CooleWebapp.Core.Utilities;

namespace CooleWebapp.Database.Pagination;

internal class QueryPaginated : IQueryPaginated
{
  public Task<Paginated<T>> Execute<T>(Page page, IQueryable<T> query, CancellationToken ct)
  {
    return query.Paginated(page, ct);
  }
}
