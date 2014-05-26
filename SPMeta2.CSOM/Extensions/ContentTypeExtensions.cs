using System;
using Microsoft.SharePoint.Client;

namespace SPMeta2.CSOM.Extensions
{
    public static class ContentTypeExtensions
    {
        #region methods

        public static ContentType FindById(this ContentTypeCollection contentTypes, string contentTypeId)
        {
            foreach (var contentType in contentTypes)
            {
                if (String.Compare(contentType.Id.ToString(), contentTypeId, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return contentType;
                }
            }

            return null;
        }

        public static ContentType FindByName(this ContentTypeCollection contentTypes, string contentTypeName)
        {
            foreach (var contentType in contentTypes)
            {
                if (String.Compare(contentType.Name, contentTypeName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return contentType;
                }
            }

            return null;
        }

        #endregion
    }
}
