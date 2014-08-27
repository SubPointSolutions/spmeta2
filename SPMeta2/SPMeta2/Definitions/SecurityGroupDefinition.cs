using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint security group.
    /// </summary>
    /// 
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPGroup", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Group", "Microsoft.SharePoint.Client")]

    [RootHostAttribute(typeof(SiteDefinition))]
    [ParentHostAttribute(typeof(SiteDefinition))]

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

        /// <summary>
        /// Flag to mimic out of the box AssociatedOwnerGroup
        /// </summary>
        public bool IsAssociatedVisitorsGroup { get; set; }

        /// <summary>
        /// Flag to mimic AssociatedMemberGroup
        /// </summary>
        public bool IsAssociatedMemberGroup { get; set; }

        /// <summary>
        /// Flag to mimic AssociatedOwnerGroup
        /// </summary>

        public bool IsAssociatedOwnerGroup { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("Name:[{0}] Description:[{1}]", Name, Description);
        }

        #endregion
    }
}
