using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Xml.Serialization;

namespace SPMeta2.Utils
{
    public class XmlSerializerUtils
    {
        #region methods

        public static string SerializeToString(object obj)
        {
            return SerializeToString(obj, new Type[] { });
        }

        public static string SerializeToString(object obj, IEnumerable<Type> extraTypes)
        {
            var serializer = new XmlSerializer(obj.GetType(), extraTypes.ToArray());

            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);

                return writer.ToString();
            }
        }

        public static T DeserializeFromString<T>(string value)
        {
            return DeserializeFromString<T>(value, new Type[] { });
        }

        public static T DeserializeFromString<T>(string value, IEnumerable<Type> extraTypes)
        {
            var result = DeserializeFromString(typeof(T), value, extraTypes);

            if (result != null)
                return (T)result;

            return default(T);
        }

        public static object DeserializeFromString(Type type, string value)
        {
            return DeserializeFromString(type, value, new Type[] { });
        }

        public static object DeserializeFromString(Type type, string value, IEnumerable<Type> extraTypes)
        {
            var serializer = new XmlSerializer(type, extraTypes.ToArray());

            if (!string.IsNullOrEmpty(value))
            {
                using (var reader = new StringReader(value))
                {
                    return serializer.Deserialize(reader);
                }
            }

            return null;
        }

        #endregion
    }
}
