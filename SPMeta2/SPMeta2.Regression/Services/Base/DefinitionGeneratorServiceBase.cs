using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Services.Base
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
    }
}
