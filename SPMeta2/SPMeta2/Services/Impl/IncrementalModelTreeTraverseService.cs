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
    public class IncrementalModelTreeTraverseService : DefaultModelTreeTraverseService
    {
        #region constuctors

        public IncrementalModelTreeTraverseService()
        {
            HashService = new MD5HashCodeServiceBase();

            CurrentModelHash = new ModelHash();
        }

        #endregion

        #region properties

        public ModelHash PreviousModelHash { get; set; }
        public ModelHash CurrentModelHash { get; set; }

        public HashCodeServiceBase HashService { get; set; }

        #endregion

        #region methods

        protected virtual string GetDefinitionIdentityKey(DefinitionBase definition)
        {
            var definitionType = definition.GetType();

            var isInstanceIdentity = definitionType.GetCustomAttributes(typeof(SingletonIdentityAttribute), true).Any();

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
                var keyValue = ConvertUtils.ToString(keyProp.GetValue(definition));

                identityKeyValues.Add(keyName, keyValue);
            }

            var resultIdentityKey = string.Join(";",
                identityKeyValues.Select(v => string.Format("{0}:{1}", v.Key, v.Value)));

            return resultIdentityKey;
        }

        protected virtual string GetDefinitionFullPath()
        {
            var currentDefinitions = CurrentModelPath.Select(p => p.Value);
            var currentDefinitionPath = string.Join("/", currentDefinitions.Select(p => GetDefinitionIdentityKey(p)));

            return currentDefinitionPath;
        }

        protected override void OnBeforeDeployModel(object modelHost, ModelNode modelNode)
        {
            // lookup node and definition in model state
            // mark as modelNode.Options.RequireSelfProcessing true/false based on state

            var currentModelNode = modelNode;
            var currentDefinition = modelNode.Value;

            var prevModelHash = GetPreviousModelHash();

            TraceService.Information(0, "Calculating hashes for node and definition");
            TraceService.InformationFormat(0, "    -node:[{0}]", modelNode);
            TraceService.InformationFormat(0, "    -definition:[{0}]", modelNode.Value);

            //var currentNodeHashHash = HashService.GetHashCode(currentModelNode);
            var currentDefinitionHash = HashService.GetHashCode(currentDefinition);

            var currentDefinitionIdentityKey = GetDefinitionIdentityKey(currentDefinition);
            var currentDefinitionIdentityHash = HashService.GetHashCode(currentDefinitionIdentityKey);

            var currentDefinitionFullPath = GetDefinitionFullPath();
            var currentDefinitionFullPathHash = HashService.GetHashCode(currentDefinitionFullPath);

            //TraceService.InformationFormat(0, "    -node hash:[{0}]", currentNodeHashHash);
            TraceService.InformationFormat(0, "    -definition hash:[{0}]", currentDefinitionHash);
            TraceService.InformationFormat(0, "    -definition full path:[{0}]", currentDefinitionFullPath);
            TraceService.InformationFormat(0, "    -definition full path hash:[{0}]", currentDefinitionFullPathHash);

            var prevModeNodeHash = prevModelHash.ModelNodes.FirstOrDefault(h => h.DefinitionFullPathHash == currentDefinitionFullPathHash);

            if (prevModeNodeHash != null)
            {
                TraceService.InformationFormat(0, "Found previous model node by path hash:[{0}]", currentDefinitionFullPathHash);

                if (prevModeNodeHash.DefinitionHash != currentDefinitionHash)
                {
                    TraceService.Information(0, "Definition hashes don't macth. Setting .Options.RequireSelfProcessing  = true if it's not.");

                    if (modelNode.Options.RequireSelfProcessing)
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

            //var currentNodeHashHash = HashService.GetHashCode(currentModelNode);
            var currentDefinitionHash = HashService.GetHashCode(currentDefinition);

            var currentDefinitionIdentityKey = GetDefinitionIdentityKey(currentDefinition);
            var currentDefinitionIdentityHash = HashService.GetHashCode(currentDefinitionIdentityKey);

            var currentDefinitionFullPath = GetDefinitionFullPath();
            var currentDefinitionFullPathHash = HashService.GetHashCode(currentDefinitionFullPath);


            CurrentModelHash.ModelNodes.Add(new ModelNodeHash
            {
                DefinitionFullPath = currentDefinitionFullPath,
                DefinitionFullPathHash = currentDefinitionFullPathHash,

                DefinitionHash = currentDefinitionHash
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
