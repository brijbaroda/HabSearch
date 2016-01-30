using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.Feature.HabSearch.Models;
using Sitecore.Foundation.HabSearch.Indexing.Models;
using Sitecore.Foundation.Multisite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.HabSearch.Repositories
{
    public class HabSearchAnalyticsRepository
    {
        public static HabSearchAnalytics GetHabSearchAnalyticsSettings()
        {
            try
            {
                var siteContext = new SiteContext();
                var siteDefinition = siteContext.GetSiteDefinition(Sitecore.Context.Item);

                if (siteDefinition?.Item == null || !siteDefinition.Item.IsDerived(Templates.HabSearchAnalyticSettings.ID))
                {
                    return null;
                }

                var habSearchAnalytics = new HabSearchAnalytics();

                CheckboxField cbIsAnalyticsEnabled = siteDefinition.Item.Fields[Templates.HabSearchAnalyticSettings.Fields.EnableHabSeachAnalytics];

                if (cbIsAnalyticsEnabled != null && cbIsAnalyticsEnabled.Checked)
                {
                    habSearchAnalytics.IsAnalyticsEnabled = true;
                    habSearchAnalytics.SearchTermsRootFolderPath = siteDefinition.Item[Templates.HabSearchAnalyticSettings.Fields.SearchTermsRootFolder];
                }

                return habSearchAnalytics;
            }
            catch (Exception ex)
            {
                Log.Error("Error while getting HabSearch Analytics settings", ex, typeof(HabSearchAnalyticsRepository));
            }

            return null;
        }
    }
}