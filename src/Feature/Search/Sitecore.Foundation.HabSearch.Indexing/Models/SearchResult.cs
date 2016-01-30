using Sitecore.Data.Items;
using Sitecore.Links;
using System;

namespace Sitecore.Foundation.HabSearch.Indexing.Models
{
    public class SearchResult : ISearchResult
    {
        private Uri _url;

        public SearchResult(Item item)
        {
            this.Item = item;
        }

        public Item Item { get; }
        public string Title { get; set; }
        public string ContentType { get; set; }
        public string Description { get; set; }

        public Uri Url
        {
            get
            {
                return this._url ?? new Uri(LinkManager.GetItemUrl(Item), UriKind.Relative);
            }
            set
            {
                this._url = value;
            }
        }
    }
}
