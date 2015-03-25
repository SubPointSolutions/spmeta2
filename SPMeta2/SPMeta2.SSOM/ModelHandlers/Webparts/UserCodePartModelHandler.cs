using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.WebPartPages;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using WebPart = System.Web.UI.WebControls.WebParts.WebPart;

namespace SPMeta2.SSOM.ModelHandlers.Webparts
{
    public class UserCodePartModelHandler : WebPartModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(UserCodeWebPartDefinition); }
        }

        #endregion

        #region methods

        protected override void OnBeforeDeployModel(WebpartPageModelHost host, WebPartDefinition webpartModel)
        {
            var typedModel = webpartModel.WithAssertAndCast<UserCodeWebPartDefinition>("webpartModel", value => value.RequireNotNull());
            typedModel.WebpartType = typeof(SPUserCodeWebPart).AssemblyQualifiedName;

        }

        protected override void ProcessWebpartProperties(WebPart webpartInstance, WebPartDefinition webpartModel)
        {
            base.ProcessWebpartProperties(webpartInstance, webpartModel);

            var typedWebpart = webpartInstance.WithAssertAndCast<SPUserCodeWebPart>("webpartInstance", value => value.RequireNotNull());
            var typedModel = webpartModel.WithAssertAndCast<UserCodeWebPartDefinition>("webpartModel", value => value.RequireNotNull());


            // TODO
            typedWebpart.SolutionId = typedModel.SolutionId;
            typedWebpart.AssemblyFullName = typedModel.AssemblyFullName;
            typedWebpart.TypeFullName = typedModel.TypeFullName;

            foreach (var prop in typedModel.UserCodeProperties)
            {
                var currentProperty = typedWebpart.Properties
                    .OfType<SPUserCodeProperty>()
                    .FirstOrDefault(p => p.Name.ToUpper() == prop.Name.ToUpper());

                if (currentProperty == null)
                {
                    currentProperty = new SPUserCodeProperty();
                    currentProperty.Name = prop.Name;

                    typedWebpart.Properties.Add(currentProperty);
                }

                currentProperty.Value = prop.Value;
            }
        }

        #endregion
    }
}
