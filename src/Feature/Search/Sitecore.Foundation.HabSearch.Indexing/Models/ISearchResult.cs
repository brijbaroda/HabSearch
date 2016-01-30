using Sitecore.Data.Items;
using System;

namespace Sitecore.Foundation.HabSearch.Indexing.Models
{
    public interface ISearchResult
    {
        Item Item { get; }
        string Title { get; set; }
        string ContentType { get; set; }
        string Description { get; set; }
        Uri Url { get; set; }
    }
}
