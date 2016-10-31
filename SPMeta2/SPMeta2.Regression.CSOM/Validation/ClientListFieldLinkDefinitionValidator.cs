using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientListFieldLinkDefinitionValidator : ListFieldLinkModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ListFieldLinkDefinition>("model", value => value.RequireNotNull());

            var list = listModelHost.HostList;
            var context = list.Context;

            var spObject = FindExistingListField(list, definition);

            context.Load(spObject);
            context.ExecuteQueryWithTrace();

            var assert = ServiceFactory.AssertService.NewAssert(definition, spObject);

            assert
                .ShouldNotBeNull(spObject)

                .ShouldBeEqualIfNotNullOrEmpty(m => m.FieldInternalName, o => o.InternalName)

                .ShouldBeEqualIfHasValue(m => m.FieldId, o => o.Id)
                .ShouldBeEqualIfHasValue(m => m.Required, o => o.Required)
                .ShouldBeEqualIfHasValue(m => m.Hidden, o => o.Hidden);

            if (!string.IsNullOrEmpty(definition.DisplayName))
                assert.ShouldBeEqual(m => m.DisplayName, o => o.Title);
            else
            {

                var regDisplayTitleProp = definition.PropertyBag.FirstOrDefault(p => p.Name == "_Reg_DisplayName");

                if (regDisplayTitleProp != null)
                {
                    // Enhance FieldDefinition with 'pushChangesToLists' option #922
                    // https://github.com/SubPointSolutions/spmeta2/issues/922

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.DisplayName);
                        var dstProp = d.GetExpressionValue(m => m.Title);

                        var isValid = regDisplayTitleProp.Value == d.Title;

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
                    assert.SkipProperty(m => m.DisplayName, "DisplayName is null or empty. Skipping");
                }
            }

            if (definition.AddFieldOptions.HasFlag(BuiltInAddFieldOptions.DefaultValue))
            {
                assert.SkipProperty(m => m.AddFieldOptions, "BuiltInAddFieldOptions.DefaultValue. Skipping.");
            }

            if (definition.AddFieldOptions.HasFlag(BuiltInAddFieldOptions.AddToAllContentTypes))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.AddFieldOptions);
                    var isValid = true;

                    var listContentTypes = list.ContentTypes;
                    context.Load(listContentTypes);
                    context.ExecuteQueryWithTrace();

                    foreach (ContentType ct in listContentTypes)
                    {
                        // TODO!
                        if (ct.Name == "Folder")
                        {
                            // skip folder content type
                            continue;
                        }

                        context.Load(ct);
                        context.Load(ct, c => c.FieldLinks);
                        context.ExecuteQueryWithTrace();

                        isValid = ct.FieldLinks.OfType<FieldLink>().Count(l => l.Id == spObject.Id) > 0;

                        if (!isValid)
                            break;
                    }

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });
            }


            if (definition.AddFieldOptions.HasFlag(BuiltInAddFieldOptions.AddFieldToDefaultView))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.AddFieldOptions);
                    var isValid = false;

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });
            }

            if (definition.AddToDefaultView)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.AddToDefaultView);

                    var field = FindExistingListField(list, definition);
                    var defaultView = list.DefaultView;

                    context.Load(defaultView);
                    context.Load(defaultView, v => v.ViewFields);

                    context.Load(field);

                    context.ExecuteQueryWithTrace();

                    var isValid = list.DefaultView
                        .ViewFields
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
