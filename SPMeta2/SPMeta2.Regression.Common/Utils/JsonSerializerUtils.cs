using System.Web.Script.Serialization;

namespace SPMeta2.Regression.Common.Utils
{
    public class JsonSerializerUtils
    {
        #region methods
        public static string SerializeToJsonString(object obj)
        {
            return new JavaScriptSerializer().Serialize(obj);
        }

        #endregion
    }
}
