using System.Linq;
using DCTMRestAPI.Models;
using DCTMRestAPI.Types;
using Xunit;

namespace DCTMRestAPI.UnitTests
{
    public class PagedListTests
    {
        private static IQueryable<int> Source(int count) => Enumerable.Range(1, count).AsQueryable();

        [Fact]
        public void Computes_page_slice_and_totals()
        {
            var page = new PagedList<int>(Source(25), pageNumber: 2, pageSize: 10);

            Assert.Equal(25, page.TotalItems);
            Assert.Equal(3, page.TotalPages);
            Assert.Equal(new[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, page.List);
        }

        [Fact]
        public void First_page_has_next_but_no_previous()
        {
            var page = new PagedList<int>(Source(25), 1, 10);

            Assert.False(page.HasPreviousPage);
            Assert.True(page.HasNextPage);
            Assert.Equal(2, page.NextPageNumber);
            Assert.Equal(1, page.PreviousPageNumber);
        }

        [Fact]
        public void Last_page_has_previous_but_no_next()
        {
            var page = new PagedList<int>(Source(25), 3, 10);

            Assert.True(page.HasPreviousPage);
            Assert.False(page.HasNextPage);
            Assert.Equal(3, page.NextPageNumber);   // clamped to TotalPages
            Assert.Equal(2, page.PreviousPageNumber);
        }

        [Fact]
        public void GetHeader_projects_paging_metadata()
        {
            var header = new PagedList<int>(Source(25), 2, 10).GetHeader();

            Assert.Equal(25, header.TotalItems);
            Assert.Equal(2, header.PageNumber);
            Assert.Equal(10, header.PageSize);
            Assert.Equal(3, header.TotalPages);
        }
    }

    public class PagingHeaderTests
    {
        [Fact]
        public void ToJson_uses_camelCase()
        {
            var json = new PagingHeader(25, 2, 10, 3).ToJson();

            Assert.Contains("\"totalItems\":25", json);
            Assert.Contains("\"pageNumber\":2", json);
            Assert.DoesNotContain("TotalItems", json);
        }

        [Fact]
        public void Records_with_same_values_are_equal()
        {
            Assert.Equal(new PagingHeader(1, 2, 3, 4), new PagingHeader(1, 2, 3, 4));
            Assert.NotEqual(new PagingHeader(1, 2, 3, 4), new PagingHeader(9, 2, 3, 4));
        }
    }
}
