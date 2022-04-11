using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;

using SPMeta2.CSOM.Services;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.Utils;
using SPMeta2.CSOM.Services.Impl;

namespace SPMeta2.CSOM.ModelHandlers
{
    public abstract class CSOMModelHandlerBase : ModelHandlerBase
    {
        #region constructors

        public CSOMModelHandlerBase()
        {
            TokenReplacementService = ServiceContainer.Instance.GetService<CSOMTokenReplacementService>();
            LocalizationService = ServiceContainer.Instance.GetService<CSOMLocalizationService>();

            ClientRuntimeQueryService = ServiceContainer.Instance.GetService<ClientRuntimeQueryServiceBase>() ?? new DefaultClientRuntimeQueryService();

            // TODO, move to ServiceContainer
            ContentTypeLookupService = new CSOMContentTypeLookupService();
            FieldLookupService = new CSOMFieldLookupService();
        }

        #endregion

        #region properties

        public CSOMFieldLookupService FieldLookupService { get; set; }
        public CSOMContentTypeLookupService ContentTypeLookupService { get; set; }
        public TokenReplacementServiceBase TokenReplacementService { get; set; }
        public LocalizationServiceBase LocalizationService { get; set; }

        public ClientRuntimeQueryServiceBase ClientRuntimeQueryService { get; set; }

        #endregion

        #region utils

        protected virtual object GetPropertyValue(object obj, string propName)
        {
            return ReflectionUtils.GetPropertyValue(obj, propName);
        }

        protected virtual bool IsSharePointOnlineContext(ClientContext context)
        {
            return ClientRuntimeContextExtensions.IsSharePointOnlineContext(context);
        }

        #endregion

        #region localization

        protected virtual void ProcessGenericLocalization(ClientObject obj, Dictionary<string, List<ValueForUICulture>> localizations)
        {
            var targetProps = localizations.Keys.ToList();
            var isSupportedRuntime = ReflectionUtils.HasProperties(obj, targetProps);

            if (!isSupportedRuntime)
            {
                TraceService.Critical((int)LogEventId.ModelProvisionCoreCall,
                      string.Format("CSOM runtime doesn't have [{0}] methods support. Update CSOM runtime to a new version. Provision is skipped",
                        string.Join(", ", targetProps.ToArray())));

                return;
            }

            var needsUpdate = false;

            foreach (var key in localizations.Keys)
            {
                var propName = key;
                var localization = localizations[key];

                if (localization.Any())
                {
                    var userResource = GetPropertyValue(obj, propName);

                    foreach (var locValue in localization)
                        LocalizationService.ProcessUserResource(obj, userResource, locValue);

                    needsUpdate = true;
                }
            }

            if (needsUpdate)
            {
                var updateMethod = ReflectionUtils.GetMethod(obj, "Update");

                if (updateMethod != null)
                {
                    if (obj is ContentType)
                    {
                        updateMethod.Invoke(obj, new object[] { true });
                    }
                    else if (obj is Field)
                    {
                        updateMethod = ReflectionUtils.GetMethod(obj, "UpdateAndPushChanges");
                        updateMethod.Invoke(obj, new object[] { true });
                    }
                    else
                    {
                        updateMethod.Invoke(obj, null);
                    }

                    obj.Context.ExecuteQueryWithTrace();
                }
                else
                {
                    throw new SPMeta2Exception(String.Format("Can't find Update() methods on client object of type:[{0}]", obj.GetType()));
                }
            }
        }

        #endregion
    }
}
