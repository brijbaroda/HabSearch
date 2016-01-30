namespace HabSearch.Controllers
{
    using System.Web.Mvc;

    public class HabSearchController : Controller
    {
        public ActionResult FacetedSearch()
        {
            return this.View("GlobalSearch", "");
        }
    }
}
