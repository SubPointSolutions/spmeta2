using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class WebPartDefinitionGenerator : TypedDefinitionGeneratorServiceBase<WebPartDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Id = Rnd.String();
                def.Title = Rnd.String();

                def.ZoneId = "FullPage";
                def.ZoneIndex = Rnd.Int(100);

                //def.WebpartXmlTemplate = DefaultWebpartTemplates.ContentEditorWebpart;
                def.WebpartFileName = BuiltInWebpartFileNames.MSContentEditor;
            });
        }
    }
}
