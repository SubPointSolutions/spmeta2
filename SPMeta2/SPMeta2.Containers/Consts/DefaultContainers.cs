using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Containers.Consts
{
    public static class DefaultContainers
    {
        public static class Sandbox
        {
            public static string FilePath = @"Containers\Sandbox\SPMeta2.Containers.SandboxSolutionContainer.wsp";
            public static Guid SolutionId = new Guid("e9a61998-07f2-45e9-ae43-9e93fa6b11bb");
        }

        public static class FarmSolution
        {
            public static string FilePath = @"Containers\Farm\SPMeta2.Containers.FarmSolutionContainer.wsp";
            public static Guid SolutionId = new Guid("a4a2146d-57d1-48bd-a02e-ba97b371903d");
        }

        public static class Apps
        {
            public static Guid ProductId = new Guid("{e81b6820-5d57-4d17-a098-5f4317f6c400}");
            public static Guid FeatureId = new Guid("{e81b6820-5d57-4d17-a098-5f4317f6c401}");

            public static string M2ClientWebPart1Name = "M2ClientWebPart1";
            public static string M2ClientWebPart2Name = "M2ClientWebPart2";

            public static string GenericVersionableAppFilePath = @"Containers\Apps\{0}\SPMeta2.Containers.AppContainer.app";

            public static class v0
            {
                public static string FilePath = @"Containers\Apps\1.0.0.0\SPMeta2.Containers.AppContainer.app";
            }

            public static class v1
            {
                public static string FilePath = @"Containers\Apps\1.0.0.1\SPMeta2.Containers.AppContainer.app";
            }

            public static class v2
            {
                public static string FilePath = @"Containers\Apps\1.0.0.2\SPMeta2.Containers.AppContainer.app";
            }

            public static class v3
            {
                public static string FilePath = @"Containers\Apps\1.0.0.3\SPMeta2.Containers.AppContainer.app";
            }
        }
    }
}
