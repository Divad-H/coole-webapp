using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Core.Utilities;

public class Pagination
{
  public Pagination(
    UInt32 totalItems,
    Page page)
  {
    var totalPages = (int)Math.Ceiling(totalItems / (decimal)page.PageSize);
    var currentPageIndex = page.PageIndex;
    if (page.PageIndex + 1 > totalPages)
      currentPageIndex = (UInt32)Math.Max(0, totalPages - 1);
    Page = new(currentPageIndex, page.PageSize);

    var startIndex = Page.PageIndex * Page.PageSize;
    var endIndex = Math.Min(startIndex + Page.PageSize, totalItems);

    TotalItems = totalItems;
    TotalPages = (UInt32)totalPages;
    StartIndex = startIndex;
    EndIndex = endIndex;
  }

  [Required] public UInt32 TotalItems { get; }
  [Required] public Page Page { get; }
  [Required] public UInt32 TotalPages { get; }
  [Required] public UInt32 StartIndex { get; }
  [Required] public UInt32 EndIndex { get; }
}

public record Page
{
  public Page(UInt32 pageIndex, UInt32 pageSize) 
  {
    PageIndex = pageIndex;
    PageSize = Math.Max((UInt32)1, pageSize);
  }

  [Required] public UInt32 PageIndex { get; }
  [Required] public UInt32 PageSize { get; }
}
