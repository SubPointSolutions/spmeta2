using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Attributes;
using SPMeta2.Definitions;
using SPMeta2.Extensions;
using SPMeta2.Models;

namespace SPMeta2.Services.Impl
{
    public class DefaultModelCompatibilityService : ModelCompatibilityServiceBase
    {
        #region methods

        public override ModelProvisionCompatibilityResult CheckProvisionCompatibility(DefinitionBase definition)
        {
            return CheckProvisionCompatibility(null, definition);
        }

        public override ModelProvisionCompatibilityResult CheckProvisionCompatibility(ModelNode model)
        {
            return CheckProvisionCompatibility(model, null);
        }

        protected ModelProvisionCompatibilityResult CheckProvisionCompatibility(ModelNode model, DefinitionBase definition)
        {
            var result = new ModelProvisionCompatibilityResult
            {
                Model = model
            };

            if (model != null)
            {
                var modelNodes = model.Flatten();
                var rootNode = model;

                foreach (var modelNode in modelNodes)
                {
                    var def = modelNode.Value;
                    CheckDefinition(result, rootNode, modelNode, def);
                }
            }
            else
            {
                CheckDefinition(result, null, null, definition);
            }

            return result;
        }

        private static void CheckDefinition(ModelProvisionCompatibilityResult result,
            ModelNode rootNode,
            ModelNode modelNode,
            DefinitionBase def)
        {
            var defType = def.GetType();

            var defResult = new ModelProvisionCompatibilityResultValue();
            var attrs = (SPObjectTypeAttribute[])defType
                            .GetCustomAttributes(typeof(SPObjectTypeAttribute), true);

            defResult.ModelNode = modelNode;
            defResult.Definition = def;

            if (attrs.Length > 0)
            {
                defResult.IsCSOMCompatible = attrs.Any(a => a.ObjectModelType == SPObjectModelType.CSOM);
                defResult.IsSSOMCompatible = attrs.Any(a => a.ObjectModelType == SPObjectModelType.SSOM);
            }

            // temporary fix for SiteDefinition, it cannot be yet provisioned with SPMeta2 CSOM
            if (def.GetType() == typeof(SiteDefinition))
            {
                if (modelNode != null)
                {
                    if (modelNode.Options.RequireSelfProcessing)
                    {
                        // that's farm / web model or an attempt to provision a new site w/ SPMeta2 
                        defResult.IsCSOMCompatible = false;
                    }
                    else
                    {
                        // SiteModel, all valid
                        defResult.IsCSOMCompatible = true;
                    }
                }
                else
                {
                    // that's definition validation call
                    // but still, we don't support it yet
                    defResult.IsCSOMCompatible = false;
                }
            }

            // fixing up root definitions
            // farm and web app model cannot be provisioned with SPMeta2 CSOM
            // handling call from model nodes & defs
            if ((modelNode == rootNode) || (modelNode == null && rootNode == null))
            {
                if (defType == typeof(FarmDefinition)
                    || defType == typeof(WebApplicationDefinition))
                {
                    defResult.IsCSOMCompatible = false;
                }
            }

            result.Result.Add(defResult);
        }

        #endregion
    }
}
