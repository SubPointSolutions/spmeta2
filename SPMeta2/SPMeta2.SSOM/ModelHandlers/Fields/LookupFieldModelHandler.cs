using System;
using System.Xml.Linq;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Utils;
using Microsoft.SharePoint.Utilities;

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

            var typedField = field as SPFieldLookup;
            var typedFieldModel = fieldModel.WithAssertAndCast<LookupFieldDefinition>("model", value => value.RequireNotNull());

            typedField.AllowMultipleValues = typedFieldModel.AllowMultipleValues;

            if (typedFieldModel.AllowMultipleValues)
                typedField.TypeAsString = "LookupMulti";
            else
                typedField.TypeAsString = "Lookup";

            if (typedFieldModel.LookupWebId.HasValue)
            {
                typedField.LookupWebId = typedFieldModel.LookupWebId.Value;
            }


            if (string.IsNullOrEmpty(typedField.LookupList))
            {
                if (!string.IsNullOrEmpty(typedFieldModel.LookupList))
                {
                    typedField.LookupList = typedFieldModel.LookupList;
                }
                else if (!string.IsNullOrEmpty(typedFieldModel.LookupListUrl))
                {
                    var site = this.GetCurrentSite();
                    var web = typedFieldModel.LookupWebId.HasValue
                        ? site.OpenWeb(typedFieldModel.LookupWebId.Value)
                        : site.RootWeb;

                    var list = web.GetList(SPUrlUtility.CombineUrl(web.ServerRelativeUrl, typedFieldModel.LookupListUrl));

                    typedField.LookupList = list.ID.ToString();

                    if (!web.IsRootWeb)
                        web.Dispose();
                }
                else if (!string.IsNullOrEmpty(typedFieldModel.LookupListTitle))
                {
                    var site = this.GetCurrentSite();
                    var web = typedFieldModel.LookupWebId.HasValue
                        ? site.OpenWeb(typedFieldModel.LookupWebId.Value)
                        : site.RootWeb;

                    var list = web.Lists[typedFieldModel.LookupListTitle];

                    typedField.LookupList = list.ID.ToString();

                    if (!web.IsRootWeb)
                        web.Dispose();
                }
            }

            if (!string.IsNullOrEmpty(typedFieldModel.LookupField))
            {
                typedField.LookupField = typedFieldModel.LookupField;
            }
        }

        protected override void ProcessSPFieldXElement(XElement fieldTemplate, FieldDefinition fieldModel)
        {
            base.ProcessSPFieldXElement(fieldTemplate, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<LookupFieldDefinition>("model", value => value.RequireNotNull());

            fieldTemplate.SetAttribute(BuiltInFieldAttributes.Mult, typedFieldModel.AllowMultipleValues.ToString().ToUpper());

            if (typedFieldModel.LookupWebId.HasValue)
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.WebId, typedFieldModel.LookupWebId.Value.ToString("B"));

            if (!string.IsNullOrEmpty(typedFieldModel.LookupList))
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.List, typedFieldModel.LookupList);
            else if (!string.IsNullOrEmpty(typedFieldModel.LookupListUrl))
            {
                var site = this.GetCurrentSite();
                var web = typedFieldModel.LookupWebId.HasValue
                    ? site.OpenWeb(typedFieldModel.LookupWebId.Value)
                    : site.RootWeb;

                var list = web.GetList(SPUrlUtility.CombineUrl(web.ServerRelativeUrl, typedFieldModel.LookupListUrl));

                fieldTemplate.SetAttribute(BuiltInFieldAttributes.List, list.ID.ToString());

                if (!web.IsRootWeb)
                    web.Dispose();
            }
            else if (!string.IsNullOrEmpty(typedFieldModel.LookupListTitle))
            {
                var site = this.GetCurrentSite();
                var web = typedFieldModel.LookupWebId.HasValue
                    ? site.OpenWeb(typedFieldModel.LookupWebId.Value)
                    : site.RootWeb;

                var list = web.Lists[typedFieldModel.LookupListTitle];

                fieldTemplate.SetAttribute(BuiltInFieldAttributes.List, list.ID.ToString());

                if (!web.IsRootWeb)
                    web.Dispose();
            }

            if (!string.IsNullOrEmpty(typedFieldModel.LookupField))
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.ShowField, typedFieldModel.LookupField);
        }

        #endregion
    }
}
