using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientListViewDefinitionValidator : ListViewModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var list = modelHost.WithAssertAndCast<List>("modelHost", value => value.RequireNotNull());
            var listViewModel = model.WithAssertAndCast<ListViewDefinition>("model", value => value.RequireNotNull());

            var context = list.Context;

            context.Load(list, l => l.Views);
            context.ExecuteQuery();

            TraceUtils.WithScope(traceScope =>
            {
                var currentView = FindViewByTitle(list.Views, listViewModel.Title);
                traceScope.WriteLine(string.Format("Validate model:[{0}] list view:[{1}]", listViewModel, currentView));

                traceScope.WithTraceIndent(trace =>
                {
                    trace.WriteLine(string.Format("Validate Title: model:[{0}] list view:[{1}]", listViewModel.Title, currentView.Title));
                    Assert.AreEqual(listViewModel.Title, currentView.Title);

                    trace.WriteLine(string.Format("Validate RowLimit: model:[{0}] list view:[{1}]", listViewModel.RowLimit, currentView.RowLimit));
                    Assert.AreEqual((uint)listViewModel.RowLimit, (uint)currentView.RowLimit);

                    trace.WriteLine(string.Format("Validate IsDefault: model:[{0}] list view:[{1}]", listViewModel.IsDefault, currentView.DefaultView));
                    Assert.AreEqual(listViewModel.IsDefault, currentView.DefaultView);

                    trace.WriteLine(string.Format("Validate fields.."));

                    context.Load(currentView, v => v.ViewFields);
                    context.ExecuteQuery();

                    traceScope.WithTraceIndent(fieldTrace =>
                    {
                        foreach (var fieldName in listViewModel.Fields)
                        {
                            trace.WriteLine(string.Format("Validate field presence: [{0}]", fieldName));

                            Assert.IsTrue(DoesFieldExist(currentView.ViewFields, fieldName));
                            trace.WriteLine(string.Format("Field [{0}] exists in view. [OK].", fieldName));
                        }
                    });
                });
            });
        }

        protected bool DoesFieldExist(ViewFieldCollection viewFields, string fieldName)
        {
            foreach (var viewField in viewFields)
            {
                if (System.String.Compare(viewField, fieldName, System.StringComparison.OrdinalIgnoreCase) == 0)
                    return true;
            }

            return false;
        }
    }
}
