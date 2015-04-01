using System;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.Standard.Definitions.Taxonomy
{
    /// <summary>
    /// Allows to define and taxonomy termset.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Taxonomy.TermSet", "Microsoft.SharePoint.Taxonomy")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Taxonomy.TermSet", "Microsoft.SharePoint.Client.Taxonomy")]

    [DefaultParentHost(typeof(TaxonomyTermGroupDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [ExpectAddHostExtensionMethod]
    [Serializable]
    [ExpectArrayExtensionMethod]

    public class TaxonomyTermSetDefinition : DefinitionBase
    {
        #region constructors

        public TaxonomyTermSetDefinition()
        {
            LCID = 1033;
            IsAvailableForTagging = true;
        }

        #endregion

        #region properties

        [ExpectValidation]
        [ExpectRequired(GroupName = "Term Identity")]
        public string Name { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        public string Description { get; set; }

        [ExpectValidation]
        [ExpectRequired(GroupName = "Term Identity")]
        public Guid? Id { get; set; }

        [ExpectValidation]
        public int LCID { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        public bool IsAvailableForTagging { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        public bool IsOpenForTermCreation { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<TaxonomyTermSetDefinition>(this)
                          .AddPropertyValue(p => p.Name)
                          .AddPropertyValue(p => p.Id)
                          .AddPropertyValue(p => p.LCID)
                          .ToString();
        }

        #endregion
    }
}
