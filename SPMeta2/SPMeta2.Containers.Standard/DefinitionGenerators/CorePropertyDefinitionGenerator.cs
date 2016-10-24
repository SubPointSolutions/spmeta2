using System;
using System.Collections.Generic;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Containers.Standard.DefinitionGenerators
{
    public class CorePropertyDefinitionGenerator : TypedDefinitionGeneratorServiceBase<CorePropertyDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Name = string.Format("UserProperty{0}", Rnd.String(5));
                def.DisplayName = Rnd.String();

                def.Type = Rnd.RandomFromArray(new string[]{
                        BuiltInPropertyDataType.String,
                        BuiltInPropertyDataType.Integer,
                        BuiltInPropertyDataType.Email
                    }
                );

                if (def.Type == BuiltInPropertyDataType.String ||
                    def.Type == BuiltInPropertyDataType.Email)
                {
                    def.Length = 1 + Rnd.Int(25);
                }
            });
        }
    }
}
