using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.SSOM.DefaultSyntax;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using System.Linq;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class ListFieldLinkDefinitionValidator : ListFieldLinkModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ListFieldLinkDefinition>("model", value => value.RequireNotNull());

            var list = listModelHost.HostList;
            var spObject = list.Fields[definition.FieldId];

            var assert = ServiceFactory.AssertService
                                      .NewAssert(definition, spObject)
                                            .ShouldBeEqual(m => m.FieldId, o => o.Id);


            if (definition.AddFieldOptions.HasFlag(BuiltInAddFieldOptions.DefaultValue))
            {
                assert.SkipProperty(m => m.AddFieldOptions, "BuiltInAddFieldOptions.DefaultValue. Skipping.");
            }
            else
            {
                // TODO
            }

            if (definition.AddToDefaultView)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.AddToDefaultView);
                    var field = list.Fields[definition.FieldId];

                    var isValid = list.DefaultView
                        .ViewFields
                        .ToStringCollection()
                        .Contains(field.InternalName);

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
                assert.SkipProperty(m => m.AddToDefaultView, "AddToDefaultView is false. Skipping.");
            }

        }
    }
}
