using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Feature.HabSearch.Infrastructure;
using Sitecore.Sites;
using System;
using System.Linq;
namespace Sitecore.Foundation.HabSearch.Indexing.Models
{
    public class SearchResultRepository
    {
        public static ISearchResult Create(SearchResultItem result)
        {
            var item = result.GetItem();
            var formattedResult = new SearchResult(item);
            FormatResultUsingFirstSupportedProvider(result, item, formattedResult);
            return formattedResult;
        }

        private static void FormatResultUsingFirstSupportedProvider(SearchResultItem result, Item item, ISearchResult formattedResult)
        {
            var formatter = FindFirstSupportedFormatter(item) ?? IndexContentProviderRepository.Default;
            formattedResult.ContentType = formatter.ContentType;
            formatter.FormatResult(result, formattedResult);
        }

        private static IndexContentProviderBase FindFirstSupportedFormatter(Item item)
        {
            var formatter = IndexContentProviderRepository.All.FirstOrDefault(provider => provider.SupportedTemplates.Any(item.IsDerived));
            return formatter;
        }
    }

    public static class ItemExtension
    {
        public static bool IsDerived(this Item item, ID templateId)
        {
            if (item == null)
            {
                return false;
            }

            return !templateId.IsNull && item.IsDerived(item.Database.Templates[templateId]);
        }

        private static bool IsDerived(this Item item, Item templateItem)
        {
            if (item == null)
            {
                return false;
            }

            if (templateItem == null)
            {
                return false;
            }
            var itemTemplate = TemplateManager.GetTemplate(item);
            return itemTemplate != null && (itemTemplate.ID == templateItem.ID || itemTemplate.DescendsFrom(templateItem.ID));
        }
        public static Item GetRootItem(this SiteContext site)
        {
            if (site == null)
                throw new ArgumentNullException(nameof(site));

            return site.Database.GetItem(Context.Site.RootPath);
        }
    }
}
