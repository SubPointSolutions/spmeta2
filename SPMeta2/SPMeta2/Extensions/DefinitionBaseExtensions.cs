using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Extensions
{
    public static class DefinitionBaseExtensions
    {
        #region property bag

        public static DefinitionBase SetPropertyBagValue(this DefinitionBase definition, string name)
        {
            return SetPropertyBagValue(definition, name, true);
        }

        public static DefinitionBase SetPropertyBagValue(this DefinitionBase definition, string name, object value)
        {
            return SetPropertyBagValue(definition, name, value != null ? definition.ToString() : string.Empty);
        }

        public static DefinitionBase SetPropertyBagValue(this DefinitionBase definition, string name, string value)
        {
            var prop = FindPropertyBagValue(definition, name);

            if (prop == null)
            {
                prop = new PropertyBagValue
                {
                    Name = name,
                    Value = value
                };

                definition.PropertyBag.Add(prop);
            }

            prop.Value = value;

            return definition;
        }

        public static string GetPropertyBagValue(this DefinitionBase definition, string name, string value)
        {
            var prop = FindPropertyBagValue(definition, name);

            if (prop != null)
                return prop.Value;

            return null;
        }

        public static PropertyBagValue FindPropertyBagValue(this DefinitionBase definition, string name)
        {
            return definition.PropertyBag.FirstOrDefault(p => p.Name.ToUpper() == name.ToUpper());
        }

        public static bool HasPropertyBagValue(this DefinitionBase definition, string name)
        {
            return FindPropertyBagValue(definition, name) != null;
        }

        #endregion
    }
}
