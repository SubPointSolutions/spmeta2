using SPMeta2.Attributes.Regression;
using System;
using System.Collections.Generic;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Definitions
{
    [Serializable]
    public abstract class ContentPageDefinitionBase : PageDefinitionBase
    {
        #region constructors

        public ContentPageDefinitionBase()
        {
            Content = new byte[0];
        }

        #endregion

        #region properties
        public byte[] Content { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("Title:[{0}] FileName:[{1}]", Title, FileName);
        }

        #endregion
    }
}
