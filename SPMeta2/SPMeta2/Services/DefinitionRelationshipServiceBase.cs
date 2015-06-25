using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }

    public abstract class DefinitionRelationshipServiceBase
    {
        public abstract IEnumerable<DefinitionRelationship> GetDefinitionRelationships();
    }
}
