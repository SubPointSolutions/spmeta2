using System;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Regression.Utils;
using SPMeta2.SSOM.Extensions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Regression.Assertion;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class ListViewDefinitionValidator : ListViewModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ListViewDefinition>("model", value => value.RequireNotNull());

            var list = listModelHost.HostList;
            var spObject = list.Views.FindByName(definition.Title);

            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                               .ShouldBeEqual(m => m.Title, o => o.Title)
                               .ShouldBeEqual(m => m.IsDefault, o => o.IsDefaul())
                               .ShouldBeEqual(m => m.Query, o => o.Query)
                               .ShouldBeEqual(m => m.RowLimit, o => (int)o.RowLimit)
                               .ShouldBeEqual(m => m.IsPaged, o => o.Paged);


            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(def => def.Fields);
                var dstProp = d.GetExpressionValue(ct => ct.ViewFields);

                var hasAllFields = true;

                foreach (var srcField in s.Fields)
                {
                    if (!d.ViewFields.ToStringCollection().Contains(srcField))
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
    }

    internal static class ViewDefault
    {
        public static bool IsDefaul(this SPView view)
        {
            return view.ParentList.DefaultView.ID == view.ID;
        }
    }
}
