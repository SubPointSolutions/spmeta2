using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using SPMeta2.Definitions;
using SPMeta2.Extensions;
using SPMeta2.Models;
using SPMeta2.Services;
using SPMeta2.Services.Impl;
using SPMeta2.Utils;

namespace SPMeta2.Syntax.Default
{
    /// <summary>
    /// Initial entry to start model build up process.
    /// </summary>
    public static class SPMeta2Model
    {
        #region static

        static SPMeta2Model()
        {
            RegisterKnownType(ReflectionUtils.GetTypesFromAssemblies<DefinitionBase>(new[] { typeof(FieldDefinition).Assembly }));
            RegisterKnownType(ReflectionUtils.GetTypesFromAssemblies<ModelNode>(new[] { typeof(FieldDefinition).Assembly }));
        }

        #endregion

        #region farm

        /// <summary>
        /// Creates a new instance of the ModelNode adding "empty model".
        /// Model is not going to be pushes by SPMeta2 API, it just required to be there for model tree processing.
        /// </summary>
        /// <returns></returns>
        public static FarmModelNode NewFarmModel()
        {
            return NewFarmModel((FarmDefinition)null);
        }

        /// <summary>
        /// Creates a new instance of the ModelNode adding "empty model".
        /// Model is not going to be pushes by SPMeta2 API, it just required to be there for model tree processing.
        /// Use action to get access to the "model node" and construct model tree.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static FarmModelNode NewFarmModel(Action<FarmModelNode> action)
        {
            var node = NewFarmModel(new FarmDefinition(), action);

            node.Options.RequireSelfProcessing = false;

            return node;
        }


        /// <summary>
        /// Creates a new instance of the ModelNode adding model provided.
        /// If RequireSelfProcessing set as 'true', then model is going to be processed and pushed by SPMeta2 API.
        /// </summary>
        /// <param name="farmDefinition"></param>
        /// <returns></returns>
        public static FarmModelNode NewFarmModel(FarmDefinition farmDefinition)
        {
            return NewFarmModel(farmDefinition, null);
        }

        /// <summary>
        /// Creates a new instance of the ModelNode adding model provided.
        /// If RequireSelfProcessing set as 'true', then site model is going to be processed and pushed by SPMeta2 API.
        /// Use action to get access to the "model node" and construct model tree.
        /// </summary>
        /// <param name="farmDefinition"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static FarmModelNode NewFarmModel(FarmDefinition farmDefinition, Action<FarmModelNode> action)
        {
            return NewModelNode<FarmDefinition, FarmModelNode>(farmDefinition, action);

        }

        #endregion

        #region web applications


        /// <summary>
        /// Creates a new instance of the ModelNode adding "empty model".
        /// Model is not going to be pushes by SPMeta2 API, it just required to be there for model tree processing.
        /// </summary>
        /// <returns></returns>
        public static WebApplicationModelNode NewWebApplicationModel()
        {
            return NewWebApplicationModel((WebApplicationDefinition)null);
        }

        /// <summary>
        /// Creates a new instance of the ModelNode adding "empty model".
        /// Model is not going to be pushes by SPMeta2 API, it just required to be there for model tree processing.
        /// Use action to get access to the "model node" and construct model tree.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static WebApplicationModelNode NewWebApplicationModel(Action<WebApplicationModelNode> action)
        {
            var node = NewWebApplicationModel(new WebApplicationDefinition
            {
                //RequireSelfProcessing = false
            }, action);

            node.Options.RequireSelfProcessing = false;

            return node;
        }

        /// <summary>
        /// Creates a new instance of the ModelNode adding model provided.
        /// If RequireSelfProcessing set as 'true', then model is going to be processed and pushed by SPMeta2 API.
        /// </summary>
        /// <param name="webApplicationDefinition"></param>
        /// <returns></returns>
        public static WebApplicationModelNode NewWebApplicationModel(WebApplicationDefinition webApplicationDefinition)
        {
            return NewWebApplicationModel(webApplicationDefinition, null);
        }

        /// <summary>
        /// Creates a new instance of the ModelNode adding model provided.
        /// If RequireSelfProcessing set as 'true', then site model is going to be processed and pushed by SPMeta2 API.
        /// Use action to get access to the "model node" and construct model tree.
        /// </summary>
        /// <param name="webApplicationDefinition"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static WebApplicationModelNode NewWebApplicationModel(WebApplicationDefinition webApplicationDefinition, Action<WebApplicationModelNode> action)
        {
            return NewModelNode<WebApplicationDefinition, WebApplicationModelNode>(webApplicationDefinition, action);

        }

        #endregion

        #region sites



        /// <summary>
        /// Creates a new instance of the ModelNode adding "empty site model".
        /// Site model is not going to be pushes by SPMeta2 API, it just required to be there for model tree processing.
        /// </summary>
        /// <returns></returns>
        public static SiteModelNode NewSiteModel()
        {
            return NewSiteModel((SiteDefinition)null);
        }

