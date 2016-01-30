using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace Sitecore.Foundation.HabSearch.Indexing.Infrastructure
{
    public interface ISiteSearchProvider
    {
        List<FacetCategory> GetFacets(string keyword, string facetFieldName, out string totalCount);
        IEnumerable<SiteSearchProvider.SiteSearchResultItem> Search(string keyword);
        IEnumerable<SiteSearchProvider.SiteSearchResultItem> Search(string keyword, string audiencefacetshortid);        
    }

    public class SiteSearchProvider : IndexSearchBase, ISiteSearchProvider
    {
        protected bool IsPageModeNormal { get; set; }
        private string IndexName = "sitecore_web_index";
        public SiteSearchProvider(bool isPageModeNormal = true)
        {
            IsPageModeNormal = isPageModeNormal;
        }

        #region Private Methods  

        private Expression<Func<SiteSearchResultItem, bool>> BuildWholeExpressionPredicate(string searchTerm)
        {
            var wholeExpression = PredicateBuilder.True<SiteSearchResultItem>();

            if (string.IsNullOrEmpty(searchTerm)) return wholeExpression;
            //Ensure latest version and searchable
            //If not it will search all content that do not have IsSearchable field
            //wholeExpression = wholeExpression.And(x => x.IsSearchable && x.IsLatestVersion);

            var searchExpression = PredicateBuilder.False<SiteSearchResultItem>();
            searchExpression = searchExpression.Or(x => x.Content.Like(searchTerm) || x.PresentationContent.Like(searchTerm));
            wholeExpression = wholeExpression.Or(wholeExpression);

            return wholeExpression;
        }

        private Expression<Func<SiteSearchResultItem, bool>> BuildSplitSearchTermPredicate(string searchTerm)
        {
            var splitSearchTermPredicate = PredicateBuilder.True<SiteSearchResultItem>();
            if (string.IsNullOrEmpty(searchTerm)) return splitSearchTermPredicate;

            //search each word
            foreach (var t in searchTerm.Split(' '))
            {
                var eachWordExpression = PredicateBuilder.False<SiteSearchResultItem>();
                var tempTerm = t.Trim();
                if (!string.IsNullOrWhiteSpace(tempTerm))
                {
                    //like is a fuzzy match, contains is an exact match
                    eachWordExpression = eachWordExpression.Or(x => x.Content.Like(tempTerm) || x.PresentationContent.Like(tempTerm));
                    splitSearchTermPredicate = splitSearchTermPredicate.And(eachWordExpression);
                }
            }
            return splitSearchTermPredicate;
        }

        private ISearchIndex GetIndex()
        {
            ISearchIndex index = ContentSearchManager.GetIndex(IndexName);
            if (index == null)
            {
                Log.Error(string.Format("Couldn't find {0} index", IndexName), this);
            }
            return index;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// GetAudienceFacets
        /// </summary>
        /// <param name="keyword"></param>
        /// <param out name="totalCount"></param>
        /// <returns></returns>
        public List<FacetCategory> GetFacets(string keyword, string facetFieldName, out string totalCount)
        {
            List<FacetCategory> categories = null;
            totalCount = "0";
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return null;
            }
            try
            {
                ISearchIndex index = GetIndex();
                if (null != index)
                {
                    var query = PredicateBuilder.True<SiteSearchResultItem>();
                    //get searchable and latest version only 
                    query = query.And<SiteSearchResultItem>(x => x.IsSearchable && x.IsLatestVersion);
                    //split the keyword with spaces
                    query = query.And(BuildSplitSearchTermPredicate(keyword));
                    
                    using (IProviderSearchContext context = index.CreateSearchContext())
                    {
                        //facet on a field
                        IQueryable<SiteSearchResultItem> source = context.GetQueryable<SiteSearchResultItem>()
                            .Where(query).FacetOn(f => f[facetFieldName]);
                        totalCount = source.Count().ToString();
                        categories = source.GetFacets().Categories;
                    }
                }

            }
            catch (Exception exception)
            {
                Log.Error("An Error occoured while getting facets", exception, this);
                categories = null;
            }
            return categories;
        }

        public IEnumerable<SiteSearchResultItem> Search(string keyword)
        {
            try
            {
                return this.Search(keyword, string.Empty);
            }
            catch (Exception exception)
            {
                Log.Error("An Error occoured during a search for: " + keyword, exception, this);
                return Enumerable.Empty<SiteSearchResultItem>();
            }
        }

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public IEnumerable<SiteSearchResultItem> Search(string keyword, string audiencefacetshortid)
        {
            if (String.IsNullOrWhiteSpace(keyword)) { return Enumerable.Empty<SiteSearchResultItem>(); }

            try
            {
                var index = GetIndex();

                if (index == null)
                {
                    return Enumerable.Empty<SiteSearchResultItem>();
                }

                //Ensure latest version and searchable
                var query = PredicateBuilder.True<SiteSearchResultItem>();
                query = query.And(x => x.IsSearchable && x.IsLatestVersion);

                query = query.And(BuildWholeExpressionPredicate(keyword));
                //split the keyword with spaces
                query = query.And(BuildSplitSearchTermPredicate(keyword));
                
                if (!string.IsNullOrEmpty(audiencefacetshortid))
                {
                    query = query.And(x => x.SiteSectionFacet == audiencefacetshortid);
                }

                //execute search
                var searchResultItems = SearchIndex<SiteSearchResultItem>(index, (q) =>
                {
                    var filteredResults = q.Where(query);
                    return filteredResults;
                });
                return searchResultItems;

            }
            catch (Exception ex)
            {
                Log.Error("An Error occoured during a search for: " + keyword, ex, this);
                return Enumerable.Empty<SiteSearchResultItem>();
            }
        }

        
        #endregion

        public class SiteSearchResultItem : SearchResultItem
        {
            [IndexField("has_presentation")]
            public virtual bool IsSearchable { get; set; }

            [IndexField("_latestversion")]
            public virtual bool IsLatestVersion { get; set; }

            [IndexField("presentationcontent")]
            public virtual string PresentationContent { get; set; }

            [IndexField("_name")]
            public virtual string Title { get; set; }
            [IndexField("contenttype")]
            public virtual string ContentType { get; set; }
            [IndexField("description")]
            public virtual string Description { get; set; }

            [IndexField("site_section_facet")]
            public virtual string SiteSectionFacet { get; set; }

        }
    }
}
