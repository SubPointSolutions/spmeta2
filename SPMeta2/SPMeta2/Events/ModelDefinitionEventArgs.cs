using System;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Events
{
    /// <summary>
    /// Internal usage only.
    /// </summary>
    public class ModelDefinitionEventArgs : EventArgs
    {
        #region properties

        public DefinitionBase Model { get; set; }

        #endregion
    }
}
