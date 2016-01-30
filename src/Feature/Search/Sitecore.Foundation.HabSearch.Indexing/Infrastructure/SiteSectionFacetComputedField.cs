using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace Sitecore.Foundation.HabSearch.Indexing.Infrastructure
{
    public class SiteSectionFacetComputedField : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var item = (SitecoreIndexableItem)indexable;

            if (item != null && item.Item != null && item.Item.Fields[Templates.FacetType.Fields.SiteSectionFacet] != null)
            {
                return item.Item.Fields[Templates.FacetType.Fields.SiteSectionFacet];
            }

            return string.Empty;
        }
    }
}
