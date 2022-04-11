using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Utils;
using System.Reflection;
using SPMeta2.Exceptions;
using SPMeta2.Services;

namespace SPMeta2.CSOM.ModelHandlers
{

    public class SupportedUICultureModelHandler : CSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(SupportedUICultureDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModel = model.WithAssertAndCast<SupportedUICultureDefinition>("model", value => value.RequireNotNull());
            var typedHost = modelHost.WithAssertAndCast<WebModelHost>("model", value => value.RequireNotNull());

            var web = typedHost.HostWeb;
            var context = web.Context;

            context.Load(web);
            context.Load(web, w => w.SupportedUILanguageIds);

            context.ExecuteQuery();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = web,
                ObjectType = typeof(Web),
                ObjectDefinition = model,
                ModelHost = modelHost
            });

            var shouldUpdate = false;
            var currentLanguages = web.SupportedUILanguageIds;

            if (!currentLanguages.Contains(webModel.LCID))
            {
                // if running nice CSOM, so that method is there and a few web's props
                var supportedRuntime = ReflectionUtils.HasProperty(web, "IsMultilingual")
                                       && ReflectionUtils.HasMethod(web, "AddSupportedUILanguage");


                if (supportedRuntime)
                {
                    // TODO, wrap up into extensions

                    // that's the trick to get all working on CSOM SP2013 SP1+
                    // once props are there, we setup them
                    // if not, giving critical messages in logs

                    // pushing IsMultilingual to true if false
                    var objectData = GetPropertyValue(web, "ObjectData");
                    var objectProperties = GetPropertyValue(objectData, "Properties") as Dictionary<string, object>;

                    var isMultilingual = Convert.ToBoolean(objectProperties["IsMultilingual"]);

                    if (!isMultilingual)
                    {
                        ClientRuntimeQueryService.SetProperty(web, "IsMultilingual", true);
                    }

                    // adding languages 
                    ClientRuntimeQueryService.InvokeMethod(web, "AddSupportedUILanguage", webModel.LCID);

                    // upating the web
                    web.Update();

                    shouldUpdate = true;
                }
                else
                {
                    TraceService.Critical((int)LogEventId.ModelProvisionCoreCall,
                        "CSOM runtime doesn't have Web.IsMultilingual and Web.AddSupportedUILanguage() methods support. Update CSOM runtime to a new version. SupportedUILanguage provision is skipped");
                }
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = web,
                ObjectType = typeof(Web),
                ObjectDefinition = model,
                ModelHost = modelHost
            });

            if (shouldUpdate)
            {
                context.ExecuteQueryWithTrace();
            }
        }



        #endregion


    }
}
