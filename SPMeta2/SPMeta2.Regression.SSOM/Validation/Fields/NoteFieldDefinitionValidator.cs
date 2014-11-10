using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;
using SPMeta2.SSOM.ModelHandlers.Fields;
using SPMeta2.SSOM.ModelHosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SPMeta2.Utils;
using Microsoft.SharePoint;

namespace SPMeta2.Regression.SSOM.Validation.Fields
{
    public class NoteFieldDefinitionValidator : ChoiceFieldModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<NoteFieldDefinition>("model", value => value.RequireNotNull());

            var site = typedModelHost.HostSite;
            var spObject = GetField(modelHost, definition) as SPFieldMultiLineText;

            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldBeEqual(m => m.Title, o => o.Title)
                                 .ShouldBeEqual(m => m.InternalName, o => o.InternalName)
                                 .ShouldBeEqual(m => m.Group, o => o.Group)
                                 .ShouldBeEqual(m => m.FieldType, o => o.TypeAsString)
                                 .ShouldBeEqual(m => m.Id, o => o.Id)
                                 .ShouldBeEqual(m => m.Description, o => o.Description)
                                 .ShouldBeEqual(m => m.Required, o => o.Required);


            // TODO

        }
    }
}
