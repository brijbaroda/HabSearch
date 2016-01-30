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
    public class AdvanceSearchController : Controller
    {
        private readonly ISearchServiceRepository searchServiceRepository;
        private readonly ISearchSettingsRepository searchSettingsRepository;
        private readonly QueryRepository queryRepository;
        public ActionResult AdvanceSearchResults(string query)
        {

            if (Tracker.Current != null && Tracker.Enabled)
            {
                SetPatternCard(query);
            }

            return this.View("~/Views/HabSearch/AdvanceSearchResults.cshtml", this.GetFacetedSearchResults(new SearchQuery { Query = query }));

        }

        private ISearchResults GetFacetedSearchResults(SearchQuery searchQuery)
        {
            ISearchResults results = null;
            if (this.HttpContext != null)
            {
                results = this.HttpContext.Items["SearchResults"] as ISearchResults;
            }

            if (results != null)
            {
                return results;
            }

            var query = this.CreateQuery(searchQuery);
            results = this.searchServiceRepository.Get().Search(query);
            //if (this.HttpContext != null)
            //{
            //    this.HttpContext.Items.Add("SearchResults", results);
            //}

            return results;
        }
        private IQuery CreateQuery(SearchQuery query)
        {
            return this.queryRepository.Get(query);
        }

        /// <summary>
        /// Set the pattern card
        /// </summary>
        /// <param name="searchedTerm"></param>
        public void SetPatternCard(string searchedTerm)
        {
            try
            {
                Sitecore.Data.Database db = Sitecore.Context.Database;

                //Get persona tag from Data folder matching the searched keyword
                var peronaDataTagsQuery = string.Format("/sitecore/content/Habitat/Global/Search Terms//*[CompareCaseInsensitive(@Text,'{0}')]", searchedTerm);
                Item matchedPeronaTag = db.SelectSingleItem(peronaDataTagsQuery);

                if (matchedPeronaTag != null)
                {
                    //Get all pattern cards which has matching persona tag as per searched keyword                
                    var query = string.Format("/sitecore/system/Marketing Control Panel/Profiles//*[@@templateid='{0}' and contains(@Related Search Terms,'{1}')]", "{4A6A7E36-2481-438F-A9BA-0453ECC638FA}", matchedPeronaTag.ID);
                    Item[] allPatternCards = db.SelectItems(query);

                    foreach (Item patternCard in allPatternCards)
                    {
                        TrackingField tField = new TrackingField(patternCard.Fields["Pattern"]);
                        var listProfiles = tField.Profiles.Where(profile1 => profile1.IsSavedInField);

                        if (listProfiles.Any())
                        {
                            var scores = new Dictionary<string, float>();

                            foreach (ContentProfile profile in listProfiles)
                            {
                                if (Tracker.Current.Interaction != null)
                                {
                                    //Set current profile with respective pattern
                                    var pageProfile = Tracker.Current.Interaction.Profiles[patternCard.Parent.Parent.Name];

                                    foreach (var key in profile.Keys)
                                    {
                                        if (key.Value != 0)
                                        {
                                            scores.Add(key.Name, key.Value * 3);
                                        }
                                    }
                                    pageProfile.Score(scores);
                                    pageProfile.UpdatePattern();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error occured in SetPatternCard " + ex.ToString(), new Exception());
            }
        }
    }
}
