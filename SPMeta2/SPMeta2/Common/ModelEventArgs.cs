using System;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Utils;

namespace SPMeta2.Common
{
    /// <summary>
    /// Internal usage only.
    /// </summary>
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

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                         .AddRawPropertyValue("EventType", EventType)

                         .AddRawPropertyValue("Object", Object)
                         .AddRawPropertyValue("ObjectType", ObjectType)

                         .AddRawPropertyValue("ObjectDefinition", ObjectDefinition)
                         .AddRawPropertyValue("ModelHost", ModelHost)
                         .AddRawPropertyValue("Model", Model)
                         .AddRawPropertyValue("CurrentModelNode", CurrentModelNode)

                         .ToString();
        }

        #endregion
    }
}
