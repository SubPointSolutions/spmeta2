using Microsoft.SharePoint.Client.Taxonomy;
using SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy;
using SPMeta2.CSOM.Standard.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Utils;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation.Taxonomy
{
    public class TaxonomyTermDefinitionValidator : TaxonomyTermModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<TaxonomyTermDefinition>("model", value => value.RequireNotNull());

            

            Term spObject = null;

            if (modelHost is TermModelHost)
                spObject = FindTermInTerm((modelHost as TermModelHost).HostTerm, definition);
            else if (modelHost is TermSetModelHost)
                spObject = FindTermInTermSet((modelHost as TermSetModelHost).HostTermSet, definition);
            else
            {
                throw new SPMeta2UnsupportedModelHostException(string.Format("Model host of type: [{0}] is not supported", modelHost.GetType()));

            }

            TermExtensions.CurrentLCID = definition.LCID;

            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldNotBeNull(spObject)
                                 .ShouldBeEqual(m => m.Name, o => o.Name)
                                 .ShouldBeEqual(m => m.Description, o => o.GetDefaultLCIDDescription());

            assert.SkipProperty(m => m.LCID, "LCID is not accessible from OM. Should be alright while provision.");


            if (definition.Id.HasValue)
            {
                assert.ShouldBeEqual(m => m.Id, o => o.Id);
            }
            else
            {
                assert.SkipProperty(m => m.Id, "Id is null. Skipping property.");
            }
        }
    }

    internal static class TermExtensions
    {
        public static int CurrentLCID { get; set; }

        public static string GetDefaultLCIDDescription(this Term term)
        {
            var context = term.Context;

            var resultValue = term.GetDescription(CurrentLCID);
            context.ExecuteQuery();

            return resultValue.Value;
        }
    }
}
