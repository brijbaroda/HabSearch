using Sitecore.Data;

namespace Sitecore.Feature.HabSearch
{
    public class Templates
    {
        public struct SearchResults
        {
            public static ID ID = new ID("{14E452CA-064D-48A8-9FF2-2744D10437A1}");

            public struct Fields
            {
                public static readonly ID SearchBoxTitle = new ID("{80E30DD8-8021-45F5-9FE1-23D2702CC206}");
                public static readonly ID Root = new ID("{CD904125-3AE5-4709-9E6D-71473C5D5007}");
            }
        }

        public struct PagedSearchResultsParameters
        {
            public static ID ID = new ID("{D1D3E60F-E571-48D2-84CF-B053EE660C13}");

            public struct Fields
            {
                public static readonly ID ResultsOnPage = new ID("{FCC7E3B4-46AB-4A51-975F-A6B259B3D214}");
                public static readonly ID PagesToShow = new ID("{FCC7E3B4-46AB-4A51-975F-A6B259B3D214}");
            }
        }

        public struct HabSearchAnalyticSettings
        {
            public static readonly ID ID = new ID("{40D19A90-F220-48FC-BC41-EFB06DB8B90B}");

            public struct Fields
            {
                public static readonly ID EnableHabSeachAnalytics = new ID("{F4469CFF-91FF-4A55-8D9C-9DEAFC3CF2E6}");
                public static readonly ID SearchTermsRootFolder = new ID("{B8E7F74D-8957-4C10-898E-A160BBF11966}");
            }
        }


    }
}
