using System;

namespace SPMeta2.Definitions
{
    public class FieldDefinition : DefinitionBase
    {
        #region contructors

        public FieldDefinition()
        {
            // it needs to be string.Empty to avoid challenges with null VS string.Empty test cases for strings
            Description = string.Empty;
        }

        #endregion

        #region properties

        public string InternalName { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public string Group { get; set; }

        public Guid Id { get; set; }

        public string FieldType { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("InternalName:[{0}] Id:[{1}] Title:[{2}]", InternalName, Id, Title);
        }

        #endregion
    }
}
