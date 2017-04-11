using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

using SPMeta2.Containers.Extensions;
using SPMeta2.Exceptions;
using SPMeta2.Models;

using SPMeta2.Regression.Tests.Utils;
using ReflectionUtils = SPMeta2.Utils.ReflectionUtils;
using SPMeta2.Regression.Utils;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class FieldScenariosSelfTest
    {
        #region default values

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultValues")]
        [TestCategory("CI.Core")]
        public void SelfDiagnostic_AllFields_Should_Have_DefaultValue_Tests()
        {
            // check if FieldScenariosTest class has all testsfor field default values
            // should not inherit SPMeta2RegresionScenarioTestBase as it will force to use (init) provisioners under CI
            // that's why it is separated

            var isValid = true;

            var methods = typeof(FieldScenariosTest).GetMethods();
            var targetTypes = new List<Type>();

            var assemblies = new[]
            {
                typeof(FieldDefinition).Assembly,
                typeof(TaxonomyFieldDefinition).Assembly
            };

            targetTypes.AddRange(ReflectionUtils.GetTypesFromAssemblies<FieldDefinition>(assemblies));

            RegressionUtils.WriteLine(string.Format("Found [{0}] fied types.", targetTypes.Count));

            foreach (var fieldType in targetTypes.OrderBy(m => m.Name))
            {
                var fullDefName = fieldType.Name;
                var shortDefName = fieldType.Name.Replace("Definition", string.Empty);

                var testName = string.Format("CanDeploy_{0}_DefaultValue", shortDefName);

                var testExists = methods.Any(m => m.Name == testName);

                if (!testExists)
                    isValid = false;

                RegressionUtils.WriteLine(string.Format("[{0}] def: {1} test method: {2}",
                        testExists, fullDefName, testName));
            }

            Assert.IsTrue(isValid);
        }
        #endregion
    }

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

            var model = SPMeta2Model.NewWebModel(web =>
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

            var types = SPMeta2.Utils.ReflectionUtils.GetTypesFromAssemblies<DefinitionBase>(new[]
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

        #region list scope

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.InternalNameLookup")]
        public void CanDeploy_SiteField_Same_InternalName()
        {
            // we should be able to deploy field with same internal name and different ID in the list
            // https://github.com/SubPointSolutions/spmeta2/issues/825

            var field1 = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();
            var field2 = ModelGeneratorService.GetRandomDefinition<FieldDefinition>(def =>
            {
                def.Id = Guid.NewGuid();
                def.InternalName = field1.InternalName;
            });

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                // should not validate fieldd, it would fail on Id validation
                // it we can deploy the second field - all good
                site.AddField(field1, f => f.RegExcludeFromValidation());
                site.AddField(field2, f => f.RegExcludeFromValidation());
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.InternalNameLookup")]
        public void CanDeploy_WebField_Same_InternalName()
        {
            // we should be able to deploy field with same internal name and different ID in the list
            // https://github.com/SubPointSolutions/spmeta2/issues/825

            var field1 = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();
            var field2 = ModelGeneratorService.GetRandomDefinition<FieldDefinition>(def =>
            {
                def.Id = Guid.NewGuid();
                def.InternalName = field1.InternalName;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                // should not validate fieldd, it would fail on Id validation
                // it we can deploy the second field - all good
                web.AddField(field1, f => f.RegExcludeFromValidation());
                web.AddField(field2, f => f.RegExcludeFromValidation());
            });

            TestModel(model);
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.InternalNameLookup")]
        public void CanDeploy_ListField_Same_InternalName()
        {
            // we should be able to deploy field with same internal name and different ID in the list
            // https://github.com/SubPointSolutions/spmeta2/issues/825

            var field1 = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();
            var field2 = ModelGeneratorService.GetRandomDefinition<FieldDefinition>(def =>
            {
                def.Id = Guid.NewGuid();
                def.InternalName = field1.InternalName;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomList(list =>
                {
                    list.AddField(field1);
                    list.AddField(field2);
                });
            });

            TestModel(model);
        }

        #endregion

        #region default formula

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultFormula")]
        public void CanDeploy_Field_WithDefaultFormula()
        {
            var fieldDef = ModelGeneratorService.GetRandomDefinition<DateTimeFieldDefinition>(def =>
            {
                def.DisplayFormat = "DateOnly";
                def.DefaultFormula = "=[Today]+30";
                def.DefaultValue = string.Empty;
            });

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(fieldDef);
            });

            TestModel(model);
        }

        #endregion

        #region o365 root collection

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.O365")]
        public void CanDeploy_Field_To_O365_RootSiteCollection()
        {
            // Fields provision seems to fail on O365 root site collection #885
            // https://github.com/SubPointSolutions/spmeta2/issues/885

            var fieldDef = new BooleanFieldDefinition
            {
                Id = Rnd.Guid(),
                Title = Rnd.String(),
                InternalName = Rnd.String(),

                Required = false,
                Hidden = false,

                //ShowInNewForm = true,
                //ShowInEditForm = true,
                //ShowInDisplayForm = true,

                Group = Rnd.String()
            };

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(fieldDef);
            });

            TestModel(model);
        }

        #endregion

        #region default values



        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultValues")]
        public void CanDeploy_BooleanField_DefaultValue()
        {
            // Cover FieldDefinition.DefaultValue with regression tests #595
            // https://github.com/SubPointSolutions/spmeta2/issues/595
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultValues")]
        public void CanDeploy_BusinessDataField_DefaultValue()
        {
            // Cover FieldDefinition.DefaultValue with regression tests #595
            // https://github.com/SubPointSolutions/spmeta2/issues/595
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultValues")]
        public void CanDeploy_CalculatedField_DefaultValue()
        {
            // Cover FieldDefinition.DefaultValue with regression tests #595
            // https://github.com/SubPointSolutions/spmeta2/issues/595
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultValues")]
        public void CanDeploy_ChoiceField_DefaultValue()
        {
            // Cover FieldDefinition.DefaultValue with regression tests #595
            // https://github.com/SubPointSolutions/spmeta2/issues/595
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultValues")]
        public void CanDeploy_ComputedField_DefaultValue()
        {
            // Cover FieldDefinition.DefaultValue with regression tests #595
            // https://github.com/SubPointSolutions/spmeta2/issues/595
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultValues")]
        public void CanDeploy_CurrencyField_DefaultValue()
        {
            // Cover FieldDefinition.DefaultValue with regression tests #595
            // https://github.com/SubPointSolutions/spmeta2/issues/595
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultValues")]
        public void CanDeploy_DateTimeField_DefaultValue()
        {
            // Cover FieldDefinition.DefaultValue with regression tests #595
            // https://github.com/SubPointSolutions/spmeta2/issues/595
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultValues")]
        public void CanDeploy_DependentLookupField_DefaultValue()
        {
            // Cover FieldDefinition.DefaultValue with regression tests #595
            // https://github.com/SubPointSolutions/spmeta2/issues/595
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultValues")]
        public void CanDeploy_Field_DefaultValue()
        {
            // Cover FieldDefinition.DefaultValue with regression tests #595
            // https://github.com/SubPointSolutions/spmeta2/issues/595
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultValues")]
        public void CanDeploy_GeolocationField_DefaultValue()
        {
            // Cover FieldDefinition.DefaultValue with regression tests #595
            // https://github.com/SubPointSolutions/spmeta2/issues/595
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultValues")]
        public void CanDeploy_GuidField_DefaultValue()
        {
            // Cover FieldDefinition.DefaultValue with regression tests #595
            // https://github.com/SubPointSolutions/spmeta2/issues/595
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultValues")]
        public void CanDeploy_HTMLField_DefaultValue()
        {
            // Cover FieldDefinition.DefaultValue with regression tests #595
            // https://github.com/SubPointSolutions/spmeta2/issues/595
            throw new NotImplementedException();
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultValues")]
        public void CanDeploy_ImageField_DefaultValue()
        {
            // Cover FieldDefinition.DefaultValue with regression tests #595
            // https://github.com/SubPointSolutions/spmeta2/issues/595
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultValues")]
        public void CanDeploy_LinkField_DefaultValue()
        {
            // Cover FieldDefinition.DefaultValue with regression tests #595
            // https://github.com/SubPointSolutions/spmeta2/issues/595
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultValues")]
        public void CanDeploy_LookupField_DefaultValue()
        {
            // Cover FieldDefinition.DefaultValue with regression tests #595
            // https://github.com/SubPointSolutions/spmeta2/issues/595
            throw new NotImplementedException();
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultValues")]
        public void CanDeploy_MediaField_DefaultValue()
        {
            // Cover FieldDefinition.DefaultValue with regression tests #595
            // https://github.com/SubPointSolutions/spmeta2/issues/595
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultValues")]
        public void CanDeploy_MultiChoiceField_DefaultValue()
        {
            // Cover FieldDefinition.DefaultValue with regression tests #595
            // https://github.com/SubPointSolutions/spmeta2/issues/595
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultValues")]
        public void CanDeploy_NoteField_DefaultValue()
        {
            // Cover FieldDefinition.DefaultValue with regression tests #595
            // https://github.com/SubPointSolutions/spmeta2/issues/595
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultValues")]
        public void CanDeploy_NumberField_DefaultValue()
        {
            // Cover FieldDefinition.DefaultValue with regression tests #595
            // https://github.com/SubPointSolutions/spmeta2/issues/595
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultValues")]
        public void CanDeploy_OutcomeChoiceField_DefaultValue()
        {
            // Cover FieldDefinition.DefaultValue with regression tests #595
            // https://github.com/SubPointSolutions/spmeta2/issues/595
            throw new NotImplementedException();
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultValues")]
        public void CanDeploy_SummaryLinkField_DefaultValue()
        {
            // Cover FieldDefinition.DefaultValue with regression tests #595
            // https://github.com/SubPointSolutions/spmeta2/issues/595
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultValues")]
        public void CanDeploy_TaxonomyField_DefaultValue()
        {
            // Cover FieldDefinition.DefaultValue with regression tests #595
            // https://github.com/SubPointSolutions/spmeta2/issues/595
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultValues")]
        public void CanDeploy_TextField_DefaultValue()
        {
            // Cover FieldDefinition.DefaultValue with regression tests #595
            // https://github.com/SubPointSolutions/spmeta2/issues/595
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultValues")]
        public void CanDeploy_URLField_DefaultValue()
        {
            // Cover FieldDefinition.DefaultValue with regression tests #595
            // https://github.com/SubPointSolutions/spmeta2/issues/595
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DefaultValues")]
        public void CanDeploy_UserField_DefaultValue()
        {
            // Cover FieldDefinition.DefaultValue with regression tests #595
            // https://github.com/SubPointSolutions/spmeta2/issues/595
            throw new NotImplementedException();
        }

        #endregion

        #region PushChangesToLists tests

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.PushChangesToLists")]
        public void CanDeploy_Field_Without_PushChangesToLists_OnSite()
        {
            InternalDeploySiteFieldWithPushChangesToLists(false);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.PushChangesToLists")]
        public void CanDeploy_Field_With_PushChangesToLists_OnSite()
        {
            InternalDeploySiteFieldWithPushChangesToLists(true);
        }

        private void InternalDeploySiteFieldWithPushChangesToLists(bool pushChangesToLists)
        {
            var oldFieldTitle = Rnd.String();
            var newFieldTitle = Rnd.String();

            var oldSharePointFieldTitle = string.Empty;
            var newSharePointFieldTitle = string.Empty;

            // defs
            var oldFieldDef = ModelGeneratorService.GetRandomDefinition<TextFieldDefinition>(def =>
            {
                def.DefaultFormula = string.Empty;
                def.ValidationFormula = string.Empty;

                def.Title = oldFieldTitle;
            });

            var newFieldDef = oldFieldDef.Inherit(def =>
            {
                def.DefaultFormula = string.Empty;
                def.ValidationFormula = string.Empty;

                def.Title = newFieldTitle;

                def.PushChangesToLists = pushChangesToLists;
            });

            var oldListDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {

            });

            var newListDef = oldListDef.Inherit(def =>
            {

            });

            var oldListFielLink = new ListFieldLinkDefinition
            {
                FieldId = oldFieldDef.Id
            };

            var newListFielLink = new ListFieldLinkDefinition
            {
                FieldId = oldFieldDef.Id
            };

            var oldSiteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(oldFieldDef);
            });

            var oldWebModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(oldListDef, list =>
                {
                    list.AddListFieldLink(oldListFielLink);
                });
            });

            var newWebModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(newListDef, list =>
                {
                    list.AddListFieldLink(newListFielLink);
                });
            });

            var newSiteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(newFieldDef);
            });

            // passing Reg_Title as definition PropertyBag
            // later, validation handlers foe ListFieldLink will use Reg_Title instead of title 
            // bit if a hack
            if (pushChangesToLists)
            {
                newListFielLink.PropertyBag.Add(new PropertyBagValue
                {
                    Name = "_Reg_DisplayName",
                    Value = newFieldTitle
                });
            }
            else
            {
                newListFielLink.PropertyBag.Add(new PropertyBagValue
                {
                    Name = "_Reg_DisplayName",
                    Value = oldFieldTitle
                });
            }

            // deploy initial field and list
            TestModel(oldSiteModel);
            TestModel(oldWebModel);

            // deploy 'new' field model
            TestModel(newSiteModel);
            TestModel(newWebModel);
        }

        private string ExtractFieldTitleFromObject(OnCreatingContext<object, DefinitionBase> context)
        {
            var obj = context.Object;

            return obj.GetPropertyValue("Title") as string;
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
