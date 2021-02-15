using System.Collections.Generic;

namespace TR.SystemOfLegalCases.Domain
{
    public class PagedList<TEntity> where TEntity : class
    {
        public PagedList(IEnumerable<TEntity> listReturn, int currentPage, int totalPages, int sizePage, int totalItems)
        {
            CurrentPage = currentPage;
            TotalPages = totalPages;
            SizePage = sizePage;
            TotalItems = totalItems;
            ListReturn = listReturn;
        }

        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int SizePage { get; private set; }
        public int TotalItems { get; private set; }
        public IEnumerable<TEntity> ListReturn { get; private set; }
    }
}
