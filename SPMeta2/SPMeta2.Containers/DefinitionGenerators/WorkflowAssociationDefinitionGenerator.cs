using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class WorkflowAssociationDefinitionGenerator : TypedDefinitionGeneratorServiceBase<WorkflowAssociationDefinition>
    {
        #region constructors

        public WorkflowAssociationDefinitionGenerator()
        {
            WorkflowName = Rnd.String();

            HistoryList = new ListDefinition
            {
                Title = Rnd.String(),
                Hidden = true,
                TemplateType = BuiltInListTemplateTypeId.WorkflowHistory,
#pragma warning disable 618
                Url = Rnd.String()
#pragma warning restore 618
            };

            TaskList = new ListDefinition
            {
                Title = Rnd.String(),
                Hidden = true,
                TemplateType = BuiltInListTemplateTypeId.Tasks,
#pragma warning disable 618
                Url = Rnd.String()
#pragma warning restore 618
            };
        }

        #endregion

        #region properties

        public string WorkflowName { get; set; }

        public ListDefinition HistoryList { get; set; }
        public ListDefinition TaskList { get; set; }

        #endregion

        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Name = Rnd.String();
                def.Description = Rnd.String();

                def.WorkflowTemplateName = BuiltInWorkflowNames.ApprovalSharePoint2010;

                def.TaskListTitle = TaskList.Title;
                def.HistoryListTitle = HistoryList.Title;
            });
        }


        public override IEnumerable<DefinitionBase> GetAdditionalArtifacts()
        {
            return new[] { 
                        BuiltInSiteFeatures.SharePoint2007Workflows.Inherit(def =>
                        {
                            def.Enable = true;
                        }),

                        BuiltInSiteFeatures.Workflows.Inherit(def =>
                        {
                            def.Enable = true;
                        }),

                        BuiltInSiteFeatures.DispositionApprovalWorkflow.Inherit(def =>
                        {
                            def.Enable = true;
                        }),

                        BuiltInSiteFeatures.ThreeStateWorkflow.Inherit(def =>
                        {
                            def.Enable = true;
                        }),

                        TaskList as DefinitionBase,
                        HistoryList as DefinitionBase
            };
        }
    }
}
