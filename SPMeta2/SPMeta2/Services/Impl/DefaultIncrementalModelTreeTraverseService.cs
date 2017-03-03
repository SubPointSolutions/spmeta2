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

namespace SPMeta2.Services.Impl
{
    public class DefaultIncrementalModelTreeTraverseService : DefaultModelTreeTraverseService
    {
        #region constuctors

        public DefaultIncrementalModelTreeTraverseService()
        {
            _hashService = new MD5HashCodeServiceBase();

            CurrentModelHash = new ModelHash();
            IgnoredModelNodes = new List<ModelNode>();

            DefaultDefinitionFullPathSeparator = "/";
            DefaultDefinitionIdentityKeySeparator = ";";
        }

        #endregion

        #region properties

        public ModelHash PreviousModelHash { get; set; }
        public ModelHash CurrentModelHash { get; private set; }

        protected HashCodeServiceBase _hashService { get; set; }

        protected List<ModelNode> IgnoredModelNodes { get; private set; }

        protected string DefaultDefinitionFullPathSeparator { get; set; }
        protected string DefaultDefinitionIdentityKeySeparator { get; set; }

        #endregion

        #region methods

        protected string GetHashString(object value)
        {
            return _hashService.GetHashCode(value);
        }

        protected virtual bool IsSingletonIdentityDefinition(DefinitionBase definition)
        {
            var definitionType = definition.GetType();
            var isInstanceIdentity = definitionType.GetCustomAttributes(typeof(SingletonIdentityAttribute), true).Any();

            return isInstanceIdentity;
        }

        protected virtual string GetDefinitionIdentityKey(DefinitionBase definition)
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

        protected override void OnBeforeDeployModel(object modelHost, ModelNode modelNode)
        {
            // lookup node and definition in model state
            // mark as modelNode.Options.RequireSelfProcessing true/false based on state

            var currentModelNode = modelNode;
            var currentDefinition = modelNode.Value;

            var prevModelHash = GetPreviousModelHash();

            var isSingleIdentity = IsSingletonIdentityDefinition(currentDefinition);

            if (isSingleIdentity)
            {
                TraceService.Information(0, "Detected singleton definition. Incremental update for such definitions isn't supported yet. Skipping.");
                return;
            }
            else
            {
                TraceService.Information(0, "Calculating hashes for node and definition");
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


        protected override void OnAfterDeployModel(object modelHost, ModelNode modelNode)
        {
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

        protected virtual ModelHash GetPreviousModelHash()
        {
            if (PreviousModelHash != null)
                return PreviousModelHash;

            // TODO, external look up later
            // load up from provider such as file, SharePoint and so on

            throw new SPMeta2Exception(".CurrentModelHash must be set");
        }

        #endregion
    }
}
