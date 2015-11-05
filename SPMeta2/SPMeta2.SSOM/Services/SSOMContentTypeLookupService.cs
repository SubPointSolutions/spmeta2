using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using SPMeta2.Exceptions;

namespace SPMeta2.SSOM.Services
{
    public class SSOMContentTypeLookupService
    {
        public virtual SPContentTypeId LookupContentTypeByName(SPList targetList, string name)
        {
            var targetContentType = targetList.ContentTypes
                .OfType<SPContentType>()
                .FirstOrDefault(ct => ct.Name.ToUpper() == name.ToUpper());

            if (targetContentType == null)
                throw new SPMeta2Exception(String.Format("Cannot find content type by name ['{0}'] in list: [{1}]",
                    name, targetList.Title));

            return targetContentType.Id;
        }

        public virtual SPContentTypeId LookupListContentTypeById(SPList targetList, string contentTypeId)
        {
            return targetList.ContentTypes.BestMatch(new SPContentTypeId(contentTypeId));
        }
    }
}