        /// <summary>
        /// Creates a new instance of the ModelNode adding "empty site model".
        /// Site model is not going to be pushes by SPMeta2 API, it just required to be there for model tree processing.
        /// Use action to get access to the "site model node" and construct model tree.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static SiteModelNode NewSiteModel(Action<SiteModelNode> action)
        {
            var node = NewSiteModel(new SiteDefinition
            {
                //RequireSelfProcessing = false
            }, action);

            node.Options.RequireSelfProcessing = false;

            return node;
        }

        /// <summary>
        /// Creates a new instance of the ModelNode adding site model provided.
        /// If RequireSelfProcessing set as 'true', then site model is going to be processed and pushed by SPMeta2 API.
        /// </summary>
        /// <param name="siteDefinition"></param>
        /// <returns></returns>
        public static SiteModelNode NewSiteModel(SiteDefinition siteDefinition)
        {
            return NewSiteModel(siteDefinition, null);
        }

        /// <summary>
        /// Creates a new instance of the ModelNode adding site model provided.
        /// If RequireSelfProcessing set as 'true', then site model is going to be processed and pushed by SPMeta2 API.
        /// Use action to get access to the "site model node" and construct model tree.
        /// </summary>
        /// <param name="siteDefinition"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static SiteModelNode NewSiteModel(SiteDefinition siteDefinition, Action<SiteModelNode> action)
        {
            return NewModelNode<SiteDefinition, SiteModelNode>(siteDefinition, action);
        }

        #endregion

        #region webs

        /// <summary>
        /// Creates a new instance of the ModelNode adding "empty web model".
        /// Web model is not going to be pushes by SPMeta2 API, it just required to be there for model tree processing.
        /// </summary>
        /// <returns></returns>
        public static WebModelNode NewWebModel()
        {
            return NewWebModel((WebDefinition)null);
        }

        /// <summary>
        /// Creates a new instance of the ModelNode adding web model provided.
        /// If RequireSelfProcessing set as 'true', then web model is going to be processed and pushed by SPMeta2 API.
        /// </summary>
        /// <param name="webDefinition"></param>
        /// <returns></returns>
        public static WebModelNode NewWebModel(WebDefinition webDefinition)
        {
            return NewWebModel(webDefinition, null);
        }

        /// <summary>
        /// Creates a new instance of the ModelNode adding "empty web model".
        /// Web model is not going to be pushes by SPMeta2 API, it just required to be there for model tree processing.
        /// Use action to get access to the "site model node" and construct model tree.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static WebModelNode NewWebModel(Action<WebModelNode> action)
        {
            var node = NewWebModel(new WebDefinition
            {
                //RequireSelfProcessing = false
            }, action);

            node.Options.RequireSelfProcessing = false;

            return node;
        }

        /// <summary>
        /// Creates a new instance of the ModelNode adding web model provided.
        /// If RequireSelfProcessing set as 'true', then web model is going to be processed and pushed by SPMeta2 API.
        /// Use action to get access to the "web model node" and construct model tree.
        /// </summary>
        /// <param name="webDefinition"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static WebModelNode NewWebModel(WebDefinition webDefinition, Action<WebModelNode> action)
        {
            return NewModelNode<WebDefinition, WebModelNode>(webDefinition, action);
        }

        #endregion

        #region webs

        /// <summary>
        /// Creates a new instance of the ModelNode adding "empty list model".
        /// List model is not going to be pushes by SPMeta2 API, it just required to be there for model tree processing.
        /// </summary>
        /// <returns></returns>
        public static ListModelNode NewListModel()
        {
            return NewListModel((ListDefinition)null);
        }

        /// <summary>
        /// Creates a new instance of the ModelNode adding web model provided.
        /// If RequireSelfProcessing set as 'true', then web model is going to be processed and pushed by SPMeta2 API.
        /// </summary>
        /// <param name="listDefinition"></param>
        /// <returns></returns>
        public static ListModelNode NewListModel(ListDefinition listDefinition)
        {
            return NewListModel(listDefinition, null);
        }

        /// <summary>
        /// Creates a new instance of the ModelNode adding "empty list model".
        /// Web model is not going to be pushes by SPMeta2 API, it just required to be there for model tree processing.
        /// Use action to get access to the "list model node" and construct model tree.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ListModelNode NewListModel(Action<ListModelNode> action)
        {
            var node = NewListModel(new ListDefinition
            {
                //RequireSelfProcessing = false
            }, action);

            node.Options.RequireSelfProcessing = false;

            return node;
        }

        /// <summary>
        /// Creates a new instance of the ModelNode adding list model provided.
        /// If RequireSelfProcessing set as 'true', then list model is going to be processed and pushed by SPMeta2 API.
        /// Use action to get access to the "list model node" and construct model tree.
        /// </summary>
        /// <param name="listDefinition"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ListModelNode NewListModel(ListDefinition listDefinition, Action<ListModelNode> action)
        {
            return NewModelNode<ListDefinition, ListModelNode>(listDefinition, action);
        }

        #endregion

        #region utils

