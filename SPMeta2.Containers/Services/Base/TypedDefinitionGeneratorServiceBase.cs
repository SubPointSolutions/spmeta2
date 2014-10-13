using System;
using SPMeta2.Containers.Services.Rnd;

namespace SPMeta2.Containers.Services.Base
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

        protected virtual TModelDefinition WithEmptyDefinition<TModelDefinition>()
        {
            return (TModelDefinition)Activator.CreateInstance<TModelDefinition>();
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
