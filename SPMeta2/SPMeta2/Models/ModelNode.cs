using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Remoting;
using System.Xml.Serialization;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;
using SPMeta2.Services;
using System.Runtime.Serialization;

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
    [DataContract]
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
    [DataContract]
    public class ModelNode
    {
        #region constructors

        public ModelNode()
        {
            TraceService = ServiceContainer.Instance.GetService<TraceServiceBase>();

            ChildModels = new Collection<ModelNode>();
            Options = new ModelNodeOptions();

            ModelEvents = new Dictionary<ModelEventType, List<object>>();
            ModelContextEvents = new Dictionary<ModelEventType, List<object>>();
        }

        #endregion

        #region properties

        #region properties

        [XmlIgnore]
        [IgnoreDataMember]
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
        [DataMember]
        public ModelNodeOptions Options { get; set; }

        [DataMember]
        public DefinitionBase Value { get; set; }

        [DataMember]
        public Collection<ModelNode> ChildModels { get; set; }

        [XmlIgnore]
        [IgnoreDataMember]
        public Dictionary<ModelEventType, List<object>> ModelEvents { get; set; }

        [XmlIgnore]
        [IgnoreDataMember]
        public Dictionary<ModelEventType, List<object>> ModelContextEvents { get; set; }

        [XmlIgnore]
        [IgnoreDataMember]
        public ModelNodeState State { get; set; }

        [XmlIgnore]
        [NonSerialized]
        [IgnoreDataMember]
        private readonly TraceServiceBase TraceService;

        #endregion

        #region events support

        public delegate void TmpAction(object arg1, object arg2);

        public virtual void InvokeOnModelContextEvents(object sender, ModelEventArgs eventArgs)
        {
            var objectType = typeof(object);
            var eventType = eventArgs.EventType;

            // type.. can be null, so?
            var spObjectType = eventArgs.ObjectType;

            if (!ModelContextEvents.ContainsKey(eventType))
            {
                TraceService.VerboseFormat((int)LogEventId.CoreCalls, "Cannot find ModelContextEvents for eventType: [{0}]", eventType);
                return;
            }
            else
            {
                TraceService.VerboseFormat((int)LogEventId.CoreCalls, "Calling ModelContextEvents for eventType: [{0}]", eventType);
            }

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
                    TraceService.ErrorFormat((int)LogEventId.ModelTreeModelContextEventIsNull,
                        "Cannot find model content instance for method: [{0}]. Throwing SPMeta2Exception.", action.Method);

                    throw new SPMeta2Exception("Cannot find a proper ModelContextEvents overload");
                }

                TraceService.VerboseFormat((int)LogEventId.CoreCalls, "Setting property: [Model]: [{0}]", eventArgs.Model);
                SetProperty(modelContextInstance, "Model", eventArgs.Model);

                TraceService.VerboseFormat((int)LogEventId.CoreCalls, "Setting property: [CurrentModelNode]: [{0}]", eventArgs.CurrentModelNode);
                SetProperty(modelContextInstance, "CurrentModelNode", eventArgs.CurrentModelNode);

                TraceService.VerboseFormat((int)LogEventId.CoreCalls, "Setting property: [Object]: [{0}]", eventArgs.Object);
                SetProperty(modelContextInstance, "Object", eventArgs.Object);

                TraceService.VerboseFormat((int)LogEventId.CoreCalls, "Setting property: [ObjectDefinition]: [{0}]", eventArgs.ObjectDefinition);
                SetProperty(modelContextInstance, "ObjectDefinition", eventArgs.ObjectDefinition);

                TraceService.VerboseFormat((int)LogEventId.CoreCalls, "Setting property: [ModelHost]: [{0}]", eventArgs.ModelHost);
                SetProperty(modelContextInstance, "ModelHost", eventArgs.ModelHost);

                TraceService.Verbose((int)LogEventId.CoreCalls, "Invoking event.");
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

        #region methods

        public override string ToString()
        {
            return Value != null
                        ? string.Format("Node value: [{0}] - {1}", Value.GetType().Name, Value)
                        : base.ToString();
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
        /// Current model host.
        /// </summary>
        public object ModelHost { get; set; }

        /// <summary>
        /// Current model node.
        /// </summary>
        public ModelNode CurrentModelNode { get; set; }

        /// <summary>
        /// Exception handling options.
        /// </summary>
        public ContinuationOptions ContinuationOption { get; set; }

#if !NET35

        /// <summary>
        /// Aggregated exception, if any.
        /// </summary>
        public AggregateException Error { get; set; }

#endif
    }
}
