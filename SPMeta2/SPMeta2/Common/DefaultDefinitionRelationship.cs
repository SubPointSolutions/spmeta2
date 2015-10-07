using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Definitions;
using SPMeta2.Services;
using SPMeta2.Utils;

namespace SPMeta2.Common
{
    public static class DefaultDefinitionRelationship
    {
        #region constructors

        static DefaultDefinitionRelationship()
        {
            Relationships = new List<DefinitionRelationship>();

            InitRelatonships();
        }

        #endregion

        #region properties

        public static List<DefinitionRelationship> Relationships { get; set; }

        #endregion

        #region methods

        private static void InitRelatonships()
        {
            InitFromParentHostCapabilityAttribute(typeof(DefaultDefinitionRelationship).Assembly);
        }

        public static void InitFromParentHostCapabilityAttribute(Assembly assembly)
        {
            var definitionTypes = ReflectionUtils.GetTypesFromAssembly<DefinitionBase>(assembly);

            foreach (var defType in definitionTypes)
                InitFromParentHostCapabilityAttribute(defType);
        }

        public static void InitFromParentHostCapabilityAttribute(Type definitionType)
        {
            var parentHostAttrs = definitionType
                .GetCustomAttributes(typeof(ParentHostCapabilityAttribute), true) as ParentHostCapabilityAttribute[];

            if (parentHostAttrs != null)
            {
                foreach (var attr in parentHostAttrs)
                {
                    if (attr.HostType == null)
                        continue;

                    var parentType = attr.HostType;

                    var currentRelationShip = Relationships.FirstOrDefault(r => r.DefinitionType == definitionType);

                    if (currentRelationShip == null)
                    {
                        currentRelationShip = new DefinitionRelationship
                        {
                            DefinitionType = definitionType
                        };

                        Relationships.Add(currentRelationShip);
                    }

                    if (!currentRelationShip.HostTypes.Contains(parentType))
                        currentRelationShip.HostTypes.Add(parentType);
                }
            }
        }

        #endregion
    }
}
