using SPMeta2.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Services.Impl
{
    public class DefaultWebPartPageTemplatesService : WebPartPageTemplatesServiceBase
    {
        #region methods

        public override string GetPageLayoutTemplate(int pageLayoutTemplate, Version spRuntimeVersion)
        {
            return LookupPageLayoutTemplate(pageLayoutTemplate, spRuntimeVersion);
        }

        protected virtual string LookupPageLayoutTemplate(int pageLayoutTemplate, Version spRuntimeVersion)
        {
            switch (spRuntimeVersion.Major)
            {
                case 14:
                    return GetSP2010WebPartPageTemplateContent(pageLayoutTemplate, spRuntimeVersion);
                case 15:
                    return GetSP2013WebPartPageTemplateContent(pageLayoutTemplate, spRuntimeVersion);
                case 16:
                    return GetSP2016WebPartPageTemplateContent(pageLayoutTemplate, spRuntimeVersion);
                default:
                    return RaiseUnsupportedRuntimeException(pageLayoutTemplate, spRuntimeVersion);
            }
        }

        protected string RaiseUnsupportedRuntimeException(int pageLayoutTemplate, Version spRuntimeVersion)
        {
            throw new Exception(string.Format("PageLayoutTemplate: [{0}] is not supported for the current SharePoint runtime [{1}]",
                        pageLayoutTemplate,
                        spRuntimeVersion));
        }

        protected string RaiseNotFoundException(int pageLayoutTemplate, Version spRuntimeVersion)
        {
            throw new Exception(string.Format("PageLayoutTemplate: [{0}] is not found for the current SharePoint runtime [{1}]",
                        pageLayoutTemplate,
                        spRuntimeVersion));
        }

        private string GetSP2016WebPartPageTemplateContent(int pageLayoutTemplate, Version spRuntimeVersion)
        {
            switch (pageLayoutTemplate)
            {
                case 1:
                    return SP2016WebPartPageTemplates.spstd1;
                case 2:
                    return SP2016WebPartPageTemplates.spstd2;
                case 3:
                    return SP2016WebPartPageTemplates.spstd3;
                case 4:
                    return SP2016WebPartPageTemplates.spstd4;
                case 5:
                    return SP2016WebPartPageTemplates.spstd5;
                case 6:
                    return SP2016WebPartPageTemplates.spstd6;
                case 7:
                    return SP2016WebPartPageTemplates.spstd7;
                case 8:
                    return SP2016WebPartPageTemplates.spstd8;
                default:
                    return RaiseNotFoundException(pageLayoutTemplate, spRuntimeVersion);
            }
        }

        protected virtual string GetSP2013WebPartPageTemplateContent(int pageLayoutTemplate, Version spRuntimeVersion)
        {
            switch (pageLayoutTemplate)
            {
                case 1:
                    return SP2013WebPartPageTemplates.spstd1;
                case 2:
                    return SP2013WebPartPageTemplates.spstd2;
                case 3:
                    return SP2013WebPartPageTemplates.spstd3;
                case 4:
                    return SP2013WebPartPageTemplates.spstd4;
                case 5:
                    return SP2013WebPartPageTemplates.spstd5;
                case 6:
                    return SP2013WebPartPageTemplates.spstd6;
                case 7:
                    return SP2013WebPartPageTemplates.spstd7;
                case 8:
                    return SP2013WebPartPageTemplates.spstd8;
                default:
                    return RaiseNotFoundException(pageLayoutTemplate, spRuntimeVersion);
            }
        }

        protected virtual string GetSP2010WebPartPageTemplateContent(int pageLayoutTemplate, Version spRuntimeVersion)
        {
            switch (pageLayoutTemplate)
            {
                case 1:
                    return SP2010WebPartPageTemplates.spstd1;
                case 2:
                    return SP2010WebPartPageTemplates.spstd2;
                case 3:
                    return SP2010WebPartPageTemplates.spstd3;
                case 4:
                    return SP2010WebPartPageTemplates.spstd4;
                case 5:
                    return SP2010WebPartPageTemplates.spstd5;
                case 6:
                    return SP2010WebPartPageTemplates.spstd6;
                case 7:
                    return SP2010WebPartPageTemplates.spstd7;
                case 8:
                    return SP2010WebPartPageTemplates.spstd8;
                default:
                    return RaiseNotFoundException(pageLayoutTemplate, spRuntimeVersion);
            }
        }

        #endregion
    }
}