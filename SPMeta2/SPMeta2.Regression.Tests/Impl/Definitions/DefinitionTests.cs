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
        public void DefinitionsShouldHaveXXX_DefinitionSyntax()
        {
            var showTrace = false;

            var methods = GetModelNodeExtensionMethods(new[]
            {
                typeof(DefinitionBase).Assembly,
                typeof(TaxonomyFieldDefinition).Assembly,
                typeof(ListDefinitionSyntax).Assembly
            });

            var hasAllXXX_DefinitionSyntax = true;

            foreach (var definitionType in DefinitionTypes)
            {
                var currentMethodHasAllXXX_DefinitionSyntax = true;
                var currentMethodHasAllXXXAction_DefinitionSyntax = true;

                var definitionName = definitionType.Name.Replace("Definition", string.Empty);

                Trace.WriteLine(string.Format("Definition: [{0}]", definitionName));

                // validate AddXXX() methods
                var addMethodName = string.Format("Add{0}", definitionName);

                Trace.WriteLine(string.Format("     MethodName: [{0}]", addMethodName));
                var targetMethods = methods.Where(m => m.Name == addMethodName);


                Trace.WriteLine(string.Format("     Add{0}(this ModelNode model, {0}Definition definition)", definitionName));
                // 1. should be one with "(this ModelNode model, XXXDefinition definition)"
                var normalAddMethod = targetMethods.FirstOrDefault(
                    m =>
                        m.GetParameters().Count() == 2 &&
                        m.GetParameters()[0].ParameterType == typeof(ModelNode) &&
                        m.GetParameters()[1].ParameterType == definitionType);

                if (normalAddMethod == null)
                    currentMethodHasAllXXX_DefinitionSyntax = false;

                if (normalAddMethod != null)
                {
                    if (normalAddMethod.ReturnType != typeof(ModelNode))
                        currentMethodHasAllXXX_DefinitionSyntax = false;

                    var normalAddMethodParams = normalAddMethod.GetParameters();

                    if (normalAddMethodParams.Count() != 2)
                        currentMethodHasAllXXX_DefinitionSyntax = false;

                    if (normalAddMethodParams[0].ParameterType != typeof(ModelNode))
                        currentMethodHasAllXXX_DefinitionSyntax = false;

                    if (normalAddMethodParams[1].ParameterType != definitionType)
                        currentMethodHasAllXXX_DefinitionSyntax = false;
                }

                Trace.WriteLine(string.Format("     Add{0}(this ModelNode model, {0}Definition definition, Action<ModelNode> action))", definitionName));
                // 2. should be one with "(this ModelNode model, XXXDefinition definition, Action<ModelNode> action)"
                var actionAddMethod = targetMethods.FirstOrDefault(
                    m =>
                        m.GetParameters().Count() == 3 &&
                        m.GetParameters()[0].ParameterType == typeof(ModelNode) &&
                        m.GetParameters()[1].ParameterType == definitionType &&
                        m.GetParameters()[2].ParameterType == typeof(Action<ModelNode>));

                if (actionAddMethod == null)
                    currentMethodHasAllXXXAction_DefinitionSyntax = false;

                if (actionAddMethod != null)
                {
                    if (actionAddMethod.ReturnType != typeof(ModelNode))
                        currentMethodHasAllXXXAction_DefinitionSyntax = false;

                    var actionAddMethodParams = actionAddMethod.GetParameters();

                    if (actionAddMethodParams.Count() != 3)
                        currentMethodHasAllXXXAction_DefinitionSyntax = false;

                    if (actionAddMethodParams[0].ParameterType != typeof(ModelNode))
                        currentMethodHasAllXXXAction_DefinitionSyntax = false;

                    if (actionAddMethodParams[1].ParameterType != definitionType)
                        currentMethodHasAllXXXAction_DefinitionSyntax = false;

                    if (actionAddMethodParams[2].ParameterType != typeof(Action<ModelNode>))
                        currentMethodHasAllXXXAction_DefinitionSyntax = false;
                }

                // trace
                if (currentMethodHasAllXXX_DefinitionSyntax != true)
                    Trace.WriteLine(string.Format("    [FALSE] - Add{0}(this ModelNode model, {0}Definition definition))", definitionName));
                else
                {
                    if (showTrace)
                        Trace.WriteLine(string.Format("    [TRUE] - Add{0}(this ModelNode model, {0}Definition definition))", definitionName));
                }

                if (currentMethodHasAllXXXAction_DefinitionSyntax != true)
                    Trace.WriteLine(string.Format("    [FALSE] - Add{0}(this ModelNode model, {0}Definition definition, Action<ModelNode> action))", definitionName));
                else
                {
                    if (showTrace)
                        Trace.WriteLine(string.Format("    [TRUE] - Add{0}(this ModelNode model, {0}Definition definition, Action<ModelNode> action))", definitionName));
                }

                // push back
                if (currentMethodHasAllXXX_DefinitionSyntax != true || currentMethodHasAllXXXAction_DefinitionSyntax != true)
                    hasAllXXX_DefinitionSyntax = false;
            }

            Assert.IsTrue(hasAllXXX_DefinitionSyntax);
        }

        #endregion
    }
}
