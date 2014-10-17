using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Regression.Assertion;
using SPMeta2.Regression.Utils;
using SPMeta2.Utils;
using System.Linq;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientListViewDefinitionValidator : ListViewModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var list = modelHost.WithAssertAndCast<List>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ListViewDefinition>("model", value => value.RequireNotNull());

            var context = list.Context;

            context.Load(list, l => l.Views);
            context.ExecuteQuery();
            var spObject = FindViewByTitle(list.Views, definition.Title);

            var assert = ServiceFactory.AssertService
                          .NewAssert(definition, spObject)
                              .ShouldBeEqual(m => m.Title, o => o.Title)
                              .ShouldBeEqual(m => m.IsDefault, o => o.DefaultView)
                              .ShouldBeEqual(m => m.Query, o => o.ViewQuery)
                              .ShouldBeEqual(m => m.RowLimit, o => (int)o.RowLimit)
                              .ShouldBeEqual(m => m.IsPaged, o => o.Paged);


            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(def => def.Fields);
                var dstProp = d.GetExpressionValue(ct => ct.ViewFields);

                var hasAllFields = true;

                foreach (var srcField in s.Fields)
                {
                    if (!d.ViewFields.ToList().Contains(srcField))
                        hasAllFields = false;
                }

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = dstProp,
                    IsValid = hasAllFields
                };
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

    internal static class ViewDefault
    {
        //public static bool IsDefaul(this View view)
        //{
        //    return view.DefaultView.DefaultView.ID == view.Id;
        //}
    }
}
