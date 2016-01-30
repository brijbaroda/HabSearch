using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Sitecore.Foundation.HabSearch.Indexing.Infrastructure
{
    public abstract class IndexSearchBase
    {
        protected IEnumerable<T> SearchIndex<T>(ISearchIndex searchIndex, Func<IQueryable<T>, IQueryable<T>> queryClauses)
        {
            if (searchIndex == null)
            {
                Log.Error("searchIndex is null, unable to perform search", this);
                return Enumerable.Empty<T>();
            }

            try
            {
                using (var searchContext = searchIndex.CreateSearchContext())
                {
                    SearchResults<T> results;

                    var query = searchContext.GetQueryable<T>();
                    if (query == null)
                    {
                        Log.Warn("Queryable is null using: " + searchIndex.Name, this);
                        return Enumerable.Empty<T>();
                    }
                    var queryClausesResults = queryClauses.Invoke(query);

                    results = queryClausesResults.GetResults();
                    if (results == null || results.Hits == null)
                    {
                        Log.Warn("Results are invalid for query using: " + searchIndex.Name, this);
                        return Enumerable.Empty<T>();
                    }

                    //Enumerate so we can have what we need from the context
                    return results.Hits.Select(x => x.Document).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error while processing search using: " + searchIndex.Name, ex, this);
                return Enumerable.Empty<T>();
            }
        }
    }
}

