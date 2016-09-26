using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;


namespace SubPointSolutions.Docs.Code.Metadata
{
   

   

   

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class SampleMetadataTagAttribute : Attribute
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
