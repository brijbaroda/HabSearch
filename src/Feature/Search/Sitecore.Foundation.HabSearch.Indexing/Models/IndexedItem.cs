using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using System.Collections.Generic;

namespace Sitecore.Foundation.HabSearch.Indexing.Models
{
    public class IndexedItem : SearchResultItem
    {
        [IndexField("")]
        public bool HasPresentation { get; set; }

        [IndexField(Templates.IndexedItem.Fields.IncludeInSearchResults_FieldName)]
        public bool ShowInSearchResults { get; set; }

        [IndexField("")]
        public List<string> AllTemplates { get; set; }

        [IndexField("")]
        public bool IsLatestVersion { get; set; }
    }
}
