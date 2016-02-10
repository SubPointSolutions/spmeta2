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

namespace SPMeta2.CSOM.ModelHandlers
{
    public abstract class CSOMModelHandlerBase : ModelHandlerBase
    {
        #region constructors

        public CSOMModelHandlerBase()
        {
            TokenReplacementService = ServiceContainer.Instance.GetService<CSOMTokenReplacementService>();
            LocalizationService = ServiceContainer.Instance.GetService<CSOMLocalizationService>();

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

        #endregion

        #region utils

        protected virtual object GetPropertyValue(object obj, string propName)
        {
            return ReflectionUtils.GetPropertyValue(obj, propName);
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
                      String.Format("CSOM runtime doesn't have [{0}] methods support. Update CSOM runtime to a new version. Provision is skipped",
                        String.Join(", ", targetProps)));

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

        /// <summary>
        /// Method to create property bag for search index properties
        /// http://blogs.msdn.com/b/vesku/archive/2013/10/12/ftc-to-cam-setting-indexed-property-bag-keys-using-csom.aspx
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static string GetEncodedValueForSearchIndexProperty(IEnumerable<string> keys)
        {
            var stringBuilder = new StringBuilder();
            foreach (var current in keys)
            {
                stringBuilder.Append(Convert.ToBase64String(Encoding.Unicode.GetBytes(current)));
                stringBuilder.Append('|');
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Decode the IndexedPropertyKeys value so it's readable
        /// https://lixuan0125.wordpress.com/2014/07/24/make-property-bags-searchable-in-sharepoint-2013/
        /// </summary>
        /// <param name="encodedValue"></param>
        /// <returns></returns>
        public static List<string> GetDecodeValueForSearchIndexProperty(string encodedValue)
        {
            var keys = encodedValue.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            return keys.Select(current => Encoding.Unicode.GetString(Convert.FromBase64String(current))).ToList();
        }

        #endregion
    }
}
