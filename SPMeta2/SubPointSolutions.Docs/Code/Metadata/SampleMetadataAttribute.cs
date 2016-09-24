using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SubPointSolutions.Docs.Code.Metadata
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SampleMetadataAttribute : Attribute
    {
        public string Title { get; set; }
        public string Description { get; set; }

        // TODO
        //public bool GenerateFullProvisionSample { get; set; }
    }
}
