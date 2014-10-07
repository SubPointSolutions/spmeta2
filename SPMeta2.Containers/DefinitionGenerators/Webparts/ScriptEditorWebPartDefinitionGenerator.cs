using System;
using System.Text;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;

namespace SPMeta2.Containers.DefinitionGenerators.Webparts
{
    public class ScriptEditorWebPartDefinitionGenerator : TypedDefinitionGeneratorServiceBase<ScriptEditorWebPartDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Id = Rnd.String();
                def.Title = Rnd.String();

                def.ZoneId = "FullPage";
                def.ZoneIndex = Rnd.Int(100);

                var script = new StringBuilder();

                script.Append("<script>");
                script.AppendFormat(" console.log('SPMeta2-ScriptEditorWebPart-Hello-{0}');", Rnd.String(16));
                script.Append("</script>");

                def.Content = script.ToString();
            });
        }
    }
}
