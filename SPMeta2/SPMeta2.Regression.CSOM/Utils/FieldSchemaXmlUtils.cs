using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Utils
{
    internal static class FieldSchemaXmlUtils
    {
        public static string GetEditFormat(this Field field)
        {
            var xml = field.SchemaXml;

            return ConvertUtils.ToString(XElement.Parse(xml).GetAttributeValue(BuiltInFieldAttributes.EditFormat));
        }

        public static int GetCurrencyLocaleId(this Field field)
        {
            var xml = field.SchemaXml;

            return ConvertUtils.ToInt(XElement.Parse(xml).GetAttributeValue(BuiltInFieldAttributes.CurrencyLocaleId)).Value;
        }

        public static bool GetRichText(this Field field)
        {
            var xml = field.SchemaXml;

            return ConvertUtils.ToBoolWithDefault(XElement.Parse(xml).GetAttributeValue(BuiltInFieldAttributes.RichText), false);
        }

        public static string GetRichTextMode(this Field field)
        {
            var xml = field.SchemaXml;

            return ConvertUtils.ToString(XElement.Parse(xml).GetAttributeValue(BuiltInFieldAttributes.RichTextMode));
        }

        public static bool GetUnlimitedLengthInDocumentLibrary(this Field field)
        {
            var xml = field.SchemaXml;

            return ConvertUtils.ToBoolWithDefault(XElement.Parse(xml).GetAttributeValue(BuiltInFieldAttributes.UnlimitedLengthInDocumentLibrary), false);
        }

        public static bool? GetShowInDisplayForm(this Field field)
        {
            var xml = field.SchemaXml;

            return ConvertUtils.ToBool(XElement.Parse(xml).GetAttributeValue(BuiltInFieldAttributes.ShowInDisplayForm));
        }

        public static bool? GetShowInEditForm(this Field field)
        {
            var xml = field.SchemaXml;

            return ConvertUtils.ToBool(XElement.Parse(xml).GetAttributeValue(BuiltInFieldAttributes.ShowInEditForm));
        }

        public static bool? GetShowInListSettings(this Field field)
        {
            var xml = field.SchemaXml;

            return ConvertUtils.ToBool(XElement.Parse(xml).GetAttributeValue(BuiltInFieldAttributes.ShowInListSettings));
        }

        public static bool? GetShowInNewForm(this Field field)
        {
            var xml = field.SchemaXml;

            return ConvertUtils.ToBool(XElement.Parse(xml).GetAttributeValue(BuiltInFieldAttributes.ShowInNewForm));
        }

        public static bool? GetShowInVersionHistory(this Field field)
        {
            var xml = field.SchemaXml;

            return ConvertUtils.ToBool(XElement.Parse(xml).GetAttributeValue(BuiltInFieldAttributes.ShowInVersionHistory));
        }

        public static bool? GetShowInViewForms(this Field field)
        {
            var xml = field.SchemaXml;

            return ConvertUtils.ToBool(XElement.Parse(xml).GetAttributeValue(BuiltInFieldAttributes.ShowInViewForms));
        }

        public static bool? GetAllowDeletion(this Field field)
        {
            var xml = field.SchemaXml;

            return ConvertUtils.ToBool(XElement.Parse(xml).GetAttributeValue(BuiltInFieldAttributes.AllowDeletion));
        }
    }
}
