﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Services.Impl
{
    public class DefaultXMLSerializationService : SerializationServiceBase
    {
        public override string Serialize(object obj)
        {
            var serializer = new DataContractSerializer(obj.GetType(), KnownTypes);

            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                ms.Position = 0;

                using (var sr = new StreamReader(ms))
                    return sr.ReadToEnd();
            }
        }

        public override object Deserialize(Type type, string objString)
        {
            var serializer = new DataContractSerializer(type, KnownTypes);

            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(objString);
                    writer.Flush();

                    stream.Position = 0;

                    return serializer.ReadObject(stream);
                }
            }
        }
    }
}