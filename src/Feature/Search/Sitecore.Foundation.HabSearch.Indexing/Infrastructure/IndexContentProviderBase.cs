﻿using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Linq.Expressions;
using Sitecore.Data;
using Sitecore.Foundation.HabSearch.Indexing.Models;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.ContentSearch.Linq.Utilities;

namespace Sitecore.Feature.HabSearch.Infrastructure
{
    public abstract class IndexContentProviderBase : ProviderBase
    {
        public abstract string ContentType { get; }
        public abstract IEnumerable<ID> SupportedTemplates { get; }
        public abstract Expression<Func<SearchResultItem, bool>> GetQueryPredicate(IQuery query);
        public abstract void FormatResult(SearchResultItem item, ISearchResult formattedResult);

        protected Expression<Func<SearchResultItem, bool>> GetFreeTextPredicate(string[] fieldNames, IQuery query)
        {
            var predicate = PredicateBuilder.False<SearchResultItem>();
            if (string.IsNullOrWhiteSpace(query.QueryText))
            {
                return predicate;
            }
            return fieldNames.Aggregate(predicate, (current, fieldName) => current.Or(i => i[fieldName].Contains(query.QueryText)));
        }
    }
}