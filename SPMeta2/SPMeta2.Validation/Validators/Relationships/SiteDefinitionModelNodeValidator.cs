using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Validation.Common;

namespace SPMeta2.Validation.Validators.Relationships
{
    public class SiteDefinitionModelNodeValidator : RelationshipValidatorBase
    {
        public override void Validate(ModelNode model, List<ValidationResult> result)
        {
            ValidateAllowedTypes<SiteDefinition>(model,
                new[]
                {
                    typeof (FieldDefinition),
                    typeof (ContentTypeDefinition),

                    typeof (FeatureDefinition),

                    typeof (UserCustomActionDefinition),

                    typeof (SecurityGroupDefinition),
                    typeof (SecurityRoleDefinition),

                }, result);
        }
    }
}
