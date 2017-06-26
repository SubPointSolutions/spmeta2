using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Services;

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

        #region provision compatibility

        public static ModelProvisionCompatibilityResult CheckProvisionCompatibility(this DefinitionBase definition)
        {
            var service = ServiceContainer.Instance.GetService<ModelCompatibilityServiceBase>();

            return service.CheckProvisionCompatibility(definition);
        }

        public static bool IsCSOMCompatible(this DefinitionBase definition)
        {
            var compatibilityResult = CheckProvisionCompatibility(definition);
            var result = compatibilityResult.Result.All(r => r.IsCSOMCompatible.HasValue
                                                       && r.IsCSOMCompatible.Value);

            return result;
        }

        public static bool IsSSOMCompatible(this DefinitionBase model)
        {
            var compatibilityResult = CheckProvisionCompatibility(model);
            var result = compatibilityResult.Result.All(r => r.IsSSOMCompatible.HasValue
                                                             && r.IsSSOMCompatible.Value);

            return result;
        }

        #endregion
    }
}
