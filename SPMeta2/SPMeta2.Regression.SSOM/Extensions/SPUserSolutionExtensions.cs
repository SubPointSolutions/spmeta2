using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;

namespace SPMeta2.Regression.SSOM.Extensions
{
    public static class SPUserSolutionExtensions
    {
        public static bool IsActivated(this SPUserSolution solution)
        {
            return solution.Status == SPUserSolutionStatus.Activated;
        }
    }
}
