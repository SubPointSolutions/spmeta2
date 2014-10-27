using System;

namespace SPMeta2.Standard.Services.Webparts.ContentByQueryWebPart
{
    public class FilterValue
    {
        #region properties

        public Guid? Field { get; set; }
        public string Value { get; set; }
        public string DisplayValue { get; set; }
        public string Type { get; set; }
        public string Operator { get; set; }

        #endregion
    }

    public enum FilterValueChainingOperator
    {
        Or,
        And
    }

    


}
