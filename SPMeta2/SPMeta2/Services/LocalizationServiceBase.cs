using System.Globalization;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;

namespace SPMeta2.Services
{
    public abstract class LocalizationServiceBase
    {
        public virtual CultureInfo GetUserResourceCultureInfo(ValueForUICulture locValue)
        {
            CultureInfo cultureInfo;

            if (locValue.CultureId.HasValue)
                cultureInfo = new CultureInfo(locValue.CultureId.Value);
            else if (!string.IsNullOrEmpty(locValue.CultureName))
                cultureInfo = new CultureInfo(locValue.CultureName);
            else
                throw new SPMeta2Exception("Either CultureId or CultureName should be defined");

            return cultureInfo;
        }

        public abstract void ProcessUserResource(object parentObject, object userResource, ValueForUICulture locValue);
    }
}
