using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Services
{
    public abstract class LocalizationServiceBase
    {
        public abstract CultureInfo GetUserResourceCultireInfo(ValueForUICulture locValue);

        public abstract void ProcessFieldUserResource(object userResource, ValueForUICulture locValue);
    }
}
