using System;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Rnd;

namespace SPMeta2.Regression.Tests.Services
{
    public abstract class ExpectUpdateValueServiceBase
    {
        public ExpectUpdateValueServiceBase()
        {
            RndService = new DefaultRandomService();
        }

        public RandomService RndService { get; set; }

        public abstract Type TargetType { get; set; }

        public abstract object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop);
    }
}
