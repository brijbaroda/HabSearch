using System.Collections.Generic;

namespace Sitecore.Foundation.HabSearch.Indexing.Models
{
    public interface ISearchResults
    {
        IEnumerable<ISearchResult> Results { get; }
        int TotalNumberOfResults { get; }
    }
}
