using System.Linq;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Containers.Extensions
{
    public static class ModelNodeExtensions
    {
        //RegExcludeFromEventsValidation

        public static ModelNode RegExcludeFromEventsValidation(this ModelNode node)
        {
            EnsurePropKey(node, "M2.RegExcludeFromEventsValidation", "1");

            return node;
        }

        public static ModelNode RegExcludeFromValidation(this ModelNode node)
        {
            EnsurePropKey(node, "M2.RegExcludeFromValidation", "1");

            return node;
        }

        public static bool RegIsExcludedFromValidation(this ModelNode node)
        {
            return HasPropKey(node, "M2.RegExcludeFromValidation");
        }

        public static bool RegIsExcludeFromEventsValidation(this ModelNode node)
        {
            return HasPropKey(node, "M2.RegExcludeFromEventsValidation");
        }

        private static bool HasPropKey(this ModelNode node, string key)
        {
            var excistringKey = node.PropertyBag.FirstOrDefault(p => p.Name.ToUpper() == key.ToUpper());

            return excistringKey != null;
        }

        private static ModelNode EnsurePropKey(this ModelNode node, string key, string value)
        {
            var excistringKey = node.PropertyBag.FirstOrDefault(p => p.Name.ToUpper() == key.ToUpper());

            if (excistringKey == null)
            {
                excistringKey = new PropertyBagValue
                {
                    Name = key
                };

                node.PropertyBag.Add(excistringKey);
            }

            excistringKey.Value = value;

            return node;
        }
    }
}
