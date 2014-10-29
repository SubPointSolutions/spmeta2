using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Standard.ModelHandlers.Fields
{
    public class TaxonomyFieldModelHandler : FieldModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(TaxonomyFieldDefinition); }
        }

        #endregion

        #region methods

        protected override void ProcessFieldProperties(Field field, FieldDefinition fieldModel)
        {
            var context = CurrentSiteModelHost.HostClientContext;

            var taxFieldModel = fieldModel.WithAssertAndCast<TaxonomyFieldDefinition>("model", value => value.RequireNotNull());
            var taxField = context.CastTo<TaxonomyField>(field);

            // let base setting be setup
            base.ProcessFieldProperties(taxField, fieldModel);
            context.ExecuteQuery();

            var termStore = TaxonomyTermStoreModelHandler.FindTermStore(CurrentSiteModelHost,
                                  taxFieldModel.SspName,
                                  taxFieldModel.SspId,
                                  taxFieldModel.UseDefaultSiteCollectionTermStore);

            if (termStore == null)
                throw new ArgumentNullException("termStore is NULL. Please define SspName, SspId or ensure there is a default term store for the giving site.");

            var storeContext = CurrentSiteModelHost.HostClientContext;

            storeContext.Load(termStore, s => s.Id);
            storeContext.ExecuteQuery();

            // TODO
            TermSet termSet = null;

            if (!string.IsNullOrEmpty(taxFieldModel.TermSetName))
            {
                var termSets = termStore.GetTermSetsByName(taxFieldModel.TermSetName, taxFieldModel.TermSetLCID);

                storeContext.Load(termSets);
                storeContext.ExecuteQuery();

                termSet = termSets.FirstOrDefault();
            }

            if (termStore != null)
                taxField.SspId = termStore.Id;

            if (termSet != null)
                taxField.TermSetId = termSet.Id;
        }

        #endregion
    }
}
