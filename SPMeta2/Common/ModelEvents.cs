using System;

namespace SPMeta2.Common
{
    public class ModelEventArgs : EventArgs
    {
        #region contructors

        public ModelEventArgs()
        {
            EventType = ModelEventType.Unknown;
        }

        #endregion

        #region properties

        public ModelEventType EventType { get; set; }

        //public DefinitionBase Model { get; set; }
        public object RawModel { get; set; }

        #endregion
    }

    public enum ModelEventType
    {
        Unknown,

        OnUpdating,
        OnUpdated,

        OnRemoving,
        OnRemoved
    }
}
