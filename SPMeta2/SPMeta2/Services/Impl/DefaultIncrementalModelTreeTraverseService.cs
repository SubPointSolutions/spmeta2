using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Attributes.Identity;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Models;
using SPMeta2.Utils;
using SPMeta2.Syntax.Default;
using SPMeta2.ModelHosts;
using SPMeta2.Extensions;

namespace SPMeta2.Services.Impl
{
    public class DefaultIncrementalModelTreeTraverseService : DefaultModelTreeTraverseService
    {
        #region constuctors

        public DefaultIncrementalModelTreeTraverseService()
        {
            CurrentModelHash = new ModelHash();
            IgnoredModelNodes = new List<ModelNode>();

            DefaultDefinitionFullPathSeparator = "/";
            DefaultDefinitionIdentityKeySeparator = ";";

            DefaultPersistenceModelIdPrefix = "spmeta2.incremental_state";

            EnableCaching = true;
        }

        #endregion

        #region properties

        public bool EnableCaching { get; set; }

        protected bool OriginalRequireSelfProcessingValue { get; set; }

        public IncrementalProvisionConfig Configuration { get; set; }

        public ModelHash PreviousModelHash { get; set; }
        public ModelHash CurrentModelHash { get; private set; }

        private HashCodeServiceBase _hashService;

        protected HashCodeServiceBase HashService
        {
            get
            {
                if (_hashService == null)
                    _hashService = ServiceContainer.Instance.GetService<HashCodeServiceBase>();

                return _hashService;
            }
            set
            {
                _hashService = value;
            }
        }

        protected List<ModelNode> IgnoredModelNodes { get; private set; }

        protected string DefaultDefinitionFullPathSeparator { get; set; }
        protected string DefaultDefinitionIdentityKeySeparator { get; set; }

        public string DefaultPersistenceModelIdPrefix { get; set; }

        #endregion

        #region methods

        protected string GetHashString(object value)
        {
            if (EnableCaching)
            {
                if (!_cache4Object2Hash.ContainsKey(value))
                {
                    var hash = HashService.GetHashCode(value);

                    TraceService.VerboseFormat(0,
                                             "GetHashString() - cold hit, adding value to cache:[{0}] -> [{1}]",
                                             new object[] { value, hash },
                                             null);

                    _cache4Object2Hash.Add(value, hash);
                }
                else
                {
                    var hash = _cache4Object2Hash[value];

                    TraceService.VerboseFormat(0,
                                             "GetHashString() - hot hit, returning value from cache:[{0}] -> [{1}]",
                                             new object[] { value, hash },
                                             null);
                }

                return _cache4Object2Hash[value];
            }
            else
            {
                var hash = HashService.GetHashCode(value);

                TraceService.VerboseFormat(0,
                                         "GetHashString() - hot hit, returning value from cache:[{0}] -> [{1}]",
                                         new object[] { value, hash },
                                         null);

                return hash;
            }
        }

        protected virtual bool IsSingletonIdentityDefinition(DefinitionBase definition)
        {
            var definitionType = definition.GetType();

            if (EnableCaching)
            {
                if (!_cache4SingletonIdentityTypes.ContainsKey(definitionType))
                {
                    var isInstanceIdentity = definitionType.GetCustomAttributes(typeof(SingletonIdentityAttribute), true).Any();

                    TraceService.VerboseFormat(0,
                                             "IsSingletonIdentityDefinition() - cold hit, adding value to cache:[{0}] -> [{1}]",
                                             new object[] { definitionType, isInstanceIdentity },
                                             null);

                    _cache4SingletonIdentityTypes.Add(definitionType, isInstanceIdentity);
                }
                else
                {
                    var isInstanceIdentity = _cache4SingletonIdentityTypes[definitionType];

                    TraceService.VerboseFormat(0,
                                             "IsSingletonIdentityDefinition() - hot hit, returning value from cache:[{0}] -> [{1}]",
                                             new object[] { definitionType, isInstanceIdentity },
                                             null);

                }

                return _cache4SingletonIdentityTypes[definitionType];
            }
            else
            {
                var isInstanceIdentity = definitionType.GetCustomAttributes(typeof(SingletonIdentityAttribute), true).Any();

                TraceService.VerboseFormat(0,
                                         "IsSingletonIdentityDefinition() - no caching. Returning value:[{0}] -> [{1}]",
                                         new object[] { definitionType, isInstanceIdentity },
                                         null);

                return isInstanceIdentity;
            }
        }

