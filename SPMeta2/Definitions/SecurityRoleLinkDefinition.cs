namespace SPMeta2.Definitions
{
    public class SecurityRoleLinkDefinition : DefinitionBase
    {
        #region contructors

        public SecurityRoleLinkDefinition()
        {

        }

        #endregion

        #region properties

        public string SecurityRoleName { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("SecurityRoleName:[{0}]", SecurityRoleName);
        }

        #endregion
    }
}
