using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class AuditSettingsDefinitionGenerator : TypedDefinitionGeneratorServiceBase<AuditSettingsDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                if (Rnd.Bool())
                {
                    def.AuditFlags = new Collection<string>{
                        BuiltInAuditMaskType.CheckIn,
                        BuiltInAuditMaskType.Copy,
                        BuiltInAuditMaskType.Move
                    };
                }

                else
                {
                    def.AuditFlags = new Collection<string>
                    {
                      BuiltInAuditMaskType.CheckIn,
                       BuiltInAuditMaskType.Move
                    };
                }
            });
        }
    }
}
