using System.Linq;


namespace PayCloud.Services.Utils.Extensions
{
    public static class IQueryableSorterExtension
    {
        public static IQueryable<T> Sort<T>(this IQueryable<T> query, string sortOrder)
        {

            //=====sorting====
            //Stakata: sortOder must match property name (case sens.)

            bool descending = false;
            if (sortOrder.EndsWith("_desc"))
            {
                sortOrder = sortOrder.Substring(0, sortOrder.Length - 5);
                descending = true;
            }

            if (descending)
            {
                query = query.OrderByDescending(sortOrder);
            }
            else
            {
                query = query.OrderBy(sortOrder);
            }

            //======end of sorting========

            return query;
        }
    }
}
