using System.Collections.ObjectModel;

namespace SPMeta2.Definitions
{
    public class SecurityRoleDefinition : DefinitionBase
    {
        #region contructors

        public SecurityRoleDefinition()
        {
            BasePermissions = new Collection<string>();
        }

        #endregion

        #region properties

        public string Name { get; set; }
        public string Description { get; set; }

        public Collection<string> BasePermissions { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("Name:[{0}] Description:[{1}]", Name, Description);
        }

        #endregion
    }
}
