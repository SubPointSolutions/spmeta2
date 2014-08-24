namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to attach security rile to the target security group.
    /// </summary>
    public class SecurityRoleLinkDefinition : DefinitionBase
    {
        #region contructors

        public SecurityRoleLinkDefinition()
        {

        }

        #endregion

        #region properties

        /// <summary>
        /// Target security role name.
        /// </summary>
        public string SecurityRoleName { get; set; }

        /// <summary>
        /// Type of the target role.
        /// 
        /// </summary>
        public string SecurityRoleType { get; set; }

        /// <summary>
        /// ID of the target security role.
        /// </summary>
        public int SecurityRoleId { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("SecurityRoleName:[{0}]", SecurityRoleName);
        }

        #endregion
    }
}
