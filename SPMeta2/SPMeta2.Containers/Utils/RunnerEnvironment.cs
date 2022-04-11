using System;
using System.Collections.Generic;
using System.Linq;

namespace SPMeta2.Containers.Utils
{
    public static class RunnerEnvironmentUtils
    {
        public static string GetEnvironmentVariable(string varName)
        {
            return InternalGetEnvironmentVariable(varName);
        }

        public static IEnumerable<string> GetEnvironmentVariables(string varName)
        {
            var varString = InternalGetEnvironmentVariable(varName);

            return string.IsNullOrEmpty(varString) ?
                    new string[0] :
                    varString.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());
        }

        private static string InternalGetEnvironmentVariable(string varName)
        {
            var result = Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.Machine);

            if (!string.IsNullOrEmpty(result))
                return result;

            result = Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.User);

            if (!string.IsNullOrEmpty(result))
                return result;

            Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.Process);

            if (!string.IsNullOrEmpty(result))
                return result;

            result = Environment.GetEnvironmentVariable(varName);

            if (!string.IsNullOrEmpty(result))
                return result;


            return result;
        }
    }
}
