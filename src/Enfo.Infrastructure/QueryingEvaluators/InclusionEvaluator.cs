﻿using System.Linq;
using Enfo.Repository.Querying;
using Microsoft.EntityFrameworkCore;

namespace Enfo.Infrastructure.QueryingEvaluators
{
    public static class InclusionEvaluator
    {
        internal static IQueryable<T> Apply<T>(
            this IQueryable<T> query,
            IInclusion<T> inclusion)
            where T : class
        {
            if (inclusion != null)
            {
                // Includes all expression-based includes
                query = inclusion.Includes.Aggregate(query, (current, include) =>
                    current.Include(include));

                // Include any string-based include statements
                query = inclusion.IncludeStrings.Aggregate(query, (current, include) =>
                    current.Include(include));
            }

            return query;
        }
    }
}
