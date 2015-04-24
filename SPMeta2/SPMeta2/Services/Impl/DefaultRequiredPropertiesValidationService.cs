using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;


namespace SPMeta2.Services.Impl
{
    public class DefaultRequiredPropertiesValidationService : PreDeploymentServiceBase
    {
        #region constructors

        public DefaultRequiredPropertiesValidationService()
        {
            PropValidationModelHandler = new DefaultRequiredPropertiesModelHandler();
        }

        #endregion

        #region properties

        public DefaultRequiredPropertiesModelHandler PropValidationModelHandler { get; set; }
        #endregion

        #region methods

        public override void DeployModel(ModelHosts.ModelHostBase modelHost, Models.ModelNode model)
        {
            var defaultTraserveService = ServiceContainer.Instance.GetService<ModelTreeTraverseServiceBase>();

            defaultTraserveService.OnModelHandlerResolve += ResolveModelHandlerForNode;

            defaultTraserveService.Traverse(modelHost, model);
        }

        protected ModelHandlerBase ResolveModelHandlerForNode(ModelNode modelNode)
        {
            return PropValidationModelHandler;
        }

        #endregion
    }

    public class DefaultRequiredPropertiesModelHandler : ModelHandlerBase
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var props = model.GetType()
                             .GetProperties()
                             .Where(p => p.GetCustomAttributes(typeof(ExpectRequired), true).Any());

            var requiredPropsGroups = props
                             .GroupBy(g => (g.GetCustomAttributes(typeof(ExpectRequired), true).First() as ExpectRequired).GroupName);

            foreach (var group in requiredPropsGroups)
            {
                // all set is requred
                if (string.IsNullOrEmpty(group.Key))
                {
                    var isAllValid = AllOfThem(model, group.ToList());

                    if (!isAllValid)
                        throw new Exception("Not all of them");
                }
                else
                {
                    // skip 'Web part content' for typed web part definitions
                    //  a big todo

                    if (group.Key == "Web part content" && (model.GetType() != typeof(WebPartDefinition)))
                        continue;

                    var oneOfThem = OneOfThem(model, group.ToList());

                    if (!oneOfThem)
                        throw new Exception("Not one of them");
                }
            }
        }

        protected class PropResult
        {
            public string Name { get; set; }
            public bool IsValid { get; set; }
        }

        private bool OneOfThem(object obj, List<PropertyInfo> props)
        {
            var result = ValidateProps(obj, props);

            return result.Any(p => p.IsValid);
        }

        private bool AllOfThem(object obj, List<PropertyInfo> props)
        {
            var result = ValidateProps(obj, props);

            if (!result.All(p => p.IsValid))
            {
                throw new Exception(string.Format("Property [{0}] is not valid.",
                    result.First(r => !r.IsValid).Name));
            }

            return result.All(p => p.IsValid);
        }

        protected List<PropResult> ValidateProps(object obj, List<PropertyInfo> props)
        {
            var results = new List<PropResult>();

            foreach (var prop in props)
            {
                var result = new PropResult();

                result.Name = prop.Name;
                result.IsValid = true;

                if (prop.PropertyType == typeof(string))
                {
                    var value = prop.GetValue(obj, null) as string;

                    if (!string.IsNullOrEmpty(value))
                    { }
                    else
                    {
                        result.IsValid = false;
                    }
                }
                else if (prop.PropertyType == typeof(Guid) ||
                   prop.PropertyType == typeof(Guid?))
                {
                    var value = prop.GetValue(obj, null) as Guid?;

                    if (value.HasValue && value.Value != default(Guid))
                    { }
                    else
                    {
                        result.IsValid = false;
                    }
                }
                else if (prop.PropertyType == typeof(int) ||
                   prop.PropertyType == typeof(int?))
                {
                    var value = prop.GetValue(obj, null) as int?;

                    if (value.HasValue && value.Value > 0)
                    { }
                    else
                    {
                        result.IsValid = false;
                    }
                }
                else if (prop.PropertyType == typeof(byte[]))
                {
                    var value = prop.GetValue(obj, null) as byte[];

                    if (value != null && value.Count() > 0)
                    { }
                    else
                    {
                        result.IsValid = false;
                    }
                }
                else if (prop.PropertyType == typeof(object))
                {
                    var value = prop.GetValue(obj, null) as object;

                    if (value != null)
                    { }
                    else
                    {
                        result.IsValid = false;
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }

                results.Add(result);
            }

            return results;
        }

        public override Type TargetType
        {
            get { return typeof(DefinitionBase); }
        }
    }
}