        private static TNodeType NewModelNode<TModelDefinition, TNodeType>(TModelDefinition model, Action<TNodeType> action)
            where TModelDefinition : DefinitionBase, new()
            where TNodeType : TypedModelNode, new()
        {
            var newModelNode = new TNodeType
            {
                Value = model ?? new TModelDefinition
                    {
                        //RequireSelfProcessing = false
                    }
            };

            if (model == null)
            {
                newModelNode.Options.RequireSelfProcessing = false;
            }
            // levacy
            // newModelNode.Options.RequireSelfProcessing = newModelNode.Value.RequireSelfProcessing;

            if (action != null)
                action(newModelNode);

            return newModelNode;
        }

        #endregion

        #region serialization

        [Obsolete("Use RegisterKnownType instead")]
        public static void RegisterKnownDefinition(Type type)
        {
            RegisterKnownDefinition(new[] { type });
        }

        [Obsolete("Use RegisterKnownType instead")]
        public static void RegisterKnownDefinition(Assembly assembly)
        {
            RegisterKnownDefinition(new[] { assembly });
        }

        [Obsolete("Use RegisterKnownType instead")]
        public static void RegisterKnownDefinition(IEnumerable<Assembly> assemblies)
        {
            RegisterKnownDefinition(ReflectionUtils.GetTypesFromAssemblies<DefinitionBase>(assemblies));
        }


        [Obsolete("Use RegisterKnownType instead")]
        public static void RegisterKnownDefinition(IEnumerable<Type> types)
        {
            RegisterKnownType(types);
        }

        #region type registration

        public static void RegisterKnownType(Type type)
        {
            RegisterKnownType(new[] { type });
        }

        public static void RegisterKnownType(Assembly assembly)
        {
            RegisterKnownType(new[] { assembly });
        }

        public static void RegisterKnownType(IEnumerable<Assembly> assemblies)
        {
            RegisterKnownType(ReflectionUtils.GetTypesFromAssemblies<DefinitionBase>(assemblies));
        }


        public static void RegisterKnownType(IEnumerable<Type> types)
        {
            foreach (var type in types)
                if (!KnownTypes.Contains(type))
                    KnownTypes.Add(type);
        }

        #endregion


        public static List<Type> KnownTypes = new List<Type>();

        public static ModelNode FromJSON(string jsonString)
        {
            var serializer = ServiceContainer.Instance.GetService<DefaultJSONSerializationService>();
            serializer.RegisterKnownTypes(KnownTypes);

            return serializer.Deserialize(typeof(ModelNode), jsonString) as ModelNode;
        }

        public static string ToJSON(ModelNode model)
        {
            EnureKnownTypes(model);

            var serializer = ServiceContainer.Instance.GetService<DefaultJSONSerializationService>();
            serializer.RegisterKnownTypes(KnownTypes);

            return serializer.Serialize(model);
        }

        public static ModelNode FromXML(string xmlString)
        {
            return FromXML<ModelNode>(xmlString);
        }

        public static TModelNode FromXML<TModelNode>(string xmlString)
            where TModelNode : ModelNode
        {
            // corrrect type convertion for the typed root models
            if (typeof(TModelNode) == typeof(ModelNode))
            {
                var xmlRootDoc = XDocument.Parse(xmlString);

                var rootModelNodeTypeUpperName = xmlRootDoc.Root.Name.LocalName.ToUpper();
                var rootModelNodeType = KnownTypes.FirstOrDefault(t => t.Name.ToUpper() == rootModelNodeTypeUpperName);

                return FromXML(rootModelNodeType, xmlString) as TModelNode;
            }

            return FromXML(typeof(TModelNode), xmlString) as TModelNode;
        }

        public static ModelNode FromXML(Type modelNodetype, string xmlString)
        {
            var serializer = ServiceContainer.Instance.GetService<DefaultXMLSerializationService>();
            serializer.RegisterKnownTypes(KnownTypes);

            return serializer.Deserialize(modelNodetype, xmlString) as ModelNode;
        }


        public static string ToXML(ModelNode model)
        {
            EnureKnownTypes(model);

            var serializer = ServiceContainer.Instance.GetService<DefaultXMLSerializationService>();
            serializer.RegisterKnownTypes(KnownTypes);

            return serializer.Serialize(model);
        }

        private static void EnureKnownTypes(ModelNode model)
        {
            var allModelNodes = model.FindNodes(n => true);

            foreach (var node in allModelNodes)
            {
                RegisterKnownType(node.GetType());
                RegisterKnownType(node.Value.GetType());
            }
        }

        #endregion

        #region compatibilities

        public static ModelProvisionCompatibilityResult CheckProvisionCompatibility(ModelNode model)
        {
            return model.CheckProvisionCompatibility();
        }

        public static bool IsCSOMCompatible(ModelNode model)
        {
            return model.IsCSOMCompatible();
        }

        public static bool IsSSOMCompatible(ModelNode model)
        {
            return model.IsSSOMCompatible();
        }

        #endregion

        #region pretty print

        public static string ToPrettyPrint(ModelNode modelNode)
        {
            return modelNode.ToPrettyPrint();
        }

        #endregion
    }
}
