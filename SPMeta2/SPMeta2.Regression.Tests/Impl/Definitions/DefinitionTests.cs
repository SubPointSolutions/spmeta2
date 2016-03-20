using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;
using SPMeta2.Standard.Definitions;
using SPMeta2.Regression.Tests.Config;
using SPMeta2.Services;
using SPMeta2.Containers.Utils;
using SPMeta2.Standard.Definitions.Webparts;

namespace SPMeta2.Regression.Tests.Impl.Definitions
{
    [TestClass]
    public class DefinitionTests
    {
        #region properties

        protected static List<Type> allDefinitionTypes;
        protected static List<Type> allModelNodesTypes;

        protected static List<Type> AllDefinitionTypes
        {
            get { return allDefinitionTypes ?? (allDefinitionTypes = LoadDefinitions().ToList()); }
        }

        protected static List<Type> AllModelNodeTypes
        {
            get { return allModelNodesTypes ?? (allModelNodesTypes = LoadModelNodes().ToList()); }
        }

        #endregion

        #region common

        [ClassInitializeAttribute]
        public static void Init(TestContext context)
        {

        }

        private static IEnumerable<Type> LoadModelNodes()
        {
            var spMetaAssembly = typeof(FieldDefinition).Assembly;
            var spMetaStandardAssembly = typeof(TaxonomyFieldDefinition).Assembly;

            return ReflectionUtils.GetTypesFromAssemblies<ModelNode>(new[]
            {
             spMetaAssembly,
             spMetaStandardAssembly
            });
        }

        protected static IEnumerable<Type> LoadDefinitions()
        {
            var spMetaAssembly = typeof(FieldDefinition).Assembly;
            var spMetaStandardAssembly = typeof(TaxonomyFieldDefinition).Assembly;

            return ReflectionUtils.GetTypesFromAssemblies<DefinitionBase>(new[]
            {
             spMetaAssembly,
             spMetaStandardAssembly
            });
        }

        [ClassCleanupAttribute]
        public static void Cleanup()
        {

        }

        #endregion

        #region properties

        #endregion

        #region base tests

        [TestMethod]
        [TestCategory("Regression.Definitions")]
        [TestCategory("CI.Core")]
        public void Can_CreateDefinition()
        {
            foreach (var definitionType in AllDefinitionTypes)
            {
                var impl = Activator.CreateInstance(definitionType) as DefinitionBase;

                Assert.IsNotNull(impl);
            }
        }

        [TestMethod]
        [TestCategory("Regression.Definitions")]
        [TestCategory("CI.Core")]
        public void DefinitionPropertyBag_ShouldNotbeNull()
        {
            foreach (var definitionType in AllDefinitionTypes)
            {
                var impl = Activator.CreateInstance(definitionType) as DefinitionBase;

                Assert.IsNotNull(impl);
                Assert.IsNotNull(impl.PropertyBag);
            }
        }

        [TestMethod]
        [TestCategory("Regression.Definitions")]
        [TestCategory("CI.Core")]
        public void DefinitionsShouldHaveToStringOverride()
        {
            foreach (var definitionType in AllDefinitionTypes)
            {
                var hasToStringOverride = definitionType.GetMethod("ToString").DeclaringType == definitionType;

                if (!hasToStringOverride)
                    Trace.WriteLine(string.Format("Checking definition type:[{0}]. Has override:[{1}]", definitionType, hasToStringOverride));

                Assert.IsTrue(hasToStringOverride);
            }
        }

