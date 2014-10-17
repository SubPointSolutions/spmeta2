using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default.Modern
{
    public static class ModernSyntax
    {
        #region on error

        public static ModelNode OnError<TObjectType>(this ModelNode model,
          Action<OnCreatingContext<TObjectType, DefinitionBase>> action)
        {
            return OnError<TObjectType, DefinitionBase>(model, action);
        }

        public static ModelNode OnError<TObjectType, TDefinitionType>(this ModelNode model,
            Action<OnCreatingContext<TObjectType, TDefinitionType>> action)
            where TDefinitionType : DefinitionBase
        {
            model.RegisterModelContextEvent(ModelEventType.OnError, action);

            return model;
        }

        #endregion

        #region on provisioned

        public static ModelNode OnProvisioned<TObjectType>(this ModelNode model,
          Action<OnCreatingContext<TObjectType, DefinitionBase>> action)
        {
            return OnProvisioned<TObjectType, DefinitionBase>(model, action);
        }

        public static ModelNode OnProvisioned<TObjectType, TDefinitionType>(this ModelNode model,
            Action<OnCreatingContext<TObjectType, TDefinitionType>> action)
            where TDefinitionType : DefinitionBase
        {
            model.RegisterModelContextEvent(ModelEventType.OnProvisioned, action);

            return model;
        }

        #endregion

        #region on provisioning

        public static ModelNode OnProvisioning<TObjectType>(this ModelNode model,
          Action<OnCreatingContext<TObjectType, DefinitionBase>> action)
        {
            return OnProvisioning<TObjectType, DefinitionBase>(model, action);
        }

        public static ModelNode OnProvisioning<TObjectType, TDefinitionType>(this ModelNode model,
            Action<OnCreatingContext<TObjectType, TDefinitionType>> action)
            where TDefinitionType : DefinitionBase
        {
            model.RegisterModelContextEvent(ModelEventType.OnProvisioning, action);

            return model;
        }

        #endregion

        #region on model host resolving

        public static ModelNode OnModelHostResolving<TObjectType>(this ModelNode model,
          Action<OnCreatingContext<TObjectType, DefinitionBase>> action)
        {
            return OnModelHostResolving<TObjectType, DefinitionBase>(model, action);
        }

        public static ModelNode OnModelHostResolving<TObjectType, TDefinitionType>(this ModelNode model,
            Action<OnCreatingContext<TObjectType, TDefinitionType>> action)
            where TDefinitionType : DefinitionBase
        {
            model.RegisterModelContextEvent(ModelEventType.OnModelHostResolving, action);

            return model;
        }

        #endregion

        #region on model host resolved

        public static ModelNode OnModelHostResolved<TObjectType>(this ModelNode model,
          Action<OnCreatingContext<TObjectType, DefinitionBase>> action)
        {
            return OnModelHostResolved<TObjectType, DefinitionBase>(model, action);
        }

        public static ModelNode OnModelHostResolved<TObjectType, TDefinitionType>(this ModelNode model,
            Action<OnCreatingContext<TObjectType, TDefinitionType>> action)
            where TDefinitionType : DefinitionBase
        {
            model.RegisterModelContextEvent(ModelEventType.OnModelHostResolved, action);

            return model;
        }

        #endregion
    }
}
