using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.Services;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;
using System;
using System.Linq;
using System.Collections.Generic;
using SPMeta2.ModelHandlers;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Utils;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Common;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{

    [TestClass]
    public class CustomDefinitions : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.Models")]
        public void RegisterCustomModelHandler()
        {
            var csomProvisionService = new CSOMProvisionService();

            csomProvisionService.RegisterModelHandler(new ChangeWebTitleAndDescriptionModelHandler());

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddDefinitionNode(new ChangeWebTitleAndDescriptionDefinition
                {
                    Title = "A new name for the web",
                    Description = "Some changes done by ChangeWebTitleAndDescriptionDefinition"
                });
            });

            using (var clientContext = new ClientContext(CSOMSiteUrl))
                csomProvisionService.DeployWebModel(clientContext, webModel);
        }

        [TestMethod]
        [TestCategory("Docs.Models")]
        public void RegisterCustomModelHandlerWithSyntax()
        {
            var csomProvisionService = new CSOMProvisionService();

            csomProvisionService.RegisterModelHandler(new ChangeWebTitleAndDescriptionModelHandler());

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddChangeWebTitleAndDescription(new ChangeWebTitleAndDescriptionDefinition
                {
                    Title = "A new name for the web",
                    Description = "Some changes done by ChangeWebTitleAndDescriptionDefinition"
                });
            });

            using (var clientContext = new ClientContext(CSOMSiteUrl))
                csomProvisionService.DeployWebModel(clientContext, webModel);
        }

        [TestMethod]
        [TestCategory("Docs.Models")]
        public void RegisterCustomModelHandlerWithEvents()
        {
            var csomProvisionService = new CSOMProvisionService();

            csomProvisionService.RegisterModelHandler(new ChangeWebTitleAndDescriptionModelHandler());

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddChangeWebTitleAndDescription(new ChangeWebTitleAndDescriptionDefinition
                {
                    Title = "A new name for the web",
                    Description = "Some changes done by ChangeWebTitleAndDescriptionDefinition"
                },
                changeWebAndTitle =>
                {
                    changeWebAndTitle.OnProvisioning<Web, ChangeWebTitleAndDescriptionDefinition>(cntx =>
                    {
                        var cntxWeb = cntx.Object;
                        var cntxDef = cntx.ObjectDefinition;

                        // do stuff
                    });

                    changeWebAndTitle.OnProvisioned<Web, ChangeWebTitleAndDescriptionDefinition>(cntx =>
                    {
                        var cntxWeb = cntx.Object;
                        var cntxDef = cntx.ObjectDefinition;

                        // do stuff
                    });
                });
            });

            using (var clientContext = new ClientContext(CSOMSiteUrl))
                csomProvisionService.DeployWebModel(clientContext, webModel);
        }

        #endregion
    }

    [Serializable]
    public class ChangeWebTitleAndDescriptionDefinition : DefinitionBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class ChangeWebTitleAndDescriptionModelHandler : CSOMModelHandlerBase
    {
        public override Type TargetType
        {
            get { return typeof(ChangeWebTitleAndDescriptionDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModeHost = modelHost.WithAssertAndCast<WebModelHost>(
                                        "model",
                                        value => value.RequireNotNull());

            var definition = model.WithAssertAndCast<ChangeWebTitleAndDescriptionDefinition>(
                                        "model",
                                        value => value.RequireNotNull());

            var currentWeb = webModeHost.HostWeb;
            var context = currentWeb.Context;

            // raise OnProvisioning event
            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentWeb,
                ObjectType = typeof(Web),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            // do stuff
            currentWeb.Title = definition.Title;
            currentWeb.Description = definition.Description;

            // raise OnProvisioned event
            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentWeb,
                ObjectType = typeof(Web),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            currentWeb.Update();
            context.ExecuteQuery();
        }
    }

    public static class ChangeWebTitleAndDescriptionDefinitionSyntax
    {
        public static ModelNode AddChangeWebTitleAndDescription(this ModelNode model,
            ChangeWebTitleAndDescriptionDefinition definition)
        {
            return AddChangeWebTitleAndDescription(model, definition, null);
        }

        public static ModelNode AddChangeWebTitleAndDescription(this ModelNode model,
            ChangeWebTitleAndDescriptionDefinition definition, Action<ModelNode>
            action)
        {
            return model.AddDefinitionNode(definition, action);
        }
    }
}