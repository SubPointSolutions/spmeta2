using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Validation.Common;

namespace SPMeta2.Validation.Validators.Relationships
{
    public class ListDefinitionModelNodeValidator : RelationshipValidatorBase
    {
        public override void Validate(ModelNode model, List<ValidationResult> result)
        {
            ValidateAllowedTypes<ListDefinition>(model,
                new[]
                {
                    typeof (ContentTypeFieldLinkDefinition),
                    
                    typeof (FolderDefinition),
                    typeof (FieldDefinition),

                    typeof (SecurityGroupLinkDefinition),

                    typeof (ListItemDefinition),

                    typeof (WikiPageDefinition),
                    // TODO, remove to SPMeta2.Standard.Validation
                    //typeof (PublishingPageDefinition),
                    typeof (WebPartPageDefinition),
                }, result);
        }
    }
}
