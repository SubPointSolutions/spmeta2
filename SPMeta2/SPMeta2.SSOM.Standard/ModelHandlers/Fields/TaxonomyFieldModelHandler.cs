using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Taxonomy;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.Standard.ModelHandlers.Fields
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

        protected override void ProcessFieldProperties(SPField field, FieldDefinition fieldModel)
        {
            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

            // process taxonomy field specific properties
            var taxField = field.WithAssertAndCast<TaxonomyField>("field", value => value.RequireNotNull());
            var taxFieldModel = fieldModel.WithAssertAndCast<TaxonomyFieldDefinition>("model", value => value.RequireNotNull());

            var site = GetCurrentSite();

            taxField.AllowMultipleValues = taxFieldModel.IsMulti;

            if (taxFieldModel.Open.HasValue)
                taxField.Open = taxFieldModel.Open.Value;

            if (taxFieldModel.CreateValuesInEditForm.HasValue)
                taxField.CreateValuesInEditForm = taxFieldModel.CreateValuesInEditForm.Value;

            if (taxFieldModel.IsPathRendered.HasValue)
                taxField.IsPathRendered = taxFieldModel.IsPathRendered.Value;

            TermStore tesmStore = LookupTermStore(site, taxFieldModel);

            if (tesmStore != null)
            {
                taxField.SspId = tesmStore.Id;

                TermSet termSet = LookupTermSet(tesmStore, taxFieldModel);
                Term term = LookupTerm(tesmStore, taxFieldModel);

                if (termSet != null)
                    taxField.TermSetId = termSet.Id;

                if (term != null)
                    taxField.AnchorId = term.Id;
            }
        }

        public static Term LookupTerm(TermStore tesmStore, TaxonomyFieldDefinition taxFieldModel)
        {
            if (taxFieldModel.TermId.HasValue)
                return tesmStore.GetTerm(taxFieldModel.TermId.Value);

            if (!string.IsNullOrEmpty(taxFieldModel.TermName))
                return tesmStore.GetTerms(taxFieldModel.TermName, taxFieldModel.TermLCID, false).FirstOrDefault();

            return null;
        }

        public static TermSet LookupTermSet(TermStore tesmStore, TaxonomyFieldDefinition taxFieldModel)
        {
            return LookupTermSet(tesmStore,
                taxFieldModel.TermSetName,
                taxFieldModel.TermSetId,
                taxFieldModel.TermSetLCID
                );
        }

        public static TermSet LookupTermSet(TermStore tesmStore, string termSetName, Guid? termSetId, int termSetLCID)
        {
            if (termSetId.HasGuidValue())
                return tesmStore.GetTermSet(termSetId.Value);

            if (!string.IsNullOrEmpty(termSetName))
                return tesmStore.GetTermSets(termSetName, termSetLCID).FirstOrDefault();

            return null;
        }

        public static TermStore LookupTermStore(SPSite site, TaxonomyFieldDefinition taxFieldModel)
        {
            var taxSession = new TaxonomySession(site);

            if (taxFieldModel.UseDefaultSiteCollectionTermStore == true)
                return taxSession.DefaultSiteCollectionTermStore;

            if (taxFieldModel.SspId.HasValue)
                return taxSession.TermStores[taxFieldModel.SspId.Value];

            if (!string.IsNullOrEmpty(taxFieldModel.SspName))
                return taxSession.TermStores[taxFieldModel.SspName];

            return null;
        }

        #endregion


    }
}
