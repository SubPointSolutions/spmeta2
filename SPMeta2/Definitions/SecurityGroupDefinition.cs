namespace SPMeta2.Definitions
{
    public class SecurityGroupDefinition : DefinitionBase
    {
        #region properties

        public string Name { get; set; }
        public string Description { get; set; }

        public string Owner { get; set; }
        public string DefaultUser { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("Name:[{0}] Description:[{1}]", Name, Description);
        }

        #endregion
    }
}
