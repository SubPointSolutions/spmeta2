using SPMeta2.Definitions;
using SPMeta2.Regression.CSOM.Validation;
using SPMeta2.Services;

namespace SPMeta2.Regression.CSOM
{
    public class CSOMValidationService : ModelServiceBase
    {
        public CSOMValidationService()
        {
            ModelHandlers.Add(typeof(FieldDefinition), new ClientFieldDefinitionValidator());
            ModelHandlers.Add(typeof(ContentTypeFieldLinkDefinition), new ClientContentTypeFieldLinkDefinitionValidator());

            ModelHandlers.Add(typeof(ContentTypeDefinition), new ClientContentTypeDefinitionValidator());
            ModelHandlers.Add(typeof(ContentTypeLinkDefinition), new ClientContentTypeLinkDefinitionValidator());

            ModelHandlers.Add(typeof(SecurityGroupDefinition), new ClientSecurityGroupDefinitionValidator());
            ModelHandlers.Add(typeof(SecurityGroupLinkDefinition), new ClientSecurityGroupLinkDefinitionValidator());

            ModelHandlers.Add(typeof(SecurityRoleDefinition), new ClientSecurityRoleDefinitionValidator());
            ModelHandlers.Add(typeof(SecurityRoleLinkDefinition), new ClientSecurityRoleLinkDefinitionValidator());

            ModelHandlers.Add(typeof(ListDefinition), new ClientListDefinitionValidator());
            ModelHandlers.Add(typeof(ListViewDefinition), new ClientListViewDefinitionValidator());

            ModelHandlers.Add(typeof(WebPartPageDefinition), new ClientWebPartPageDefinitionValidator());
            ModelHandlers.Add(typeof(WikiPageDefinition), new ClientWikiPageDefinitionValidator());

            ModelHandlers.Add(typeof(WebDefinition), new ClientWebDefinitionValidator());
            ModelHandlers.Add(typeof(SiteDefinition), new ClientSiteDefinitionValidator());
        }
    }
}
