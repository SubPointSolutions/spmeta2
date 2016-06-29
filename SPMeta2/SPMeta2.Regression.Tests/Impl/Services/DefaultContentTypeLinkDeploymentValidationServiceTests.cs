using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Services;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Services.Impl;
using SPMeta2.Syntax.Default;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Services.Impl.Validation;
using SPMeta2.Definitions.ContentTypes;

namespace SPMeta2.Regression.Tests.Impl.Services
{
    [TestClass]
    public class DefaultContentTypeLinkDeploymentValidationServiceTests : SPMeta2DefinitionRegresionTestBase
    {
        #region constructors

        public DefaultContentTypeLinkDeploymentValidationServiceTests()
        {
            Service = new DefaultContentTypeLinkValidationService();
        }

        #endregion

        #region properties

        public DefaultContentTypeLinkValidationService Service { get; set; }

        #endregion

        #region AddContentTypeLink

        //[TestMethod]
        //[TestCategory("Regression.Services.DefaultContentTypeLinkDeploymentValidationService")]
        //[TestCategory("CI.Core")]
        //public void ShouldPass_On_Valid_AddContentTypeLink()
        //{
        //    var model = SPMeta2Model.NewWebModel(web =>
        //    {
        //        web.AddRandomList(list =>
        //        {
        //            list.AddContentTypeLink(new ContentTypeLinkDefinition
        //            {
        //                ContentTypeName = Rnd.String()
        //            });
        //        });
        //    });

        //    Service.DeployModel(null, model);
        //}

        //[TestMethod]
        //[TestCategory("Regression.Services.DefaultContentTypeLinkDeploymentValidationService")]
        //[TestCategory("CI.Core")]
        //public void ShouldFail_On_Invalid_AddContentTypeLink_Both()
        //{
        //    var model = SPMeta2Model.NewWebModel(web =>
        //    {
        //        web.AddRandomList(list =>
        //        {
        //            list.AddContentTypeLink(new ContentTypeLinkDefinition
        //            {
        //                ContentTypeName = Rnd.String(),
        //                ContentTypeId = Rnd.String()
        //            });
        //        });
        //    });

        //    WithExceptionValidation(() =>
        //    {
        //        Service.DeployModel(null, model);
        //    });
        //}

