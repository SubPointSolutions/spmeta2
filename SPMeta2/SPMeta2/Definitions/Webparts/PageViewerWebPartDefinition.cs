﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using System.Runtime.Serialization;
using SPMeta2.Attributes.Capabilities;

namespace SPMeta2.Definitions.Webparts
{
    /// <summary>
    /// Allows to define and deploy 'apps part' web part.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "System.Web.UI.WebControls.WebParts.WebPart", "System.Web")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.WebParts.WebPart", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(WebPartPageDefinition))]

    [Serializable]
    [DataContract]

    [ExpectManyInstances]
    public class PageViewerWebPartDefinition : WebPartDefinition
    {
        #region constructors


        #endregion

        #region properties

        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        [ExpectUpdateAsUrl]

        [SiteCollectionTokenCapability]
        [WebTokenCapability]
        public string ContentLink { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        public string SourceType { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<PageViewerWebPartDefinition>(this, base.ToString())
                          .AddPropertyValue(p => p.ContentLink)
                          .AddPropertyValue(p => p.SourceType)
                          .ToString();
        }

        #endregion
    }
}
