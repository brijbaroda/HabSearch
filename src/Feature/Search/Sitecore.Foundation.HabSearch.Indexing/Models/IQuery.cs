﻿namespace Sitecore.Foundation.HabSearch.Indexing.Models
{

    public interface IQuery
    {
        string QueryText { get; set; }
        int IndexOfFirstResult { get; set; }
        int NoOfResults { get; set; }
    }
}
