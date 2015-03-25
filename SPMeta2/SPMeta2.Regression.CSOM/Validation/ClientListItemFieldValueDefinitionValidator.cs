using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientListItemFieldValueDefinitionValidator : ListItemFieldValueModelHandler
    {
        public override void DeployModel(object modelHost, Definitions.DefinitionBase model)
        {
            var listItemModelHost = modelHost.WithAssertAndCast<ListItemModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ListItemFieldValueDefinition>("model", value => value.RequireNotNull());

            var spObject = listItemModelHost.HostListItem;

            var assert = ServiceFactory.AssertService
                            .NewAssert(definition, spObject)
                                  .ShouldNotBeNull(spObject);

            if (definition.FieldId.HasValue)
            {
                assert.SkipProperty(m => m.Value, string.Format("Skip validation FieldId to get field value is not supported by CSOM."));
                assert.SkipProperty(m => m.FieldId, "Skip validation FieldId to get field value is not supported by CSOM.");
                
                //assert.ShouldBeEqual((p, s, d) =>
                //{
                //    var srcProp = s.GetExpressionValue(m => m.FieldId);

                //    return new PropertyValidationResult
                //    {
                //        Tag = p.Tag,
                //        Src = srcProp,
                //        Dst = null,
                //        IsValid = object.Equals(s.Value, d[s.FieldId.Value])
                //    };
                //});

                //assert.SkipProperty(m => m.Value, string.Format("Value validated with FieldId and actual value:[{0}]", spObject[definition.FieldId.Value]));
            }
            else
            {
                assert.SkipProperty(m => m.FieldId, "FieldId is null. Skipping.");
            }

            if (!string.IsNullOrEmpty(definition.FieldName))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.FieldName);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = object.Equals(s.Value, d[s.FieldName])
                    };
                });

                assert.SkipProperty(m => m.Value, string.Format("Value validated with FieldName and actual value:[{0}]", spObject[definition.FieldName]));
            }
            else
            {
                assert.SkipProperty(m => m.FieldName, "FieldName is null. Skipping.");
            }
        }

        private void ValidateFieldValue(Microsoft.SharePoint.Client.ListItem listItem, ListItemFieldValueDefinition fieldValue)
        {
            // TODO
        }

    }
}
