using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;

namespace SPMeta2.Regression.SSOM.Extensions
{
    internal static class SPEventReceiverDefinitionExtensions
    {
        public static string GetSynchronization(this SPEventReceiverDefinition def)
        {
            return def.Synchronization.ToString();
        }

        public static string GetEventReceiverType(this SPEventReceiverDefinition def)
        {
            return def.Type.ToString();
        }
    }
}
