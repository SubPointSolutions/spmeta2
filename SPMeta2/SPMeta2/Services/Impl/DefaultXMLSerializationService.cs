using System;
using System.IO;
using System.Runtime.Serialization;

namespace SPMeta2.Services.Impl
{
    public class DefaultXMLSerializationService : SerializationServiceBase
    {
        public override string Serialize(object obj)
        {
            var serializer = new DataContractSerializer(obj.GetType(), KnownTypes);

            Stream ms = null;
            StreamReader sr = null;

            try
            {
                ms = new MemoryStream();

                serializer.WriteObject(ms, obj);
                ms.Position = 0;

                sr = new StreamReader(ms);

                return sr.ReadToEnd();
            }
            finally
            {
                if (sr != null)
                    sr.Dispose();
                else if (ms != null)
                    ms.Dispose();
            }
        }

        public override object Deserialize(Type type, string objString)
        {
            var serializer = new DataContractSerializer(type, KnownTypes);

            Stream stream = null;
            StreamWriter writer = null;

            try
            {
                stream = new MemoryStream();
                writer = new StreamWriter(stream);

                writer.Write(objString);
                writer.Flush();

                stream.Position = 0;

                return serializer.ReadObject(stream);

            }
            finally
            {
                if (writer != null)
                    writer.Dispose();
                else if (stream != null)
                    stream.Dispose();
            }
        }
    }
}
