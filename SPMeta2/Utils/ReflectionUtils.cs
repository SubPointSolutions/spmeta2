using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SPMeta2.Utils
{
    public static class ReflectionUtils
    {
        #region methods

        public static IEnumerable<Type> GetTypesFromCurrentDomain<TType>()
        {
            return GetTypesFromAssemblies<TType>(AppDomain.CurrentDomain.GetAssemblies());
        }

        public static IEnumerable<Type> GetTypesFromAssembly<TType>(Assembly assembly)
        {
            return GetTypesFromAssemblies<TType>(new[] { assembly });
        }

        public static IEnumerable<Type> GetTypesFromAssemblies<TType>(IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(a => a.GetTypes())
                       .Where(t => typeof(TType).IsAssignableFrom(t) && !t.IsAbstract);
        }

        #endregion
    }
}
