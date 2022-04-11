using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace SPMeta2.Services
{
    public abstract class HashCodeServiceBase : IDisposable
    {
        #region constructors

        public HashCodeServiceBase()
        {
            KnownTypes = new List<Type>();
        }

        #endregion

        #region methods

        public abstract string GetHashCode(object instance);
        public List<Type> KnownTypes { get; set; }

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

    public static class HashCodeServiceBaseExtensions
    {
        public static void RegisterKnownTypes(this HashCodeServiceBase service, IEnumerable<Type> types)
        {
            foreach (var type in types)
                if (!service.KnownTypes.Contains(type))
                    service.KnownTypes.Add(type);
        }
    }
}
