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

        protected override Type GetTargetFieldType(FieldDefinition model)
        {
            return typeof(TaxonomyField);
        }

        #endregion

        #region methods

        protected override void ProcessFieldProperties(Field field, FieldDefinition fieldModel)
        {
            var context = CurrentSiteModelHost.HostClientContext;

            var taxFieldModel = fieldModel.WithAssertAndCast<TaxonomyFieldDefinition>("model", value => value.RequireNotNull());


            var termStore = LookupTermStore(CurrentSiteModelHost, taxFieldModel);
            var storeContext = CurrentSiteModelHost.HostClientContext;

            var termSet = LookupTermSet(CurrentSiteModelHost, termStore, taxFieldModel); ;
            var term = LookupTerm(CurrentSiteModelHost, termStore, taxFieldModel); ;

            var taxField = context.CastTo<TaxonomyField>(field);

            // let base setting be setup
            base.ProcessFieldProperties(taxField, fieldModel);
            //context.ExecuteQueryWithTrace();

            taxField.AllowMultipleValues = taxFieldModel.IsMulti;

            taxField.Description = string.IsNullOrEmpty(taxFieldModel.Description)
               ? string.Empty
               : taxFieldModel.Description;

            taxField.Required = taxFieldModel.Required;

            if (termStore != null)
                taxField.SspId = termStore.Id;

            if (termSet != null)
                taxField.TermSetId = termSet.Id;

            if (term != null)
                taxField.AnchorId = term.Id;

            //context.ExecuteQueryWithTrace();
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

        public static Term LookupTerm(SiteModelHost currentSiteModelHost, TermStore termStore, TaxonomyFieldDefinition termModel)
        {
            var context = currentSiteModelHost.HostClientContext;
            Term result = null;

            if (termModel.TermId.HasValue)
            {
                var scope = new ExceptionHandlingScope(context);
                using (scope.StartScope())
                {
                    using (scope.StartTry())
                    {
                        result = termStore.GetTerm(termModel.TermId.Value);
                        context.Load(result);
                    }

                    using (scope.StartCatch())
                    {

                    }
                }

                context.ExecuteQueryWithTrace();
            }
            else if (!string.IsNullOrEmpty(termModel.TermName))
            {
                var terms = termStore.GetTerms(new LabelMatchInformation(context)
                {
                    Lcid = termModel.TermLCID,
                    TermLabel = termModel.TermName,
                    TrimUnavailable = false
                });

                context.Load(terms);
                context.ExecuteQueryWithTrace();

                result = terms.FirstOrDefault();
            }

            if (result != null && result.ServerObjectIsNull == false)
            {
                context.Load(result);
                context.ExecuteQueryWithTrace();

                return result;
            }

            return null;
        }
    }
}
