using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;

namespace SPMeta2.Regression.Tests.Impl.Definitions
{
    [TestClass]
    public class DefinitionTests
    {
        #region properties

        protected static List<Type> DefinitionTypes = new List<Type>();

        #endregion

        #region common

        [ClassInitializeAttribute]
        public static void Init(TestContext context)
        {
            LoadDefinitions();
        }

        private static void LoadDefinitions()
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
        public void DefinitionsShouldHaveXXX_DefinitionSyntax()
        {
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
                var hasAddDefinitionWithCallbackMethod = methods.FirstOrDefault(m =>
                                   m.Name == addDefinitionMethodName &&
                                   m.GetParameters().Count() == 3 &&
                                    m.GetParameters()[0].ParameterType == typeof(ModelNode) &&
                                    m.GetParameters()[1].ParameterType == definitionType &&
                                    m.GetParameters()[2].ParameterType == typeof(Action<ModelNode>)) != null;


                Trace.WriteLine(string.Format("    [{0}] - Add{1}(this ModelNode model, {1}Definition definition))", hasAddDefinitionMethod.ToString().ToUpper(), definitionName));
                Trace.WriteLine(string.Format("    [{0}] - Add{1}(this ModelNode model, {1}Definition definition, Action<ModelNode> action))", hasAddDefinitionWithCallbackMethod.ToString().ToUpper(), definitionName));


                #endregion


                #region AddHostXXX()

                var shouldCheckAddHostOverload = definitionType.GetCustomAttributes(typeof(ExpectAddHostExtensionMethod)).Any();

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
                    hasAddHostDefinitionMethod != true || hasAddHostDefinitionWithCallbackMethod != true)
                    hasAllAddMethods = false;
            }

            Assert.IsTrue(hasAllAddMethods);
        }

        #endregion

        #region utils


        static IEnumerable<MethodInfo> GetExtensionMethods(Assembly assembly, Type extendedType)
        {
            var query = from type in assembly.GetTypes()
                        where type.IsSealed && !type.IsGenericType && !type.IsNested
                        from method in type.GetMethods(BindingFlags.Static
                            | BindingFlags.Public | BindingFlags.NonPublic)
                        where method.IsDefined(typeof(ExtensionAttribute), false)
                        where method.GetParameters()[0].ParameterType == extendedType
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
                    var methodInfos = GetExtensionMethods(asm, typeof(ModelNode));
                    foreach (var method in methodInfos)
                        if (!methods.Contains(method))
                            methods.Add(method);
                }
            }

            return methods;
        }

        [TestMethod]
        [TestCategory("Regression.Definitions")]
        public void DefinitionsShouldHaveWithXXX_DefinitionSyntax()
        {
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
    }
}
