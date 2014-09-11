using System;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Events;

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

        public abstract Type TargetType { get; }

        #endregion

        #region events

        public EventHandler<ModelEventArgs> OnModelEvent;

        //public EventHandler<ModelDefinitionEventArgs> OnDeployingModel;
        //public EventHandler<ModelDefinitionEventArgs> OnDeployedModel;

        //public EventHandler<ModelDefinitionEventArgs> OnRetractingModel;
        //public EventHandler<ModelDefinitionEventArgs> OnRetractedModel;

        //protected virtual void InvokeOnDeployingModel(DefinitionBase model)
        //{
        //    if (OnDeployingModel != null) OnDeployingModel(this, new ModelDefinitionEventArgs { Model = model });
        //}

        //protected virtual void InvokeOnDeployedModel(DefinitionBase model)
        //{
        //    if (OnDeployedModel != null) OnDeployedModel(this, new ModelDefinitionEventArgs { Model = model });
        //}

        //protected virtual void InvokeOnRetractingModel(DefinitionBase model)
        //{
        //    if (OnRetractingModel != null) OnRetractingModel(this, new ModelDefinitionEventArgs { Model = model });
        //}

        //protected virtual void InvokeOnRetractedModel(DefinitionBase model)
        //{
        //    if (OnRetractedModel != null) OnRetractedModel(this, new ModelDefinitionEventArgs { Model = model });
        //}


        #endregion

        #region methods

        public virtual void DeployModel(object modelHost, DefinitionBase model)
        {
            WithDeployModelEvents(model, m => DeployModelInternal(modelHost, m));
        }

        public virtual void RetractModel(object modelHost, DefinitionBase model)
        {
            WithRetractingModelEvents(model, m => RetractModelInternal(modelHost, m));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelHost"></param>
        /// <param name="model"></param>
        /// <param name="childModelType"></param>
        /// <param name="action"></param>
        public virtual void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            action(modelHost);
        }

        protected virtual void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            //throw new NotImplementedException("DeployModelInternal");
        }

        protected virtual void RetractModelInternal(object modelHost, DefinitionBase model)
        {
            //throw new NotImplementedException("RetractModelInternal");
        }

        protected void WithDeployModelEvents(DefinitionBase model, Action<DefinitionBase> action)
        {
            //InvokeOnDeployingModel(model);

            action(model);

            //InvokeOnDeployedModel(model);
        }

        protected void WithRetractingModelEvents(DefinitionBase model, Action<DefinitionBase> action)
        {
            //InvokeOnModelEvents(model, );

            action(model);

            //InvokeOnRetractedModel(model);
        }

        protected void InvokeOnModelEvent(object sender, ModelEventArgs args)
        {
            if (OnModelEvent != null)
            {
                OnModelEvent.Invoke(this, args);
            }
        }

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
