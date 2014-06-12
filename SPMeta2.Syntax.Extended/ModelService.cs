using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Extended.Definitions;
using SPMeta2.CSOM.DefaultSyntax;

namespace SPMeta2.Syntax.Extended
{
    public class ModelService
    {
        #region methods

        public ModelNode GetSiteModel()
        {
            var model = SPMeta2Model
                .NewSiteModel(site =>
                {
                    site
                        .AddField(FieldModels.ClientFeedback, field =>
                        {
                            field.OnCreated((FieldDefinition def, Field spField) =>
                            {
                                spField
                                    .Title = "new title";
                            });

                            field.OnCreated<Field>(context =>
                            {
                                context.Object
                                    .Title = "new title";

                            });

                            field.OnError<Field, FieldDefinition>(context =>
                            {

                            });

                            field.OnCreated<Field, FieldDefinition>(context =>
                            {
                                context.Object
                                   .Title = "new title";

                                context.ObjectDefinition.InternalName = "sds";
                            });
                        });
                });

            return model;
        }

        #endregion
    }

    public static class ExtendedSyntax
    {
        public static ModelNode OnCreated<TObjectType>(this ModelNode model,
            Action<OnCreatingContext<TObjectType, DefinitionBase>> action)
        {
            return OnCreated<TObjectType, DefinitionBase>(model, action);
        }

        public static ModelNode OnCreated<TObjectType, TDefinitionType>(this ModelNode model,
            Action<OnCreatingContext<TObjectType, TDefinitionType>> action)
            where TDefinitionType : DefinitionBase
        {
            model.RegisterModelContextEvent(ModelEventType.OnUpdated, action);

            return model;
        }

        public static ModelNode OnCreating<TObjectType>(this ModelNode model,
            Action<OnCreatingContext<TObjectType, DefinitionBase>> action)
        {
            return OnCreating<TObjectType, DefinitionBase>(model, action);
        }

        public static ModelNode OnCreating<TObjectType, TDefinitionType>(this ModelNode model,
            Action<OnCreatingContext<TObjectType, TDefinitionType>> action)
            where TDefinitionType : DefinitionBase
        {
            model.RegisterModelContextEvent(ModelEventType.OnUpdating, action);

            return model;
        }

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
    }
}
