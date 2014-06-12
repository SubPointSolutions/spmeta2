using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Regression.Model.Definitions;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl
{
    [TestClass]
    public class FieldTests : SPMeta2RegresionTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Regression.SiteFields")]
        public void CanProvision_SiteBooleanField()
        {
            var model = SPMeta2Model
                 .NewSiteModel(site => site.AddField(RegSiteFields.BooleanField));

            WithProvisionRunners(runner => runner.DeploySiteModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.SiteFields")]
        public void CanProvision_SiteChoiceField()
        {
            var model = SPMeta2Model
                 .NewSiteModel(site => site.AddField(RegSiteFields.ChoiceField));

            WithProvisionRunners(runner => runner.DeploySiteModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.SiteFields")]
        public void CanProvision_SiteCurrencyField()
        {
            var model = SPMeta2Model
                 .NewSiteModel(site => site.AddField(RegSiteFields.CurrencyField));

            WithProvisionRunners(runner => runner.DeploySiteModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.SiteFields")]
        public void CanProvision_SiteDateTimeField()
        {
            var model = SPMeta2Model
                 .NewSiteModel(site => site.AddField(RegSiteFields.DateTimeField));

            WithProvisionRunners(runner => runner.DeploySiteModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.SiteFields")]
        public void CanProvision_SiteGuidField()
        {
            var model = SPMeta2Model
                 .NewSiteModel(site => site.AddField(RegSiteFields.GuidField));

            WithProvisionRunners(runner => runner.DeploySiteModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.SiteFields")]
        public void CanProvision_SiteLookupField()
        {
            var model = SPMeta2Model
                 .NewSiteModel(site => site.AddField(RegSiteFields.LookupField));

            WithProvisionRunners(runner => runner.DeploySiteModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.SiteFields")]
        public void CanProvision_SiteMultiChoiceField()
        {
            var model = SPMeta2Model
                 .NewSiteModel(site => site.AddField(RegSiteFields.MultiChoiceField));

            WithProvisionRunners(runner => runner.DeploySiteModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.SiteFields")]
        public void CanProvision_SiteNoteField()
        {
            var model = SPMeta2Model
                 .NewSiteModel(site => site.AddField(RegSiteFields.NoteField));

            WithProvisionRunners(runner => runner.DeploySiteModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.SiteFields")]
        public void CanProvision_SiteNumberField()
        {
            var model = SPMeta2Model
                 .NewSiteModel(site => site.AddField(RegSiteFields.NumberField));

            WithProvisionRunners(runner => runner.DeploySiteModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.SiteFields")]
        public void CanProvision_SiteTextField()
        {
            var model = SPMeta2Model
                 .NewSiteModel(site => site.AddField(RegSiteFields.TextField));

            WithProvisionRunners(runner => runner.DeploySiteModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.SiteFields")]
        public void CanProvision_SiteUrlField()
        {
            var model = SPMeta2Model
                 .NewSiteModel(site => site.AddField(RegSiteFields.UrlField));

            WithProvisionRunners(runner => runner.DeploySiteModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.SiteFields")]
        public void CanProvision_SiteUserField()
        {
            var model = SPMeta2Model
                 .NewSiteModel(site => site.AddField(RegSiteFields.UserField));

            WithProvisionRunners(runner => runner.DeploySiteModel(model));
        }

        #endregion
    }
}
