using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using SPMeta2.Common;
using SPMeta2.Definitions;

namespace SPMeta2.Models
{
    public enum ModelNodeState
    {
        None,
        Processing,
        Processed
    }

    public class ModelNode
    {
        #region contructors

        public ModelNode()
        {
            ChildModels = new Collection<ModelNode>();
            ModelEvents = new Dictionary<ModelEventType, List<object>>();
        }

        #endregion

        #region properties

        #region properties

        public string ObjectType
        {
            get { return GetType().Name; }
            set
            {

            }
        }

        #endregion

        public DefinitionBase Value { get; set; }
        public Collection<ModelNode> ChildModels { get; set; }

        [XmlIgnore]
        public virtual Dictionary<ModelEventType, List<object>> ModelEvents { get; set; }

        /// <summary>
        /// Experimental, may be removed in later releases.
        /// </summary>
        [XmlIgnore]
        [Obsolete]
        public ModelNodeState State { get; set; }

        #endregion

        #region events support

        public delegate void TmpAction(object arg1, object arg2);

        public virtual void InvokeOnModelEvents(object rawObject, ModelEventType eventType)
        {
            if (!ModelEvents.ContainsKey(eventType)) return;

            var targetEvents = ModelEvents[eventType];

            // yeap, shity yet
            foreach (MulticastDelegate action in targetEvents)
                action.DynamicInvoke(this.Value, rawObject);
        }

        //private void InvokeOnModelUpdatedEvents<TModelDefinition, TSPObject>(TSPObject rawObject)
        //    where TModelDefinition : DefinitionBase
        //{
        //    if (!ModelEvents.ContainsKey(Common.ModelEvents.Internal.OnUpdated)) return;

        //    var that = this as TModelDefinition;
        //    var onUpdatedEvents = ModelEvents[Common.ModelEvents.Internal.OnUpdated].Where(e => e is Action<TModelDefinition, TSPObject>);

        //    foreach (Action<TModelDefinition, TSPObject> action in onUpdatedEvents)
        //        action(that, rawObject);
        //}

        //private void InvokeOnModelUpdatingEvents<TModelDefinition, TSPObject>(TSPObject rawObject)
        //   where TModelDefinition : DefinitionBase
        //{
        //    if (!ModelEvents.ContainsKey(Common.ModelEvents.Internal.OnUpdating)) return;

        //    var that = this as TModelDefinition;
        //    var onUpdatingEvents = ModelEvents[Common.ModelEvents.Internal.OnUpdating].Where(e => e is Action<TModelDefinition, TSPObject>);

        //    foreach (Action<TModelDefinition, TSPObject> action in onUpdatingEvents)
        //        action(that, rawObject);
        //}

        public virtual void RegisterModelUpdateEvents<TModelDefinition, TSPObject>(
            Action<TModelDefinition, TSPObject> onUpdating, Action<TModelDefinition, TSPObject> onUpdated)
        {
            RegisterModelUpdatingEvent(onUpdating);
            RegisterModelUpdatedEvent(onUpdated);
        }

        public virtual void RegisterModelUpdatingEvent<TModelDefinition, TSPObject>(Action<TModelDefinition, TSPObject> action)
        {
            RegisterModelEvent(ModelEventType.OnUpdating, action);
        }

        public virtual void RegisterModelUpdatedEvent<TModelDefinition, TSPObject>(Action<TModelDefinition, TSPObject> action)
        {
            RegisterModelEvent(ModelEventType.OnUpdated, action);
        }

        public virtual void RegisterModelEvent<TModelDefinition, TSPObject>(ModelEventType actionType, Action<TModelDefinition, TSPObject> action)
        {
            if (action == null) return;

            if (!ModelEvents.ContainsKey(actionType))
                ModelEvents.Add(actionType, new List<object>());

            ModelEvents[actionType].Add(action);
        }

        #endregion
    }
}
