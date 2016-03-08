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
        public override ModelProvisionCompatibilityResult CheckProvisionCompatibility(ModelNode model)
        {
            var result = new ModelProvisionCompatibilityResult
            {
                Model = model
            };

            var modelNodes = model.Flatten();
            var rootNode = model;

            foreach (var modelNode in modelNodes)
            {
                var def = modelNode.Value;
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

                // temporary fix for SiteDefinition, it cannot be yet provisioned with M2 CSOM
                if (def.GetType() == typeof(SiteDefinition))
                {
                    if (modelNode.Options.RequireSelfProcessing)
                    {
                        // that's farm / web model or an attempt to provision a new site w/ M2
                        defResult.IsCSOMCompatible = false;
                    }
                    else
                    {
                        // SiteModel, all valid
                        defResult.IsCSOMCompatible = true;
                    }
                }

                // fixing up root definitions
                // farm and web app model cannot be provisioned with M2
                if (modelNode == rootNode)
                {
                    if (defType == typeof(FarmDefinition)
                        || defType == typeof(WebApplicationDefinition))
                    {
                        defResult.IsCSOMCompatible = false;
                    }
                }

                result.Result.Add(defResult);
            }

            return result;
        }

        #endregion
    }
}
