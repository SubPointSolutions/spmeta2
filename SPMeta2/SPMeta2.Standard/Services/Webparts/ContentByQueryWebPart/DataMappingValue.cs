using System;

namespace SPMeta2.Standard.Services.Webparts.ContentByQueryWebPart
{
    public class DataMappingValue
    {
        #region properties

        public string SlotName { get; set; }
        public Guid? Id { get; set; }
        public string InternalName { get; set; }
        public string FieldType { get; set; }

        #endregion
    }
}