        protected virtual void ClearCaches()
        {
            // don't need to clear that one
            // can reuse Type->Boolean mapping
            // _cache4SingletonIdentityTypes.Clear();

            // clear identity keys for definitions
            _cache4DefinitionIdentityKey.Clear();

            // clear actual object cache - Object->Hash
            _cache4Object2Hash.Clear();
        }

        protected Dictionary<DefinitionBase, string> _cache4DefinitionIdentityKey = new Dictionary<DefinitionBase, string>();
        protected Dictionary<Type, bool> _cache4SingletonIdentityTypes = new Dictionary<Type, bool>();
        protected Dictionary<object, string> _cache4Object2Hash = new Dictionary<object, string>();

        protected virtual string GetDefinitionIdentityKey(DefinitionBase definitionValue)
        {
            Func<DefinitionBase, string> calculateDefinitionIdentityKey = (definition) =>
            {
                var definitionType = definition.GetType();

                var isInstanceIdentity = IsSingletonIdentityDefinition(definition);

                if (isInstanceIdentity)
                {
                    throw new SPMeta2Exception(string.Format(
                        "definitions of type:[{0}] don't support incremental updates yet",
                        definitionType));
                }

                var keyProperties = definitionType.GetProperties()
                                                  .Where(p => p.GetCustomAttributes(typeof(IdentityKeyAttribute), true).Any())
                                                  .OrderBy(p => p.Name);

                var identityKeyValues = new Dictionary<string, string>();

                foreach (var keyProp in keyProperties)
                {
                    var keyName = keyProp.Name;
                    var keyValue = ConvertUtils.ToString(ReflectionUtils.GetPropertyValue(definition, keyProp.Name));

                    identityKeyValues.Add(keyName, keyValue);
                }

                var resultIdentityKey = string.Join(DefaultDefinitionIdentityKeySeparator,
                    identityKeyValues.Select(v => string.Format("{0}:{1}", v.Key, v.Value)).ToArray());

                return resultIdentityKey;
            };

            if (EnableCaching)
            {
                if (!_cache4DefinitionIdentityKey.ContainsKey(definitionValue))
                {
                    var resultIdentityKey = calculateDefinitionIdentityKey(definitionValue);

                    TraceService.VerboseFormat(0,
                                             "GetDefinitionIdentityKey() - cold hit, adding value to cache:[{0}] -> [{1}]",
                                             new object[] { definitionValue, resultIdentityKey },
                                             null);


                    _cache4DefinitionIdentityKey.Add(definitionValue, resultIdentityKey);
                }
                else
                {
                    var resultIdentityKey = _cache4DefinitionIdentityKey[definitionValue];

                    TraceService.VerboseFormat(0,
                                             "GetDefinitionIdentityKey() - hot hit, returning value from cache:[{0}] -> [{1}]",
                                             new object[] { definitionValue, resultIdentityKey },
                                             null);
                }

                return _cache4DefinitionIdentityKey[definitionValue];
            }
            else
            {
                var resultIdentityKey = calculateDefinitionIdentityKey(definitionValue);

                TraceService.VerboseFormat(0,
                                        "GetDefinitionIdentityKey() - no caching. Returning value:[{0}] -> [{1}]",
                                        new object[] { definitionValue, resultIdentityKey },
                                        null);

                return resultIdentityKey;
            }
        }

        protected virtual string GetDefinitionFullPath(bool asHash)
        {
            var result = string.Empty;

            // get site -> web -> list -> so on
            var currentDefinitions = CurrentModelPath.Reverse().Select(p => p.Value).ToList();

            // always remove the root definition as 
            // 1) they may not have identity key (farm definition)
            // 2) definition path should always be 'root' relative
            currentDefinitions.RemoveAt(0);

            if (asHash)
            {
                result = string.Join(DefaultDefinitionFullPathSeparator, currentDefinitions.Select(p => GetHashString(GetDefinitionIdentityKey(p))).ToArray());
            }
            else
            {
                result = string.Join(DefaultDefinitionFullPathSeparator, currentDefinitions.Select(p => GetDefinitionIdentityKey(p)).ToArray());
                // full path is 'hash(identitykey)/hash(identitykey)/hash(identitykey)'
            }

            return result;
        }

