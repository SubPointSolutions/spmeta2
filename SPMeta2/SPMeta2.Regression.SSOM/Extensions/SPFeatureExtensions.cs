using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;

namespace SPMeta2.Regression.SSOM.Extensions
{
  internal   static  class SPFeatureExtensions
    {
      public static Guid GetFeatureId(this SPFeature feature)
      {
          return feature.Definition.Id;
      }
    }
}