        private void WithExceptionValidation(Action action)
        {
            WithExceptionValidation(action, 1);
        }
        private void WithExceptionValidation(Action action, int count)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                ValidateException(e, count);
            }
        }

        //[TestMethod]
        //[TestCategory("Regression.Services.DefaultContentTypeLinkDeploymentValidationService")]
        //[TestCategory("CI.Core")]
        //public void ShouldFail_On_Invalid_AddContentTypeLink_ContentTypeId()
        //{
        //    var model = SPMeta2Model.NewWebModel(web =>
        //    {
        //        web.AddRandomList(list =>
        //        {
        //            list.AddContentTypeLink(new ContentTypeLinkDefinition
        //            {
        //                ContentTypeId = Rnd.String()
        //            });
        //        });
        //    });

        //    WithExceptionValidation(() =>
        //    {
        //        Service.DeployModel(null, model);
        //    });
        //}

        #endregion

        #region AddHideContentTypeLinks

        [TestMethod]
        [TestCategory("Regression.Services.DefaultContentTypeLinkDeploymentValidationService")]
        [TestCategory("CI.Core")]
        public void ShouldPass_On_Valid_AddHideContentTypeLinks()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomList(list =>
                {
                    list.AddHideContentTypeLinks(new HideContentTypeLinksDefinition
                    {
                        ContentTypes = new List<ContentTypeLinkValue>
                        {
                            new ContentTypeLinkValue() {
                                ContentTypeName = Rnd.String()
                            }
                        }
                    });
                });
            });

            Service.DeployModel(null, model);
        }

        [TestMethod]
        [TestCategory("Regression.Services.DefaultContentTypeLinkDeploymentValidationService")]
        [TestCategory("CI.Core")]
        public void ShouldFail_On_Invalid_AddHideContentTypeLinks_Both()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomList(list =>
                {
                    list.AddHideContentTypeLinks(new HideContentTypeLinksDefinition
                    {
                        ContentTypes = new List<ContentTypeLinkValue>
                        {
                            new ContentTypeLinkValue() {
                                ContentTypeName = Rnd.String(),
                                ContentTypeId = Rnd.String()
                            }
                        }
                    });
                });
            });

            WithExceptionValidation(() =>
            {
                Service.DeployModel(null, model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Services.DefaultContentTypeLinkDeploymentValidationService")]
        [TestCategory("CI.Core")]
        public void ShouldFail_On_Invalid_AddHideContentTypeLinks_ContentTypeId()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomList(list =>
                {
                    list.AddHideContentTypeLinks(new HideContentTypeLinksDefinition
                    {
                        ContentTypes = new List<ContentTypeLinkValue>
                        {
                            new ContentTypeLinkValue() {
                                ContentTypeId = Rnd.String()
                            }
                        }
                    });
                });
            });

            WithExceptionValidation(() =>
            {
                Service.DeployModel(null, model);
            });
        }

        #endregion

        #region AddRemoveContentTypeLinks

        [TestMethod]
        [TestCategory("Regression.Services.DefaultContentTypeLinkDeploymentValidationService")]
        [TestCategory("CI.Core")]
        public void ShouldPass_On_Valid_AddRemoveContentTypeLinks()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomList(list =>
                {
                    list.AddRemoveContentTypeLinks(new RemoveContentTypeLinksDefinition
                    {
                        ContentTypes = new List<ContentTypeLinkValue>
                        {
                            new ContentTypeLinkValue() {
                                ContentTypeName = Rnd.String()
                            }
                        }
                    });
                });
            });

            Service.DeployModel(null, model);
        }

        [TestMethod]
        [TestCategory("Regression.Services.DefaultContentTypeLinkDeploymentValidationService")]
        [TestCategory("CI.Core")]
        public void ShouldFail_On_Invalid_AddRemoveContentTypeLinks_Both()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomList(list =>
                {
                    list.AddRemoveContentTypeLinks(new RemoveContentTypeLinksDefinition
                    {
                        ContentTypes = new List<ContentTypeLinkValue>
                        {
                            new ContentTypeLinkValue() {
                                ContentTypeName = Rnd.String(),
                                ContentTypeId = Rnd.String()
                            }
                        }
                    });
                });
            });

            WithExceptionValidation(() =>
            {
                Service.DeployModel(null, model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Services.DefaultContentTypeLinkDeploymentValidationService")]
        [TestCategory("CI.Core")]
        public void ShouldFail_On_Invalid_AddRemoveContentTypeLinks_ContentTypeId()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomList(list =>
                {
                    list.AddRemoveContentTypeLinks(new RemoveContentTypeLinksDefinition
                    {
                        ContentTypes = new List<ContentTypeLinkValue>
                        {
                            new ContentTypeLinkValue() {
                                ContentTypeId = Rnd.String()
                            }
                        }
                    });
                });
            });

            WithExceptionValidation(() =>
            {
                Service.DeployModel(null, model);
            });
        }

        #endregion

        #region AddRemoveContentTypeLinks

        [TestMethod]
        [TestCategory("Regression.Services.DefaultContentTypeLinkDeploymentValidationService")]
        [TestCategory("CI.Core")]
        public void ShouldPass_On_Valid_AddUniqueContentTypeOrderDefinition()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomList(list =>
                {
                    list.AddUniqueContentTypeOrder(new UniqueContentTypeOrderDefinition
                    {
                        ContentTypes = new List<ContentTypeLinkValue>
                        {
                            new ContentTypeLinkValue() {
                                ContentTypeName = Rnd.String()
                            }
                        }
                    });
                });
            });

            Service.DeployModel(null, model);
        }

        [TestMethod]
        [TestCategory("Regression.Services.DefaultContentTypeLinkDeploymentValidationService")]
        [TestCategory("CI.Core")]
        public void ShouldFail_On_Invalid_AddUniqueContentTypeOrder_Both()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomList(list =>
                {
                    list.AddUniqueContentTypeOrder(new UniqueContentTypeOrderDefinition
                    {
                        ContentTypes = new List<ContentTypeLinkValue>
                        {
                            new ContentTypeLinkValue() {
                                ContentTypeName = Rnd.String(),
                                ContentTypeId = Rnd.String()
                            }
                        }
                    });
                });
            });

            WithExceptionValidation(() =>
            {
                Service.DeployModel(null, model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Services.DefaultContentTypeLinkDeploymentValidationService")]
        [TestCategory("CI.Core")]
        public void ShouldFail_On_Invalid_AddUniqueContentTypeOrder_ContentTypeId()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomList(list =>
                {
                    list.AddUniqueContentTypeOrder(new UniqueContentTypeOrderDefinition
                    {
                        ContentTypes = new List<ContentTypeLinkValue>
                        {
                            new ContentTypeLinkValue() {
                                ContentTypeId = Rnd.String()
                            }
                        }
                    });
                });
            });

            WithExceptionValidation(() =>
            {
                Service.DeployModel(null, model);
            });
        }

        #endregion

        #region utils

        protected virtual void ValidateException(Exception ex)
        {
            ValidateException(ex, 1);
        }
        protected virtual void ValidateException(Exception ex, int count)
        {
            Assert.IsTrue(ex is SPMeta2ModelDeploymentException);
            Assert.IsTrue(ex.InnerException is SPMeta2AggregateException);
            Assert.AreEqual(count, (ex.InnerException as SPMeta2AggregateException).InnerExceptions.Count);
        }

        #endregion
    }
}