        protected override void OnBeforeDeployModelNode(object modelHost, ModelNode modelNode)
        {
            // temporary measure
            // we need to restoreoriginal value of .RequireSelfProcessing to avoid any model changes
            // set 'ProcessedRequireSelfProcessingValue' in property bag for the further
            OriginalRequireSelfProcessingValue = modelNode.Options.RequireSelfProcessing;

            // lookup node and definition in model state
            // mark as modelNode.Options.RequireSelfProcessing true/false based on state

            var currentModelNode = modelNode;
            var currentDefinition = modelNode.Value;

            var prevModelHash = GetPreviousModelHash();

            var isSingleIdentity = IsSingletonIdentityDefinition(currentDefinition);

            if (isSingleIdentity)
            {
                TraceService.InformationFormat(0,
                                             "Detected singleton definition [{0}]. Incremental update for such definitions isn't supported yet. Skipping.",
                                             currentDefinition);
                return;
            }
            else
            {
                TraceService.InformationFormat(0,
                                               "Calculating hashes for node and definition:[{0}]",
                                               currentDefinition);
            }

            //var currentNodeHashHash = HashService.GetHashCode(currentModelNode);
            var currentDefinitionHash = GetHashString(currentDefinition);

            var currentDefinitionIdentityKey = GetDefinitionIdentityKey(currentDefinition);
            var currentDefinitionIdentityHash = GetHashString(currentDefinitionIdentityKey);

            var currentDefinitionFullPath = GetDefinitionFullPath(false);
            var currentDefinitionFullPathHash = GetDefinitionFullPath(true);

            //TraceService.InformationFormat(0, "    -node hash:[{0}]", currentNodeHashHash);
            TraceService.InformationFormat(0, "    - definition hash:[{0}]", currentDefinitionHash);
            TraceService.InformationFormat(0, "    - definition full path:[{0}]", currentDefinitionFullPath);
            TraceService.InformationFormat(0, "    - definition full path hash:[{0}]", currentDefinitionFullPathHash);

            var prevModeNodeHashes = prevModelHash.ModelNodes
                                                  .Where(h => h.DefinitionFullPathHash == currentDefinitionFullPathHash);

            // same definition is added multiple times 
            // this could be
            // - toggling, such as feature activation toggling
            // - intentional toggling of the field or something
            // - intentional adding definition twice
            // we don't change anyting with the yet preferring to skip incremental provision detection
            if (prevModeNodeHashes.Count() > 1)
            {
                TraceService.InformationFormat(0, "Found more than one previous model node by path hash:[{0}]", currentDefinitionFullPathHash);
                TraceService.Information(0, "Not changing anything, incremental provision can't detect right path to change here.");

                IgnoredModelNodes.Add(currentModelNode);

                return;
            }

            var prevModeNodeHash = prevModeNodeHashes.FirstOrDefault();

            if (prevModeNodeHash != null)
            {
                TraceService.InformationFormat(0, "Found previous model node by path hash:[{0}]", currentDefinitionFullPathHash);

                if (prevModeNodeHash.DefinitionHash != currentDefinitionHash)
                {
                    TraceService.Information(0, "Definition hashes don't macth. Setting .Options.RequireSelfProcessing  = true if it's not.");

                    if (!modelNode.Options.RequireSelfProcessing)
                    {
                        modelNode.Options.RequireSelfProcessing = true;
                    }
                }
                else
                {
                    TraceService.Information(0, "Definition hashes match. Setting .Options.RequireSelfProcessing  = false.");
                    modelNode.Options.RequireSelfProcessing = false;
                }
            }
            else
            {
                TraceService.InformationFormat(0,
                                              "Cannot find previous model hash. Leaving .Options.RequireSelfProcessing as it is:[{0}]",
                                              currentModelNode.Options.RequireSelfProcessing);
            }
        }


