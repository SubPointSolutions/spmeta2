using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace SPMeta2.Services
{
    public class MD5HashCodeServiceBase : HashCodeServiceBase
    {
        #region properties

        private MD5CryptoServiceProvider _cryptoServiceProvider = new MD5CryptoServiceProvider();

        #endregion

        #region methods

        protected virtual List<Type> GetKnownTypesFromInstance(object instance)
        {
            var result = new List<Type>();

            var instanceType = instance.GetType();
            var props = instanceType.GetProperties();

            result.Add(instanceType);

            foreach (var prop in props)
            {
                if (!result.Contains(prop.PropertyType))
                    result.Add(prop.PropertyType);
            }

            return result;
        }

        public override string GetHashCode(object instance)
        {
            // this is a bit lazy
            // we should not expect hashed for non-string and non-object types
            if (instance is string)
            {
                var instanceStringArray = Encoding.UTF8.GetBytes(instance as string);

                _cryptoServiceProvider.ComputeHash(instanceStringArray);
                return Convert.ToBase64String(_cryptoServiceProvider.Hash);
            }
            else
            {
                // accumulating known types for serialization from the actual type
                this.RegisterKnownTypes(GetKnownTypesFromInstance(instance));

                var serializer = new DataContractSerializer(instance.GetType(), KnownTypes);

                using (var memoryStream = new MemoryStream())
                {
                    serializer.WriteObject(memoryStream, instance);

                    _cryptoServiceProvider.ComputeHash(memoryStream.ToArray());
                    return Convert.ToBase64String(_cryptoServiceProvider.Hash);
                }
            }
        }

        protected override void InternalDispose(bool disposing)
        {
            base.InternalDispose(disposing);

            if (disposing)
            {
                if (_cryptoServiceProvider != null)
                {
#if !NET35
                    // OMG
                    _cryptoServiceProvider.Dispose();
#endif
                }
            }
        }

        #endregion
    }
}
