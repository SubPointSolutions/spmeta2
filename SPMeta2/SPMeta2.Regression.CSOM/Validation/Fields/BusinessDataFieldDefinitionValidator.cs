using SPMeta2.CSOM.ModelHandlers.Fields;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SPMeta2.Utils;
using Microsoft.SharePoint.Client;
using SPMeta2.Exceptions;
using System.Xml.Linq;

namespace SPMeta2.Regression.CSOM.Validation.Fields
{
    public class BusinessDataFieldDefinitionValidator : BusinessDataFieldModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<BusinessDataFieldDefinition>("model", value => value.RequireNotNull());

            Field spObject = null;

            if (modelHost is SiteModelHost)
                spObject = FindSiteField(modelHost as SiteModelHost, definition);
            else if (modelHost is ListModelHost)
                spObject = FindListField((modelHost as ListModelHost).HostList, definition);
            else
            {
                throw new SPMeta2NotSupportedException(
                    string.Format("Validation for artifact of type [{0}] under model host [{1}] is not supported.",
                    definition.GetType(),
                    modelHost.GetType()));
            }

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            assert
                .ShouldBeEqual(m => m.Title, o => o.Title)
                    .ShouldBeEqual(m => m.InternalName, o => o.InternalName)
                    .ShouldBeEqual(m => m.Id, o => o.Id)
                    .ShouldBeEqual(m => m.Required, o => o.Required)
                    .ShouldBeEqual(m => m.Description, o => o.Description)
                    .ShouldBeEqual(m => m.FieldType, o => o.TypeAsString)
                    .ShouldBeEqual(m => m.Group, o => o.Group)

                    .ShouldBeEqual(m => m.SystemInstanceName, o => o.GetSystemInstanceName())
                    .ShouldBeEqual(m => m.EntityNamespace, o => o.GetEntityNamespace())
                    .ShouldBeEqual(m => m.EntityName, o => o.GetEntityName())
                    .ShouldBeEqual(m => m.BdcFieldName, o => o.GetBdcFieldName());

        }
    }

    internal static class BCSFieldExt
    {
        public static string GetSystemInstanceName(this Field field)
        {
            var xml = XElement.Parse(field.SchemaXml);
            return xml.Attribute("SystemInstance").Value;
        }

        public static string GetEntityNamespace(this Field field)
        {
            var xml = XElement.Parse(field.SchemaXml);
            return xml.Attribute("EntityNamespace").Value;
        }

        public static string GetEntityName(this Field field)
        {
            var xml = XElement.Parse(field.SchemaXml);
            return xml.Attribute("EntityName").Value;
        }

        public static string GetBdcFieldName(this Field field)
        {
            var xml = XElement.Parse(field.SchemaXml);
            return xml.Attribute("BdcField").Value;
        }
    }
}
