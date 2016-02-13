using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Exceptions;
using SPMeta2.ModelHosts;
using SPMeta2.Models;

namespace SPMeta2.Services.Impl.Validation
{
    public abstract class PreDeploymentValidationServiceBase : PreDeploymentServiceBase
    {
        #region constructors

        public PreDeploymentValidationServiceBase()
        {
            Exceptions = new List<SPMeta2ModelValidationException>();
            ModelTraverseService.OnException += OnException;
        }

        private void OnException(object sender, ModelTreeTraverseServiceExceptionEventArgs e)
        {
            if (e.Exception is SPMeta2ModelValidationException)
            {
                Exceptions.Add(e.Exception as SPMeta2ModelValidationException);
                e.Handled = true;
            }
            else if (e.Exception is SPMeta2AggregateException)
            {
                Exceptions.AddRange((e.Exception as SPMeta2AggregateException).InnerExceptions
                                            .OfType<SPMeta2ModelValidationException>());
                e.Handled = true;
            }
        }

        #endregion

        #region properties
        public string Title { get; protected set; }
        public string Description { get; protected set; }

        public List<SPMeta2ModelValidationException> Exceptions { get; set; }

        #endregion

        #region methods

        //public override void DeployModel(ModelHostBase modelHost, ModelNode model)
        //{

        //}

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Title))
                return Title;

            return base.ToString();
        }

        #endregion
    }
}
