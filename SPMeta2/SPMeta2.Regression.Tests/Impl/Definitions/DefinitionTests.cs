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
            // find all static extension methods

            //AddList(this ModelNode model, ListDefinition definition)

            var methods = GetModelNodeExtensionMethods(new[]
            {
                typeof(DefinitionBase).Assembly,
                typeof(TaxonomyFieldDefinition).Assembly,
                typeof(ListDefinitionSyntax).Assembly
            });

            foreach (var definitionType in DefinitionTypes)
            {
                var definitionName = definitionType.Name.Replace("Definition", string.Empty);

                Trace.WriteLine(string.Format("Definition: [{0}]", definitionName));

                // validate AddXXX() methods
                var addMethodName = string.Format("Add{0}", definitionName);

                Trace.WriteLine(string.Format("     MethodName: [{0}]", addMethodName));
                var targetMethods = methods.Where(m => m.Name == addMethodName);


                Trace.WriteLine(string.Format("     Add{0}(this ModelNode model, {0}Definition definition)", definitionName));
                // 1. should be one with "(this ModelNode model, XXXDefinition definition)"
                var normalAddMethod = targetMethods.FirstOrDefault(m => m.GetParameters().Count() == 2);
                Assert.IsNotNull(normalAddMethod);

                Assert.IsTrue(normalAddMethod.ReturnType == typeof(ModelNode));

                var normalAddMethodParams = normalAddMethod.GetParameters();
                Assert.IsTrue(normalAddMethodParams.Count() == 2);
                Assert.IsTrue(normalAddMethodParams[0].ParameterType == typeof(ModelNode));
                Assert.IsTrue(normalAddMethodParams[1].ParameterType == definitionType);

                Trace.WriteLine(string.Format("     Add{0}(this ModelNode model, {0}Definition definition, Action<ModelNode> action))", definitionName));
                // 2. should be one with "(this ModelNode model, XXXDefinition definition, Action<ModelNode> action)"
                var actionAddMethod = targetMethods.FirstOrDefault(m => m.GetParameters().Count() == 3);
                Assert.IsNotNull(actionAddMethod);

                Assert.IsTrue(actionAddMethod.ReturnType == typeof(ModelNode));

                var actionAddMethodParams = actionAddMethod.GetParameters();
                Assert.IsTrue(actionAddMethodParams.Count() == 3);
                Assert.IsTrue(actionAddMethodParams[0].ParameterType == typeof(ModelNode));
                Assert.IsTrue(actionAddMethodParams[1].ParameterType == definitionType);
                Assert.IsTrue(actionAddMethodParams[2].ParameterType == typeof(Action<ModelNode>));
            }
        }

        #endregion
    }
}
