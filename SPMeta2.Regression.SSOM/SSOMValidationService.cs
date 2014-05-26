using SPMeta2.Definitions;
using SPMeta2.Regression.SSOM.Validation;
using SPMeta2.Regression.Validation.ServerModelHandlers;
using SPMeta2.Services;

namespace SPMeta2.Regression.SSOM
{
    public class SSOMValidationService : ModelServiceBase
    {
        public SSOMValidationService()
        {
            ModelHandlers.Add(typeof(FieldDefinition), new FieldDefinitionValidator());
            ModelHandlers.Add(typeof(ContentTypeFieldLinkDefinition), new ContentTypeFieldLinkDefinitionValidator());

            ModelHandlers.Add(typeof(ContentTypeDefinition), new ContentTypeDefinitionValidator());
            ModelHandlers.Add(typeof(ContentTypeLinkDefinition), new ContentTypeLinkDefinitionValidator());

            ModelHandlers.Add(typeof(SecurityGroupDefinition), new SecurityGroupDefinitionValidator());
            ModelHandlers.Add(typeof(SecurityRoleDefinition), new SecurityRoleDefinitionValidator());

            ModelHandlers.Add(typeof(SecurityGroupLinkDefinition), new SecurityGroupLinkDefinitionValidator());
            ModelHandlers.Add(typeof(SecurityRoleLinkDefinition), new SecurityRoleLinkDefinitionValidator());

            ModelHandlers.Add(typeof(ListDefinition), new ListDefinitionValidator());
            ModelHandlers.Add(typeof(ListViewDefinition), new ListViewDefinitionValidator());

            ModelHandlers.Add(typeof(WebPartPageDefinition), new WebPartPageDefinitionValidator());
            ModelHandlers.Add(typeof(WikiPageDefinition), new WikiPageDefinitionValidator());

            ModelHandlers.Add(typeof(WebDefinition), new WebDefinitionValidator());
            ModelHandlers.Add(typeof(SiteDefinition), new SiteDefinitionValidator());
        }
    }
}
