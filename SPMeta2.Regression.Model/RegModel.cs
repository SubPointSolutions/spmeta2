using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Regression.Model.Definitions;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;

namespace SPMeta2.Regression.Model
{
    public class RegModel
    {
        #region methods

        public ModelNode GetSiteFieldsModel()
        {
            return SPMeta2Model
                 .NewSiteModel(site =>
                     {
                         var defs = ReflectionUtils
                             .GetStaticFieldValues(typeof(RegSiteFields))
                             .Where(f => f != null && f.GetType() == typeof(FieldDefinition))
                             .Cast<FieldDefinition>();

                         foreach (var def in defs)
                         {
                             site.AddField(def);
                         }
                     });
        }

        public ModelNode GetListFieldsModel()
        {
            // TODO, webs/lists

            throw new NotImplementedException();

            return SPMeta2Model
                .NewSiteModel(site =>
                {
                    site
                        .AddField(RegSiteFields.TextField);
                });
        }

        public ModelNode GetRootLevelWebsModel()
        {
            return SPMeta2Model
                .NewWebModel(site =>
                {
                    var defs = ReflectionUtils
                              .GetStaticFieldValues(typeof(RegWebs))
                              .Where(f => f != null && f.GetType() == typeof(WebDefinition))
                              .Cast<WebDefinition>();

                    foreach (var def in defs)
                    {
                        site.AddWeb(def);
                    }
                });

        }

        #endregion
    }
}
