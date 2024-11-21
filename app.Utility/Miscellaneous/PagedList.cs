using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Utility.Miscellaneous
{
    public class PagedList
    {
        public PagedList()
        {

        }
        public int PageIndex { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public int TotalPages { get; }
        public int PreviousPage { get; }
        public int NextPage { get; }
        public PagedList(int totalCount, int page, int pageSize = 20)
        {
            int totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)pageSize);
            int currentPage = page;
            int startPage = currentPage - 5;
            int endPage = currentPage + 4;
            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            TotalCount = totalCount;
            PageIndex = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            PreviousPage = startPage;
            NextPage = endPage;

        }
    }
}
