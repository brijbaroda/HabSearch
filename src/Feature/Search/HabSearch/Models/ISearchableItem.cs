namespace Sitecore.Feature.HabSearch.Models
{
    public interface ISearchableItem
    {
        bool IsSearchable { get; }

        string Title { get; }

        string SearchResultAbstract { get; }

        string Url { get; }
    }
}
