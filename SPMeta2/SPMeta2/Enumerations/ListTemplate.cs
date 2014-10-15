using System;

namespace SPMeta2.Enumerations
{
    public class ListTemplate
    {
        #region properties

        public string Name { get; set; }
        public string InternalName { get; set; }
        public string Description { get; set; }

        public Guid FeatureId { get; set; }
        public bool Hidden { get; set; }

        #endregion
    }
}
