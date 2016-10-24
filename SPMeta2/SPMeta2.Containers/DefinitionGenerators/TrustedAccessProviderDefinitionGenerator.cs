using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Containers.Utils;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class TrustedAccessProviderDefinitionGenerator : TypedDefinitionGeneratorServiceBase<TrustedAccessProviderDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Name = Rnd.String();
                //def.MetadataEndPoint = string.Format("https://localhost/{0}.aspx", Rnd.String());

                def.Certificate = X509Utils.GenerateRandomSelfSignedCertificate_AsCertBytes(Rnd.String(), Rnd.String());
            });
        }
    }
}
