using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Regression.Tests.Impl.Scenarios;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;

namespace SPMeta2.Regression.Tests.Base
{
    public class ListViewWebPartScenariosTestBase : SPMeta2RegresionScenarioTestBase
    {
        #region utils

        protected Guid ExtractViewId(Models.OnCreatingContext<object, DefinitionBase> context)
        {
            var obj = context.Object;
            var objType = context.Object.GetType();

            if (objType.ToString().Contains("Microsoft.SharePoint.Client.View"))
            {
                return (Guid)obj.GetPropertyValue("Id");
            }
            else if (objType.ToString().Contains("Microsoft.SharePoint.SPView"))
            {
                return (Guid)obj.GetPropertyValue("ID");
            }
            else
            {
                throw new SPMeta2NotImplementedException(string.Format("ID property extraction is not implemented for type: [{0}]", objType));
            }
        }

        protected Guid ExtractListId(Models.OnCreatingContext<object, DefinitionBase> context)
        {
            var obj = context.Object;
            var objType = context.Object.GetType();

            if (objType.ToString().Contains("Microsoft.SharePoint.Client.List"))
            {
                return (Guid)obj.GetPropertyValue("Id");
            }
            else if (objType.ToString().Contains("Microsoft.SharePoint.SPList"))
            {
                return (Guid)obj.GetPropertyValue("ID");
            }
            else
            {
                throw new SPMeta2NotImplementedException(string.Format("ID property extraction is not implemented for type: [{0}]", objType));
            }
        }

        #endregion
    }
}
