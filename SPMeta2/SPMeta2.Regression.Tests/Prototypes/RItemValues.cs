using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Containers.Services;
using SPMeta2.Definitions.Fields;

namespace SPMeta2.Regression.Tests.Prototypes
{
    public static class RItemValues
    {
        public static TextFieldDefinition GetRequiredTextField(ModelGeneratorService service)
        {
            return service.GetRandomDefinition<TextFieldDefinition>(def =>
            {

                def.ShowInDisplayForm = true;
                def.ShowInEditForm = true;
                def.ShowInListSettings = true;
                def.ShowInNewForm = true;
                def.ShowInVersionHistory = true;
                def.ShowInViewForms = true;

                def.ValidationFormula = null;
                def.ValidationMessage = null;

                def.Indexed = false;
                def.Hidden = false;

                def.DefaultValue = string.Empty;
                def.Required = true;

            });
        }

        public static TextFieldDefinition GetRandomTextField(ModelGeneratorService service)
        {
            return service.GetRandomDefinition<TextFieldDefinition>(def =>
            {
                def.ShowInDisplayForm = true;
                def.ShowInEditForm = true;
                def.ShowInListSettings = true;
                def.ShowInNewForm = true;
                def.ShowInVersionHistory = true;
                def.ShowInViewForms = true;

                def.ValidationFormula = null;
                def.ValidationMessage = null;

                def.Indexed = false;
                def.Hidden = false;

                def.DefaultValue = string.Empty;
                def.Required = true;

            });
        }
    }
}