        protected override void OnAfterDeployModelNode(object modelHost, ModelNode modelNode)
        {
            var incrementalRequireSelfProcessingValue = modelNode.SetNonPersistentPropertyBagValue(
                                                                DefaultModelNodePropertyBagValue.Sys.IncrementalRequireSelfProcessingValue,
                                                                modelNode.Options.RequireSelfProcessing.ToString());

            // restore model state
            modelNode.Options.RequireSelfProcessing = OriginalRequireSelfProcessingValue;

            var currentModelNode = modelNode;
            var currentDefinition = modelNode.Value;

            var isSingleIdentity = IsSingletonIdentityDefinition(currentDefinition);

            if (isSingleIdentity)
            {
                return;
            }

            //var currentNodeHashHash = HashService.GetHashCode(currentModelNode);
            var currentDefinitionHash = GetHashString(currentDefinition);

            var currentDefinitionIdentityKey = GetDefinitionIdentityKey(currentDefinition);
            var currentDefinitionIdentityHash = GetHashString(currentDefinitionIdentityKey);

            var currentDefinitionFullPath = GetDefinitionFullPath(false);
            var currentDefinitionFullPathHash = GetDefinitionFullPath(true);

            CurrentModelHash.ModelNodes.Add(new ModelNodeHash
            {
                DefinitionHash = currentDefinitionHash,

                DefinitionIdentityKey = currentDefinitionIdentityKey,
                DefinitionIdentityKeyHash = currentDefinitionIdentityHash,

                DefinitionFullPath = currentDefinitionFullPath,
                DefinitionFullPathHash = currentDefinitionFullPathHash
            });
        }

        protected override void OnBeforeDeployModel(object modelHost, ModelNode modelNode)
        {
            base.OnBeforeDeployModel(modelHost, modelNode);

            // clean up current model hash
            CurrentModelHash = new ModelHash();
            ClearCaches();

            TraceService.InformationFormat(0, "Starting incremental provision with EnableCaching = {0}", EnableCaching);

            var storages = ResolvePersistenceStorages(modelHost, modelNode);

            // restore previous one
            if (Configuration != null && storages.Count() > 0)
            {
                TraceService.Information(0, "Model hash restore: found [{0}] storage impl in Configuration.PersistenceStorages. Automatic model hash management is used");

                var modelIdProperty = modelNode.GetPropertyBagValue(DefaultModelNodePropertyBagValue.Sys.IncrementalProvision.PersistenceStorageModelId);

                if (modelIdProperty == null)
                    throw new SPMeta2Exception("IncrementalProvisionModelId is not set. Either clean PersistenceStorages and handle model hash persistence manually or set .PersistenceStorageModelId");

                var modelId = modelIdProperty;
                var objectId = string.Format("{0}.{1}", DefaultPersistenceModelIdPrefix, modelId);

                var serializer = ServiceContainer.Instance.GetService<DefaultXMLSerializationService>();
                serializer.RegisterKnownTypes(new[]
                {
                    typeof(ModelHash),
                    typeof(ModelNodeHash)
                });

                foreach (var storage in storages)
                {
                    TraceService.Information(0, string.Format("Restoring model hash with object id:[{0}] using storage impl [{1}]",
                                                   objectId, storage.GetType()));

                    var data = storage.LoadObject(objectId);

                    if (data != null)
                    {
                        var dataString = Encoding.UTF8.GetString(data);
                        var dataObject = serializer.Deserialize(typeof(ModelHash), dataString) as ModelHash;

                        if (dataObject != null)
                        {
                            PreviousModelHash = dataObject;

                            TraceService.Information(0, string.Format("Restored model hash with object id:[{0}] using storage impl [{1}]",
                                                   objectId, storage.GetType()));
                            break;
                        }
                    }
                    else
                    {
                        TraceService.Information(0, string.Format("Restored model hash with object id:[{0}] using storage impl [{1}]",
                                                  objectId, storage.GetType()));
                    }
                }

                TraceService.Information(0, string.Format("Coudn't restore model hash with object id:[{0}]. Either first provision is user or storage is wrong.", objectId));
            }
            else
            {
                TraceService.Information(0, "Model hash restore: can't find any persistence storage impl in Configuration.PersistenceStorages. Assuming manual model hash management is used");
            }
        }

