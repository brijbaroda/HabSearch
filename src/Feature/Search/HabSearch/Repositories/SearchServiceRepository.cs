﻿
using Sitecore.Foundation.HabSearch.Indexing.Models;

namespace Sitecore.Feature.HabSearch.Repositories
{
    public class SearchServiceRepository : ISearchServiceRepository
    {
        private readonly ISearchSettingsRepository settingsRepository;
        public virtual SearchService Get()
        {
            return new SearchService(this.settingsRepository.Get());
        }
    }
}
