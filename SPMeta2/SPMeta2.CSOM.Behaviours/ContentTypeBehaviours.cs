using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WorkflowServices;

namespace SPMeta2.CSOM.Behaviours
{
    public static class ContentTypeBehaviours
    {
        #region common extensions

        /// <summary>
        /// Setup the same url for NewFormUrl/DisplayFormUrl/EditFormUrl
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static ContentType MakeCustomFormUrl(this ContentType contentType, string url)
        {
            MakeNewFormUrl(contentType, url);
            MakeDisplayFormUrl(contentType, url);
            MakeEditFormUrl(contentType, url);

            return contentType;
        }

        public static ContentType MakeNewFormUrl(this ContentType contentType, string url)
        {
            contentType.NewFormUrl = url;

            return contentType;
        }

        public static ContentType MakeDisplayFormUrl(this ContentType contentType, string url)
        {
            contentType.DisplayFormUrl = url;

            return contentType;
        }

        public static ContentType MakeEditFormUrl(this ContentType contentType, string url)
        {
            contentType.EditFormUrl = url;

            return contentType;
        }

        public static ContentType MakeDocumentTemplate(this ContentType contentType, string serverRelativeUrl)
        {
            contentType.DocumentTemplate = serverRelativeUrl;

            return contentType;
        }

        #endregion
    }
}
