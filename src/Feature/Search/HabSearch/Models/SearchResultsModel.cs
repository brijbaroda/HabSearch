using Sitecore.Foundation.HabSearch.Indexing.Infrastructure;
using Sitecore.Foundation.HabSearch.Indexing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Feature.HabSearch.Models
{
    public class SearchResultsModel
    {
        public string Keyword { get; set; }
        public string QueryStringKey { get; set; }
        public string AudienceFacet { get; set; }
        public string AudienceFacetQueryStringKey { get; set; }
        public string PageUrl { get; set; }
        //public int PageSize { get; set; }
        private IEnumerable<SiteSearchProvider.SiteSearchResultItem> _searchResults;
        public IEnumerable<SiteSearchProvider.SiteSearchResultItem> SearchResults
        {
            get
            {
                return _searchResults ?? Enumerable.Empty<SiteSearchProvider.SiteSearchResultItem>();

            }
            set
            {
                _searchResults = value;
            }
        }
        public Dictionary<string, string> AudienceFacets { get; set; }
        public string AllResultsCount { get; set; }
        public string ResultsCount { get; set; }
        //this might work?
        public int StartResultNumber { get; set; }
        public int EndResultNumber { get; set; }
        public SearchResultsModel()
        {
            //PageSize = 20;
        }
        public string SelectedFacetCount { get; set; }
        //public PagedListRenderOptions MobileOptions
        //{
        //    get
        //    {
        //        return new PagedListRenderOptions
        //        {
        //            DisplayLinkToFirstPage = PagedListDisplayMode.Never,
        //            DisplayLinkToLastPage = PagedListDisplayMode.Never,
        //            DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
        //            DisplayLinkToNextPage = PagedListDisplayMode.Always,
        //            UlElementClasses = new[] { "pager visible-xs" },
        //            DisplayLinkToIndividualPages = false,
        //            LinkToPreviousPageFormat = "&larr; Prev",
        //            LinkToNextPageFormat = "Next &rarr;"
        //        };
        //    }
        //}
        //public PagedListRenderOptions DesktopOptions
        //{
        //    get
        //    {
        //        return new PagedListRenderOptions
        //        {
        //            UlElementClasses = new[] { "pagination hidden-xs" },
        //            DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
        //            DisplayLinkToNextPage = PagedListDisplayMode.Always,
        //            LinkToPreviousPageFormat = "&laquo;",
        //            LinkToNextPageFormat = "&raquo;"
        //        };
        //    }
        //}
    }
}
