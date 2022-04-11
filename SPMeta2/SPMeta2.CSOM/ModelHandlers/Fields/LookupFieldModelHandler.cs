﻿using System;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.Services;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Services;
using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHosts;

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

        protected override bool PreloadProperties(Field field)
        {
            base.PreloadProperties(field);

            var context = field.Context;
            context.Load(field, f => f.SchemaXml);

            return true;
        }


        protected override void ProcessFieldProperties(Field field, FieldDefinition fieldModel)
        {
            var typedFieldModel = fieldModel.WithAssertAndCast<LookupFieldDefinition>("model", value => value.RequireNotNull());

            var site = HostSite;
            var context = site.Context;

            // CountRelated in Lookups in CSOM #1018
            // https://github.com/SubPointSolutions/spmeta2/issues/673
            if (typedFieldModel.CountRelated.HasValue)
            {
                var fieldXml = XDocument.Parse(field.SchemaXml);
                fieldXml.Root.SetAttribute("CountRelated", typedFieldModel.CountRelated.ToString().ToUpper());

                field.SchemaXml = fieldXml.ToString();
            }

            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

            var typedField = field.Context.CastTo<FieldLookup>(field);

            if (!typedField.IsPropertyAvailable("LookupList"))
            {
                if (field.Context.HasPendingRequest)
                {
                    field.Update();
                    field.Context.ExecuteQueryWithTrace();
                }

                typedField.Context.Load(typedField);
                typedField.Context.ExecuteQueryWithTrace();
            }

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

                typedField.LookupWebId = targetWeb.Id;
            }

            if (!string.IsNullOrEmpty(typedFieldModel.RelationshipDeleteBehavior))
            {
                var value = (RelationshipDeleteBehaviorType)Enum.Parse(typeof(RelationshipDeleteBehaviorType), typedFieldModel.RelationshipDeleteBehavior);
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

                    if (!targetWeb.IsPropertyAvailable("ServerRelativeUrl"))
                    {
                        context.Load(targetWeb, w => w.ServerRelativeUrl);
                        context.ExecuteQueryWithTrace();
                    }

                    var list = targetWeb.QueryAndGetListByUrl(UrlUtility.CombineUrl(targetWeb.ServerRelativeUrl, typedFieldModel.LookupListUrl));
                    typedField.LookupList = list.Id.ToString();
                }
                else if (!string.IsNullOrEmpty(typedFieldModel.LookupListTitle))
                {
                    var targetWeb = GetTargetWeb(site, typedFieldModel);
                    var list = targetWeb.Lists.GetByTitle(typedFieldModel.LookupListTitle);

                    context.Load(list);
                    context.ExecuteQueryWithTrace();

                    typedField.LookupList = list.Id.ToString();
                }
            }

            if (!string.IsNullOrEmpty(typedFieldModel.LookupField))
            {
                typedField.LookupField = typedFieldModel.LookupField;
            }
        }

        public Web GetTargetWeb(Site site, LookupFieldDefinition definition)
        {
            return GetTargetWeb(site, definition.LookupWebUrl, definition.LookupWebId);
        }

        protected Web GetTargetWeb(Site site, string webUrl, Guid? webId)
        {
            return GetTargetWeb(site, webUrl, webId, ModelHost);
        }

        public Web GetTargetWeb(Site site, string webUrl, Guid? webId, object replacementObject)
        {
            var context = site.Context;

            if (webId.HasGuidValue())
            {
                var targetWeb = site.OpenWebById(webId.Value);

                context.Load(targetWeb);
                context.ExecuteQueryWithTrace();

                return targetWeb;
            }
            else if (!string.IsNullOrEmpty(webUrl))
            {
                if (replacementObject == null)
                    throw new ArgumentNullException("replacementObject");

                //var oldValue = CSOMTokenReplacementService.AllowClientContextAsTokenReplacementContext;

                try
                {
                    // restrict, only site / web
                    // Tokens in LookupWebUrl #1013
                    // https://github.com/SubPointSolutions/spmeta2/issues/1013

                    //CSOMTokenReplacementService.AllowClientContextAsTokenReplacementContext = false;

                    var targetWebUrl = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                    {
                        Value = webUrl,
                        Context = replacementObject
                    }).Value;

                    // server relative url, ensure / in the beginning
                    targetWebUrl = UrlUtility.RemoveStartingSlash(targetWebUrl);
                    targetWebUrl = "/" + targetWebUrl;

                    var targetWeb = site.OpenWeb(targetWebUrl);

                    context.Load(targetWeb);
                    context.ExecuteQueryWithTrace();

                    return targetWeb;
                }
                finally
                {
                    //CSOMTokenReplacementService.AllowClientContextAsTokenReplacementContext = oldValue;
                }
            }

            // root web by default
            return site.RootWeb;
        }

        protected override void ProcessSPFieldXElement(XElement fieldTemplate, FieldDefinition fieldModel)
        {
            var site = HostSite;
            var context = HostSite.Context;

            base.ProcessSPFieldXElement(fieldTemplate, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<LookupFieldDefinition>("model", value => value.RequireNotNull());

            fieldTemplate.SetAttribute(BuiltInFieldAttributes.Mult, typedFieldModel.AllowMultipleValues.ToString().ToUpper());

            if (typedFieldModel.LookupWebId.HasGuidValue())
            {
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.WebId, typedFieldModel.LookupWebId.Value.ToString("B"));
            }
            else if (!string.IsNullOrEmpty(typedFieldModel.LookupWebUrl))
            {
                var targetWeb = GetTargetWeb(site, typedFieldModel);

                fieldTemplate.SetAttribute(BuiltInFieldAttributes.WebId, targetWeb.Id.ToString("B"));
            }
            if (!string.IsNullOrEmpty(typedFieldModel.LookupList))
            {
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.List, typedFieldModel.LookupList);
            }
            else if (!string.IsNullOrEmpty(typedFieldModel.LookupListUrl))
            {
                var targetWeb = GetTargetWeb(site, typedFieldModel);

                if (!targetWeb.IsPropertyAvailable("ServerRelativeUrl"))
                {
                    context.Load(targetWeb, w => w.ServerRelativeUrl);
                    context.ExecuteQueryWithTrace();
                }

                var list = targetWeb.QueryAndGetListByUrl(UrlUtility.CombineUrl(targetWeb.ServerRelativeUrl, typedFieldModel.LookupListUrl));

                fieldTemplate.SetAttribute(BuiltInFieldAttributes.List, list.Id.ToString());
            }
            else if (!string.IsNullOrEmpty(typedFieldModel.LookupListTitle))
            {
                var targetWeb = GetTargetWeb(site, typedFieldModel);
                var list = targetWeb.Lists.GetByTitle(typedFieldModel.LookupListTitle);

                context.Load(list);
                context.ExecuteQueryWithTrace();

                fieldTemplate.SetAttribute(BuiltInFieldAttributes.List, list.Id.ToString());
            }

            if (!string.IsNullOrEmpty(typedFieldModel.LookupField))
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.ShowField, typedFieldModel.LookupField);
        }

        #endregion
    }
}
