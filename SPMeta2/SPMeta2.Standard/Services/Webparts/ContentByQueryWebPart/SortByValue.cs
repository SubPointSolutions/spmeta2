using System;

namespace SPMeta2.Standard.Services.Webparts.ContentByQueryWebPart
{
    public class SortByValue
    {
        #region properties

        public Guid? SortBy { get; set; }
        public string SortByDirection { get; set; }
        public string SortByFieldType { get; set; }

        #endregion
    }
}
