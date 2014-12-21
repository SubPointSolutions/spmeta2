using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

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

            context.Load(list, l => l.Fields);
            context.Load(list, l => l.Views.Include(
                v => v.ViewFields,
                 o => o.Title,
                o => o.DefaultView,
                o => o.ViewQuery,
                o => o.RowLimit,
                o => o.Paged,
                o => o.JSLink,
                o => o.ServerRelativeUrl,
                v => v.Title));
            context.ExecuteQuery();

            var spObject = FindViewByTitle(list.Views, definition.Title);
            var assert = ServiceFactory.AssertService
                                      .NewAssert(definition, spObject)
                                          .ShouldBeEqual(m => m.Title, o => o.Title)
                                          .ShouldBeEqual(m => m.IsDefault, o => o.DefaultView)
                                          .ShouldBeEqual(m => m.Query, o => o.ViewQuery)
                                          .ShouldBeEqual(m => m.RowLimit, o => (int)o.RowLimit)
                                          .ShouldBeEqual(m => m.IsPaged, o => o.Paged);

            assert.ShouldBePartOf(m => m.JSLink, o => o.JSLink);

            if (string.IsNullOrEmpty(definition.Url))
                assert.SkipProperty(m => m.Url, "Url is null or empty. Skipping.");
            else
                assert.ShouldBePartOf(m => m.Url, o => o.ServerRelativeUrl);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(def => def.Fields);
                var dstProp = d.GetExpressionValue(ct => ct.ViewFields);

                var hasAllFields = true;

                foreach (var srcField in s.Fields)
                {
                    var listField = list.Fields.ToList().FirstOrDefault(f => f.StaticName == srcField);

                    // if list-scoped field we need to check by internal name
                    // internal name is changed for list scoped-fields
                    // that's why to check by BOTH, definition AND real internal name

                    if (!d.ViewFields.ToList().Contains(srcField) &&
                         !d.ViewFields.ToList().Contains(listField.InternalName))
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
