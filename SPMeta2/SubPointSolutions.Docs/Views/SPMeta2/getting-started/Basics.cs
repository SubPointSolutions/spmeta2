using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.Services;
using SPMeta2.Definitions;
using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;
using SubPointSolutions.Docs.Code.Consts;
using System;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    [TestClass]
    public class Basics : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.Basics")]
        public void ABigPictureSample(ClientContext clientContext)
        {
            // Step 1, create 'definitions' - a bunch of CSharp POCO objects 
            var clientDescriptionField = new FieldDefinition
            {
                Title = "Client Description",
                InternalName = "dcs_ClientDescription",
                Group = DocConsts.DefaulFieldsGroup,
                Id = new Guid("06975b67-01f5-47d7-9e2e-2702dfb8c217"),
                FieldType = BuiltInFieldTypes.Note,
            };

            var customerAccountContentType = new ContentTypeDefinition
            {
                Name = "Customer Account",
                Id = new Guid("ddc46a66-19a0-460b-a723-c84d7f60a342"),
                ParentContentTypeId = BuiltInContentTypeId.Item,
                Group = DocConsts.DefaultContentTypeGroup
            };

            // step 2, define relationships between definitions
            // we need to build a logical 'model tree'

            // fields and content types live under site
            // so use SiteModel and add fields/content types under site
            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(clientDescriptionField);
                site.AddContentType(customerAccountContentType);
            });

            // step 3, deploy site model via CSOM
            var csomProvisionService = new CSOMProvisionService();
            csomProvisionService.DeploySiteModel(clientContext, siteModel);
        }

        #endregion
    }
}