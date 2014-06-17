using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Syntax.Extended.Definitions;
using SPMeta2.CSOM.DefaultSyntax;

namespace SPMeta2.Syntax.Extended
{
    public class ModelService
    {
        #region methods

        public ModelNode GetSiteModel()
        {
            var model = SPMeta2Model
                .NewSiteModel(site =>
                {
                    site
                        .AddField(FieldModels.ClientFeedback, field =>
                        {
                            field
                                .OnCreated((FieldDefinition definition, Field type) =>
                                {

                                });
                        });

                    site
                        .AddField(FieldModels.ClientFeedback, field =>
                        {
                            field
                                .OnError<Field>(context =>
                                {

                                });
                        });



                    site
                        .AddField(FieldModels.ClientFeedback, field =>
                        {
                            field.OnError<Field, FieldDefinition>(context =>
                            {

                            });
                        });
                });

            return model;
        }

        #endregion
    }

    
}
