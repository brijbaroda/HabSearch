namespace Sitecore.Feature.HabSearch.Controllers
{
    using Models;
    using Sitecore.Foundation.HabSearch.Indexing.Models;
    using System.Web.Mvc;

    public class AdvanceSearchController : Controller
    {
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

            //var query = this.CreateQuery(searchQuery);
            //results = this.searchServiceRepository.Get().Search(query);
            //if (this.HttpContext != null)
            //{
            //    this.HttpContext.Items.Add("SearchResults", results);
            //}

            return results;
        }
    }
}
