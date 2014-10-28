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

        protected virtual XElement GetMinimalFieldXml()
        {
            return new XElement("Field",
                new XAttribute("Type", BuiltInFieldTypes.TaxonomyFieldType),
                new XAttribute("Name", string.Empty),
                new XAttribute("Title", string.Empty),
                new XAttribute("StaticName", string.Empty),
                new XAttribute("DisplayName", string.Empty),
                new XAttribute("Required", "FALSE"),
                new XAttribute("ID", String.Empty));
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

            var taxSession = new TaxonomySession(site);
            TermStore tesmStore = null;

            if (taxFieldModel.UseDefaultSiteCollectionTermStore == true)
                tesmStore = taxSession.DefaultSiteCollectionTermStore;
            else if (taxFieldModel.SspId.HasValue)
                tesmStore = taxSession.TermStores[taxFieldModel.SspId.Value];
            else if (!string.IsNullOrEmpty(taxFieldModel.SspName))
                tesmStore = taxSession.TermStores[taxFieldModel.SspName];

            TermSet termSet = null;

            if (taxFieldModel.TermSetId.HasValue)
                termSet = tesmStore.GetTermSet(taxFieldModel.TermSetId.Value);
            else if (!string.IsNullOrEmpty(taxFieldModel.TermSetName))
                termSet = tesmStore.GetTermSets(taxFieldModel.TermSetName, taxFieldModel.TermSetLCID).FirstOrDefault();

            Term term = null;

            if (taxFieldModel.TermId.HasValue)
                term = tesmStore.GetTerm(taxFieldModel.TermId.Value);
            else if (!string.IsNullOrEmpty(taxFieldModel.TermName))
                term = tesmStore.GetTerms(taxFieldModel.TermName, taxFieldModel.TermLCID, false).FirstOrDefault();

            taxField.SspId = tesmStore.Id;

            if (termSet != null)
                taxField.TermSetId = termSet.Id;
            else if (term != null)
                taxField.TermSetId = term.Id;
        }

        protected override string GetTargetSPFieldXmlDefinition(FieldDefinition fieldModel)
        {
            var taxFieldModel = fieldModel.WithAssertAndCast<TaxonomyFieldDefinition>("model", value => value.RequireNotNull());
            var taxFieldXml = GetMinimalFieldXml();

            taxFieldXml
                .SetAttribute("Title", taxFieldModel.Title)
                .SetAttribute("DisplayName", taxFieldModel.Title)

                .SetAttribute("Required", taxFieldModel.Required.ToString())

                .SetAttribute("Name", taxFieldModel.InternalName)
                .SetAttribute("StaticName", taxFieldModel.InternalName)

                .SetAttribute("ID", taxFieldModel.Id.ToString("B"));

            return taxFieldXml.ToString();
        }

        #endregion
    }
}
