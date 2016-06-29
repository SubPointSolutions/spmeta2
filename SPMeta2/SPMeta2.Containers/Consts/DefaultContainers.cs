using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Containers.Consts
{
    public static class DefaultContainers
    {
        public static class WebTemplates
        {
            public static class M2CustomTeamSite
            {
                public static Guid SolutionId = new Guid("{52b034ae-13e4-4147-b8f1-469f2b21f2d7}");
                public static string FilePath = @"Containers\WebTemplates\M2CustomTeamSite.wsp";

                public static string WebTemplateName = "{7DA6B762-9AB9-4D81-870A-E93B586B7534}#M2CustomTeamSite";
            }

            public static class M2CustomWebAsTemplate
            {
                public static Guid SolutionId = new Guid("{0b56e6f8-9d35-4eda-b095-ad55b5f3b2ec}");
                public static string FilePath = @"Containers\WebTemplates\M2CustomWebAsTemplate.wsp";

                public static string WebTemplateName = "M2CustomWebAsTemplate";
            }
        }

        public static class Sandbox
        {
            public static string FilePath = @"Containers\Sandbox\SPMeta2.Containers.SandboxSolutionContainer.wsp";
            public static Guid SolutionId = new Guid("e9a61998-07f2-45e9-ae43-9e93fa6b11bb");

            public static Guid SiteFeatureId = new Guid("32dc6bed-0298-4fca-a72f-e9b9c0d6f09a");
            public static Guid WebFeatureId = new Guid("b997a462-8efb-44cf-92c0-457e75c81798");
            public static Guid WebPartFeatureId = new Guid("9acf0f59-3cdc-412b-a647-4185dd4cd9bc");
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
