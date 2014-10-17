using System;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Events;
using SPMeta2.Models;

namespace SPMeta2.ModelHandlers
{
    /// <summary>
    /// Base handler for model provision.
    /// </summary>
    public abstract class ModelHandlerBase
    {
        #region constructors


        #endregion

        #region properties


        /// <summary>
        /// Type of the definition which is handled by current model handler.
        /// </summary>
        public abstract Type TargetType { get; }

        #endregion

        #region events

        public EventHandler<ModelEventArgs> OnModelEvent;

        #endregion

        #region methods

        /// <summary>
        /// Handles model provision under particular modelHost. 
        /// </summary>
        /// <param name="modelHost"></param>
        /// <param name="model"></param>
        public virtual void DeployModel(object modelHost, DefinitionBase model)
        {

        }

        /// <summary>
        /// Handles model retraction under particular model host.
        /// </summary>
        /// <param name="modelHost"></param>
        /// <param name="model"></param>
        public virtual void RetractModel(object modelHost, DefinitionBase model)
        {

        }

        /// <summary>
        /// Resolves a new model host per particular child definition type.
        /// </summary>
        /// <param name="modelHost"></param>
        /// <param name="model"></param>
        /// <param name="childModelType"></param>
        /// <param name="action"></param>
        [Obsolete("Use WithResolvingModelHost(ModelHostContext context) method instead.")]
        public virtual void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            action(modelHost);
        }

        /// <summary>
        /// Resolves a new model host per particular child definition type.
        /// </summary>
        /// <param name="context"></param>
        public virtual void WithResolvingModelHost(ModelHostResolveContext context)
        {
            WithResolvingModelHost(
                context.ModelHost,
                context.Model,
                context.ChildModelType,
                context.Action);
        }


        /// <summary>
        /// Promotes a model event outside of the model handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void InvokeOnModelEvent(object sender, ModelEventArgs args)
        {
            if (OnModelEvent != null)
            {
                OnModelEvent.Invoke(this, args);
            }
        }

        /// <summary>
        /// Promotes a model event outside of the model handler.
        /// </summary>
        /// <typeparam name="TModelDefinition"></typeparam>
        /// <typeparam name="TSPObject"></typeparam>
        /// <param name="rawObject"></param>
        /// <param name="eventType"></param>
        [Obsolete("Use InvokeOnModelEvents((object sender, ModelEventArgs args) with passing full ModelEventArgs")]
        protected void InvokeOnModelEvent<TModelDefinition, TSPObject>(TSPObject rawObject, ModelEventType eventType)
        {
            InvokeOnModelEvent(this, new ModelEventArgs
            {
                Object = rawObject,
                EventType = eventType
            });
        }

        #endregion


    }
}
