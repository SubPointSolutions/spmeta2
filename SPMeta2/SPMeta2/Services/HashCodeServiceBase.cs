using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace SPMeta2.Services
{
    public abstract class HashCodeServiceBase : IDisposable
    {
        #region methods

        public abstract string GetHashCode(object instance);

        public void Dispose()
        {
            InternalDispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void InternalDispose(bool disposing)
        {
            if (disposing)
            {

            }
        }

        #endregion
    }



    public class MD5HashCodeServiceBase : HashCodeServiceBase
    {
        #region properties

        private MD5CryptoServiceProvider _cryptoServiceProvider = new MD5CryptoServiceProvider();

        #endregion

        #region methods

        public override string GetHashCode(object instance)
        {
            var instanceType = instance.GetType();

            var knowTypes = new List<Type>();
            knowTypes.Add(instanceType);

            var props = instanceType.GetProperties();
            foreach (var prop in props)
            {
                if (!knowTypes.Contains(prop.PropertyType))
                    knowTypes.Add(prop.PropertyType);
            }

            var serializer = new DataContractSerializer(instance.GetType(), knowTypes);

            using (var memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, instance);

                _cryptoServiceProvider.ComputeHash(memoryStream.ToArray());
                return Convert.ToBase64String(_cryptoServiceProvider.Hash);
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
