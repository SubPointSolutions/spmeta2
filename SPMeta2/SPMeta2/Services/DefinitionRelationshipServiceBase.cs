using System;
using System.Collections.Generic;

namespace SPMeta2.Services
{
    public class DefinitionRelationship
    {
        public DefinitionRelationship()
        {
            HostTypes = new List<Type>();
        }

        public Type DefinitionType { get; set; }

        public List<Type> HostTypes { get; set; }

        public override string ToString()
        {
            if (DefinitionType != null)
                return DefinitionType.Name;

            return base.ToString();
        }
    }

    public abstract class DefinitionRelationshipServiceBase
    {
        public abstract IEnumerable<DefinitionRelationship> GetDefinitionRelationships();
    }
}
