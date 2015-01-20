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

            TermStore tesmStore = LookupTermStore(site, taxFieldModel);
            TermSet termSet = LookupTermSet(tesmStore, taxFieldModel);
            Term term = LookupTerm(tesmStore, taxFieldModel);

            taxField.AllowMultipleValues = taxFieldModel.IsMulti;
            taxField.SspId = tesmStore.Id;

            if (termSet != null)
                taxField.TermSetId = termSet.Id;

            if (term != null)
                taxField.AnchorId = term.Id;
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
            if (taxFieldModel.TermSetId.HasValue)
                return tesmStore.GetTermSet(taxFieldModel.TermSetId.Value);

            if (!string.IsNullOrEmpty(taxFieldModel.TermSetName))
                return tesmStore.GetTermSets(taxFieldModel.TermSetName, taxFieldModel.TermSetLCID).FirstOrDefault();

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
