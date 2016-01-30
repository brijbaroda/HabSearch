﻿using Sitecore.ContentSearch.SearchTypes;
using System.Collections.Generic;
using System.Linq;
using Sitecore.ContentSearch.Linq;
namespace Sitecore.Foundation.HabSearch.Indexing.Models
{
    public class SearchResultsRepository
    {
        public static ISearchResults Create(SearchResults<SearchResultItem> results, IQuery query)
        {
            var searchResults = CreateSearchResults(results);
            return new SearchResults
            {
                Results = searchResults,
                TotalNumberOfResults = results.TotalSearchResults,
                Query = query
            };
        }

        private static IEnumerable<ISearchResult> CreateSearchResults(SearchResults<SearchResultItem> results)
        {
            return results.Hits.Select(h => SearchResultRepository.Create(h.Document)).ToArray();
        }
    }
}
