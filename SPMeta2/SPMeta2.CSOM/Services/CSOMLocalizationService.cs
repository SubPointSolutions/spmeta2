using System;
using System.Globalization;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Services;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Services
{
    public class CSOMLocalizationService : LocalizationServiceBase
    {
        public override void ProcessUserResource(object parentObject, object userResource, ValueForUICulture locValue)
        {
            var typedUserResource = userResource as ClientObject;

            if (typedUserResource == null)
                throw new SPMeta2Exception("userResource should be an instance of ClientObject");

            if (typedUserResource.GetType().Name.ToUpper() != "UserResource".ToUpper())
                throw new SPMeta2Exception("userResource should be an instance of UserResource");

            var context = typedUserResource.Context;
            var cultureInfo = GetUserResourceCultureInfo(locValue);

            var query = new ClientActionInvokeMethod(typedUserResource, "SetValueForUICulture", new object[]
            {
                        cultureInfo.Name, 
                        locValue.Value,
            });

            context.AddQuery(query);
        }
    }
}
