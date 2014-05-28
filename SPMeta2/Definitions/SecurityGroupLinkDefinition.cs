namespace SPMeta2.Definitions
{
    public class SecurityGroupLinkDefinition : DefinitionBase
    {
        public SecurityGroupLinkDefinition()
        {

        }

        #region properties

        public string SecurityGroupName { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("SecurityGroupName:[{0}]", SecurityGroupName);
        }

        #endregion
    }
}
