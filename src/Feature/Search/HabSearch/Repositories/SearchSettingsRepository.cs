using System.Web;
using Sitecore.Feature.HabSearch.Models;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using Sitecore.Sites;
using Sitecore.Mvc.Presentation;
using Sitecore.Links;


namespace Sitecore.Feature.HabSearch.Repositories
{
    public class SearchSettingsRepository : ISearchSettingsRepository
    {
        public virtual SearchSettings Get(string query)
        {
            if (query == null)
            {
                query = HttpContext.Current == null ? "" : HttpContext.Current.Request["query"];
            }

            var configurationItem = RenderingContext.Current.Rendering.Item;
            if (configurationItem == null || !configurationItem.IsDerived(Templates.SearchResults.ID))
            {
                return null;
            }

            return new SearchSettings
            {
                ConfigurationItem = configurationItem,
                Query = query,
                SearchBoxTitle = configurationItem[Templates.SearchResults.Fields.SearchBoxTitle],
                SearchResultsUrl = LinkManager.GetItemUrl(configurationItem),
                Root = GetRootItem(configurationItem)
            };
        }

        private Item GetRootItem(Item configurationItem)
        {
            Item rootItem = null;
            if (configurationItem.Fields[Templates.SearchResults.Fields.Root].HasValue)
            {
                rootItem = ((ReferenceField)configurationItem.Fields[Templates.SearchResults.Fields.Root]).TargetItem;
            }
            return rootItem ?? Context.Site.GetRootItem();
        }

        public SearchSettings Get()
        {
            return this.Get(null);
        }
        
    }
}