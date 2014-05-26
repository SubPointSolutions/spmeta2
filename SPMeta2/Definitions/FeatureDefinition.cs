using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Definitions
{
    public enum FeatureDefinitionScope
    {
        Farm,
        WebApplication,
        Site,
        Web
    }

    public class FeatureDefinition : DefinitionBase
    {
        #region properties

        public string Title { get; set; }

        public Guid Id { get; set; }
        public bool ForceActivate { get; set; }
        public bool Enable { get; set; }

        public FeatureDefinitionScope Scope { get; set; }

        #endregion
    }
}
