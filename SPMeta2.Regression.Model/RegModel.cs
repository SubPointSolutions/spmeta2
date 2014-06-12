using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Models;
using SPMeta2.Regression.Model.Definitions;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Model
{
    public class RegModel
    {
        #region methods

        public ModelNode GetSiteFields()
        {
            return SPMeta2Model
                 .NewSiteModel(site =>
                 {
                     site
                         .AddField(RegFields.TextField);
                 });
        }

        public ModelNode GetListFields()
        {
            return SPMeta2Model
                .NewSiteModel(site =>
                {
                    site
                        .AddField(RegFields.TextField);
                });
        }

        #endregion
    }
}
