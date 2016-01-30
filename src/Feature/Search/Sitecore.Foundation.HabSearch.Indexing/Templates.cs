
namespace Sitecore.Foundation.HabSearch.Indexing
{
    using Sitecore.Data;

    public class Templates
    {
        internal struct IndexedItem
        {
            public static ID ID = new ID("{8FD6C8B6-A9A4-4322-947E-90CE3D94916D}");

            public struct Fields
            {
                public static readonly ID IncludeInSearchResults = new ID("{8D5C486E-A0E3-4DBE-9A4A-CDFF93594BDA}");
                public const string IncludeInSearchResults_FieldName = "IncludeInSearchResults";
            }
        }

        internal struct FacetType
        {
            public static ID ID = new ID("{388C441F-98B6-4911-AB40-649440970B6B}");

            public struct Fields
            {
                public static readonly ID SiteSectionFacet = new ID("{F77A9410-3E65-4E9B-9CD9-7A9CBC4FA81D}");
                public const string SiteSectionFacet_FieldName = "SiteSectionFacet";
            }
        }

        internal struct Facet
        {
            public static ID ID = new ID("{799C6BF2-A816-4899-AE00-34CE4E8AC6FA}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{78AB750B-B634-4D68-8919-5451C7BAAB4C}");
                public const string Title_FieldName = "Title";
            }
        }
    }
}
