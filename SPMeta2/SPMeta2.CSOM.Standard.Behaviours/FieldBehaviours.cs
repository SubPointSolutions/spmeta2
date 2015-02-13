using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;

namespace SPMeta2.CSOM.Standard.Behaviours
{
    public static class FieldBehaviours
    {
        #region methods

        public static Field MakeConnectionToTermSet(this Field field, Guid sspId, Guid termSetId)
        {
            var taxonomyField = field.Context.CastTo<TaxonomyField>(field);

            if (taxonomyField != null)
            {
                taxonomyField.SspId = sspId;
                taxonomyField.TermSetId = termSetId;
            }

            return field;
        }

        #endregion
    }
}
