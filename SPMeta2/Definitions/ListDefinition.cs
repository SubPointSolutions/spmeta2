namespace SPMeta2.Definitions
{
    public class ListDefinition : DefinitionBase
    {
        public ListDefinition()
        {
            Description = string.Empty;
        }

        #region properties

        public string Title { get; set; }
        public string Description { get; set; }

        public string Url { get; set; }

        public int TemplateType { get; set; }
        public string TemplateName { get; set; }

        public bool ContentTypesEnabled { get; set; }

        public bool NeedBreakRoleInheritance { get; set; }
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
