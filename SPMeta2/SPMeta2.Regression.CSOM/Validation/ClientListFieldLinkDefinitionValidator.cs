using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
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
            context.ExecuteQuery();

            var assert = ServiceFactory.AssertService
                .NewAssert(definition, spObject)
                .ShouldNotBeNull(spObject);
            //.ShouldBeEqual(m => m.FieldId, o => o.Id);

            if (!string.IsNullOrEmpty(definition.FieldInternalName))
                assert.ShouldBeEqual(m => m.FieldInternalName, o => o.InternalName);
            else
                assert.SkipProperty(m => m.FieldInternalName, "FieldInternalName is null or empty. Skipping");

            if (definition.FieldId.HasGuidValue())
                assert.ShouldBeEqual(m => m.FieldId, o => o.Id);
            else
                assert.SkipProperty(m => m.FieldId, "FieldId is null or empty. Skipping");

            if (!string.IsNullOrEmpty(definition.DisplayName))
                assert.ShouldBeEqual(m => m.DisplayName, o => o.Title);
            else
                assert.SkipProperty(m => m.DisplayName, "DisplayName is null or empty. Skipping");

            if (definition.Required.HasValue)
                assert.ShouldBeEqual(m => m.Required, o => o.Required);
            else
                assert.SkipProperty(m => m.Required, "Required is null or empty. Skipping");

            if (definition.Hidden.HasValue)
                assert.ShouldBeEqual(m => m.Hidden, o => o.Hidden);
            else
                assert.SkipProperty(m => m.Hidden, "Hidden is null or empty. Skipping");

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
                    context.ExecuteQuery();

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
                        context.ExecuteQuery();

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

                    context.ExecuteQuery();

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
