namespace Sitecore.Feature.HabSearch.Repositories
{
    public class SearchServiceRepository : ISearchServiceRepository
    {
        private readonly ISearchSettingsRepository settingsRepository;
        public virtual SearchService Get()
        {
            return new SearchService(this.settingsRepository.Get());
        }
        public SearchServiceRepository() : this(new SearchSettingsRepository())
        {
        }

        public SearchServiceRepository(ISearchSettingsRepository settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }
    }
}
