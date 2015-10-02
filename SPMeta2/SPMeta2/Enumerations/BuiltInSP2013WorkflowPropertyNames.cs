﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Enumerations
{
    public static class BuiltInSP2013WorkflowPropertyNames
    {
        public static string SubscriptionName = "SubscriptionName";
        public static string SubscriptionId = "SubscriptionId";

        public static string StatusColumnCreated = "StatusColumnCreated";
        public static string StatusFieldName = "StatusFieldName";

        public static class SPDConfig
        {
            public static string StartManually = "SPDConfig.StartManually";
            public static string StartOnCreate = "SPDConfig.StartManually";
            public static string StartOnChange = "SPDConfig.StartOnChange";
        }
    }
}
