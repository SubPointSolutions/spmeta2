using Microsoft.SharePoint.Client;

namespace SPMeta2.CSOM.Extensions
{
    public class CamlQueryTemplates
    {
        #region methods

        public static CamlQuery ItemsByFieldValueBeginsWithQuery(string fieldName, string fieldValue)
        {
            return new CamlQuery
            {
                ViewXml = string.Format("<View><Query><Where><BeginsWith><FieldRef Name='{0}' /><Value Type='{1}'>{2}</Value></BeginsWith></Where></Query></View>",
                        fieldName, "Text", fieldValue)
            };
        }

        public static CamlQuery ItemByFieldValueQuery(string fieldName, string fieldValue)
        {
            return GetItemByFieldQuery(fieldName, "Text", fieldValue);
        }

        public static CamlQuery GetItemByFieldQuery(string fieldName, string fieldType, string fieldValue)
        {
            return new CamlQuery
            {
                ViewXml = string.Format("<View><Query><Where><Eq><FieldRef Name='{0}' /><Value Type='{1}'>{2}</Value></Eq></Where></Query><RowLimit>1</RowLimit></View>",
                        fieldName, fieldType, fieldValue)
            };
        }

        public static CamlQuery ItemByFileNameQuery(string fileName)
        {
            return ItemByFieldValueQuery("FileLeafRef", fileName);
        }

        #endregion
    }
}
