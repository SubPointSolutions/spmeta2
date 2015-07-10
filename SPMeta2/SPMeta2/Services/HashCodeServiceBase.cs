using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;


namespace SPMeta2.Services
{
    public abstract class HashCodeServiceBase
    {
        #region methods

        public abstract string GetHashCode(object instance);

        #endregion
    }



    public class MD5HashCodeServiceBase : HashCodeServiceBase
    {
        private readonly MD5CryptoServiceProvider _cryptoServiceProvider = new MD5CryptoServiceProvider();

        public override string GetHashCode(object instance)
        {
            var serializer = new DataContractSerializer(instance.GetType());

            using (var memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, instance);

                _cryptoServiceProvider.ComputeHash(memoryStream.ToArray());
                return Convert.ToBase64String(_cryptoServiceProvider.Hash);
            }
        }
    }
}
