using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Sitecore.Foundation.HabSearch.Indexing.Models
{
    public interface ISearchSettings
    {
        Item Root { get; set; }

        IEnumerable<ID> Templates { get; set; }
    }
}
