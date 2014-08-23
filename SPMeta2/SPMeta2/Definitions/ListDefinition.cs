namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint list.
    /// </summary>
    public class ListDefinition : DefinitionBase
    {
        public ListDefinition()
        {
            Description = string.Empty;
        }

        #region properties

        /// <summary>
        /// Title of the target list.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description of the target list.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// URL of the target list.
        /// Don't use "list/my-list-name" as URL is calculated by TemplateType/TemplateName properties.
        /// Provision automatically adds '/lists/' if necessary.
        /// 
        /// Use ListDefinition.GetListUrl() methods to resolve particular list URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Template type of the target list.
        /// BuiltInListTemplateTypeId class can be used to utilize out of the box SharePoint list types.
        /// 
        /// Skip TemplateType and use TemplateName property to create list based on custom template.
        /// </summary>
        public int TemplateType { get; set; }

        /// <summary>
        /// Template name of the target list. 
        /// This property is used to allow list creation based on custom templates.
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// Enables content type support on the target list.
        /// </summary>
        public bool ContentTypesEnabled { get; set; }

        /// <summary>
        /// Reserved for the future. Is not used.
        /// </summary>
        public bool NeedBreakRoleInheritance { get; set; }

        /// <summary>
        /// Reserved for the future. Is not used.
        /// </summary>
        public bool? NeedToCopyRoleAssignmets { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("Title: [{0}] Url: [{1}] TemplateType:[{2}] TemplateName:[{3}]",
                            new[] {
                                Title,
                                Url,
                                TemplateType.ToString(),
                                TemplateName                                
                            });
        }

        #endregion
    }
}
