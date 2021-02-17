using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Enfo.Repository.Querying
{
    public interface ISorting<T>
        where T : class
    {
        List<Ordering<T>> OrderBy { get; }

        //Expression<Func<T, object>> GroupBy { get; }
    }

    public class Ordering<T>
    {
        public Expression<Func<T, object>> OrderByExpression { get; set; }
        public SortDirection SortDirection { get; set; }
    }

    public enum SortDirection
    {
        Ascending,
        Descending
    }
}
