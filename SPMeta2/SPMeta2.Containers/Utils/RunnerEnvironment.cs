using System;
using System.Collections.Generic;
using System.Linq;

namespace SPMeta2.Containers.Utils
{
    public static class RunnerEnvironmentUtils
    {
        public static string GetEnvironmentVariable(string varName)
        {
            return Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.Machine);
        }

        public static IEnumerable<string> GetEnvironmentVariables(string varName)
        {
            var varString = Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.Machine);

            return string.IsNullOrEmpty(varString) ?
                    new string[0] :
                    varString.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());
        }
    }
}
