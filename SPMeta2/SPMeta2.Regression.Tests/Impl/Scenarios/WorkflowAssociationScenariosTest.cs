using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers.DefinitionGenerators;
using SPMeta2.Containers.Services;

using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Syntax.Default.Utils;
using SPMeta2.Utils;
using SPMeta2.Exceptions;

using SPMeta2.Containers.Extensions;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class WorkflowAssociationScenariosTest : SPMeta2RegresionScenarioTestBase
    {
        #region common

        [ClassInitializeAttribute]
        public static void Init(TestContext context)
        {
            InternalInit();
        }

        [ClassCleanupAttribute]
        public static void Cleanup()
        {
            InternalCleanup();
        }

        #endregion

        //[TestMethod]
        //[TestCategory("Regression.Scenarios. WorkflowAssociation")]
        //public void CanDeploy_WorkflowAssociation_UnderSite()
        //{
        //    var workflowDef = ModelGeneratorService.GetRandomDefinition<WorkflowAssociationDefinition>(def =>
        //    {

        //    });

        //    var model = SPMeta2Model.NewSiteModel(site =>
        //    {
        //        site.AddWorkflowAssociation(workflowDef);
        //    });

        //    TestModel(model);
        //}



        [TestMethod]
        [TestCategory("Regression.Scenarios.WorkflowAssociation")]
        public void CanDeploy_WorkflowAssociation_UnderWeb()
        {
            var taskList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.Hidden = true;
                def.TemplateType = BuiltInListTemplateTypeId.Tasks;
            });

            var historyList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.Hidden = true;
                def.TemplateType = BuiltInListTemplateTypeId.WorkflowHistory;
            });

            var workflowDef = ModelGeneratorService.GetRandomDefinition<WorkflowAssociationDefinition>(def =>
            {
                def.TaskListTitle = taskList.Title;
                def.HistoryListTitle = historyList.Title;
            });

            // changability 
            // deploy the same association with different props
            var workflowDefChanges = workflowDef.Inherit(def =>
            {
                var value = Rnd.Bool();

                def.AllowManual = value;
                def.AutoStartChange = !value;
                def.AutoStartCreate = value;

                def.AssociationData = Rnd.String();
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                AddDefaultWorkflowFeatures(site);
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(taskList);
                web.AddList(historyList);

                web.AddWorkflowAssociation(workflowDef);
                web.AddWorkflowAssociation(workflowDefChanges);
            });

            TestModel(siteModel, model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.WorkflowAssociation")]
        public void CanDeploy_WorkflowAssociation_UnderList()
        {
            var taskList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.Hidden = true;
                def.TemplateType = BuiltInListTemplateTypeId.Tasks;
            });

            var historyList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.Hidden = true;
                def.TemplateType = BuiltInListTemplateTypeId.WorkflowHistory;
            });

            var workflowDef = ModelGeneratorService.GetRandomDefinition<WorkflowAssociationDefinition>(def =>
            {
                def.TaskListTitle = taskList.Title;
                def.HistoryListTitle = historyList.Title;
            });

            // changability 
            // deploy the same association with different props
            var workflowDefChanges = workflowDef.Inherit(def =>
            {
                var value = Rnd.Bool();

                def.AllowManual = value;
                def.AutoStartChange = !value;
                def.AutoStartCreate = value;

                def.AssociationData = Rnd.String();
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
           {
               AddDefaultWorkflowFeatures(site);
           });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(taskList);
                web.AddList(historyList);

                web.AddRandomList(list =>
                {
                    list.AddWorkflowAssociation(workflowDef);
                    list.AddWorkflowAssociation(workflowDefChanges);
                });
            });

            TestModel(siteModel, model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.WorkflowAssociation")]
        public void CanDeploy_WorkflowAssociation_UnderContentTypeLink()
        {
            var contentTypeDef = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {

            });

            var taskList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.Hidden = true;
                def.TemplateType = BuiltInListTemplateTypeId.Tasks;
                def.ContentTypesEnabled = true;
            });

            var historyList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.Hidden = true;
                def.TemplateType = BuiltInListTemplateTypeId.WorkflowHistory;
            });

            var workflowDef = ModelGeneratorService.GetRandomDefinition<WorkflowAssociationDefinition>(def =>
            {
                def.TaskListTitle = taskList.Title;
                def.HistoryListTitle = historyList.Title;
            });

            // changability 
            // deploy the same association with different props
            var workflowDefChanges = workflowDef.Inherit(def =>
            {
                var value = Rnd.Bool();

                def.AllowManual = value;
                def.AutoStartChange = !value;
                def.AutoStartCreate = value;

                def.AssociationData = Rnd.String();
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddContentType(contentTypeDef);
                AddDefaultWorkflowFeatures(site);
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(taskList);
                web.AddList(historyList);

                web.AddRandomList(list =>
                {
                    ((list.Value) as ListDefinition).ContentTypesEnabled = true;

                    list.AddContentTypeLink(contentTypeDef, contentTypeLink =>
                    {
                        contentTypeLink.AddWorkflowAssociation(workflowDef);
                        contentTypeLink.AddWorkflowAssociation(workflowDefChanges);
                    });
                });
            });

            TestModel(siteModel, model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.WorkflowAssociation")]
        public void CanDeploy_WorkflowAssociation_UnderContentTypeLink_ReadOnly()
        {
            var contentTypeDef = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ReadOnly = true;
            });

            var taskList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.Hidden = true;
                def.TemplateType = BuiltInListTemplateTypeId.Tasks;
                def.ContentTypesEnabled = true;
            });

            var historyList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.Hidden = true;
                def.TemplateType = BuiltInListTemplateTypeId.WorkflowHistory;
            });

            var workflowDef = ModelGeneratorService.GetRandomDefinition<WorkflowAssociationDefinition>(def =>
            {
                def.TaskListTitle = taskList.Title;
                def.HistoryListTitle = historyList.Title;
            });

            // changability 
            // deploy the same association with different props
            var workflowDefChanges = workflowDef.Inherit(def =>
            {
                var value = Rnd.Bool();

                def.AllowManual = value;
                def.AutoStartChange = !value;
                def.AutoStartCreate = value;

                def.AssociationData = Rnd.String();
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddContentType(contentTypeDef, contentType =>
                {
                    contentType.RegExcludeFromValidation();
                });

                AddDefaultWorkflowFeatures(site);
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(taskList);
                web.AddList(historyList);

                web.AddRandomList(list =>
                {
                    list.AddContentTypeLink(contentTypeDef, contentTypeLink =>
                    {
                        contentTypeLink.AddWorkflowAssociation(workflowDef);
                        contentTypeLink.AddWorkflowAssociation(workflowDefChanges);
                    });
                });
            });

            TestModel(siteModel, model);
        }

        private void AddDefaultWorkflowFeatures(SiteModelNode site)
        {
            site.AddSiteFeature(BuiltInSiteFeatures.Workflows.Inherit(f =>
            {
                f.Enable = true;
            }));

            site.AddSiteFeature(BuiltInSiteFeatures.SharePoint2007Workflows.Inherit(f =>
            {
                f.Enable = true;
            }));

            //site.AddSiteFeature(BuiltInSiteFeatures.DispositionApprovalWorkflow.Inherit(f =>
            //{
            //    f.Enable = true;
            //}));

            //site.AddSiteFeature(BuiltInSiteFeatures.PublishingApprovalWorkflow.Inherit(f =>
            //{
            //    f.Enable = true;
            //}));

            //site.AddSiteFeature(BuiltInSiteFeatures.ThreeStateWorkflow.Inherit(f =>
            //{
            //    f.Enable = true;
            //}));
        }



        [TestMethod]
        [TestCategory("Regression.Scenarios.WorkflowAssociation")]
        public void CanDeploy_WorkflowAssociation_UnderContentType()
        {
            // Enhance WorkflowAssociationDefinition - support deployment under content type #867
            // https://github.com/SubPointSolutions/spmeta2/issues/867

            var taskList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.Hidden = true;
                def.TemplateType = BuiltInListTemplateTypeId.Tasks;
            });

            var historyList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.Hidden = true;
                def.TemplateType = BuiltInListTemplateTypeId.WorkflowHistory;
            });

            var workflowDef = ModelGeneratorService.GetRandomDefinition<WorkflowAssociationDefinition>(def =>
            {
                def.TaskListTitle = taskList.Title;
                def.HistoryListTitle = historyList.Title;
            });

            var initialSiteModel = SPMeta2Model.NewSiteModel(site =>
            {
                AddDefaultWorkflowFeatures(site);
            });

            // changability 
            // deploy the same association with different props
            var workflowDefChanges = workflowDef.Inherit(def =>
            {
                var value = Rnd.Bool();

                def.AllowManual = value;
                def.AutoStartChange = !value;
                def.AutoStartCreate = value;

                def.AssociationData = Rnd.String();
            });

            // lists are to be deployed before contet type
            // workflow association on the cotnent type references lists
            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(historyList);
                web.AddList(taskList);
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomContentType(contentType =>
                {
                    contentType.AddWorkflowAssociation(workflowDef);
                    contentType.AddWorkflowAssociation(workflowDefChanges);
                });
            });

            TestModels(new ModelNode[] { 
                initialSiteModel,
                webModel,
                siteModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.WorkflowAssociation")]
        public void CanDeploy_WorkflowAssociation_UnderContentType_ReadOnly()
        {
            // .AddWorkflowAssociation() for Read Only Content Types throws error #1001
            // https://github.com/SubPointSolutions/spmeta2/issues/1001

            var taskList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.Hidden = true;
                def.TemplateType = BuiltInListTemplateTypeId.Tasks;
            });

            var historyList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.Hidden = true;
                def.TemplateType = BuiltInListTemplateTypeId.WorkflowHistory;
            });

            var workflowDef = ModelGeneratorService.GetRandomDefinition<WorkflowAssociationDefinition>(def =>
            {
                def.TaskListTitle = taskList.Title;
                def.HistoryListTitle = historyList.Title;
            });

            var initialSiteModel = SPMeta2Model.NewSiteModel(site =>
            {
                AddDefaultWorkflowFeatures(site);
            });

            var readOnlyContentTypeDef = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ReadOnly = true;
            });

            // changability 
            // deploy the same association with different props
            var workflowDefChanges = workflowDef.Inherit(def =>
            {
                var value = Rnd.Bool();

                def.AllowManual = value;
                def.AutoStartChange = !value;
                def.AutoStartCreate = value;

                def.AssociationData = Rnd.String();
            });

            // lists are to be deployed before contet type
            // workflow association on the cotnent type references lists
            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(historyList);
                web.AddList(taskList);
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddContentType(readOnlyContentTypeDef, contentType =>
                {
                    contentType.RegExcludeFromValidation();

                    contentType.AddWorkflowAssociation(workflowDef);
                    contentType.AddWorkflowAssociation(workflowDefChanges);
                });
            });

            TestModels(new ModelNode[] { 
                initialSiteModel,
                webModel,
                siteModel });
        }
    }
}
