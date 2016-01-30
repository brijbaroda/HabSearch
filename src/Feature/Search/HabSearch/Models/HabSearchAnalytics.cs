using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.HabSearch.Models
{
    public class HabSearchAnalytics
    {
        public bool IsAnalyticsEnabled { get; set; }

        public string SearchTermsRootFolderPath { get; set; }
    }
}