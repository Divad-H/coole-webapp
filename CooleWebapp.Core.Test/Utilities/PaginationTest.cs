using CooleWebapp.Core.Utilities;

namespace CooleWebapp.Core.Test.Utilities;

[TestClass]
public class PaginationTest
{
  [TestMethod]
  public void TotalPagesAreComputedCorrectly()
  {
    Pagination pagination = new(10, new(0, 10));
    Assert.AreEqual((UInt32)1, pagination.TotalPages);

    pagination = new(11, new(0, 10));
    Assert.AreEqual((UInt32)2, pagination.TotalPages);

    pagination = new(100, new(0, 10));
    Assert.AreEqual((UInt32)10, pagination.TotalPages);

    pagination = new(0, new(0, 10));
    Assert.AreEqual((UInt32)0, pagination.TotalPages);
  }

  [TestMethod]
  public void PageIndexIsClamped()
  {
    Pagination pagination = new(10, new(1, 10));
    Assert.AreEqual((UInt32)0, pagination.Page.PageIndex);

    pagination = new(11, new(1, 10));
    Assert.AreEqual((UInt32)1, pagination.Page.PageIndex);

    pagination = new(100, new(11, 10));
    Assert.AreEqual((UInt32)9, pagination.Page.PageIndex);

    pagination = new(0, new(1, 10));
    Assert.AreEqual((UInt32)0, pagination.Page.PageIndex);
  }

  [TestMethod]
  public void StartEndIndicesAreComputedCorrectly()
  {
    Pagination pagination = new(0, new(0, 10));
    Assert.AreEqual((UInt32)0, pagination.StartIndex);
    Assert.AreEqual((UInt32)0, pagination.EndIndex);

    pagination = new(15, new(1, 10));
    Assert.AreEqual((UInt32)10, pagination.StartIndex);
    Assert.AreEqual((UInt32)15, pagination.EndIndex);

    pagination = new(100, new(5, 10));
    Assert.AreEqual((UInt32)50, pagination.StartIndex);
    Assert.AreEqual((UInt32)60, pagination.EndIndex);
  }
}
