using System;
using System.Collections.Generic;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Syntax.Default.Utils;

namespace SPMeta2.Containers.DefinitionGenerators.Webparts
{
    public class UserCodeWebPartDefinitionGenerator : TypedDefinitionGeneratorServiceBase<UserCodeWebPartDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Id = Rnd.String();
                def.Title = Rnd.String();

                def.ZoneId = "FullPage";
                def.ZoneIndex = Rnd.Int(100);

                def.SolutionId = new Guid("e9a61998-07f2-45e9-ae43-9e93fa6b11bb");

                def.AssemblyFullName = "SPMeta2.Containers.SandboxSolutionContainer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a18c22c531026525";
                def.TypeFullName = "SPMeta2.Containers.SandboxSolutionContainer.WebParts.ContainerWebPart.ContainerWebPart";


                def.UserCodeProperties.Add(new UserCodeProperty
                {
                    Name = "StringProperty",
                    Value = Rnd.String()
                });

                def.UserCodeProperties.Add(new UserCodeProperty
                {
                    Name = "IntegerProperty",
                    Value = Rnd.Int(512).ToString()
                });
            });
        }

        public override IEnumerable<DefinitionBase> GetAdditionalArtifacts()
        {
            var sandboxSolution = new SandboxSolutionDefinition
            {
                FileName = string.Format("{0}.wsp", Rnd.String()),
                Activate = true,

                SolutionId = new Guid("e9a61998-07f2-45e9-ae43-9e93fa6b11bb"),
                Content = ModuleFileUtils.FromResource(GetType().Assembly, "SPMeta2.Containers.Templates.Apps.SPMeta2.Containers.SandboxSolutionContainer.wsp")
            };

            var webpartFeature = new FeatureDefinition
            {
                Scope = FeatureDefinitionScope.Site,
                Id = new Guid("9acf0f59-3cdc-412b-a647-4185dd4cd9bc"),
                Enable = true
            };

            return new DefinitionBase[] { sandboxSolution, webpartFeature };
        }
    }
}
