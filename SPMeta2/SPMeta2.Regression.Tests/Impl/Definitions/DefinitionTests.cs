using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
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

namespace SPMeta2.Regression.Tests.Impl.Definitions
{
    [TestClass]
    public class DefinitionTests
    {
        #region properties

        protected static List<Type> DefinitionTypes = new List<Type>();
        protected static List<Type> ModelNodeTypes = new List<Type>();

        #endregion

        #region common

        [ClassInitializeAttribute]
        public static void Init(TestContext context)
        {
            LoadDefinitions();
            LoadModelNodes();
        }

        private static void LoadModelNodes()
        {
            var spMetaAssembly = typeof(FieldDefinition).Assembly;
            var spMetaStandardAssembly = typeof(TaxonomyFieldDefinition).Assembly;

            ModelNodeTypes.AddRange(ReflectionUtils.GetTypesFromAssemblies<ModelNode>(new[]
            {
             spMetaAssembly,
             spMetaStandardAssembly
            }));
        }

        protected static void LoadDefinitions()
        {
            var spMetaAssembly = typeof(FieldDefinition).Assembly;
            var spMetaStandardAssembly = typeof(TaxonomyFieldDefinition).Assembly;

            DefinitionTypes.AddRange(ReflectionUtils.GetTypesFromAssemblies<DefinitionBase>(new[]
            {
             spMetaAssembly,
             spMetaStandardAssembly
            }));
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
        public void Can_CreateDefinition()
        {
            foreach (var definitionType in DefinitionTypes)
            {
                var impl = Activator.CreateInstance(definitionType) as DefinitionBase;

                Assert.IsNotNull(impl);
            }
        }

        [TestMethod]
        [TestCategory("Regression.Definitions")]
        public void DefinitionPropertyBag_ShouldNotbeNull()
        {
            foreach (var definitionType in DefinitionTypes)
            {
                var impl = Activator.CreateInstance(definitionType) as DefinitionBase;

                Assert.IsNotNull(impl);
                Assert.IsNotNull(impl.PropertyBag);
            }
        }

        [TestMethod]
        [TestCategory("Regression.Definitions")]
        public void DefinitionsShouldHaveToStringOverride()
        {
            foreach (var definitionType in DefinitionTypes)
            {
                var hasToStringOverride = definitionType.GetMethod("ToString").DeclaringType == definitionType;

                if (!hasToStringOverride)
                    Trace.WriteLine(string.Format("Checking definition type:[{0}]. Has override:[{1}]", definitionType, hasToStringOverride));

                Assert.IsTrue(hasToStringOverride);
            }
        }

        [TestMethod]
        [TestCategory("Regression.Definitions")]
        public void DefinitionsShouldBeMarkedAsSerializable()
        {
            var showOnlyFails = true;
            var result = true;

            foreach (var definitionType in DefinitionTypes)
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
        public void DefinitionsShouldBeMarkedAsDataContract()
        {
            var showOnlyFails = true;
            var result = true;

            foreach (var definitionType in DefinitionTypes)
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
        [TestCategory("Regression.Definitions.Identity")]
        public void DefinitionsShouldHasIdentityOrIdentityKey()
        {
            var showOnlyFails = true;
            var result = true;

            foreach (var definitionType in DefinitionTypes)
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
        public void ModelNodeTypesPublicPropsShouldBeMarkedAsDataMemberOrIgnoreDataMemberAttr()
        {
            var showOnlyFails = true;
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
        public void DefinitionsPublicPropsShouldBeMarkedAsDataMemberOrIgnoreDataMemberAttr()
        {
            var showOnlyFails = true;
            var result = true;
            var errors = 0;

            foreach (var definitionType in DefinitionTypes)
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

        #endregion

        #region API and Syntax

        [TestMethod]
        [TestCategory("Regression.Definitions.Syntax.v11")]
        public void DefinitionsShouldHaveAddXXX_DefinitionSyntax_v11()
        {
            if (!M2RegressionRuntime.IsV11)
                return;

            var showTrace = false;

            var methods = GetModelNodeExtensionMethods(new[]
            {
                typeof(DefinitionBase).Assembly,
                typeof(TaxonomyFieldDefinition).Assembly,
                typeof(ListDefinitionSyntax).Assembly
            });

            var hasAllAddMethods = true;

            foreach (var definitionType in DefinitionTypes)
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
        public void DefinitionsShouldHaveWithXXX_DefinitionSyntax_v11()
        {
            if (M2RegressionRuntime.CurrentAPIVersion < M2Consts.APIv12)
                return;

            var showSkipping = false;

            var methods = GetModelNodeExtensionMethods(new[]
            {
                typeof(DefinitionBase).Assembly,
                typeof(TaxonomyFieldDefinition).Assembly,
                typeof(ListDefinitionSyntax).Assembly
            });

            var hasAllAddMethods = true;

            foreach (var definitionType in DefinitionTypes)
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

        [TestMethod]
        [TestCategory("Regression.Definitions.Syntax.v12")]
        public void DefinitionsShouldHave_RelationshipMap_v12()
        {
            if (!M2RegressionRuntime.IsV12)
                return;

            var showFails = true;

            DefaultDefinitionRelationship.InitFromParentHostCapabilityAttribute(typeof(FieldDefinition).Assembly);
            DefaultDefinitionRelationship.InitFromParentHostCapabilityAttribute(typeof(TaxonomyFieldDefinition).Assembly);

            var relationshipService = ServiceContainer.Instance.GetService<DefinitionRelationshipServiceBase>();
            var allRelationships = relationshipService.GetDefinitionRelationships().ToList();

            allRelationships = allRelationships.OrderBy(s => s.DefinitionType.Name).ToList();

            var passed = true;

            foreach (var definitionType in DefinitionTypes)
            {
                var hasRelationship = allRelationships.Any(r => r.DefinitionType == definitionType);

                if (showFails)
                {
                    if (!hasRelationship)
                    {
                        Trace.WriteLine(string.Format("Definition: [{0}]", definitionType.Name));
                        Trace.WriteLine(string.Format(" - Has relationship: [{0}]", hasRelationship.ToString().ToUpper()));
                    }
                }
                else
                {
                    Trace.WriteLine(string.Format("Definition: [{0}]", definitionType.Name));
                    Trace.WriteLine(string.Format(" - Has relationship: [{0}]", hasRelationship.ToString().ToUpper()));
                }

                if (!hasRelationship)
                    passed = false;
            }

            Assert.IsTrue(passed);
        }

        [TestMethod]
        [TestCategory("Regression.Definitions.Syntax.v12")]
        public void DefinitionsShouldHaveAddXXX_DefinitionSyntax_v12()
        {
            if (!M2RegressionRuntime.IsV12)
                return;

            var showTrace = false;

            var methods = GetModelNodeExtensionMethods(new[]
            {
                typeof(DefinitionBase).Assembly,
                typeof(TaxonomyFieldDefinition).Assembly,
                typeof(ListDefinitionSyntax).Assembly
            });

            var hasAllAddMethods = true;

            DefaultDefinitionRelationship.InitFromParentHostCapabilityAttribute(typeof(FieldDefinition).Assembly);
            DefaultDefinitionRelationship.InitFromParentHostCapabilityAttribute(typeof(TaxonomyFieldDefinition).Assembly);

            var relationshipService = ServiceContainer.Instance.GetService<DefinitionRelationshipServiceBase>();
            var allRelationships = relationshipService.GetDefinitionRelationships();

            var missedModelNodeTypes = new List<string>();
            var falseMethodsCount = 0;

            var missedRelationships = new List<string>();
            var missedMethods = new List<string>();

            foreach (var definitionType in DefinitionTypes)
            {
                var definitionRelationships = allRelationships.FirstOrDefault(r => r.DefinitionType == definitionType);

                if (definitionRelationships == null)
                {
                    var missedRelationshipMessage = string.Format("[FALSE] Miss definition relationship for type: [{0}]", definitionType.Name);
                    Trace.WriteLine(missedRelationshipMessage);

                    missedRelationships.Add(missedRelationshipMessage);

                    hasAllAddMethods = false;
                    continue;
                }

                var definitionName = definitionType.Name.Replace("Definition", string.Empty);

                Trace.WriteLine(string.Format("Definition: [{0}]", definitionName));

                #region AddXXX()

                // validate (this ModelNode model, XXXDefinition definition)
                Trace.WriteLine(string.Format("    Add{0}(this ModelNode model, {0}Definition definition)", definitionName));
                var addDefinitionMethodName = string.Format("Add{0}", definitionName);

                var hasAddDefinitionMethod = true;

                if (definitionRelationships.HostTypes != null)
                {
                    var parentModelNodeTypeNames = definitionRelationships.HostTypes
                        .Select(t =>
                        {
                            var defName = t.Name.Replace("Definition", string.Empty);
                            return string.Format("{0}ModelNode", defName);
                        });

                    var parentModelNodeTypes = ModelNodeTypes.Where(m => parentModelNodeTypeNames.Contains(m.Name));

                    if (parentModelNodeTypes.Count() != parentModelNodeTypeNames.Count())
                    {
                        Trace.WriteLine(string.Format("[FALSE] Miss ModelNode types: [{0}]",
                            string.Join(" , ", parentModelNodeTypeNames)));

                        foreach (var m in parentModelNodeTypeNames)
                        {
                            if (parentModelNodeTypes.Any(t => t.Name == m))
                                continue;

                            if (!missedModelNodeTypes.Contains(m))
                                missedModelNodeTypes.Add(m);
                        }
                    }


                    // V12
                    foreach (var parentNodeModelType in parentModelNodeTypes)
                    {
                        hasAddDefinitionMethod = methods.Any(m =>
                            m.Name == addDefinitionMethodName
                            && m.GetParameters().Count() == 2
                            && m.ReturnType == parentNodeModelType
                            && m.GetParameters()[0].ParameterType == parentNodeModelType
                            && m.GetParameters()[1].ParameterType == definitionType);

                        if (!hasAddDefinitionMethod)
                        {
                            var missMethod =
                                (string.Format(
                                    "[FALSE] public static {1} Add{0}(this {1} model, {0}Definition definition)",
                                    definitionName, parentNodeModelType.Name));

                            Trace.WriteLine(missMethod);
                            missedMethods.Add(missMethod);

                            falseMethodsCount++;
                        }
                    }
                }

                // V11
                //hasAddDefinitionMethod = methods.FirstOrDefault(m =>
                //                                       m.Name == addDefinitionMethodName &&
                //                                       m.GetParameters().Count() == 2 &&
                //                                       m.GetParameters()[0].ParameterType == typeof(ModelNode) &&
                //                                       m.GetParameters()[1].ParameterType == definitionType) != null;

                //Trace.WriteLine(string.Format("     Add{0}(this ModelNode model, {0}Definition definition, Action<ModelNode> action))", definitionName));
                // 2. should be one with "(this ModelNode model, XXXDefinition definition, Action<ModelNode> action)"
                var hasAddDefinitionWithCallbackMethod = true;

                //  hasAddDefinitionWithCallbackMethod = methods.FirstOrDefault(m =>
                //                   m.Name == addDefinitionMethodName &&
                //                   m.GetParameters().Count() == 3 &&
                //                    m.GetParameters()[0].ParameterType == typeof(ModelNode) &&
                //                    m.GetParameters()[1].ParameterType == definitionType &&
                //                    m.GetParameters()[2].ParameterType == typeof(Action<ModelNode>)) != null;


                //Trace.WriteLine(string.Format("    [{0}] - Add{1}(this ModelNode model, {1}Definition definition))", hasAddDefinitionMethod.ToString().ToUpper(), definitionName));
                //Trace.WriteLine(string.Format("    [{0}] - Add{1}(this ModelNode model, {1}Definition definition, Action<ModelNode> action))", hasAddDefinitionWithCallbackMethod.ToString().ToUpper(), definitionName));

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
                    if (definitionType == typeof(ManagedPropertyDefinition))
                        addArrayDefinitionMethodName = string.Format("AddManagedProperties");
                    if (definitionType == typeof(DiagnosticsServiceBaseDefinition))
                        addArrayDefinitionMethodName = string.Format("AddDiagnosticsServices");
                    if (definitionType == typeof(ListItemFieldValuesDefinition))
                        addArrayDefinitionMethodName = string.Format("ListItemFieldValues");

                    var arrayTypoe = typeof(IEnumerable<>);
                    var arrayDefinitionType = arrayTypoe.MakeGenericType(definitionType);

                    //hasAddArrayDefinitionMethod = methods.FirstOrDefault(m =>
                    //    m.Name == addArrayDefinitionMethodName &&
                    //    m.GetParameters().Count() == 2 &&
                    //    m.GetParameters()[0].ParameterType == typeof(ModelNode) &&
                    //    m.GetParameters()[1].ParameterType == arrayDefinitionType) != null;

                    //Trace.WriteLine(
                    //  string.Format("    [{0}] - {1}(this ModelNode model, IEnumerable<{2}Definition> definitions))",
                    //      hasAddArrayDefinitionMethod.ToString().ToUpper(), addArrayDefinitionMethodName, definitionName));

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

                    //hasAddHostDefinitionMethod = methods.FirstOrDefault(m =>
                    //    m.Name == addHostDefinitionMethodName &&
                    //    m.GetParameters().Count() == 2 &&
                    //    m.GetParameters()[0].ParameterType == typeof(ModelNode) &&
                    //    m.GetParameters()[1].ParameterType == definitionType) != null;

                    Trace.WriteLine(
                        string.Format(
                            "     AddHost{0}(this ModelNode model, {0}Definition definition, Action<ModelNode> action))",
                            definitionName));

                    // 2. should be one with "(this ModelNode model, XXXDefinition definition, Action<ModelNode> action)"
                    //hasAddHostDefinitionWithCallbackMethod = methods.FirstOrDefault(m =>
                    //    m.Name == addHostDefinitionMethodName &&
                    //    m.GetParameters().Count() == 3 &&
                    //    m.GetParameters()[0].ParameterType == typeof(ModelNode) &&
                    //    m.GetParameters()[1].ParameterType == definitionType &&
                    //    m.GetParameters()[2].ParameterType == typeof(Action<ModelNode>)) != null;


                    //Trace.WriteLine(
                    //    string.Format("    [{0}] - AddHost{1}(this ModelNode model, {1}Definition definition))",
                    //        hasAddHostDefinitionMethod.ToString().ToUpper(), definitionName));
                    //Trace.WriteLine(
                    //    string.Format(
                    //        "    [{0}] - AddHost{1}(this ModelNode model, {1}Definition definition, Action<ModelNode> action))",
                    //        hasAddHostDefinitionWithCallbackMethod.ToString().ToUpper(), definitionName));
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

            Trace.WriteLine("S: Missed model node types count:" + missedModelNodeTypes.Count);
            foreach (var m in missedModelNodeTypes)
                Trace.WriteLine("   " + m);

            Trace.WriteLine("S: Missed methods: " + falseMethodsCount);
            foreach (var m in missedMethods.OrderBy(m =>
                m.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[4]))
                Trace.WriteLine("   " + m);


            Assert.IsTrue(hasAllAddMethods);
        }

        [TestMethod]
        [TestCategory("Regression.Definitions.Syntax.v12")]
        public void DefinitionsShouldHaveWithXXX_DefinitionSyntax_v12()
        {
            if (!M2RegressionRuntime.IsV12)
                return;

            var showSkipping = false;

            var methods = GetModelNodeExtensionMethods(new[]
            {
                typeof(DefinitionBase).Assembly,
                typeof(TaxonomyFieldDefinition).Assembly,
                typeof(ListDefinitionSyntax).Assembly
            });

            var hasAllAddMethods = true;

            foreach (var definitionType in DefinitionTypes)
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

        private List<MethodInfo> GetModelNodeExtensionMethods(IEnumerable<Assembly> assemblies)
        {
            var methods = new List<MethodInfo>();

            foreach (var asm in assemblies)
            {
                foreach (var definitionType in DefinitionTypes)
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
