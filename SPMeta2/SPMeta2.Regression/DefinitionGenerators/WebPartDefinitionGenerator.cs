using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.DefinitionGenerators
{
    public class WebPartDefinitionGenerator : TypedDefinitionGeneratorServiceBase<WebPartDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Id = Rnd.String();
                def.Title = Rnd.String();

                def.ZoneId = "Main";
                def.ZoneIndex = 10;

                //def.WebpartXmlTemplate = DefaultWebpartTemplates.ContentEditorWebpart;
                def.WebpartFileName = BuiltInWebpartFileNames.MSContentEditor;
            });
        }
    }
}
