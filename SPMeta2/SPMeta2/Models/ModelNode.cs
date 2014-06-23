using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Remoting;
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
        #region constructors

        public ModelNode()
        {
            ChildModels = new Collection<ModelNode>();

            ModelEvents = new Dictionary<ModelEventType, List<object>>();
            ModelContextEvents = new Dictionary<ModelEventType, List<object>>();
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
        public Dictionary<ModelEventType, List<object>> ModelEvents { get; set; }

        [XmlIgnore]
        public Dictionary<ModelEventType, List<object>> ModelContextEvents { get; set; }

        [XmlIgnore]
        public ModelNodeState State { get; set; }

        #endregion

        #region events support

        public delegate void TmpAction(object arg1, object arg2);

        public virtual void InvokeOnModelContextEvents(object sender, ModelEventArgs eventArgs)
        {
            var eventType = eventArgs.EventType;

            // type.. can be null, so?
            var spObjectType = eventArgs.ObjectType;

            if (!ModelContextEvents.ContainsKey(eventType)) return;

            var targetEvents = ModelContextEvents[eventType];

            // yea, yea..
            foreach (MulticastDelegate action in targetEvents)
            {
                var modelContextType = typeof(OnCreatingContext<,>);

                var nonDefinition = new Type[] { spObjectType, typeof(DefinitionBase) };
                var withDefinition = new Type[] { spObjectType, eventArgs.ObjectDefinition.GetType() };

                var modelNonDefInstanceType = modelContextType.MakeGenericType(nonDefinition);
                var modelWithDefInstanceType = modelContextType.MakeGenericType(withDefinition);

                object modelContextInstance = null;

                if (action.Method.GetParameters()[0].ParameterType.IsAssignableFrom(modelNonDefInstanceType))
                {
                    modelContextInstance = Activator.CreateInstance(modelNonDefInstanceType);
                }
                else if (action.Method.GetParameters()[0].ParameterType.IsAssignableFrom(modelWithDefInstanceType))
                {
                    modelContextInstance = Activator.CreateInstance(modelWithDefInstanceType);
                }

                SetProperty(modelContextInstance, "Model", eventArgs.Model);
                SetProperty(modelContextInstance, "CurrentModelNode", eventArgs.CurrentModelNode);

                SetProperty(modelContextInstance, "Object", eventArgs.Object);
                SetProperty(modelContextInstance, "ObjectDefinition", eventArgs.ObjectDefinition);

                action.DynamicInvoke(modelContextInstance);
            }
        }

        private static void SetProperty(object obj, string propName, object propValue)
        {
            var propertyInfo = obj.GetType().GetProperty(propName);
            propertyInfo.SetValue(obj, propValue, null);

        }

        public virtual void InvokeOnModelEvents(object rawObject, ModelEventType eventType)
        {
            if (!ModelEvents.ContainsKey(eventType)) return;

            var targetEvents = ModelEvents[eventType];

            // yeap, shity yet
            foreach (MulticastDelegate action in targetEvents)
                action.DynamicInvoke(this.Value, rawObject);
        }

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

        public virtual void RegisterModelContextEvent<TSPObject, TDefinitionType>(
            ModelEventType actionType,
            Action<OnCreatingContext<TSPObject, TDefinitionType>> action)
             where TDefinitionType : DefinitionBase
        {
            if (action == null) return;

            if (!ModelContextEvents.ContainsKey(actionType))
                ModelContextEvents.Add(actionType, new List<object>());

            ModelContextEvents[actionType].Add(action);
        }

        #endregion
    }

    public enum ContinuationOptions
    {
        StopAndThrowException,
        Continue,
        Stop,
    }

    public class OnCreatingContext<TObjectType, TDefinitionType>
        where TDefinitionType : DefinitionBase
    {
        public TObjectType Object { get; set; }
        public TDefinitionType ObjectDefinition { get; set; }

        public ModelNode Model { get; set; }
        public ModelNode CurrentModelNode { get; set; }

        public ContinuationOptions ContinuationOption { get; set; }
        public AggregateException Error { get; set; }
    }
}
