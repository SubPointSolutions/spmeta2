namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint security group.
    /// </summary>
    public class SecurityGroupDefinition : DefinitionBase
    {
        #region properties

        /// <summary>
        /// Name of the target security group.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the target security group.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Login name of the owner for the target security group.
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// Default user login for the target security group.
        /// </summary>
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
