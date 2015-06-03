using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHandlers.Base;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class MasterPagePreviewModelHandler : ContentFileModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(MasterPagePreviewDefinition); }
        }

        #endregion

        #region methods


        #endregion

        public override string FileExtension
        {
            get { return "preview"; }
            set
            {

            }
        }

        protected override void MapProperties(object modelHost, System.Collections.Hashtable fileProperties, ContentPageDefinitionBase definition)
        {
            var typedTemplateModel = definition.WithAssertAndCast<MasterPagePreviewDefinition>("model", value => value.RequireNotNull());

            fileProperties[BuiltInInternalFieldNames.ContentTypeId] = BuiltInContentTypeId.MasterPagePreview;

            if (typedTemplateModel.UIVersion.Count > 0)
            {
                var value = new SPFieldMultiChoiceValue();

                foreach (var v in typedTemplateModel.UIVersion)
                    value.Add(v);

                fileProperties["UIVersion"] = value.ToString();
            }
        }
    }
}
