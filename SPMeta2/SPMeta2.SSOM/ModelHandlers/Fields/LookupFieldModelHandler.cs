using System;
using System.Xml.Linq;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Utils;
using Microsoft.SharePoint.Utilities;
using SPMeta2.Services;

namespace SPMeta2.SSOM.ModelHandlers.Fields
{
    public class LookupFieldModelHandler : FieldModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(LookupFieldDefinition); }
        }

        protected override Type GetTargetFieldType(FieldDefinition model)
        {
            return typeof(SPFieldLookup);
        }

        #endregion

        #region methods

        protected override void ProcessFieldProperties(SPField field, FieldDefinition fieldModel)
        {
            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

            var site = this.GetCurrentSite();

            var typedField = field as SPFieldLookup;
            var typedFieldModel = fieldModel.WithAssertAndCast<LookupFieldDefinition>("model", value => value.RequireNotNull());

            typedField.AllowMultipleValues = typedFieldModel.AllowMultipleValues;

            if (typedFieldModel.AllowMultipleValues)
                typedField.TypeAsString = "LookupMulti";
            else
                typedField.TypeAsString = "Lookup";

            if (typedFieldModel.LookupWebId.HasGuidValue())
            {
                typedField.LookupWebId = typedFieldModel.LookupWebId.Value;
            }
            else if (!string.IsNullOrEmpty(typedFieldModel.LookupWebUrl))
            {
                var targetWeb = GetTargetWeb(site, typedFieldModel);

                typedField.LookupWebId = targetWeb.ID;

                if (!targetWeb.IsRootWeb)
                    targetWeb.Dispose();
            }

            if (typedFieldModel.CountRelated.HasValue)
            {
                typedField.CountRelated = typedFieldModel.CountRelated.Value;
            }

            if (!string.IsNullOrEmpty(typedFieldModel.RelationshipDeleteBehavior))
            {
                var value = (SPRelationshipDeleteBehavior)Enum.Parse(typeof(SPRelationshipDeleteBehavior), typedFieldModel.RelationshipDeleteBehavior);
                typedField.RelationshipDeleteBehavior = value;
            }

            if (string.IsNullOrEmpty(typedField.LookupList))
            {
                if (!string.IsNullOrEmpty(typedFieldModel.LookupList))
                {
                    typedField.LookupList = typedFieldModel.LookupList;
                }
                else if (!string.IsNullOrEmpty(typedFieldModel.LookupListUrl))
                {
                    var targetWeb = GetTargetWeb(site, typedFieldModel);
                    var list = targetWeb.GetList(SPUrlUtility.CombineUrl(targetWeb.ServerRelativeUrl, typedFieldModel.LookupListUrl));

                    typedField.LookupList = list.ID.ToString();

                    if (!targetWeb.IsRootWeb)
                        targetWeb.Dispose();
                }
                else if (!string.IsNullOrEmpty(typedFieldModel.LookupListTitle))
                {
                    var targetWeb = GetTargetWeb(site, typedFieldModel);
                    var list = targetWeb.Lists[typedFieldModel.LookupListTitle];

                    typedField.LookupList = list.ID.ToString();

                    if (!targetWeb.IsRootWeb)
                        targetWeb.Dispose();
                }
            }

            if (!string.IsNullOrEmpty(typedFieldModel.LookupField))
            {
                typedField.LookupField = typedFieldModel.LookupField;
            }
        }

        public SPWeb GetTargetWeb(SPSite site, LookupFieldDefinition definition)
        {
            return GetTargetWeb(site, definition.LookupWebUrl, definition.LookupWebId);
        }

        public SPWeb GetTargetWeb(SPSite site, string webUrl, Guid? webId)
        {
            if (webId.HasGuidValue())
            {
                return site.OpenWeb(webId.Value);
            }
            else if (!string.IsNullOrEmpty(webUrl))
            {
                var targetWebUrl = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                {
                    Value = webUrl,
                    Context = site
                }).Value;

                // server relative URl, always
                targetWebUrl = UrlUtility.RemoveStartingSlash(targetWebUrl);
                targetWebUrl = "/" + targetWebUrl;

                var targetWeb = site.OpenWeb(targetWebUrl);

                return targetWeb;
            }

            // root web by default
            return site.RootWeb;
        }

        protected override void ProcessSPFieldXElement(XElement fieldTemplate, FieldDefinition fieldModel)
        {
            base.ProcessSPFieldXElement(fieldTemplate, fieldModel);

            var site = this.GetCurrentSite();
            var typedFieldModel = fieldModel.WithAssertAndCast<LookupFieldDefinition>("model", value => value.RequireNotNull());

            fieldTemplate.SetAttribute(BuiltInFieldAttributes.Mult, typedFieldModel.AllowMultipleValues.ToString().ToUpper());

            if (typedFieldModel.LookupWebId.HasGuidValue())
            {
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.WebId, typedFieldModel.LookupWebId.Value.ToString("B"));
            }
            else if (!string.IsNullOrEmpty(typedFieldModel.LookupWebUrl))
            {
                var targetWeb = GetTargetWeb(site, typedFieldModel);

                fieldTemplate.SetAttribute(BuiltInFieldAttributes.WebId, targetWeb.ID.ToString("B"));

                if (!targetWeb.IsRootWeb)
                    targetWeb.Dispose();
            }

            if (!string.IsNullOrEmpty(typedFieldModel.LookupList))
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.List, typedFieldModel.LookupList);
            else if (!string.IsNullOrEmpty(typedFieldModel.LookupListUrl))
            {
                var targetWeb = GetTargetWeb(site, typedFieldModel);
                var list = targetWeb.GetList(SPUrlUtility.CombineUrl(targetWeb.ServerRelativeUrl, typedFieldModel.LookupListUrl));

                fieldTemplate.SetAttribute(BuiltInFieldAttributes.List, list.ID.ToString());

                if (!targetWeb.IsRootWeb)
                    targetWeb.Dispose();
            }
            else if (!string.IsNullOrEmpty(typedFieldModel.LookupListTitle))
            {
                var targetWeb = GetTargetWeb(site, typedFieldModel);
                var list = targetWeb.Lists[typedFieldModel.LookupListTitle];

                fieldTemplate.SetAttribute(BuiltInFieldAttributes.List, list.ID.ToString());

                if (!targetWeb.IsRootWeb)
                    targetWeb.Dispose();
            }

            if (!string.IsNullOrEmpty(typedFieldModel.LookupField))
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.ShowField, typedFieldModel.LookupField);
        }

        #endregion
    }
}
