using System;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers.Fields
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
            return typeof(FieldLookup);
        }

        #endregion

        #region methods

        protected override void ProcessFieldProperties(Field field, FieldDefinition fieldModel)
        {
            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

            var typedField = field.Context.CastTo<FieldLookup>(field);
            var typedFieldModel = fieldModel.WithAssertAndCast<LookupFieldDefinition>("model", value => value.RequireNotNull());

            if (!typedField.IsPropertyAvailable("LookupList"))
            {
                if (field.Context.HasPendingRequest)
                {
                    field.Update();
                    field.Context.ExecuteQuery();
                }

                typedField.Context.Load(typedField);
                typedField.Context.ExecuteQuery();
            }

            typedField.AllowMultipleValues = typedFieldModel.AllowMultipleValues;

            if (typedFieldModel.AllowMultipleValues)
                typedField.TypeAsString = "LookupMulti";
            else
                typedField.TypeAsString = "Lookup";

            if (typedFieldModel.LookupWebId.HasValue)
                typedField.LookupWebId = typedFieldModel.LookupWebId.Value;

            if (string.IsNullOrEmpty(typedField.LookupList))
            {
                if (!string.IsNullOrEmpty(typedFieldModel.LookupList))
                {
                    typedField.LookupList = typedFieldModel.LookupList;
                }
                else if (!string.IsNullOrEmpty(typedFieldModel.LookupListUrl))
                {
                    var site = HostSite;
                    var context = site.Context;

                    var web = typedFieldModel.LookupWebId.HasValue
                        ? site.OpenWebById(typedFieldModel.LookupWebId.Value)
                        : site.RootWeb;

                    if (!web.IsPropertyAvailable("ServerRelativeUrl"))
                    {
                        context.Load(web, w => w.ServerRelativeUrl);
                        context.ExecuteQuery();
                    }

                    var list = web.QueryAndGetListByUrl(UrlUtility.CombineUrl(web.ServerRelativeUrl, typedFieldModel.LookupListUrl));

                    typedField.LookupList = list.Id.ToString();
                }
                else if (!string.IsNullOrEmpty(typedFieldModel.LookupListTitle))
                {
                    var site = HostSite;
                    var context = site.Context;

                    var web = typedFieldModel.LookupWebId.HasValue
                        ? site.OpenWebById(typedFieldModel.LookupWebId.Value)
                        : site.RootWeb;

                    var list = web.Lists.GetByTitle(typedFieldModel.LookupListTitle);

                    context.Load(list);
                    context.ExecuteQuery();

                    typedField.LookupList = list.Id.ToString();
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
                var site = HostSite;
                var context = site.Context;

                var web = typedFieldModel.LookupWebId.HasValue
                    ? site.OpenWebById(typedFieldModel.LookupWebId.Value)
                    : site.RootWeb;

                if (!web.IsPropertyAvailable("ServerRelativeUrl"))
                {
                    context.Load(web, w => w.ServerRelativeUrl);
                    context.ExecuteQuery();
                }

                var list = web.QueryAndGetListByUrl(UrlUtility.CombineUrl(web.ServerRelativeUrl, typedFieldModel.LookupListUrl));

                fieldTemplate.SetAttribute(BuiltInFieldAttributes.List, list.Id.ToString());
            }
            else if (!string.IsNullOrEmpty(typedFieldModel.LookupListTitle))
            {
                var site = HostSite;
                var context = site.Context;

                var web = typedFieldModel.LookupWebId.HasValue
                    ? site.OpenWebById(typedFieldModel.LookupWebId.Value)
                    : site.RootWeb;

                var list = web.Lists.GetByTitle(typedFieldModel.LookupListTitle);

                context.Load(list);
                context.ExecuteQuery();

                fieldTemplate.SetAttribute(BuiltInFieldAttributes.List, list.Id.ToString());
            }

            if (!string.IsNullOrEmpty(typedFieldModel.LookupField))
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.ShowField, typedFieldModel.LookupField);
        }

        #endregion
    }
}
