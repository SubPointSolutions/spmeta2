using System.Collections.Generic;

namespace SPMeta2.Definitions.Base
{
    public abstract class TemplateDefinitionBase : PageDefinitionBase
    {
        public TemplateDefinitionBase()
        {
            Content = new byte[0];

            TargetControlTypes = new List<string>();
        }

        public string Description { get; set; }
        public bool HiddenTemplate { get; set; }

        public List<string> TargetControlTypes { get; set; }

        public byte[] Content { get; set; }

        public string PreviewURL { get; set; }
        public string PreviewDescription { get; set; }
    }
}