        protected override void OnAfterDeployModel(object modelHost, ModelNode modelNode)
        {
            base.OnAfterDeployModel(modelHost, modelNode);

            // save model hash to a persistan storages
            var storages = ResolvePersistenceStorages(modelHost, modelNode);

            if (Configuration != null && storages.Count() > 0)
            {
                TraceService.Information(0, "Model hash save: found [{0}] storage impl in Configuration.PersistenceStorages. Automatic model hash management is used");

                var modelIdProperty = modelNode.GetPropertyBagValue(DefaultModelNodePropertyBagValue.Sys.IncrementalProvision.PersistenceStorageModelId);

                if (modelIdProperty == null)
                    throw new SPMeta2Exception(
                        "IncrementalProvisionModelId is not set. Either clean PersistenceStorages and handle model hash persistence manually or set .PersistenceStorageModelId");

                var modelId = modelIdProperty;
                var objectId = string.Format("{0}.{1}", DefaultPersistenceModelIdPrefix, modelId);

                var serializer = ServiceContainer.Instance.GetService<DefaultXMLSerializationService>();
                serializer.RegisterKnownTypes(new[]
                {
                    typeof(ModelHash),
                    typeof(ModelNodeHash)
                });

                var data = Encoding.UTF8.GetBytes(serializer.Serialize(CurrentModelHash));

                foreach (var storage in storages)
                {
                    TraceService.Information(0, string.Format("Saving model hash with object id:[{0}] using storage impl [{1}]. Size:[{2}] bytes",
                                                    objectId, storage.GetType(), data.LongLength));

                    storage.SaveObject(objectId, data);
                }
            }
            else
            {
                TraceService.Information(0, "Model hash save: can't find any persistence storage impl in Configuration... Assuming manual model hash management is used");
            }
        }

        protected virtual ModelHash GetPreviousModelHash()
        {
            if (PreviousModelHash != null)
                return PreviousModelHash;

            // TODO, external look up later
            // load up from provider such as file, SharePoint and so on

            throw new SPMeta2Exception(".CurrentModelHash must be set");
        }

        // AutoDetectSharePointPersistenceStorage

        protected virtual List<PersistenceStorageServiceBase> ResolvePersistenceStorages(object modelHost, ModelNode modelNode)
        {
            if (Configuration == null)
                return new List<PersistenceStorageServiceBase>();

            if (Configuration.AutoDetectSharePointPersistenceStorage)
            {
                var rootModelNodeType = modelNode.Value.GetType();

                TraceService.InformationFormat(0, "Detecting SharePoint persistence storage implementation for root definitin type:[{0}]", rootModelNodeType);

                var currentStorageServices = ServiceContainer.Instance.GetServices<SharePointPersistenceStorageServiceBase>();
                SharePointPersistenceStorageServiceBase targetService = null;

                var typedModelHost = modelHost.WithAssertAndCast<ModelHostBase>("modelHost", v => v.RequireNotNull());

                foreach (var service in currentStorageServices)
                {
                    if (service.TargetDefinitionTypes.Contains(rootModelNodeType))
                    {
                        var serviceTypeName = service.GetType().Name;

                        // cause it could be multiple service registrations here by both CSOM and SSOM
                        // we need to know the context coming from the top and then resolve correct service for the API
                        if (typedModelHost.IsSSOM && serviceTypeName.Contains("SSOM"))
                        {
                            targetService = service;
                            break;
                        }

                        if (typedModelHost.IsCSOM && serviceTypeName.Contains("CSOM"))
                        {
                            targetService = service;
                            break;
                        }
                    }
                }

                if (targetService == null)
                {
                    throw new SPMeta2Exception(
                        string.Format("Coudn't find SharePoint persistence storage implementation for root definitin type:[{0}]",
                        rootModelNodeType));
                }

                TraceService.InformationFormat(0, "Initialilize persistence storage [{0}] with model host", targetService.GetType());
                targetService.InitialiseFromModelHost(modelHost);

                TraceService.Information(0, "Returning persistence storage implementation");

                var result = new List<PersistenceStorageServiceBase>();

                result.Add(targetService);
                return result;
            }

            return Configuration.PersistenceStorages;
        }

        #endregion
    }
}
