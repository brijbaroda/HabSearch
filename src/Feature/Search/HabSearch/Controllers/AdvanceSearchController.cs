namespace Sitecore.Feature.HabSearch.Controllers
{
    using Models;
    using Repositories;
    using Sitecore.Foundation.HabSearch.Indexing.Models;
    using System.Web.Mvc;

    public class AdvanceSearchController : Controller
    {
        private readonly ISearchServiceRepository searchServiceRepository;
        private readonly ISearchSettingsRepository searchSettingsRepository;
        private readonly QueryRepository queryRepository;
        public ActionResult AdvanceSearchResults(string query)
        {
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
    }
}
