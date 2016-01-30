using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using Sitecore.Foundation.HabSearch.Indexing.Models;

namespace Sitecore.Foundation.HabSearch.Indexing.Infrastructure
{
    public class SiteSectionFacetComputedField : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var item = (SitecoreIndexableItem)indexable;

            if (item != null && item.Item != null && item.Item.IsDerived(Templates.FacetType.ID))
            {
                if (item.Item.Fields[Templates.FacetType.Fields.SiteSectionFacet] != null)
                {
                    Sitecore.Data.Fields.ReferenceField referenceField = item.Item.Fields[Templates.FacetType.Fields.SiteSectionFacet];
                    if (referenceField != null && referenceField.TargetItem != null)
                    {
                        if (!string.IsNullOrWhiteSpace(referenceField.TargetItem["Title"]))
                            return referenceField.TargetItem["Title"];
                    }
                }
            }

            return string.Empty;
        }
    }
}
