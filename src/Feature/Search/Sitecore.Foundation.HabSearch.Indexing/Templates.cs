
namespace Sitecore.Foundation.HabSearch.Indexing
{
    using Sitecore.Data;

    public class Templates
    {
        public struct HasPageContent
        {
            public static ID ID = new ID("{AF74A00B-8CA7-4C9A-A5C1-156A68590EE2}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{C30A013F-3CC8-4961-9837-1C483277084A}");
                public const string Title_FieldName = "Title";
                public static readonly ID Summary = new ID("{AC3FD4DB-8266-476D-9635-67814D91E901}");
                public const string Summary_FieldName = "Summary";
                public static readonly ID Body = new ID("{D74F396D-5C5E-4916-BD0A-BFD58B6B1967}");
                public const string Body_FieldName = "Body";
                public static readonly ID Image = new ID("{9492E0BB-9DF9-46E7-8188-EC795C4ADE44}");
            }
        }
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
