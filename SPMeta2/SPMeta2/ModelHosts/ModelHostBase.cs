using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.ModelHosts
{
    /// <summary>
    /// Base model host for provision flow.
    /// </summary>
    public class ModelHostBase : ICloneable
    {
        #region constructors

        public ModelHostBase()
        {
            ShouldUpdateHost = true;
        }

        #endregion

        #region static

        public static ModelHostBase Inherit(ModelHostBase modelHost)
        {
            return Inherit<ModelHostBase>(modelHost);
        }

        public static T Inherit<T>(ModelHostBase modelHost)
            where T : ModelHostBase, new()
        {
            return Inherit<T>(modelHost, null);
        }

        public static T Inherit<T>(ModelHostBase modelHost, Action<T> action)
           where T : ModelHostBase, new()
        {
            var source = modelHost.Clone() as ModelHostBase;
            var result = new T();

            CopyProperties(source, result);

            if (action != null)
                action(result);

            return result;
        }

        internal static void CopyProperties(object source, object target)
        {
            var customerType = target.GetType();
            foreach (var prop in source.GetType().GetProperties())
            {
                var propGetter = prop.GetGetMethod();

                var targetProperty = customerType.GetProperty(prop.Name);

                if (targetProperty != null)
                {
                    var propSetter = customerType.GetProperty(prop.Name).GetSetMethod();
                    var valueToSet = propGetter.Invoke(source, null);

                    propSetter.Invoke(target, new[] { valueToSet });
                }
            }
        }

        #endregion

        #region properties

        public bool ShouldUpdateHost { get; set; }

        #endregion

        #region methods

        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }
}
