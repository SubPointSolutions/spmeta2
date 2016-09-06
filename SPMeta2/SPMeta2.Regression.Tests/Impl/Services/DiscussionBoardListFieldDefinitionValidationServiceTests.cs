using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Services;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Services.Impl;
using SPMeta2.Syntax.Default;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Services.Impl.Validation;

namespace SPMeta2.Regression.Tests.Impl.Services
{
    [TestClass]
    public class DiscussionBoardListFieldDefinitionValidationServiceTests : SPMeta2DefinitionRegresionTestBase
    {
        #region constructors

        public DiscussionBoardListFieldDefinitionValidationServiceTests()
        {
            Service = new DiscussionBoardListFieldDefinitionValidationService();
        }

        #endregion

        #region properties

        public DiscussionBoardListFieldDefinitionValidationService Service { get; set; }

        #endregion


        #region tests

        [TestMethod]
        [TestCategory("Regression.Services.DiscussionBoardListFieldDefinitionValidationService")]
        [TestCategory("CI.Core")]
        public void ShouldPass_On_Non_DiscussionBoardList()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomList();
                web.AddRandomList();
            });

            Service.DeployModel(null, model);
        }

        [TestMethod]
        [TestCategory("Regression.Services.DiscussionBoardListFieldDefinitionValidationService")]
        [TestCategory("CI.Core")]
        public void ShouldFail_On_DiscussionBoardList_WithoutContenTypes_ByTemplateType()
        {
            var isValid = false;

            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(list =>
            {
                list.TemplateType = BuiltInListTemplateTypeId.DiscussionBoard;
                list.ContentTypesEnabled = false;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(listDef);
            });

            try
            {
                Service.DeployModel(null, model);
            }
            catch (Exception e)
            {
                isValid = IsCorrectValidationException(e);
            }

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("Regression.Services.DiscussionBoardListFieldDefinitionValidationService")]
        [TestCategory("CI.Core")]
        public void ShouldFail_On_DiscussionBoardList_WithoutContenTypes_ByTemplateName()
        {
            var isValid = false;

            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(list =>
            {
                list.TemplateName = BuiltInListTemplates.DiscussionBoard.InternalName;
                list.ContentTypesEnabled = false;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(listDef);
            });

            try
            {
                Service.DeployModel(null, model);
            }
            catch (Exception e)
            {
                isValid = IsCorrectValidationException(e);
            }

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("Regression.Services.DiscussionBoardListFieldDefinitionValidationService")]
        [TestCategory("CI.Core")]
        public void ShouldPass_On_DiscussionBoardList_WithoutContenTypes_ByTemplateType()
        {
            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(list =>
            {
                list.TemplateType = BuiltInListTemplateTypeId.DiscussionBoard;
                list.ContentTypesEnabled = true;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(listDef);
            });

            Service.DeployModel(null, model);
        }

        [TestMethod]
        [TestCategory("Regression.Services.DiscussionBoardListFieldDefinitionValidationService")]
        [TestCategory("CI.Core")]
        public void ShouldPass_On_DiscussionBoardList_WithoutContenTypes_ByTemplateName()
        {
            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(list =>
            {
                list.TemplateName = BuiltInListTemplates.DiscussionBoard.InternalName;
                list.ContentTypesEnabled = true;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(listDef);
            });

            Service.DeployModel(null, model);
        }

        #endregion
    }
}
