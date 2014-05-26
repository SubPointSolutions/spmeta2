using System;
using SPDevLab.SPMeta2.Definitions;

namespace SPMeta2.Definitions
{
    public class ContentTypeDefinition : DefinitionBase
    {
        #region properties

        public Guid Id { get; set; }
        public string IdNumberValue { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Group { get; set; }

        public string ParentContentTypeId { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("Name:[{0}] Id:[{1}] IdNumberValue:[{2}]", Name, Id, IdNumberValue);
        }

        #endregion
    }
}
