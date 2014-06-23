using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Common
{
    public static class DefaultModelWegh
    {
        #region static

        static DefaultModelWegh()
        {
            Weighs = new List<ModelWeigh>();

            // site
            Weighs.Add(new ModelWeigh(
                typeof(SiteDefinition),
                new[]{
                     typeof(FeatureDefinition),
                     typeof(FieldDefinition),
                     typeof(ContentTypeDefinition),
                     typeof(WebDefinition)
                }));

            // web
            Weighs.Add(new ModelWeigh(
                typeof(WebDefinition),
                new[]{
                     typeof(FeatureDefinition),
                     typeof(ListDefinition)
                }));

            // list
            Weighs.Add(new ModelWeigh(
                typeof(ListDefinition),
                new[]{
                     typeof(ContentTypeLinkDefinition),
                     typeof(FolderDefinition),
                     typeof(ListViewDefinition),
                     typeof(ModuleFileDefinition),
                }));
        }

        #endregion

        #region properties

        public static List<ModelWeigh> Weighs { get; set; }

        #endregion
    }
}
