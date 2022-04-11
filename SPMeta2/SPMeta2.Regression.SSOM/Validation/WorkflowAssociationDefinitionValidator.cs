using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class WorkflowAssociationDefinitionValidator : WorkflowAssociationModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<WorkflowAssociationDefinition>("model", value => value.RequireNotNull());
            var spObject = FindExistringWorkflowAssotiation(modelHost, definition);

            var assert = ServiceFactory.AssertService.NewAssert(definition, spObject);

            assert
                .ShouldNotBeNull(spObject)
                .ShouldBeEqual(m => m.Name, o => o.Name);

            if (!string.IsNullOrEmpty(definition.WorkflowTemplateName))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.WorkflowTemplateName);
                    var isValid = s.WorkflowTemplateName == d.BaseTemplate.Name;

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.WorkflowTemplateName);
            }

            if (!string.IsNullOrEmpty(definition.Description))
                assert.ShouldBeEqual(m => m.Description, o => o.Description);
            else
                assert.SkipProperty(m => m.Description);

            if (!string.IsNullOrEmpty(definition.AssociationData))
                assert.ShouldBeEqual(m => m.AssociationData, o => o.AssociationData);
            else
                assert.SkipProperty(m => m.AssociationData);

            if (definition.Enabled.HasValue)
                assert.ShouldBeEqual(m => m.Enabled, o => o.Enabled);
            else
                assert.SkipProperty(m => m.Enabled);

            if (definition.AllowManual.HasValue)
                assert.ShouldBeEqual(m => m.AllowManual, o => o.AllowManual);
            else
                assert.SkipProperty(m => m.AllowManual);

            if (definition.AutoStartChange.HasValue)
                assert.ShouldBeEqual(m => m.AutoStartChange, o => o.AutoStartChange);
            else
                assert.SkipProperty(m => m.AutoStartChange);

            if (definition.AutoStartCreate.HasValue)
                assert.ShouldBeEqual(m => m.AutoStartCreate, o => o.AutoStartCreate);
            else
                assert.SkipProperty(m => m.AutoStartCreate);

            if (!string.IsNullOrEmpty(definition.TaskListTitle))
                assert.ShouldBeEqual(m => m.TaskListTitle, o => o.TaskListTitle);
            else
                assert.SkipProperty(m => m.TaskListTitle);

            if (!string.IsNullOrEmpty(definition.HistoryListTitle))
                assert.ShouldBeEqual(m => m.HistoryListTitle, o => o.HistoryListTitle);
            else
                assert.SkipProperty(m => m.HistoryListTitle);
        }
    }
}
