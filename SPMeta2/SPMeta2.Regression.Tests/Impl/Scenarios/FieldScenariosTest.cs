using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Containers.Services;
using SPMeta2.Definitions.Fields;
using SPMeta2.Regression.Tests.Impl.Definitions;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Utils;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class FieldScenariosTest : SPMeta2RegresionScenarioTestBase
    {
        #region constructors

        public FieldScenariosTest()
        {
            RegressionService.ProvisionGenerationCount = 2;
        }

        #endregion

        #region internal

        [ClassInitializeAttribute]
        public static void Init(TestContext context)
        {
            InternalInit();
        }

        [ClassCleanupAttribute]
        public static void Cleanup()
        {
            InternalCleanup();
        }

        #endregion

        #region raw XML

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.RawXml")]
        public void CanDeploy_Field_WithRawXml()
        {
            WithDisabledPropertyUpdateValidation(() =>
            {
                var internalName = Rnd.String();
                var id = Guid.NewGuid();
                var title = Rnd.String();
                var group = Rnd.String();

                var xmlElement = new XElement("Field",
                    new XAttribute(BuiltInFieldAttributes.ID, id.ToString("B")),
                    new XAttribute(BuiltInFieldAttributes.StaticName, internalName),
                    new XAttribute(BuiltInFieldAttributes.DisplayName, title),
                    new XAttribute(BuiltInFieldAttributes.Title, title),
                    new XAttribute(BuiltInFieldAttributes.Name, internalName),
                    new XAttribute(BuiltInFieldAttributes.Type, BuiltInFieldTypes.Text),
                    new XAttribute(BuiltInFieldAttributes.Group, group));

                var def = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();

                // ID/InternalName should be defined to be able to lookup the field 
                def.Id = id;
                def.FieldType = BuiltInFieldTypes.Text;
                def.InternalName = internalName;
                def.Title = title;
                def.Group = group;

                def.RawXml = xmlElement.ToString();

                var siteModel = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddField(def);
                });

                TestModel(siteModel);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.RawXml")]
        public void CanDeploy_Field_WithRawXmlAndAdditionalAttributes()
        {
            WithDisabledPropertyUpdateValidation(() =>
            {
                var internalName = Rnd.String();
                var id = Guid.NewGuid();
                var title = Rnd.String();
                var group = Rnd.String();

                var xmlElement = new XElement("Field",
                    new XAttribute(BuiltInFieldAttributes.ID, id.ToString("B")),
                    new XAttribute(BuiltInFieldAttributes.StaticName, internalName),
                    new XAttribute(BuiltInFieldAttributes.DisplayName, title),
                    new XAttribute(BuiltInFieldAttributes.Title, title),
                    new XAttribute(BuiltInFieldAttributes.Name, internalName),
                    new XAttribute(BuiltInFieldAttributes.Type, BuiltInFieldTypes.Text),
                    new XAttribute(BuiltInFieldAttributes.Group, group));

                var def = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();

                // ID/InternalName should be defined to be able to lookup the field 
                def.Id = id;
                def.FieldType = BuiltInFieldTypes.Text;
                def.InternalName = internalName;
                def.Title = title;
                def.Group = group;

                def.RawXml = xmlElement.ToString();

                def.AdditionalAttributes.Add(new FieldAttributeValue("Commas", Rnd.Bool().ToString().ToUpper()));
                def.AdditionalAttributes.Add(new FieldAttributeValue("AllowDuplicateValues",
                    Rnd.Bool().ToString().ToUpper()));

                var siteModel = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddField(def);
                });

                TestModel(siteModel);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.Attributes")]
        public void CanDeploy_Field_WithAdditionalAttributes()
        {
            var def = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();

            def.AdditionalAttributes.Add(new FieldAttributeValue("Commas", Rnd.Bool().ToString().ToUpper()));
            def.AdditionalAttributes.Add(new FieldAttributeValue("AllowDuplicateValues", Rnd.Bool().ToString().ToUpper()));

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(def);
            });

            TestModel(siteModel);
        }

        #endregion

        #region default

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_BooleanField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.Boolean;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_CalculatedField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.Calculated;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        [SiteCollectionIsolation]
        public void CanDeploy_ChoiceField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.Choice;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_ComputedField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.Computed;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_CurrencyField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.Currency;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_DateTimeField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.DateTime;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_GeolocationField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.Geolocation;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_GuidField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.Guid;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_LookupField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.Lookup;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        [SiteCollectionIsolation]
        public void CanDeploy_MultiChoiceField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.MultiChoice;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_NoteField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.Note;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_NumberField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.Number;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        [SiteCollectionIsolation]
        public void CanDeploy_OutcomeChoiceField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.OutcomeChoice;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_TaxonomyFieldTypeField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.TaxonomyFieldType;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_TaxonomyFieldTypeMultiField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.TaxonomyFieldTypeMulti;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_TextField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.Text;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_URLField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.URL;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_UserField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.User;
            });
        }

        #endregion

        #region field scopes

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.Scopes")]
        public void CanDeploy_SiteScoped_Field()
        {
            var field = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();

            var model = SPMeta2Model
                   .NewSiteModel(site =>
                   {
                       site.AddField(field);
                   });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.Scopes")]
        public void CanDeploy_WebScoped_Field()
        {
            var field = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();

            var model = SPMeta2Model
                   .NewWebModel(web =>
                   {
                       web.AddRandomWeb(subWeb =>
                       {
                           subWeb.AddField(field);
                       });
                   });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.Scopes")]
        public void CanDeploy_ListScoped_Field()
        {
            var field = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();

            var model = SPMeta2Model
                   .NewWebModel(web =>
                   {
                       web.AddRandomList(list =>
                       {
                           list.AddField(field);
                       });
                   });

            TestModel(model);
        }


        #endregion

        #region full scope regression

        protected List<FieldDefinition> GetAllRandomFields()
        {
            var spMetaAssembly = typeof(FieldDefinition).Assembly;
            var spMetaStandardAssembly = typeof(TaxonomyFieldDefinition).Assembly;

            var types = ReflectionUtils.GetTypesFromAssemblies<DefinitionBase>(new[]
            {
             spMetaAssembly,
             spMetaStandardAssembly
            });

            var result = new List<FieldDefinition>();
            var allFieldDefinitiontypesDefinitions = types.Where(t =>
                t.IsSubclassOf(typeof(FieldDefinition))
                && !(t == typeof(DependentLookupFieldDefinition)));

            foreach (var type in allFieldDefinitiontypesDefinitions)
                result.Add(ModelGeneratorService.GetRandomDefinition(type) as FieldDefinition);
            //ModelGeneratorService.GetRandomDefinition()

            return result;
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.Scopes")]
        public void CanDeploy_AllFields_UnderSite()
        {
            var fields = GetAllRandomFields();
            //ModelGeneratorService.GetRandomDefinition<FieldDefinition>();

            var model = SPMeta2Model
                   .NewSiteModel(site =>
                   {
                       site.AddFields(fields);
                   });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.Scopes")]
        public void CanDeploy_AllFields_UnderWeb()
        {
            var fields = GetAllRandomFields();

            var model = SPMeta2Model
                   .NewWebModel(web =>
                   {
                       web.AddRandomWeb(subWeb =>
                       {
                           subWeb.AddFields(fields);
                       });
                   });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.Scopes")]
        public void CanDeploy_AllFields_UnderList()
        {
            WithDisabledDefinitionImmutabilityValidation(() =>
            {

                var fields = GetAllRandomFields();

                fields.OfType<CalculatedFieldDefinition>()
                     .ToList()
                     .ForEach(f =>
                     {
                         // clean fomula, that's not gonna work in list
                         // there is a separated test for it
                         f.Formula = String.Empty;
                     });

                var model = SPMeta2Model
                       .NewWebModel(web =>
                       {
                           web.AddRandomList(list =>
                           {
                               foreach (var fieldDef in fields)
                               {
                                   // honest regression testing will update Formula
                                   // need to reset oit before provision
                                   // same-same with ValidationFormula/ValidationMessage

                                   if (fieldDef is CalculatedFieldDefinition)
                                   {
                                       list.AddField(fieldDef, field =>
                                       {
                                           field.OnProvisioning<object, CalculatedFieldDefinition>(cntx =>
                                           {
                                               cntx.ObjectDefinition.ValidationFormula = string.Empty;
                                               cntx.ObjectDefinition.ValidationMessage = string.Empty;

                                               cntx.ObjectDefinition.Formula = "=5*ID";

                                               // SSOM: weird, but we can't pass this test unless turn off toggling or TRUE for ndexed value
                                               cntx.ObjectDefinition.Indexed = false;
                                           });
                                       });
                                   }
                                   else
                                   {
                                       list.AddField(fieldDef, field =>
                                       {
                                           field.OnProvisioning<object>(cntx =>
                                           {
                                               var def = cntx.ObjectDefinition as FieldDefinition;

                                               def.ValidationFormula = string.Empty;
                                               def.ValidationMessage = string.Empty;

                                               // SSOM: weird, but we can't pass this test unless turn off toggling or TRUE for ndexed value
                                               if (def is MultiChoiceFieldDefinition)
                                               {
                                                   def.Indexed = false;
                                               }

                                               // CSOM: weird, but we can't pass this test unless turn off toggling or TRUE for ndexed value
                                               if (def is URLFieldDefinition
                                                   || def is ImageFieldDefinition
                                                   || def is LinkFieldDefinition
                                                   || def is ComputedFieldDefinition
                                                   || def is SummaryLinkFieldDefinition
                                                   || def is MediaFieldDefinition
                                                   || def is HTMLFieldDefinition
                                                   || def is GeolocationFieldDefinition
                                                  )
                                               {
                                                   def.Indexed = false;
                                               }
                                           });
                                       });
                                   }
                               }
                           });
                       });

                TestModel(model);
            });
        }

        #endregion

        #region field options

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.Options")]
        public void CanDeploy_ListScopedField_AsAddToDefaultView()
        {
            var field = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();

            field.AddToDefaultView = true;

            var model = SPMeta2Model
                   .NewWebModel(web =>
                   {
                       web.AddRandomList(list =>
                       {
                           list.AddField(field);
                       });
                   });

            TestModel(model);
        }

        #endregion

        #region fields localization

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.Localization")]
        public void CanDeploy_Localized_Site_Field()
        {
            var field = GetLocalizedFieldDefinition();

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(field);
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.Localization")]
        public void CanDeploy_Localized_Web_Field()
        {
            var rootWeb = GetLocalizedFieldDefinition();
            var subWebField = GetLocalizedFieldDefinition();

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddField(rootWeb);

                web.AddRandomWeb(subWeb =>
                {
                    web.AddField(subWebField);
                });
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.Localization")]
        public void CanDeploy_Localized_List_Field()
        {
            var field = GetLocalizedFieldDefinition();

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddField(field);
            });

            TestModel(model);
        }

        #endregion

        #region utils

        protected FieldDefinition GetLocalizedFieldDefinition()
        {
            var definition = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();
            var localeIds = Rnd.LocaleIds();

            foreach (var localeId in localeIds)
            {
                definition.TitleResource.Add(new ValueForUICulture
                {
                    CultureId = localeId,
                    Value = string.Format("LocalizedTitle_{0}", localeId)
                });

                definition.DescriptionResource.Add(new ValueForUICulture
                {
                    CultureId = localeId,
                    Value = string.Format("LocalizedDescription_{0}", localeId)
                });
            }

            return definition;
        }

        #endregion
    }
}
