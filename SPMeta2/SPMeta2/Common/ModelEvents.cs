using System;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Common
{
    public class ModelEventArgs : EventArgs
    {
        #region constructors

        public ModelEventArgs()
        {
            EventType = ModelEventType.Unknown;
        }

        #endregion

        #region properties

        public ModelEventType EventType { get; set; }

        public object Object { get; set; }
        public Type ObjectType { get; set; }

        public DefinitionBase ObjectDefinition { get; set; }

        public object ModelHost { get; set; }
        public ModelNode Model { get; set; }
        public ModelNode CurrentModelNode { get; set; }

        #endregion
    }

    public enum ModelEventType
    {
        Unknown,

        OnProvisioning,
        OnProvisioned,

        OnUpdating,
        OnUpdated,

        OnRetracting,
        OnRetracted,

        OnError
    }
}
