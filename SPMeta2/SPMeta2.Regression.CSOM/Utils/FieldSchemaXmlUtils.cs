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
        public static string GetSystemInstanceName(this Field field)
        {
            var xml = field.SchemaXml;
            return ConvertUtils.ToString(XElement.Parse(xml).GetAttributeValue(BuiltInFieldAttributes.SystemInstance));
        }

        public static string GetEntityNamespace(this Field field)
        {
            var xml = field.SchemaXml;
            return ConvertUtils.ToString(XElement.Parse(xml).GetAttributeValue(BuiltInFieldAttributes.EntityNamespace));
        }

        public static string GetEntityName(this Field field)
        {
            var xml = field.SchemaXml;
            return ConvertUtils.ToString(XElement.Parse(xml).GetAttributeValue(BuiltInFieldAttributes.EntityName));
        }

        public static string GetBdcFieldName(this Field field)
        {
            var xml = field.SchemaXml;
            return ConvertUtils.ToString(XElement.Parse(xml).GetAttributeValue(BuiltInFieldAttributes.BdcField));
        }

        public static string GetEditFormat(this Field field)
        {
            var xml = field.SchemaXml;
            return ConvertUtils.ToString(XElement.Parse(xml).GetAttributeValue(BuiltInFieldAttributes.Format));
        }

        public static string GetFriendlyDisplayFormat(this FieldDateTime field)
        {
            return field.FriendlyDisplayFormat.ToString();
        }

        public static string GetDisplayFormat(this FieldDateTime field)
        {
            return field.DisplayFormat.ToString();
        }

        public static string GetCalendarType(this FieldDateTime field)
        {
            return field.DateTimeCalendarType.ToString();
        }



        public static IEnumerable<string> GetFieldReferences(this Field field)
        {
            var xml = field.SchemaXml;

            var result = new List<string>();

            var fieldRefRootNode = XElement.Parse(xml).Descendants("FieldRefs").FirstOrDefault();
            var fieldRefNodes = fieldRefRootNode.Descendants("FieldRef");

            foreach (var fieldRefNode in fieldRefNodes)
                result.Add(fieldRefNode.GetAttributeValue("Name"));

            return result;
        }

        public static string GetOutputType(this Field field)
        {
            var xml = field.SchemaXml;

            return ConvertUtils.ToString(XElement.Parse(xml).GetAttributeValue(BuiltInFieldAttributes.ResultType));
        }

        public static string GetDisplayFormatString(this Field field)
        {
            var value = GetDisplayFormat(field);

            if (value == 0)
                return BuiltInNumberFormatTypes.Automatic;

            if (value == 1)
                return BuiltInNumberFormatTypes.OneDecimal;

            if (value == 2)
                return BuiltInNumberFormatTypes.TwoDecimals;

            if (value == 3)
                return BuiltInNumberFormatTypes.ThreeDecimals;

            if (value == 4)
                return BuiltInNumberFormatTypes.FourDecimals;

            if (value == 5)
                return BuiltInNumberFormatTypes.FiveDecimals;



            throw new ArgumentException("BuiltInNumberFormatTypes");

        }


        public static int GetDisplayFormat(this Field field)
        {
            var xml = field.SchemaXml;

            return ConvertUtils.ToInt(XElement.Parse(xml).GetAttributeValue(BuiltInFieldAttributes.Decimals)).Value;
        }

        public static string GetDateFormatString(this Field field)
        {
            var value = GetDateFormat(field);

            if (value == 0)
                return BuiltInDateTimeFieldFormatType.DateOnly;

            if (value == 1)
                return BuiltInDateTimeFieldFormatType.DateTime;

            throw new ArgumentException("BuiltInDateTimeFieldFormatType");

        }

        public static int GetDateFormat(this Field field)
        {
            var xml = field.SchemaXml;

            return ConvertUtils.ToInt(XElement.Parse(xml).GetAttributeValue(BuiltInFieldAttributes.Format)).Value;
        }

        public static int GetCurrencyLocaleId(this Field field)
        {
            var xml = field.SchemaXml;

            return ConvertUtils.ToInt(XElement.Parse(xml).GetAttributeValue(BuiltInFieldAttributes.LCID)).Value;
        }



        public static bool GetShowAsPercentage(this Field field)
        {
            var xml = field.SchemaXml;

            return ConvertUtils.ToBool(XElement.Parse(xml).GetAttributeValue(BuiltInFieldAttributes.Percentage)).Value;
        }

        public static int GetDecimals(this Field field)
        {
            var xml = field.SchemaXml;

            return ConvertUtils.ToInt(XElement.Parse(xml).GetAttributeValue(BuiltInFieldAttributes.Decimals)).Value;
        }

        public static string GetDecimalsAsString(this Field field)
        {
            var xml = field.SchemaXml;

            return GetDecimalsValue(GetDecimals(field));
        }

        private static string GetDecimalsValue(int value)
        {
            if (value == -1)
                return BuiltInNumberFormatTypes.Automatic;

            if (value == 0)
                return BuiltInNumberFormatTypes.NoDecimal;

            if (value == 1)
                return BuiltInNumberFormatTypes.OneDecimal;

            if (value == 2)
                return BuiltInNumberFormatTypes.TwoDecimals;

            if (value == 3)
                return BuiltInNumberFormatTypes.ThreeDecimals;

            if (value == 4)
                return BuiltInNumberFormatTypes.FourDecimals;

            if (value == 5)
                return BuiltInNumberFormatTypes.FiveDecimals;

            throw new ArgumentException("Decimals");
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
