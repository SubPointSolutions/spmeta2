using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Utils
{
    public class CamlQueryUtils
    {
        #region methods

        public static string WhereItemsByFieldValueBeginsWithQuery(string fieldName, string fieldValue)
        {
            return
                string.Format(
                    "<Where><BeginsWith><FieldRef Name='{0}' /><Value Type='{1}'>{2}</Value></BeginsWith></Where>",
                    fieldName, "Text", fieldValue);
        }

        public static string WhereItemByFieldValueQuery(string fieldName, string fieldValue)
        {
            return WhereItemByFieldQuery(fieldName, "Text", fieldValue);
        }

        public static string WhereItemByFieldQuery(string fieldName, string fieldType, string fieldValue)
        {
            return
                string.Format(
                    "<Where><Eq><FieldRef Name='{0}' /><Value Type='{1}'>{2}</Value></Eq></Where>",
                    fieldName, fieldType, fieldValue);
        }

        public static string WhereItemByFileLeafRefQuery(string fileName)
        {
            return WhereItemByFieldValueQuery("FileLeafRef", fileName);
        }

        #endregion
    }
}
