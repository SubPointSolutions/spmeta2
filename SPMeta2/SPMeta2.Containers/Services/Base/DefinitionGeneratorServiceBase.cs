using System;
using System.Collections.Generic;
using System.Linq;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Containers.Services.Base
{
    public abstract class DefinitionGeneratorServiceBase
    {
        #region properties

        public abstract Type TargetType { get; }

        #endregion

        #region methods

        public virtual DefinitionBase GetCustomParenHost()
        {
            return null;
        }

        public virtual IEnumerable<DefinitionBase> GetAdditionalArtifacts()
        {
            return Enumerable.Empty<DefinitionBase>();
        }

        public virtual DefinitionBase GenerateRandomDefinition()
        {
            return GenerateRandomDefinition(null);
        }

        public abstract DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action);

        #endregion

        public virtual IEnumerable<DefinitionBase> GetReplacementArtifacts()
        {
            return Enumerable.Empty<DefinitionBase>();
        }
    }
}
