using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint content database.
    /// </summary>
    /// 
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.SPContentDatabase", "Microsoft.SharePoint")]

    [DefaultParentHostAttribute(typeof(WebApplicationDefinition))]
    [DefaultRootHost(typeof(WebApplicationDefinition))]

    [Serializable]

    [ExpectWithExtensionMethod]
    public class ContentDatabaseDefinition : DefinitionBase
    {
        #region constructors

        public ContentDatabaseDefinition()
        {
            Status = 0;
        }

        #endregion

        #region properties

        [ExpectValidation]
        public string ServerName { get; set; }

        [ExpectValidation]
        public string DbName { get; set; }

        public string UserName { get; set; }
        public string UserPassword { get; set; }

        [ExpectValidation]
        public int WarningSiteCollectionNumber { get; set; }

        [ExpectValidation]
        public int MaximumSiteCollectionNumber { get; set; }

        public int Status { get; set; }

        #endregion

        #region methods
      
        public override string ToString()
        {
            return new ToStringResult<ContentDatabaseDefinition>(this)
                          .AddPropertyValue(p => p.ServerName)
                          .AddPropertyValue(p => p.DbName)
                          .AddPropertyValue(p => p.WarningSiteCollectionNumber)
                          .AddPropertyValue(p => p.MaximumSiteCollectionNumber)
                          .ToString();
        }

        #endregion
    }
}
