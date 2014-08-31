using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Regression.Model.Definitions;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.Definitions
{
    //[TestClass]
    public class FieldTests : SPMeta2RegresionTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Regression.SiteFields")]
        public void CanProvision_SiteBooleanField()
        {
            var model = SPMeta2Model
                 .NewSiteModel(site => site.AddField(RegSiteFields.BooleanField));

            // WithProvisionRunnerContext(runner => runner.DeploySiteModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.SiteFields")]
        public void CanProvision_SiteChoiceField()
        {
            var model = SPMeta2Model
                 .NewSiteModel(site => site.AddField(RegSiteFields.ChoiceField));

            //  WithProvisionRunnerContext(runner => runner.DeploySiteModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.SiteFields")]
        public void CanProvision_SiteCurrencyField()
        {
            var model = SPMeta2Model
                 .NewSiteModel(site => site.AddField(RegSiteFields.CurrencyField));

            // WithProvisionRunnerContext(runner => runner.DeploySiteModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.SiteFields")]
        public void CanProvision_SiteDateTimeField()
        {
            var model = SPMeta2Model
                 .NewSiteModel(site => site.AddField(RegSiteFields.DateTimeField));

            // WithProvisionRunnerContext(runner => runner.DeploySiteModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.SiteFields")]
        public void CanProvision_SiteGuidField()
        {
            var model = SPMeta2Model
                 .NewSiteModel(site => site.AddField(RegSiteFields.GuidField));

            // WithProvisionRunnerContext(runner => runner.DeploySiteModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.SiteFields")]
        public void CanProvision_SiteLookupField()
        {
            var model = SPMeta2Model
                 .NewSiteModel(site => site.AddField(RegSiteFields.LookupField));

            // WithProvisionRunnerContext(runner => runner.DeploySiteModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.SiteFields")]
        public void CanProvision_SiteMultiChoiceField()
        {
            var model = SPMeta2Model
                 .NewSiteModel(site => site.AddField(RegSiteFields.MultiChoiceField));

            //WithProvisionRunnerContext(runner => runner.DeploySiteModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.SiteFields")]
        public void CanProvision_SiteNoteField()
        {
            var model = SPMeta2Model
                 .NewSiteModel(site => site.AddField(RegSiteFields.NoteField));

            // WithProvisionRunnerContext(runner => runner.DeploySiteModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.SiteFields")]
        public void CanProvision_SiteNumberField()
        {
            var model = SPMeta2Model
                 .NewSiteModel(site => site.AddField(RegSiteFields.NumberField));

            // WithProvisionRunnerContext(runner => runner.DeploySiteModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.SiteFields")]
        public void CanProvision_SiteTextField()
        {
            var model = SPMeta2Model
                 .NewSiteModel(site => site.AddField(RegSiteFields.TextField));

            // WithProvisionRunnerContext(runner => runner.DeploySiteModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.SiteFields")]
        public void CanProvision_SiteUrlField()
        {
            var model = SPMeta2Model
                 .NewSiteModel(site => site.AddField(RegSiteFields.UrlField));

            //  WithProvisionRunnerContext(runner => runner.DeploySiteModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.SiteFields")]
        public void CanProvision_SiteUserField()
        {
            var model = SPMeta2Model
                 .NewSiteModel(site => site.AddField(RegSiteFields.UserField));

            //  WithProvisionRunnerContext(runner => runner.DeploySiteModel(model));
        }

        #endregion
    }
}
