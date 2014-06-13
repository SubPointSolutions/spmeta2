using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Runners.Utils
{
    public static class RunnerEnvironment
    {
        public static string GetEnvironmentVariable(string varName)
        {
            var result = Environment.GetEnvironmentVariable(varName);

            if (string.IsNullOrEmpty(result))
                result = Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.Machine);

            return result;
        }
    }
}
