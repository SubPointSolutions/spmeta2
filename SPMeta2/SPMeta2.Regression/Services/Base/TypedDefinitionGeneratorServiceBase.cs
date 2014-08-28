using SPMeta2.Regression.Services.Rnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Services.Base
{
    public abstract class TypedDefinitionGeneratorServiceBase<TModelDefinition> : DefinitionGeneratorServiceBase
    {
        public TypedDefinitionGeneratorServiceBase()
        {
            Rnd = new DefaultRandomService();
        }

        protected RandomService Rnd { get; set; }

        public override Type TargetType
        {
            get { return typeof(TModelDefinition); }
        }

        protected virtual TModelDefinition WithEmptyDefinition()
        {
            return WithEmptyDefinition(null);
        }

        protected virtual TModelDefinition WithEmptyDefinition<TModelDefinition>(Action<TModelDefinition> action)
        {
            return WithEmptyDefinition(action);
        }

        protected virtual TModelDefinition WithEmptyDefinition(Action<TModelDefinition> action)
        {
            var definition = (TModelDefinition)Activator.CreateInstance<TModelDefinition>();

            if (action != null)
                action(definition);

            return definition;
        }

    }
}
