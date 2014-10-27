using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Remoting;
using System.Xml.Serialization;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;

namespace SPMeta2.Models
{
    /// <summary>
    /// Internal usage only.
    /// </summary>
    public enum ModelNodeState
    {
        /// <summary>
        /// Model node has not been changed.
        /// </summary>
        None,

        /// <summary>
        /// Model node is being processed.
        /// </summary>
        Processing,

        /// <summary>
        /// Model node has been processed.
        /// </summary>
        Processed
    }

    /// <summary>
    /// Allows to adjust particular mode node processing behaviour.
    /// </summary>
    [Serializable]
    public class ModelNodeOptions
    {
        #region constructors

        public ModelNodeOptions()
        {
            RequireSelfProcessing = true;
        }

        #endregion

        #region properties

        /// <summary>
        /// Indicates of model node needs to be processed by model handler.
        /// </summary>
        public bool RequireSelfProcessing { get; set; }

        #endregion

        #region static


        public static ModelNodeOptions New()
        {
            return new ModelNodeOptions();
        }

        #endregion
    }

    public static class ModelNodeOptionsSyntax
    {
        public static ModelNodeOptions NoSelfProcessing(this ModelNodeOptions options)
        {
            options.RequireSelfProcessing = false;

            return options;
        }
    }

    /// <summary>
    /// Base tree model node implementation. 
    /// </summary>
    [Serializable]
    public class ModelNode
    {
        #region constructors

        public ModelNode()
        {
            ChildModels = new Collection<ModelNode>();
            Options = new ModelNodeOptions();

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

        /// <summary>
        /// Allows to adjust particular mode node processing behaviour.
        /// </summary>
        public ModelNodeOptions Options { get; set; }

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
            var objectType = typeof(object);
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
                var nonObjectDefinition = new Type[] { objectType, typeof(DefinitionBase) };
                var withObjectDefinition = new Type[] { objectType, eventArgs.ObjectDefinition.GetType() };

                var modelNonDefInstanceType = modelContextType.MakeGenericType(nonDefinition);
                var modelWithDefInstanceType = modelContextType.MakeGenericType(withDefinition);
                var modelNonObjectDefInstanceType = modelContextType.MakeGenericType(nonObjectDefinition);
                var modelWithObjectDefInstanceType = modelContextType.MakeGenericType(withObjectDefinition);

                object modelContextInstance = null;

                if (action.Method.GetParameters()[0].ParameterType.IsAssignableFrom(modelNonDefInstanceType))
                {
                    modelContextInstance = Activator.CreateInstance(modelNonDefInstanceType);
                }
                else if (action.Method.GetParameters()[0].ParameterType.IsAssignableFrom(modelWithDefInstanceType))
                {
                    modelContextInstance = Activator.CreateInstance(modelWithDefInstanceType);
                }
                else if (action.Method.GetParameters()[0].ParameterType.IsAssignableFrom(modelNonObjectDefInstanceType))
                {
                    modelContextInstance = Activator.CreateInstance(modelNonObjectDefInstanceType);
                }
                else if (action.Method.GetParameters()[0].ParameterType.IsAssignableFrom(modelWithObjectDefInstanceType))
                {
                    modelContextInstance = Activator.CreateInstance(modelWithObjectDefInstanceType);
                }

                if (modelContextInstance == null)
                {
                    throw new SPMeta2Exception("Cannot find a proper ModelContextEvents overload");
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

    /// <summary>
    /// Internal usage only.
    /// Indicates the strategy for the provision exception handling.
    /// </summary>
    public enum ContinuationOptions
    {
        /// <summary>
        /// Throw an exception
        /// </summary>
        StopAndThrowException,

        /// <summary>
        /// Silently continue.
        /// </summary>
        Continue,

        /// <summary>
        /// Silently stop.
        /// </summary>
        Stop,
    }

    /// <summary>
    /// Model creaeting/creating context.
    /// </summary>
    /// <typeparam name="TObjectType"></typeparam>
    /// <typeparam name="TDefinitionType"></typeparam>
    public class OnCreatingContext<TObjectType, TDefinitionType>
        where TDefinitionType : DefinitionBase
    {
        /// <summary>
        /// Typed SharePoint object instance.
        /// </summary>
        public TObjectType Object { get; set; }

        /// <summary>
        /// Current model definition.
        /// </summary>
        public TDefinitionType ObjectDefinition { get; set; }

        /// <summary>
        /// Current model, the root node.
        /// </summary>
        public ModelNode Model { get; set; }

        /// <summary>
        /// Current model node.
        /// </summary>
        public ModelNode CurrentModelNode { get; set; }

        /// <summary>
        /// Exception handling options.
        /// </summary>
        public ContinuationOptions ContinuationOption { get; set; }

        /// <summary>
        /// Aggregated exception, if any.
        /// </summary>
        public AggregateException Error { get; set; }
    }
}
