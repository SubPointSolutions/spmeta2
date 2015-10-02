using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using SPMeta2.Utils;

namespace SPMeta2.SSOM.Services
{
    public class SSOMLocalizationService : LocalizationServiceBase
    {
        public override CultureInfo GetUserResourceCultureInfo(ValueForUICulture locValue)
        {
            CultureInfo cultureInfo = null;

            if (locValue.CultureId.HasValue)
                cultureInfo = new CultureInfo(locValue.CultureId.Value);
            else if (!string.IsNullOrEmpty(locValue.CultureName))
                cultureInfo = new CultureInfo(locValue.CultureName);
            else
                throw new SPMeta2Exception(string.Format("Either CultureId or CultureName should be defined"));

            return cultureInfo;
        }

        public override void ProcessUserResource(object parentObject, object userResource, ValueForUICulture locValue)
        {
            var typedResource = userResource.WithAssertAndCast<SPUserResource>("userResource", value => value.RequireNotNull());

            var cultureInfo = GetUserResourceCultureInfo(locValue);

            typedResource.SetValueForUICulture(cultureInfo, locValue.Value);
            typedResource.Update();
        }
    }
}
