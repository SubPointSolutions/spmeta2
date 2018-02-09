using System.Linq;

using Microsoft.SharePoint;

using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Regression.SSOM.Extensions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class UserDefinitionValidator : UserModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<SecurityGroupModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<UserDefinition>("model", value => value.RequireNotNull());

            var spObject = GetUserInGroup(typedModelHost.SecurityGroup.ParentWeb,
                                          typedModelHost.SecurityGroup,
                                          definition);

            if (spObject == null && !string.IsNullOrEmpty(definition.Email))
            {
                // local domain testing, reverting username from the email
                var userName = definition.Email.Split('@')[0];

                spObject = typedModelHost.SecurityGroup.Users.OfType<SPUser>()
                                         .FirstOrDefault(u => u.LoginName.ToUpper().EndsWith(userName.ToUpper()));
            }

            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, definition, spObject)
                                             .ShouldNotBeNull(spObject);

            if (!string.IsNullOrEmpty(definition.Email))
            {
                // skiping email as it might be empty on the local testing
                // confirming loging name match

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.Email);
                    var isValid = false;

                    if (!string.IsNullOrEmpty(d.Email))
                    {
                        // check via login if present
                        if (!string.IsNullOrEmpty(s.LoginName))
                        {
                            isValid = s.LoginName.ToUpper() == d.LoginName.ToUpper();
                        }
                        else
                        {
                            isValid = s.Email.ToUpper() == d.Email.ToUpper();
                        }
                    }
                    else
                    {
                        // local domain testing, reverting username from the email
                        var userName = definition.Email.Split('@')[0];

                        isValid = spObject.LoginName.ToUpper().EndsWith(userName.ToUpper());
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
            else
            {
                assert.SkipProperty(m => m.Email);
            }

            if (!string.IsNullOrEmpty(definition.LoginName))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.LoginName);
                    var isValid = s.LoginName.ToUpper() == d.LoginName.ToUpper();

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
                assert.SkipProperty(m => m.LoginName);
        }
    }
}
