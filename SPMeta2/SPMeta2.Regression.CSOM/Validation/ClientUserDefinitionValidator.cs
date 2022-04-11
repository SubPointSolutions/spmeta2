using System;
using System.Linq;

using Microsoft.SharePoint.Client;

using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Regression.CSOM.Extensions;
using SPMeta2.Services;
using SPMeta2.Utils;
using System.Text;
using SPMeta2.CSOM.ModelHosts;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientUserDefinitionValidator : UserModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<SecurityGroupModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<UserDefinition>("model", value => value.RequireNotNull());
            var context = typedModelHost.HostClientContext;

            var spObject = GetUserInGroup(typedModelHost.HostWeb,
                                          typedModelHost.SecurityGroup,
                                          definition);

            if (spObject == null && !string.IsNullOrEmpty(definition.Email))
            {
                // local domain testing, reverting username from the email
                var userName = definition.Email.Split('@')[0];
                var domainName = Environment.UserDomainName;
                
                spObject = typedModelHost.SecurityGroup.Users.GetByLoginName(domainName + @"\" + userName);
                context.ExecuteQueryWithTrace();
            }

            context.Load(spObject);
            context.Load(spObject, u => u.LoginName);
            context.Load(spObject, u => u.Email);

            context.ExecuteQueryWithTrace();

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
                        isValid = s.LoginName.ToUpper() == d.LoginName.ToUpper();
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
