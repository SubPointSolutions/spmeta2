using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
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
            context.ExecuteQueryWithTrace();

            var termStore = LookupTermStore(CurrentSiteModelHost, taxFieldModel);
            var storeContext = CurrentSiteModelHost.HostClientContext;

            taxField.AllowMultipleValues = taxFieldModel.IsMulti;

            TermSet termSet = LookupTermSet(CurrentSiteModelHost, termStore, taxFieldModel); ;

            if (termStore != null)
                taxField.SspId = termStore.Id;

            if (termSet != null)
                taxField.TermSetId = termSet.Id;
        }

        public static TermStore LookupTermStore(SiteModelHost currentSiteModelHost, TaxonomyFieldDefinition taxFieldModel)
        {
            var termStore = TaxonomyTermStoreModelHandler.FindTermStore(currentSiteModelHost,
                                  taxFieldModel.SspName,
                                  taxFieldModel.SspId,
                                  taxFieldModel.UseDefaultSiteCollectionTermStore);

            if (termStore == null)
                throw new ArgumentNullException("termStore is NULL. Please define SspName, SspId or ensure there is a default term store for the giving site.");

            var storeContext = currentSiteModelHost.HostClientContext;

            storeContext.Load(termStore, s => s.Id);
            storeContext.ExecuteQueryWithTrace();

            return termStore;
        }

        #endregion

        public static TermSet LookupTermSet(SiteModelHost currentSiteModelHost, TermStore termStore, TaxonomyFieldDefinition taxFieldModel)
        {
            var storeContext = currentSiteModelHost.HostClientContext;

            if (!string.IsNullOrEmpty(taxFieldModel.TermSetName))
            {
                var termSets = termStore.GetTermSetsByName(taxFieldModel.TermSetName, taxFieldModel.TermSetLCID);

                storeContext.Load(termSets);
                storeContext.ExecuteQueryWithTrace();

                return termSets.FirstOrDefault();
            }

            if (taxFieldModel.TermSetId.HasValue)
            {
                TermSet termSet = null;

                var scope = new ExceptionHandlingScope(storeContext);
                using (scope.StartScope())
                {
                    using (scope.StartTry())
                    {
                        termSet = termStore.GetTermSet(taxFieldModel.TermSetId.Value);
                        storeContext.Load(termSet);
                    }

                    using (scope.StartCatch())
                    {

                    }
                }

                storeContext.ExecuteQueryWithTrace();

                if (termSet != null && termSet.ServerObjectIsNull == false)
                {
                    storeContext.Load(termSet, g => g.Id);
                    storeContext.ExecuteQueryWithTrace();

                    return termSet;
                }
            }

            return null;
        }
    }
}
