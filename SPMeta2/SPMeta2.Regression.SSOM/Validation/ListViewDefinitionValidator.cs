using System;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.SSOM.Extensions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class ListViewDefinitionValidator : ListViewModelHandler
    {
        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var listViewModel = model.WithAssertAndCast<ListViewDefinition>("model", value => value.RequireNotNull());

            var list = listModelHost.CurrentList;

            TraceUtils.WithScope(traceScope =>
            {
                var spView = list.Views.FindByName(listViewModel.Title);

                traceScope.WriteLine(string.Format("Validate model:[{0}] view:[{1}]", listViewModel, spView));

                // assert base properties
                traceScope.WithTraceIndent(trace =>
                {
                    trace.WriteLine(string.Format("Validate Title: model:[{0}] view:[{1}]", listViewModel.Title, spView.Title));
                    Assert.AreEqual(listViewModel.Title, spView.Title);

                    trace.WriteLine(string.Format("Validate IsDefault: model:[{0}] view:[{1}]", listViewModel.IsDefault, spView.DefaultView));
                    Assert.AreEqual(listViewModel.IsDefault, spView.DefaultView);

                    trace.WriteLine(string.Format("Validate IsPaged: model:[{0}] view:[{1}]", listViewModel.IsPaged, spView.Paged));
                    Assert.AreEqual(listViewModel.IsPaged, spView.Paged);

                    trace.WriteLine(string.Format("Validate RowLimit: model:[{0}] view:[{1}]", listViewModel.RowLimit, spView.RowLimit));
                    Assert.AreEqual((uint)listViewModel.RowLimit, spView.RowLimit);

                    trace.WriteLine(string.Format("Validate fields.."));

                    traceScope.WithTraceIndent(fieldTrace =>
                    {
                        foreach (var fieldName in listViewModel.Fields)
                        {
                            trace.WriteLine(string.Format("Validate field presence: [{0}]", fieldName));
                            Assert.IsTrue(spView
                                            .ViewFields
                                            .Cast<String>()
                                            .Any(f => System.String.Compare(f, fieldName, System.StringComparison.OrdinalIgnoreCase) == 0));
                            trace.WriteLine(string.Format("Field [{0}] exists in view. [OK].", fieldName));
                        }
                    });
                });
            });
        }
    }
}