        [TestMethod]
        [TestCategory("Regression.Definitions")]
        [TestCategory("CI.Core")]
        public void DefinitionsShouldBeMarkedAsSerializable()
        {
            var result = true;

            foreach (var definitionType in AllDefinitionTypes)
            {
                var hasAttr = definitionType.GetCustomAttributes(typeof(SerializableAttribute)).Any();

                Trace.WriteLine(string.Format("[{2}] - Checking definition type:[{0}]. Has SerializableAttribute:[{1}]",
                    definitionType, hasAttr, hasAttr.ToString().ToUpper()));

                if (!hasAttr)
                    result = false;
            }

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Regression.Definitions")]
        [TestCategory("CI.Core")]
        public void DefinitionsShouldBeMarkedAsDataContract()
        {
            var result = true;

            foreach (var definitionType in AllDefinitionTypes)
            {
                var hasAttr = definitionType.GetCustomAttributes(typeof(DataContractAttribute)).Any();

                if (!hasAttr)
                {
                    Trace.WriteLine(string.Format("[{2}] - Checking definition type:[{0}]. Has DataContractAttribute:[{1}]",
                        definitionType, hasAttr, hasAttr.ToString().ToUpper()));

                }

                if (!hasAttr)
                    result = false;
            }

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Regression.Definitions")]
        [TestCategory("CI.Core")]
        public void ModelNodesShouldBeMarkedAsDataContract_v12()
        {
            var result = true;

            foreach (var modelNodeType in AllModelNodeTypes)
            {
                var hasAttr = modelNodeType.GetCustomAttributes(typeof(DataContractAttribute)).Any();

                if (!hasAttr)
                {
                    Trace.WriteLine(string.Format("[{2}] - Checking model node type:[{0}]. Has DataContractAttribute:[{1}]",
                        modelNodeType, hasAttr, hasAttr.ToString().ToUpper()));

                }

                if (!hasAttr)
                    result = false;
            }

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Regression.Definitions.Identity")]
        [TestCategory("CI.Core")]
        public void DefinitionsShouldHasIdentityOrIdentityKey()
        {
            var result = true;

            foreach (var definitionType in AllDefinitionTypes)
            {
                var isSingleIdenity = definitionType.GetCustomAttributes(typeof(SingletonIdentityAttribute), true).Any();
                var isInstanceIdentity = !definitionType.GetCustomAttributes(typeof(SingletonIdentityAttribute), true).Any();

                if (isSingleIdenity)
                {
                    //Trace.WriteLine(string.Format("[{1}] - Checking SINGLE type:[{0}].", definitionType, bool.TrueString.ToUpper()));

                    continue;
                }

                if (isInstanceIdentity)
                {
                    var hasKeys = definitionType
                                        .GetProperties()
                                        .SelectMany(p => p.GetCustomAttributes(typeof(IdentityKeyAttribute)))
                                        .Any();
                    if (!hasKeys)
                    {
                        Trace.WriteLine(string.Format("[{2}] - Checking INSTANCE type:[{0}]. Has keys:[{1}]",
                            definitionType, hasKeys, hasKeys.ToString().ToUpper()));

                    }

                    if (!hasKeys)
                        result = false;
                }
            }

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Regression.Definitions")]
        [TestCategory("CI.Core")]
        public void ModelNodeTypesPublicPropsShouldBeMarkedAsDataMemberOrIgnoreDataMemberAttr()
        {
            var result = true;
            var errors = 0;

            var types = new List<Type>();
            types.AddRange(ReflectionUtils.GetTypesFromAssembly<ModelNode>(typeof(ModelNode).Assembly));

            foreach (var definitionType in types)
            {
                var props = definitionType.GetProperties();

                foreach (var prop in props)
                {
                    var hasAttr = prop.GetCustomAttributes(typeof(DataMemberAttribute)).Any()
                        || prop.GetCustomAttributes(typeof(IgnoreDataMemberAttribute)).Any();

                    if (!hasAttr)
                    {
                        Trace.WriteLine(string.Format("[{2}] - Checking definition type:[{0}]. Prop:[{1}]",
                            definitionType.Name, prop.Name, hasAttr));
                    }

                    if (!hasAttr)
                    {
                        errors++;
                        result = false;
                    }
                }
            }

            Trace.WriteLine(string.Format("Errors: [{0}]", errors));

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Regression.Definitions")]
        [TestCategory("CI.Core")]
        public void DefinitionsPublicPropsShouldBeMarkedAsDataMemberOrIgnoreDataMemberAttr()
        {
            var targetTypes = AllDefinitionTypes;
            var errors = CheckDataMemberOrIgnoreDataMemberAttr(targetTypes);

            Trace.WriteLine(string.Format("Errors: [{0}]", errors));

            Assert.IsTrue(errors == 0);
        }

        [TestMethod]
        [TestCategory("Regression.Definitions")]
        [TestCategory("CI.Core")]
        public void TypedWebPartDefinitions_Should_Have_ExpectWebpartType_Attr()
        {
            var typedWebPartTypes = AllDefinitionTypes.Where(t => t.IsSubclassOf(typeof(WebPartDefinition)));

            var errors = 0;

            foreach (var definitionType in typedWebPartTypes)
            {
                var hasAttr = definitionType.GetCustomAttributes(typeof(ExpectWebpartType), true).Any();

                if (!hasAttr)
                {
                    Trace.WriteLine(string.Format(" - Checking type:[{0}]. Has:[{1}] Attr:[ExpectWebpartType]",
                        definitionType.Name, hasAttr));
                }

                if (!hasAttr)
                {
                    errors++;
                }
            }

            Trace.WriteLine(string.Format("Errors: [{0}]", errors));

            Assert.IsTrue(errors == 0);
        }

        private static int CheckDataMemberOrIgnoreDataMemberAttr(List<Type> types)
        {
            var errors = 0;

            foreach (var definitionType in types)
            {
                var props = definitionType.GetProperties();

                foreach (var prop in props)
                {
                    var hasAttr = prop.GetCustomAttributes(typeof(DataMemberAttribute)).Any()
                                  || prop.GetCustomAttributes(typeof(IgnoreDataMemberAttribute)).Any();

                    if (!hasAttr)
                    {
                        Trace.WriteLine(string.Format("[{2}] - Checking type:[{0}]. Prop:[{1}]",
                            definitionType.Name, prop.Name, hasAttr));
                    }

                    if (!hasAttr)
                    {
                        errors++;
                    }
                }
            }
            return errors;
        }

        [TestMethod]
        [TestCategory("Regression.Definitions")]
        [TestCategory("CI.Core")]
        public void AllSerializablesPublicPropsShouldBeMarkedAsDataMemberOrIgnoreDataMemberAttr()
        {
            var targetTypes = new List<Type>();

            var assemblies = new[]
            {
                typeof(FieldDefinition).Assembly,
                typeof(TaxonomyFieldDefinition).Assembly,
            };

            targetTypes.AddRange(assemblies.SelectMany(a => a.GetTypes())
                                 .Where(t => t.GetCustomAttributes(typeof(DataContractAttribute)).Any()));

            var errors = CheckDataMemberOrIgnoreDataMemberAttr(targetTypes);

            Trace.WriteLine(string.Format("Errors: [{0}]", errors));

            Assert.IsTrue(errors == 0);
        }

        #endregion

        #region API and Syntax

        [TestMethod]
        [TestCategory("Regression.Definitions.Syntax.v11")]
        [TestCategory("CI.Core")]
        public void DefinitionsShouldHaveAddXXX_DefinitionSyntax_v11()
        {
            if (!M2RegressionRuntime.IsV11)
                return;

            var methods = GetModelNodeExtensionMethods(new[]
            {
                typeof(DefinitionBase).Assembly,
                typeof(TaxonomyFieldDefinition).Assembly,
                typeof(ListDefinitionSyntax).Assembly
            });

            var hasAllAddMethods = true;

            foreach (var definitionType in AllDefinitionTypes)
            {
                var definitionName = definitionType.Name.Replace("Definition", string.Empty);

                Trace.WriteLine(string.Format("Definition: [{0}]", definitionName));

                #region AddXXX()

                // validate (this ModelNode model, XXXDefinition definition)
                Trace.WriteLine(string.Format("     Add{0}(this ModelNode model, {0}Definition definition)", definitionName));
                var addDefinitionMethodName = string.Format("Add{0}", definitionName);

                var hasAddDefinitionMethod = methods.FirstOrDefault(m =>
                                                        m.Name == addDefinitionMethodName &&
                                                        m.GetParameters().Count() == 2 &&
                                                        m.GetParameters()[0].ParameterType == typeof(ModelNode) &&
                                                        m.GetParameters()[1].ParameterType == definitionType) != null;

                Trace.WriteLine(string.Format("     Add{0}(this ModelNode model, {0}Definition definition, Action<ModelNode> action))", definitionName));
                // 2. should be one with "(this ModelNode model, XXXDefinition definition, Action<ModelNode> action)"
                var hasAddDefinitionWithCallbackMethod = true;

                //hasAddDefinitionWithCallbackMethod = methods.FirstOrDefault(m =>
                //                   m.Name == addDefinitionMethodName &&
                //                   m.GetParameters().Count() == 3 &&
                //                    m.GetParameters()[0].ParameterType == typeof(ModelNode) &&
                //                    m.GetParameters()[1].ParameterType == definitionType &&
                //                    m.GetParameters()[2].ParameterType == typeof(Action<ModelNode>)) != null;


                Trace.WriteLine(string.Format("    [{0}] - Add{1}(this ModelNode model, {1}Definition definition))", hasAddDefinitionMethod.ToString().ToUpper(), definitionName));
                Trace.WriteLine(string.Format("    [{0}] - Add{1}(this ModelNode model, {1}Definition definition, Action<ModelNode> action))", hasAddDefinitionWithCallbackMethod.ToString().ToUpper(), definitionName));

                #endregion

                #region AddXXXs array overload

                var hasAddArrayDefinitionMethod = true;
                var shouldCheckArrayOverload = definitionType.GetCustomAttributes(typeof(ExpectArrayExtensionMethod)).Any();

                if (shouldCheckArrayOverload)
                {
                    // validate (this ModelNode model, XXXDefinition definition)
                    Trace.WriteLine(
                        string.Format(
                            "     Add{0}s(this ModelNode model, IEnumerable<{0}Definition> definitions, Action<ModelNode> action))",
                            definitionName));
                    var addArrayDefinitionMethodName = string.Format("Add{0}s", definitionName);

                    if (definitionType == typeof(PrefixDefinition))
                        addArrayDefinitionMethodName = string.Format("{0}es", definitionName);
                    if (definitionType == typeof(PropertyDefinition))
                        addArrayDefinitionMethodName = string.Format("AddProperties");
                    if (definitionType == typeof(SiteDocumentsDefinition))
                        addArrayDefinitionMethodName = string.Format("AddSiteDocuments");
                    if (definitionType == typeof(ManagedPropertyDefinition))
                        addArrayDefinitionMethodName = string.Format("AddManagedProperties");
                    if (definitionType == typeof(DiagnosticsServiceBaseDefinition))
                        addArrayDefinitionMethodName = string.Format("AddDiagnosticsServices");
                    if (definitionType == typeof(ListItemFieldValuesDefinition))
                        addArrayDefinitionMethodName = string.Format("ListItemFieldValues");

                    var arrayTypoe = typeof(IEnumerable<>);
                    var arrayDefinitionType = arrayTypoe.MakeGenericType(definitionType);

                    hasAddArrayDefinitionMethod = methods.FirstOrDefault(m =>
                        m.Name == addArrayDefinitionMethodName &&
                        m.GetParameters().Count() == 2 &&
                        m.GetParameters()[0].ParameterType == typeof(ModelNode) &&
                        m.GetParameters()[1].ParameterType == arrayDefinitionType) != null;

                    Trace.WriteLine(
                      string.Format("    [{0}] - {1}(this ModelNode model, IEnumerable<{2}Definition> definitions))",
                          hasAddArrayDefinitionMethod.ToString().ToUpper(), addArrayDefinitionMethodName, definitionName));

                }
                else
                {
                    Trace.WriteLine(string.Format("    [SKIPPING] Skipping AddXXXs() arrary overload as there is no ExpectArrayExtensionMethod attr"));
                }

                #endregion

                #region AddHostXXX()

                var shouldCheckAddHostOverload = definitionType.GetCustomAttributes(
                    typeof(ExpectAddHostExtensionMethod), false).Any();

                var hasAddHostDefinitionMethod = true;
                var hasAddHostDefinitionWithCallbackMethod = true;

                if (shouldCheckAddHostOverload)
                {
                    // validate (this ModelNode model, XXXDefinition definition)
                    Trace.WriteLine(string.Format("     AddHost{0}(this ModelNode model, {0}Definition definition)",
                        definitionName));
                    var addHostDefinitionMethodName = string.Format("AddHost{0}", definitionName);

                    hasAddHostDefinitionMethod = methods.FirstOrDefault(m =>
                        m.Name == addHostDefinitionMethodName &&
                        m.GetParameters().Count() == 2 &&
                        m.GetParameters()[0].ParameterType == typeof(ModelNode) &&
                        m.GetParameters()[1].ParameterType == definitionType) != null;

                    Trace.WriteLine(
                        string.Format(
                            "     AddHost{0}(this ModelNode model, {0}Definition definition, Action<ModelNode> action))",
                            definitionName));
                    // 2. should be one with "(this ModelNode model, XXXDefinition definition, Action<ModelNode> action)"
                    hasAddHostDefinitionWithCallbackMethod = methods.FirstOrDefault(m =>
                        m.Name == addHostDefinitionMethodName &&
                        m.GetParameters().Count() == 3 &&
                        m.GetParameters()[0].ParameterType == typeof(ModelNode) &&
                        m.GetParameters()[1].ParameterType == definitionType &&
                        m.GetParameters()[2].ParameterType == typeof(Action<ModelNode>)) != null;


                    Trace.WriteLine(
                        string.Format("    [{0}] - AddHost{1}(this ModelNode model, {1}Definition definition))",
                            hasAddHostDefinitionMethod.ToString().ToUpper(), definitionName));
                    Trace.WriteLine(
                        string.Format(
                            "    [{0}] - AddHost{1}(this ModelNode model, {1}Definition definition, Action<ModelNode> action))",
                            hasAddHostDefinitionWithCallbackMethod.ToString().ToUpper(), definitionName));
                }
                else
                {
                    Trace.WriteLine(string.Format("    [SKIPPING] Skipping AddHostXXX() methods as there is no ExpectAddHostExtensionMethod attr"));
                }

                #endregion

                // push back
                if (hasAddDefinitionMethod != true || hasAddDefinitionWithCallbackMethod != true ||
                    hasAddHostDefinitionMethod != true || hasAddHostDefinitionWithCallbackMethod != true ||
                    hasAddArrayDefinitionMethod != true)
                    hasAllAddMethods = false;
            }

            Assert.IsTrue(hasAllAddMethods);
        }

        [TestMethod]
        [TestCategory("Regression.Definitions.Syntax.v11")]
        [TestCategory("CI.Core")]
        public void DefinitionsShouldHaveWithXXX_DefinitionSyntax_v11()
        {
            if (!M2RegressionRuntime.IsV11)
                return;

            var showSkipping = false;

            var methods = GetModelNodeExtensionMethods(new[]
            {
                typeof(DefinitionBase).Assembly,
                typeof(TaxonomyFieldDefinition).Assembly,
                typeof(ListDefinitionSyntax).Assembly
            });

            var hasAllAddMethods = true;

            foreach (var definitionType in AllDefinitionTypes.OrderBy(d => d.Name))
            {
                var definitionName = definitionType.Name.Replace("Definition", string.Empty);

                Trace.WriteLine(string.Format("Definition: [{0}]", definitionName));

                var shouldCheckWithMethod = definitionType.GetCustomAttributes(typeof(ExpectWithExtensionMethod), false).Any();


                #region WithXXX()

                // validate (this ModelNode model, XXXDefinition definition)
                var addDefinitionMethodName = string.Format("With{0}s", definitionName);

                if (definitionType == typeof(PrefixDefinition))
                    addDefinitionMethodName = string.Format("With{0}es", definitionName);
                if (definitionType == typeof(PropertyDefinition))
                    addDefinitionMethodName = string.Format("WithProperties");
                if (definitionType == typeof(DiagnosticsServiceBaseDefinition))
                    addDefinitionMethodName = string.Format("WithDiagnosticsServices");
                if (definitionType == typeof(RootWebDefinition))
                    addDefinitionMethodName = string.Format("WithRootWeb");

                if (!shouldCheckWithMethod)
                {
                    if (showSkipping)
                        Trace.WriteLine(string.Format("     {0}(this ModelNode model, {0}Definition definition, Action<ModelNode> action)) - SKIPPING", addDefinitionMethodName));

                    continue;
                }


                var hasWithMethod = methods.FirstOrDefault(m =>
                                                        m.Name == addDefinitionMethodName &&
                                                        m.GetParameters().Count() == 2 &&
                                                        m.GetParameters()[0].ParameterType == typeof(ModelNode) &&
                                                        m.GetParameters()[1].ParameterType == typeof(Action<ModelNode>)) != null;

                if (hasWithMethod)
                    Trace.WriteLine(string.Format("     {0}(this ModelNode model, {0}Definition definition, Action<ModelNode> action)) - TRUE", addDefinitionMethodName));
                else
                    Trace.WriteLine(string.Format("     {0}(this ModelNode model, {0}Definition definition, Action<ModelNode> action)) - FALSE", addDefinitionMethodName));


                #endregion

                // push back
                if (hasWithMethod != true)
                    hasAllAddMethods = false;
            }

            Assert.IsTrue(hasAllAddMethods);
        }

        #region v12


        protected static IEnumerable<DefinitionRelationship> AllDefinitionRelationships
        {
            get
            {
                DefaultDefinitionRelationship.InitFromParentHostCapabilityAttribute(typeof(FieldDefinition).Assembly);
                DefaultDefinitionRelationship.InitFromParentHostCapabilityAttribute(typeof(TaxonomyFieldDefinition).Assembly);

                return ServiceContainer.Instance
                                       .GetService<DefinitionRelationshipServiceBase>()
                                       .GetDefinitionRelationships();
            }
        }

        protected static List<MethodInfo> allExtensionMethods;

        protected static List<MethodInfo> AllExtensionMethods
        {
            get
            {
                if (allExtensionMethods == null)
                    allExtensionMethods = GetModelNodeExtensionMethods(new[]{
                    typeof(DefinitionBase).Assembly,
                    typeof(TaxonomyFieldDefinition).Assembly,
                    typeof(ListDefinitionSyntax).Assembly
                });

                return allExtensionMethods;
            }
        }

        [TestMethod]
        [TestCategory("Regression.Definitions.Syntax.v12")]
        [TestCategory("CI.Core")]
        public void DefinitionsShouldHave_ParentHostCapabilityAttribute_v12()
        {
            if (!M2RegressionRuntime.IsV12)
                return;

            var passed = true;

            // definitions must have a relationship attt

            var defTypesWithoutRelationships = AllDefinitionTypes.Where(d => AllDefinitionRelationships.All(r => r.DefinitionType != d));

            Trace.WriteLine("Cheking ParentHostCapability attr presence for all definitions");

            TraceUtils.WithScope(trace =>
            {
                foreach (var def in defTypesWithoutRelationships)
                    Trace.WriteLine(string.Format("missing relationship for definition:[{0}]", def.Name));
            });

            if (defTypesWithoutRelationships.Any())
                passed = false;

            if (defTypesWithoutRelationships.Any())
                Trace.WriteLine("[FALSE] Missing definition relationships detected");
            else
                Trace.WriteLine("[TRUE] Missing definition relationships detected");

            Assert.IsTrue(passed);
        }


        [TestMethod]
        [TestCategory("Regression.Definitions.Syntax.v12")]
        [TestCategory("CI.Core")]
        public void DefinitionsShouldHave_TypedModelNodes_v12()
        {
            Trace.WriteLine("Checking typed model nodes");
            Trace.WriteLine("");

            var passes = true;
            var showOnlyFalseOutput = true;

            TraceUtils.WithScope(trace =>
            {
                foreach (var defType in AllDefinitionTypes.OrderBy(d => d.Name))
                {
                    var pureDefName = defType.Name.Replace("Definition", string.Empty);

                    var targetModelNodeTypeName = string.Format("{0}ModelNode", pureDefName);
                    var modelNodelType = AllModelNodeTypes.FirstOrDefault(n => n.Name == targetModelNodeTypeName);

                    if (!showOnlyFalseOutput)
                        trace.WriteLine(defType.Name);

                    if (modelNodelType == null)
                    {
                        passes = false;

                        if (showOnlyFalseOutput)
                            trace.WriteLine(defType.Name);

                        trace.WriteLine(string.Format("[FALSE] Missing model node: [{0}]", targetModelNodeTypeName));
                        trace.WriteLine("");
                    }
                }
            });

            Assert.IsTrue(passes);
        }

        [TestMethod]
        [TestCategory("Regression.Definitions.Syntax.v12")]
        [TestCategory("CI.Core")]
        public void DefinitionsShouldHaveAddXXX_DefinitionSyntax_v12()
        {
            if (!M2RegressionRuntime.IsV12)
                return;

            var passed = true;
            var showOnlyFalseOutput = true;

            var allCount = AllDefinitionTypes.Count;
            var missesCount = 0;

            // add definitions should have AddXXX() methods

            // - must be generic
            // - typed model node
            // - definitions type as a second param
            // - overload with typed node call back
            // - allback with typed model node

            // AddField<TModelNode>(this TModelNode model, FieldDefinition definition)
            // where TModelNode : ModelNode, IFieldHostModelNode, new()

            // AddField<TModelNode>(this TModelNode model, FieldDefinition definition, Action<FieldModelNode> action)
            // where TModelNode : ModelNode, IFieldHostModelNode, new()

            Trace.WriteLine("Checking AddXXX() method specs");
            Trace.WriteLine("");

            TraceUtils.WithScope(trace =>
            {
                foreach (var defType in AllDefinitionTypes.OrderBy(d => d.Name))
                {
                    var isRootDef = (defType.GetCustomAttributes(typeof(ParentHostCapabilityAttribute))
                                     .FirstOrDefault() as ParentHostCapabilityAttribute).IsRoot;

                    var isSelfHostDefinition = (defType.GetCustomAttributes(typeof(SelfHostCapabilityAttribute)).Any());

                    if (isRootDef)
                    {
                        trace.WriteLine(string.Format("Skipping root def"));
                        continue;
                    }

                    var completedtype = true;

                    var pureDefName = defType.Name.Replace("Definition", string.Empty);

                    var shouldCheckArrayOverload = defType.GetCustomAttributes(typeof(ExpectArrayExtensionMethod), false).Any();
                    var shouldCheckAddHostMethod = defType.GetCustomAttributes(typeof(ExpectAddHostExtensionMethod), false).Any();

                    var targetModelNodeTypeName = string.Format("{0}ModelNode", pureDefName);
                    var modelNodelType = AllModelNodeTypes.FirstOrDefault(n => n.Name == targetModelNodeTypeName);

                    trace.WriteLine(defType.Name);

                    if (modelNodelType == null)
                    {
                        trace.WriteLine(string.Format("[FALSE] Missing model node: [{0}]", targetModelNodeTypeName));
                    }

                    trace.WithTraceIndent(addXXXTrace =>
                    {
                        if (!showOnlyFalseOutput)
                            trace.WriteLine("Checking AddXXX() method");

                        var relationships = AllDefinitionRelationships.FirstOrDefault(r => r.DefinitionType == defType);

                        var addXXXMethodName = string.Format("Add{0}", pureDefName);
                        var addXXXArrayDefinitionMethodName = string.Format("Add{0}s", pureDefName);

                        var addXXXHostMethodName = string.Format("AddHost{0}", pureDefName);

                        var lastChar = pureDefName[pureDefName.Length - 1];

                        switch (lastChar)
                        {
                            case 'y':
                                {
                                    addXXXArrayDefinitionMethodName = string.Format("Add{0}", pureDefName);
                                    addXXXArrayDefinitionMethodName = addXXXArrayDefinitionMethodName.Substring(0, addXXXArrayDefinitionMethodName.Length - 1);
                                    addXXXArrayDefinitionMethodName += "ies";
                                }
                                ;
                                break;

                            case 'x':
                            case 'e':
                                {
                                    addXXXArrayDefinitionMethodName = string.Format("Add{0}", pureDefName);
                                    addXXXArrayDefinitionMethodName += "s";

                                } break;

                            case 's':
                                {
                                    addXXXArrayDefinitionMethodName = string.Format("Add{0}", pureDefName);
                                    addXXXArrayDefinitionMethodName += "es";
                                } break;
                        }

                        if (defType == typeof(SiteDocumentsDefinition))
                            addXXXArrayDefinitionMethodName = string.Format("AddSiteDocuments");

                        if (defType == typeof(MetadataNavigationSettingsDefinition))
                            addXXXArrayDefinitionMethodName = string.Format("AddMetadataNavigationSettings");

                        // host

                        Func<MethodInfo, bool> addHostMethodPlainSpec = (m) =>
                        {
                            // public static TModelNode AddAlternateUrl<TModelNode>(this TModelNode model, AlternateUrlDefinition definition)
                            // where TModelNode : ModelNode, IWebApplicationHostModelNode, new()

                            return m.Name == addXXXHostMethodName
                                   && m.IsGenericMethod

                                   && (m.GetParameters().Count() == 2)
                                   && m.ReturnType.Name == "TModelNode"

                                   && m.GetGenericArguments()[0].Name == "TModelNode"
                                   && m.GetGenericArguments()[0].BaseType == typeof(ModelNode)

                                   && m.GetParameters()[1].ParameterType == defType
                                ;
                        };

                        Func<MethodInfo, bool> addHostMethodWithCallbackSpec = (m) =>
                        {
                            // public static TModelNode AddAlternateUrl<TModelNode>(this TModelNode model, AlternateUrlDefinition definition,
                            // Action<ContentTypeModelNode> action)
                            // where TModelNode : ModelNode, IWebApplicationHostModelNode, new()

                            var callbackType = typeof(Action<>);
                            var typedCallbackType = callbackType.MakeGenericType(modelNodelType);

                            Func<MethodInfo, bool> selfHostCallbackCheck = (method => true);
                            selfHostCallbackCheck = (method) => method.GetParameters()[2].ParameterType == typedCallbackType;

                            return m.Name == addXXXHostMethodName
                                   && m.IsGenericMethod

                                   && (m.GetParameters().Count() == 3)

                                   && m.ReturnType.Name == "TModelNode"

                                   && m.GetGenericArguments()[0].Name == "TModelNode"
                                   && m.GetGenericArguments()[0].BaseType == typeof(ModelNode)

                                   && m.GetParameters()[1].ParameterType == defType
                                   && selfHostCallbackCheck(m);
                        };



                        // add

                        Func<MethodInfo, bool> addMethodPlainSpec = (m) =>
                        {
                            // public static TModelNode AddAlternateUrl<TModelNode>(this TModelNode model, AlternateUrlDefinition definition)
                            // where TModelNode : ModelNode, IWebApplicationHostModelNode, new()

                            return m.Name == addXXXMethodName
                                   && m.IsGenericMethod

                                   && (m.GetParameters().Count() == 2)
                                   && m.ReturnType.Name == "TModelNode"

                                   && m.GetGenericArguments()[0].Name == "TModelNode"
                                   && m.GetGenericArguments()[0].BaseType == typeof(ModelNode)

                                   && m.GetParameters()[1].ParameterType == defType
                                ;
                        };

                        Func<MethodInfo, bool> addMethodWithCallbackSpec = (m) =>
                        {
                            // public static TModelNode AddAlternateUrl<TModelNode>(this TModelNode model, AlternateUrlDefinition definition,
                            // Action<ContentTypeModelNode> action)
                            // where TModelNode : ModelNode, IWebApplicationHostModelNode, new()

                            var callbackType = typeof(Action<>);
                            var typedCallbackType = callbackType.MakeGenericType(modelNodelType);

                            Func<MethodInfo, bool> selfHostCallbackCheck = (method => true);

                            if (isSelfHostDefinition)
                            {
                                selfHostCallbackCheck = (method) =>
                                {
                                    // self return
                                    // callback has the same type as TModelNode
                                    // that works for ResetRoleInheritance/BreakRoleInheritance as they 'pass' the current object further to the chain

                                    // public static TModelNode AddResetRoleInheritance<TModelNode>(this TModelNode model, ResetRoleInheritanceDefinition definition)
                                    // where TModelNode : ModelNode, ISecurableObjectHostModelNode, new()

                                    return typeof(Action<>).MakeGenericType(method.GetParameters()[0].ParameterType) == method.GetParameters()[2].ParameterType;
                                };
                            }
                            else
                            {
                                selfHostCallbackCheck = (method) => method.GetParameters()[2].ParameterType == typedCallbackType;
                            }

                            return m.Name == addXXXMethodName
                                   && m.IsGenericMethod

                                   && (m.GetParameters().Count() == 3)

                                   && m.ReturnType.Name == "TModelNode"

                                   && m.GetGenericArguments()[0].Name == "TModelNode"
                                   && m.GetGenericArguments()[0].BaseType == typeof(ModelNode)

                                   && m.GetParameters()[1].ParameterType == defType
                                   && selfHostCallbackCheck(m);
                        };

                        Func<MethodInfo, bool> addMethodWithArraySpec = (m) =>
                        {
                            //  public static TModelNode AddAlternateUrls<TModelNode>(this TModelNode model, IEnumerable<AlternateUrlDefinition> definitions)
                            //  where TModelNode : ModelNode, IWebApplicationHostModelNode, new()

                            var arrayType = typeof(IEnumerable<>);
                            var arrayDefinitionType = arrayType.MakeGenericType(defType);

                            return m.Name == addXXXArrayDefinitionMethodName
                                   && m.IsGenericMethod
                                   && (m.GetParameters().Count() == 2)
                                   && m.ReturnType.Name == "TModelNode"

                                   && m.GetGenericArguments()[0].Name == "TModelNode"
                                   && m.GetGenericArguments()[0].BaseType == typeof(ModelNode)

                                   && m.GetParameters()[1].ParameterType == arrayDefinitionType
                                   ;
                        };


                        var addMethodPlain = AllExtensionMethods.FirstOrDefault(addMethodPlainSpec);
                        var addMethodPlainWithCallBack = AllExtensionMethods.FirstOrDefault(addMethodWithCallbackSpec);

                        if (addMethodPlain == null)
                        {
                            passed = false;
                            completedtype = false;

                            addXXXTrace.WriteLine("[FALSE] AddXXX()");
                        }
                        else
                        {
                            var missedRelationshipModelNodeTypes = GetUnsupportedRelationshipModelNodeTypes(addMethodPlainWithCallBack, relationships);

                            if (missedRelationshipModelNodeTypes.Any())
                            {
                                passed = false;
                                completedtype = false;

                                addXXXTrace.WriteLine(string.Format(
                                        "[FALSE] AddXXX() misses relationships: [{0}]",
                                        string.Join(",", missedRelationshipModelNodeTypes)));
                            }
                        }

                        if (addMethodPlainWithCallBack == null)
                        {
                            passed = false;
                            completedtype = false;

                            addXXXTrace.WriteLine("[FALSE] AddXXX(callback)");
                        }
                        else
                        {
                            var missedRelationshipModelNodeTypes = GetUnsupportedRelationshipModelNodeTypes(addMethodPlainWithCallBack, relationships);

                            if (missedRelationshipModelNodeTypes.Any())
                            {
                                passed = false;
                                completedtype = false;

                                addXXXTrace.WriteLine(string.Format(
                                        "[FALSE] AddXXX(callback) misses relationships: [{0}]",
                                        string.Join(",", missedRelationshipModelNodeTypes)));
                            }
                        }

                        if (shouldCheckArrayOverload)
                        {
                            var addMethodWithArraySupport = AllExtensionMethods.FirstOrDefault(addMethodWithArraySpec);

                            if (addMethodWithArraySupport == null)
                            {
                                passed = false;
                                completedtype = false;

                                addXXXTrace.WriteLine("[FALSE] AddXXXs()");
                            }
                            else
                            {
                                var missedRelationshipModelNodeTypes = GetUnsupportedRelationshipModelNodeTypes(addMethodWithArraySupport, relationships);

                                if (missedRelationshipModelNodeTypes.Any())
                                {
                                    passed = false;
                                    completedtype = false;

                                    addXXXTrace.WriteLine(string.Format(
                                            "[FALSE] AddXXXs() misses relationships: [{0}]",
                                            string.Join(",", missedRelationshipModelNodeTypes)));
                                }
                            }
                        }


                        if (shouldCheckAddHostMethod)
                        {
                            var addHostMethodPlain = AllExtensionMethods.FirstOrDefault(addHostMethodPlainSpec);
                            var addHostMethodPlainWithCallBack = AllExtensionMethods.FirstOrDefault(addHostMethodWithCallbackSpec);

                            if (addHostMethodPlain == null)
                            {
                                passed = false;
                                completedtype = false;

                                addXXXTrace.WriteLine("[FALSE] AddHostXXX()");
                            }
                            else
                            {
                                var missedRelationshipModelNodeTypes = GetUnsupportedRelationshipModelNodeTypes(addHostMethodPlain, relationships);

                                if (missedRelationshipModelNodeTypes.Any())
                                {
                                    passed = false;
                                    completedtype = false;

                                    addXXXTrace.WriteLine(string.Format(
                                            "[FALSE] AddHostXXX() misses relationships: [{0}]",
                                            string.Join(",", missedRelationshipModelNodeTypes)));
                                }
                            }

                            if (addHostMethodPlainWithCallBack == null)
                            {
                                passed = false;
                                completedtype = false;

                                addXXXTrace.WriteLine("[FALSE] AddHostXXX(callback)");
                            }
                            else
                            {
                                var missedRelationshipModelNodeTypes = GetUnsupportedRelationshipModelNodeTypes(addHostMethodPlainWithCallBack, relationships);

                                if (missedRelationshipModelNodeTypes.Any())
                                {
                                    passed = false;
                                    completedtype = false;

                                    addXXXTrace.WriteLine(string.Format(
                                            "[FALSE] AddHostXXX(callback) misses relationships: [{0}]",
                                            string.Join(",", missedRelationshipModelNodeTypes)));
                                }
                            }
                        }
                    });

                    if (!showOnlyFalseOutput)
                        trace.WriteLine("");

                    if (!completedtype)
                        missesCount++;
                }
            });

            Trace.WriteLine("");
            Trace.WriteLine(string.Format("{0}/{1}", missesCount, allCount));

            Assert.IsTrue(passed);
        }

        private static IEnumerable<string> GetUnsupportedRelationshipModelNodeTypes(
            MethodInfo modelSyntaxMethodInfo,
            DefinitionRelationship relationships)
        {
            if (modelSyntaxMethodInfo == null)
                return Enumerable.Empty<string>();

            var currentInterfaceTypes = modelSyntaxMethodInfo.GetGenericArguments()[0]
                .GetInterfaces()
                .Where(i => i != typeof(IHostModelNode))
                .ToList();

            var currentModelModeTypeNames = AllModelNodeTypes.Where(n => n.GetInterfaces()
                .Any(i => currentInterfaceTypes.Contains(i)))
                .Select(t => t.Name);

            var expectedModelNodeTypeNames =
                relationships.HostTypes.Select(
                    t => string.Format("{0}ModelNode", t.Name.Replace("Definition", string.Empty)));

            var unsupportedRelationshipModelNodeTypes =
                expectedModelNodeTypeNames.Where(i => !currentModelModeTypeNames.Contains(i));

            return unsupportedRelationshipModelNodeTypes;
        }


        [TestMethod]
        [TestCategory("Regression.Definitions.Syntax.v12")]
        [TestCategory("CI.Core")]
        public void DefinitionsShouldHaveWithXXX_DefinitionSyntax_v12()
        {
            if (!M2RegressionRuntime.IsV12)
                return;


        }

        #endregion

        #endregion

        #region utils

        private static IEnumerable<MethodInfo> GetExtensionMethods(Assembly assembly, Type extendedType)
        {
            return GetExtensionMethods(assembly, extendedType, false);
        }

        static IEnumerable<MethodInfo> GetExtensionMethods(Assembly assembly, Type extendedType, bool includeSubclasses)
        {
            var query = from type in assembly.GetTypes()
                        where type.IsSealed && !type.IsGenericType && !type.IsNested
                        from method in type.GetMethods(BindingFlags.Static
                            | BindingFlags.Public | BindingFlags.NonPublic)
                        where method.IsDefined(typeof(ExtensionAttribute), false)
                        where ((method.GetParameters()[0].ParameterType == extendedType)
                                   || (includeSubclasses && method.GetParameters()[0].ParameterType.IsSubclassOf(extendedType))
                               )
                        select method;

            return query;
        }

        private static List<MethodInfo> GetModelNodeExtensionMethods(IEnumerable<Assembly> assemblies)
        {
            var methods = new List<MethodInfo>();

            foreach (var asm in assemblies)
            {
                foreach (var definitionType in AllDefinitionTypes)
                {
                    var methodInfos = GetExtensionMethods(asm, typeof(ModelNode), true);
                    foreach (var method in methodInfos)
                        if (!methods.Contains(method))
                            methods.Add(method);
                }
            }

            return methods;
        }



        #endregion
    }
}
