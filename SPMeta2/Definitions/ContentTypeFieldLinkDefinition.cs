using System;
using SPDevLab.SPMeta2.Definitions;

namespace SPMeta2.Definitions
{
    public class ContentTypeFieldLinkDefinition : DefinitionBase
    {
        #region properties

        public Guid FieldId { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("FieldId:[{0}]", FieldId);
        }

        #endregion
    }
}
