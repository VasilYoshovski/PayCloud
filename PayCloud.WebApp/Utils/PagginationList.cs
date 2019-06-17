using System;
using System.Collections.Generic;

namespace PayCloud.WebApp.Utils
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }

        public PaginatedList(IEnumerable<T> items, int count, int pageIndex, int pageSize, bool canEdit)
        {
            this.PageIndex = pageIndex;
            this.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.PageSize = pageSize;
            this.CanEdit = canEdit;
            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (this.PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (this.PageIndex < this.TotalPages);
            }
        }

        public bool CanEdit { get; set; }

        public static PaginatedList<T> Create(IEnumerable<T> source, int count, int pageIndex, int pageSize, bool canEdit)
        {
            return new PaginatedList<T>(source, count, pageIndex, pageSize, canEdit);
        }
    }
}