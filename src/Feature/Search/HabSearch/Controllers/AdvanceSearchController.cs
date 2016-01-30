namespace Sitecore.Feature.HabSearch.Controllers
{
    using Analytics.Data;
    using Data.Items;
    using Models;
    using Repositories;
    using Sitecore.Foundation.HabSearch.Indexing.Models;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Linq;
    using Analytics;
    using Configuration;
    using ContentSearch.Linq;
    using Links;
    using Foundation.HabSearch.Indexing.Infrastructure;
    using Diagnostics;
    public class AdvanceSearchController : Controller
    {
        protected readonly string QueryStringKey = "query";
        protected readonly string FacetQueryStringKey = "f";
        protected readonly ISiteSearchProvider SearchProvider = new SiteSearchProvider();
        public ActionResult FacetSearchResults(string query)
        {
            //var siteSearchPage = SitecoreContext.GetCurrentItem<SiteSearchPage>();

            var searchTerm = query;
            var audienceFacet = Request.QueryString[FacetQueryStringKey];
            var model = new SearchResultsModel
            {
                QueryStringKey = QueryStringKey,
                AudienceFacetQueryStringKey = FacetQueryStringKey,
                AudienceFacet = audienceFacet
            };
            model.Keyword = searchTerm;
            string totalCount = "0";
            string selFacetCount = string.Empty;
            model.AudienceFacets = GetAudienceFacets(searchTerm, out totalCount);
            if (!string.IsNullOrEmpty(audienceFacet))
            {
                model.AudienceFacets.TryGetValue(audienceFacet, out selFacetCount);
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
                AudienceFacetQueryStringKey = FacetQueryStringKey,

            };

            try
            {
                var audienceFacet = Request.QueryString[FacetQueryStringKey];
                model.Keyword = searchTerm;
                model.AudienceFacet = audienceFacet;

                //string selFacetShortID = string.Empty;
                //if (!string.IsNullOrEmpty(audienceFacet))
                //{
                //    selFacetShortID = GetAudienceShortID(audienceFacet);

                //}
                //run search
                var results = SearchProvider.Search(searchTerm);
                
                model.SearchResults = results;
                model.ResultsCount = results.Count().ToString();
                string totalCount = "0";
                model.AudienceFacets = GetAudienceFacets(searchTerm, out totalCount);
                model.AllResultsCount = totalCount;
                if (int.Parse(model.ResultsCount) < model.EndResultNumber)
                {
                    model.EndResultNumber = int.Parse(model.ResultsCount);
                }

                string selFacetCount = string.Empty;
                if (!string.IsNullOrEmpty(audienceFacet))
                {
                    model.AudienceFacets.TryGetValue(audienceFacet, out selFacetCount);
                }
                model.AllResultsCount = totalCount;
                if (!string.IsNullOrEmpty(selFacetCount))
                {
                    model.SelectedFacetCount = selFacetCount.Split('|')[1];
                }

                return View("~/Views/HabSearch/AdvanceSearchResults.cshtml", model);
            }
            catch (Exception ex)
            {
                Log.Error("Something went wrong while performing a site search", ex, this);
                return View("~/Views/HabSearch/AdvanceSearchResults.cshtml");
            }
        }

        public Dictionary<string, string> GetAudienceFacets(string searchTerm, out string totalCount)
        {
            Dictionary<string, string> audienceDict = new Dictionary<string, string>();

            //Get list of audience facets and totalcount
            List<FacetCategory> AudienceFacets = SearchProvider.GetFacets(searchTerm, "site_section_facet", out totalCount);
            //var AudienceNavigation = GetAudienceNavigation();
            //if (AudienceNavigation != null)
            //{
            //    audienceDict = new Dictionary<string, string>(AudienceNavigation.ToDictionary(x => x.ID.ToShortID().ToString().ToLower(), x => string.Format("{0}|(0)", x["title"])));
            //}

            //if (AudienceFacets != null && AudienceFacets.Count > 0)
            //{
            //    //facet.name returns a shortid so get the name from sitecore item
            //    foreach (var category in AudienceFacets)
            //    {
            //        foreach (var facet in category.Values.OrderByDescending(i => i.AggregateCount))
            //        {
            //            if (audienceDict.ContainsKey(facet.Name))
            //            {
            //                audienceDict[facet.Name] = audienceDict[facet.Name].Replace("0", facet.AggregateCount.ToString());
            //            }
            //        }
            //    }
            //}
            //SortedDictionary so that sorting is done based on Facet Name chronologically
            //audienceDict.ToDictionary is required as audienceDict will still have sort ids for facets that have zero result count
            SortedDictionary<string, string> retDict = new SortedDictionary<string, string>(audienceDict.ToDictionary(x => x.Value.Split('|')[0], x => x.Value));
            return retDict.ToDictionary(x => x.Key, x => x.Value);
        }

        #region habitat...

        //private readonly ISearchServiceRepository searchServiceRepository;
        //private readonly ISearchSettingsRepository searchSettingsRepository;
        //private readonly QueryRepository queryRepository;
        //protected readonly string QueryStringKey = "query";
        //protected readonly string FacetQueryStringKey = "f";

        //public AdvanceSearchController() : this(new SearchServiceRepository(), new SearchSettingsRepository(), new QueryRepository())
        //{
        //}

        //public AdvanceSearchController(ISearchServiceRepository serviceRepository, ISearchSettingsRepository settingsRepository, QueryRepository queryRepository)
        //{
        //    this.searchServiceRepository = serviceRepository;
        //    this.queryRepository = queryRepository;
        //    this.searchSettingsRepository = settingsRepository;

        //}

        //public ActionResult AdvanceSearchResults(string query)
        //{

        //    if (Tracker.Current != null && Tracker.Enabled)
        //    {
        //        SetPatternCard(query);
        //    }
        //    var searchTerm = query;
        //    var siteFacet = FacetQueryStringKey;
        //    var model = new SearchResultsModel
        //    {

        //        QueryStringKey = QueryStringKey,
        //        AudienceFacetQueryStringKey = FacetQueryStringKey,
        //        AudienceFacet = siteFacet
        //    };
        //    model.Keyword = searchTerm;
        //    string totalCount = "0";
        //    string selFacetCount = string.Empty;
        //    model.AudienceFacets = GetFacets(searchTerm, out totalCount);
        //    if (!string.IsNullOrEmpty(siteFacet))
        //    {
        //        model.AudienceFacets.TryGetValue(siteFacet, out selFacetCount);
        //    }
        //    model.AllResultsCount = totalCount;
        //    if (!string.IsNullOrEmpty(selFacetCount))
        //    {
        //        model.SelectedFacetCount = selFacetCount.Split('|')[1];
        //    }
        //    model.PageUrl = LinkManager.GetItemUrl(Sitecore.Context.Item);
        //    var objquery = this.CreateQuery(new SearchQuery { Query = query });
        //    model.SearchResults = this.searchServiceRepository.Get().Search(objquery).Results;
        //    return this.View("~/Views/HabSearch/AdvanceSearchResults.cshtml", model);

        //}
        //public Dictionary<string, string> GetFacets(string searchTerm, out string totalCount)
        //{
        //    Dictionary<string, string> audienceDict = new Dictionary<string, string>();

        //    //Get list of audience facets and totalcount

        //    List<FacetCategory> AudienceFacets = this.searchServiceRepository.Get().GetFacets(searchTerm, "site_section_facet", out totalCount);
        //    //var AudienceNavigation = GetAudienceNavigation();
        //    //if (AudienceNavigation != null)
        //    //{
        //    //    audienceDict = new Dictionary<string, string>(AudienceNavigation.ToDictionary(x => x.ID.ToShortID().ToString().ToLower(), x => string.Format("{0}|(0)", x["title"])));
        //    //}

        //    //if (AudienceFacets != null && AudienceFacets.Count > 0)
        //    //{
        //    //    //facet.name returns a shortid so get the name from sitecore item
        //    //    foreach (var category in AudienceFacets)
        //    //    {
        //    //        foreach (var facet in category.Values.OrderByDescending(i => i.AggregateCount))
        //    //        {
        //    //            if (audienceDict.ContainsKey(facet.Name))
        //    //            {
        //    //                audienceDict[facet.Name] = audienceDict[facet.Name].Replace("0", facet.AggregateCount.ToString());
        //    //            }
        //    //        }
        //    //    }
        //    //}
        //    //SortedDictionary so that sorting is done based on Facet Name chronologically
        //    //audienceDict.ToDictionary is required as audienceDict will still have sort ids for facets that have zero result count
        //    SortedDictionary<string, string> retDict = new SortedDictionary<string, string>(audienceDict.ToDictionary(x => x.Value.Split('|')[0], x => x.Value));
        //    return retDict.ToDictionary(x => x.Key, x => x.Value);
        //}
        //private IQuery CreateQuery(SearchQuery query)
        //{
        //    return this.queryRepository.Get(query);
        //}

        ///// <summary>
        ///// Set the pattern card
        ///// </summary>
        ///// <param name="searchedTerm"></param>
        //public void SetPatternCard(string searchedTerm)
        //{
        //    try
        //    {
        //        Sitecore.Data.Database db = Sitecore.Context.Database;

        //        //Get persona tag from Data folder matching the searched keyword
        //        var peronaDataTagsQuery = string.Format("/sitecore/content/Habitat/Global/Search Terms//*[CompareCaseInsensitive(@Text,'{0}')]", searchedTerm);
        //        Item matchedPeronaTag = db.SelectSingleItem(peronaDataTagsQuery);

        //        if (matchedPeronaTag != null)
        //        {
        //            //Get all pattern cards which has matching persona tag as per searched keyword                
        //            var query = string.Format("/sitecore/system/Marketing Control Panel/Profiles//*[@@templateid='{0}' and contains(@Related Search Terms,'{1}')]", "{4A6A7E36-2481-438F-A9BA-0453ECC638FA}", matchedPeronaTag.ID);
        //            Item[] allPatternCards = db.SelectItems(query);

        //            foreach (Item patternCard in allPatternCards)
        //            {
        //                TrackingField tField = new TrackingField(patternCard.Fields["Pattern"]);
        //                var listProfiles = tField.Profiles.Where(profile1 => profile1.IsSavedInField);

        //                if (listProfiles.Any())
        //                {
        //                    var scores = new Dictionary<string, float>();

        //                    foreach (ContentProfile profile in listProfiles)
        //                    {
        //                        if (Tracker.Current.Interaction != null)
        //                        {
        //                            //Set current profile with respective pattern
        //                            var pageProfile = Tracker.Current.Interaction.Profiles[patternCard.Parent.Parent.Name];

        //                            foreach (var key in profile.Keys)
        //                            {
        //                                if (key.Value != 0)
        //                                {
        //                                    scores.Add(key.Name, key.Value * 3);
        //                                }
        //                            }
        //                            pageProfile.Score(scores);
        //                            pageProfile.UpdatePattern();
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Sitecore.Diagnostics.Log.Error("Error occured in SetPatternCard " + ex.ToString(), new Exception());
        //    }
        //}

        #endregion
    }
}
