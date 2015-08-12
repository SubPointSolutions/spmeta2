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

            var termStore = LookupTermStore(CurrentSiteModelHost, taxFieldModel, false);

            TermSet termSet = null;
            Term term = null;

            if (termStore != null)
            {
                termSet = LookupTermSet(CurrentSiteModelHost, termStore, taxFieldModel);
                term = LookupTerm(CurrentSiteModelHost, termStore, taxFieldModel);
            }

            var taxField = context.CastTo<TaxonomyField>(field);

            // let base setting be setup
            base.ProcessFieldProperties(taxField, fieldModel);

            taxField.AllowMultipleValues = taxFieldModel.IsMulti;

            if (taxFieldModel.Open.HasValue)
                taxField.Open = taxFieldModel.Open.Value;

            if (taxFieldModel.IsPathRendered.HasValue)
                taxField.IsPathRendered = taxFieldModel.IsPathRendered.Value;

            if (taxFieldModel.CreateValuesInEditForm.HasValue)
                taxField.CreateValuesInEditForm = taxFieldModel.CreateValuesInEditForm.Value;

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
        }

        public static TermStore LookupTermStore(SiteModelHost currentSiteModelHost,
            TaxonomyFieldDefinition taxFieldModel)
        {
            return LookupTermStore(currentSiteModelHost, taxFieldModel, false);
        }

        public static TermStore LookupTermStore(SiteModelHost currentSiteModelHost,
            TaxonomyFieldDefinition taxFieldModel, bool raiseNullRefException)
        {
            var termStore = TaxonomyTermStoreModelHandler.FindTermStore(currentSiteModelHost,
                                  taxFieldModel.SspName,
                                  taxFieldModel.SspId,
                                  taxFieldModel.UseDefaultSiteCollectionTermStore);

            if (termStore == null && raiseNullRefException)
                throw new ArgumentNullException("termStore is NULL. Please define SspName, SspId or ensure there is a default term store for the giving site.");

            if (termStore != null)
            {
                var storeContext = currentSiteModelHost.HostClientContext;

                storeContext.Load(termStore, s => s.Id);
                storeContext.ExecuteQueryWithTrace();
            }

            return termStore;
        }

        #endregion

        public static TermSet LookupTermSet(
            SiteModelHost currentSiteModelHost,
            TermStore termStore,
            TaxonomyFieldDefinition taxFieldModel)
        {
            var storeContext = currentSiteModelHost.HostClientContext;

            return LookupTermSet(storeContext, currentSiteModelHost.HostSite, termStore, taxFieldModel);
        }

        public static TermSet LookupTermSet(ClientRuntimeContext context, Site site,
            TermStore termStore, TaxonomyFieldDefinition taxFieldModel)
        {
            return LookupTermSet(context, termStore, site,
                taxFieldModel.TermGroupName, taxFieldModel.TermGroupId, taxFieldModel.IsSiteCollectionGroup,
                taxFieldModel.TermSetName, taxFieldModel.TermSetId, taxFieldModel.TermSetLCID);
        }

        public static TermSet LookupTermSet(ClientRuntimeContext context, TermStore termStore,
            Site site,
            string termGroupName, Guid? termGroupId, bool? isSiteCollectionGroup,
            string termSetName, Guid? termSetId, int termSetLCID)
        {
            var storeContext = context;

            TermGroup currenGroup = null;

            if (!string.IsNullOrEmpty(termGroupName))
            {
                currenGroup = termStore.Groups.GetByName(termGroupName);

                storeContext.Load(currenGroup);
                storeContext.ExecuteQueryWithTrace();
            }
            else if (termGroupId != null && termGroupId.HasGuidValue())
            {
                currenGroup = termStore.Groups.GetById(termGroupId.Value);

                storeContext.Load(currenGroup);
                storeContext.ExecuteQueryWithTrace();
            }
            else if (isSiteCollectionGroup == true)
            {
                currenGroup = termStore.GetSiteCollectionGroup(site, false);

                storeContext.Load(currenGroup);
                storeContext.ExecuteQueryWithTrace();
            }

            if (!string.IsNullOrEmpty(termSetName))
            {
                if (currenGroup != null && (currenGroup.ServerObjectIsNull == false))
                {
                    TermSet termSet = null;

                    var scope = new ExceptionHandlingScope(storeContext);
                    using (scope.StartScope())
                    {
                        using (scope.StartTry())
                        {
                            termSet = currenGroup.TermSets.GetByName(termSetName);
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
                else
                {
                    var termSets = termStore.GetTermSetsByName(termSetName, termSetLCID);

                    storeContext.Load(termSets);
                    storeContext.ExecuteQueryWithTrace();

                    return termSets.FirstOrDefault();
                }
            }

            if (termSetId.HasGuidValue())
            {
                if (currenGroup != null && (currenGroup.ServerObjectIsNull == false))
                {
                    TermSet termSet = null;

                    var scope = new ExceptionHandlingScope(storeContext);
                    using (scope.StartScope())
                    {
                        using (scope.StartTry())
                        {
                            termSet = currenGroup.TermSets.GetById(termSetId.Value);
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
                else
                {
                    TermSet termSet = null;

                    var scope = new ExceptionHandlingScope(storeContext);
                    using (scope.StartScope())
                    {
                        using (scope.StartTry())
                        {
                            termSet = termStore.GetTermSet(termSetId.Value);
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
            }

            return null;
        }

        public static Term LookupTerm(SiteModelHost currentSiteModelHost, TermStore termStore, TaxonomyFieldDefinition termModel)
        {
            var context = currentSiteModelHost.HostClientContext;
            var site = currentSiteModelHost.HostSite;

            Term result = null;

            TermGroup currenGroup = null;

            var termGroupName = termModel.TermGroupName;
            var termGroupId = termModel.TermGroupId;
            var isSiteCollectionGroup = termModel.IsSiteCollectionGroup;

            if (!string.IsNullOrEmpty(termGroupName))
            {
                currenGroup = termStore.Groups.GetByName(termGroupName);

                context.Load(currenGroup);
                context.ExecuteQueryWithTrace();
            }
            else if (termGroupId != null && termGroupId.HasGuidValue())
            {
                currenGroup = termStore.Groups.GetById(termGroupId.Value);

                context.Load(currenGroup);
                context.ExecuteQueryWithTrace();
            }
            else if (isSiteCollectionGroup == true)
            {
                currenGroup = termStore.GetSiteCollectionGroup(site, false);

                context.Load(currenGroup);
                context.ExecuteQueryWithTrace();
            }

            if (currenGroup != null)
            {
                if (termModel.TermId.HasValue)
                {
                    // by ID, the only one match

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

                    context.Load(terms, t => t.Include(
                                                i => i.Id,
                                                i => i.Name,
                                                i => i.TermSet,
                                                i => i.TermSet.Group,
                                                i => i.TermSet.Group.Name
                                                ));
                    context.ExecuteQueryWithTrace();

                    result = terms.FirstOrDefault(t => t.TermSet.Group.Name == currenGroup.Name);
                }
            }

            else
            {

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
