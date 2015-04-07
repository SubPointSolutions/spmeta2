using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Services
{
    public abstract class SerializationServiceBase
    {
        #region constructors

        public SerializationServiceBase()
        {
            KnownTypes = new List<Type>();
        }

        #endregion

        #region properties

        protected List<Type> KnownTypes { get; set; }

        #endregion

        #region methods

        public void RegisterKnownTypes(IEnumerable<Type> types)
        {
            foreach (var type in types)
                RegisterKnownType(type);
        }

        public void RegisterKnownType(Type type)
        {
            if (!KnownTypes.Contains(type))
                KnownTypes.Add(type);

        }

        public abstract string Serialize(object obj);
        public abstract object Deserialize(Type type, string objString);

        #endregion

    }
}
