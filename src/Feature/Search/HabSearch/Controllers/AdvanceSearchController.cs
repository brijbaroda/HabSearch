namespace Sitecore.Feature.HabSearch.Controllers
{
    using Analytics;
    using Analytics.Data;
    using Configuration;
    using ContentSearch.Linq;
    using Data;
    using Data.Items;
    using Diagnostics;
    using Foundation.HabSearch.Indexing.Infrastructure;
    using Links;
    using Models;
    using Repositories;
    using Sitecore.Foundation.HabSearch.Indexing.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class AdvanceSearchController : Controller
    {
        protected readonly string QueryStringKey = "query";
        protected readonly string FacetQueryStringKey = "f";
        protected readonly ISiteSearchProvider SearchProvider = new SiteSearchProvider();

        public ActionResult FacetSearchResults(string query)
        {
            //var siteSearchPage = SitecoreContext.GetCurrentItem<SiteSearchPage>();

            var searchTerm = query;
            var selFacet = Request.QueryString[FacetQueryStringKey];
            var model = new SearchResultsModel
            {
                QueryStringKey = QueryStringKey,
                FacetQueryStringKey = FacetQueryStringKey,
                Facet = selFacet
            };
            model.Keyword = searchTerm;
            string totalCount = "0";
            string selFacetCount = string.Empty;
            model.Facets = GetFacets(searchTerm, out totalCount);
            if (!string.IsNullOrEmpty(selFacet))
            {
                model.Facets.TryGetValue(selFacet, out selFacetCount);
            }
            model.AllResultsCount = totalCount;
            if (!string.IsNullOrEmpty(selFacetCount))
            {
                model.SelectedFacetCount = selFacetCount.Split('|')[1];
            }

            return this.View("~/Views/HabSearch/AdvanceSearchResults.cshtml", model);
        }

        public ActionResult AdvanceSearchResults(int? page)
        {
            var searchTerm = Request.QueryString[QueryStringKey];
            var model = new SearchResultsModel
            {

                QueryStringKey = QueryStringKey,
                FacetQueryStringKey = FacetQueryStringKey,

            };

            try
            {
                var selFacet = Request.QueryString[FacetQueryStringKey];
                model.Keyword = searchTerm;
                model.Facet = selFacet;

                var results = SearchProvider.Search(searchTerm);

                model.SearchResults = results;
                model.ResultsCount = results.Count().ToString();
                string totalCount = "0";
                model.Facets = GetFacets(searchTerm, out totalCount);
                model.AllResultsCount = totalCount;
                if (int.Parse(model.ResultsCount) < model.EndResultNumber)
                {
                    model.EndResultNumber = int.Parse(model.ResultsCount);
                }

                string selFacetCount = string.Empty;
                if (!string.IsNullOrEmpty(selFacet))
                {
                    model.Facets.TryGetValue(selFacet, out selFacetCount);
                }
                model.AllResultsCount = totalCount;
                if (!string.IsNullOrEmpty(selFacetCount))
                {
                    model.SelectedFacetCount = selFacetCount.Split('|')[1];
                }

                var searchResultsModel = HttpContext.Items["SearchModel"] as SearchResultsModel;
                if (searchResultsModel == null)
                    HttpContext.Items["SearchModel"] = model;

                return View("~/Views/HabSearch/AdvanceSearchResults.cshtml", model);
            }
            catch (Exception ex)
            {
                Log.Error("Something went wrong while performing a site search", ex, this);
                return View("~/Views/HabSearch/AdvanceSearchResults.cshtml");
            }
        }

        private IEnumerable<Item> GetFacetsList()
        {
            string retShortID = string.Empty;
            Item facetNavFolder = Sitecore.Context.Database.GetItem(Sitecore.Foundation.HabSearch.Indexing.Constants.Facets.FacetFolderID);
            if (facetNavFolder != null)
            {
                List<Item> lstfacetItems = new List<Item>();

                var childItems = facetNavFolder.GetChildren();
                if (childItems != null && childItems.Count() > 0)
                {
                    return childItems;
                }
            };
            return null;
        }

        public Dictionary<string, string> GetFacets(string searchTerm, out string totalCount)
        {
            Dictionary<string, string> facetsDict = new Dictionary<string, string>();

            List<FacetCategory> Facets = SearchProvider.GetFacets(searchTerm, "site_section_facet", out totalCount);
            var FacetList = GetFacetsList();
            if (FacetList != null)
            {
                facetsDict = new Dictionary<string, string>(FacetList.ToDictionary(x => x.ID.ToShortID().ToString().ToLower(), x => string.Format("{0}|(0)", x["title"])));
            }

            if (Facets != null && Facets.Count > 0)
            {
                //facet.name returns a shortid so get the name from sitecore item
                foreach (var category in Facets)
                {
                    foreach (var facet in category.Values.OrderByDescending(i => i.AggregateCount))
                    {
                        if (facetsDict.ContainsKey(facet.Name))
                        {
                            facetsDict[facet.Name] = facetsDict[facet.Name].Replace("0", facet.AggregateCount.ToString());
                        }
                    }
                }
            }
            //SortedDictionary so that sorting is done based on Facet Name chronologically

            SortedDictionary<string, string> retDict = new SortedDictionary<string, string>(facetsDict.ToDictionary(x => x.Value.Split('|')[0], x => x.Value));
            return retDict.ToDictionary(x => x.Key, x => x.Value);
        }

        public ActionResult FacetDetails()
        {
            try
            {
                var searchResultsModel = HttpContext.Items["SearchModel"] as SearchResultsModel;
                if (searchResultsModel != null)
                    return View("~/Views/HabSearch/FacetDetails.cshtml", searchResultsModel);
                else
                    return new EmptyResult();
            }
            catch (Exception ex)
            {
                Log.Error("Something went wrong in FacetDetails", ex, this);
                return new EmptyResult();
            }
        }
    }
}
