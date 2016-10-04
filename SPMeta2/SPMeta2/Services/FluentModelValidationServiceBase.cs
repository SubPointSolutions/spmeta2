using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Services
{
    #region classes

    public class TypedModelValidationResult<TDefinition>
   where TDefinition : DefinitionBase
    {
        public TypedModelValidationResult()
        {
            ValidationContext = new TypedModelValidationContext<TDefinition>();
        }

        public TypedModelValidationContext<TDefinition> ValidationContext { get; set; }
        public string Message { get; set; }
        public bool IsValid { get; set; }
    }

    public class TypedModelValidationContext<TDefinition>
        where TDefinition : DefinitionBase
    {
        public virtual ModelNode Model { get; set; }
        public virtual ModelNode CurrentModelNode { get; set; }
        public virtual TDefinition CurrentDefinition { get; set; }

        public virtual ModelNode ParentModelNode { get; set; }
        public virtual DefinitionBase ParentDefinition { get; set; }

        public virtual IEnumerable<ModelNode> ChildModelNodes { get; set; }
        public virtual IEnumerable<TDefinition> ChildDefinitions { get; set; }

        public virtual IEnumerable<TDef> GetChildDefinitions<TDef>()
              where TDef : DefinitionBase
        {
            if (ChildDefinitions == null)
                return Enumerable.Empty<TDef>();

            return ChildDefinitions.OfType<TDef>();
        }

        public virtual IEnumerable<ModelNode> GetChildModelNodes<TDef>()
              where TDef : DefinitionBase
        {
            if (ChildDefinitions == null)
                return Enumerable.Empty<ModelNode>();

            return ChildModelNodes.Where(m => m.Value is TDef);
        }
    }

    public class ModelValidationResultSet<TDefinition>
           where TDefinition : DefinitionBase
    {

        public ModelValidationResultSet()
        {
            ValidationResults = new List<TypedModelValidationResult<TDefinition>>();
        }
        public List<TypedModelValidationResult<TDefinition>> ValidationResults { get; set; }

        public virtual bool IsValid
        {
            get
            {
                return ValidationResults.All(r => r != null && r.IsValid);
            }
        }

    }

    #endregion

    public abstract class FluentModelValidationServiceBase
    {
        #region methods

        public abstract ModelValidationResultSet<DefinitionBase> Validate(
            ModelNode node,
            Action<TypedModelValidationResult<DefinitionBase>> action);

        public abstract ModelValidationResultSet<TDefinition> Validate<TDefinition>(
            ModelNode node,
            Action<TypedModelValidationResult<TDefinition>> action)
            where TDefinition : DefinitionBase;

        #endregion
    }
}
