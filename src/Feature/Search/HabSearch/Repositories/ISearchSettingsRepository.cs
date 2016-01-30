using Sitecore.Feature.HabSearch.Models;
namespace Sitecore.Feature.HabSearch.Repositories
{
    public interface ISearchSettingsRepository
    {
        SearchSettings Get();
        SearchSettings Get(string query);
    }
}
