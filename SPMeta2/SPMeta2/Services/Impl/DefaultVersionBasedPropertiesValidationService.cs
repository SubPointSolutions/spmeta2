using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SPMeta2.Exceptions;
using SPMeta2.Services.ServiceModelHandlers;


namespace SPMeta2.Services.Impl
{
    public class DefaultVersionBasedPropertiesValidationService :
        DefaultPreDeploymentTreeTraverseServiceBase<DefaultVersionBasedPropertiesModelHandler>
    {

    }
}
