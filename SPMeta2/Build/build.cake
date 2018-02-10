// load up common tools
#load tools/SubPointSolutions.CakeBuildTools/scripts/SubPointSolutions.CakeBuild.Core.cake

// clear default doc merge task
defaultActionDocsMerge.Task.Actions.Clear();

// replace default buil
defaultActionBuild.Task.Actions.Clear();
defaultActionBuild
    .Does(() => 
{
    if(jsonConfig["customProjectBuildProfiles"] == null)
    {
        Verbose("No custom project profiles detected. Switching to normal *.sln build");
        return;
    }

    var customProjectBuildProfiles = jsonConfig["customProjectBuildProfiles"];

    var currentBuildProfileIndex = 0;
    var buildProfilesCount = customProjectBuildProfiles.Count();

    foreach(var buildProfile in customProjectBuildProfiles) {

        currentBuildProfileIndex++;
        var profileName = (string)buildProfile["ProfileName"];

        Information(string.Format("[{0}/{1}] Building project profile:[{2}]",
            new object[] {
                currentBuildProfileIndex,
                buildProfilesCount,
                profileName
            }));

        var projectFiles = buildProfile["ProjectFiles"].Select(p => (string)p);
        var buildParameters = buildProfile["BuildParameters"].Select(p => (string)p);

        var currentProjectFileIndex = 0;
        var projecFilesCount = projectFiles.Count();

        foreach(var projectFile in projectFiles)
        {
            currentProjectFileIndex++;
            var fullProjectFilePath = ResolveFullPathFromSolutionRelativePath(projectFile);

            Information(string.Format(" [{0}/{1}] Building project file:[{2}]",
                new object[] {
                    currentProjectFileIndex,
                    projecFilesCount,
                    projectFile
            }));

            Verbose(string.Format(" - file path:[{0}]", fullProjectFilePath)); 
            
            var buildSettings =  new MSBuildSettings{

            };

            var buildParametersString = String.Empty;
            var solutionDirectoryParam = "/p:SolutionDir=" + "\"" + defaultSolutionDirectory + "\"";

            buildParametersString += " " + solutionDirectoryParam;
            buildParametersString += " " + String.Join(" ", buildParameters);

            buildSettings.ArgumentCustomization = args => {
                
                foreach(var arg in buildParameters) {
                    args.Append(arg);
                }

                args.Append(solutionDirectoryParam);
                return args;
            };

            Verbose(string.Format(" - params:[{0}]", buildParametersString)); 
            MSBuild(fullProjectFilePath, buildSettings);
        }
    }            
});

Task("Action-API-NuGet-Regression")
    .Does(() => {

        Information("Running Pester testing...");

        Information("- ensuring peter is installed...");
        StartPowershellFile("Pester/_install.ps1", args =>
        {
            // args.Append("Username", "admin")
            //     .AppendSecret("Password", "pass1");
        });

        Information("- running regression...");

        StartPowershellFile("Pester/pester.run.ps1", args =>
        {
            // args.Append("Username", "admin")
            //     .AppendSecret("Password", "pass1");
        });
    });

// add one more for defaultActionCLIZipPackaging
// it is run after Action-API-NuGet-Packaging
// testing that NuGet packages were done in the right way
defaultActionCLIZipPackaging
    .IsDependentOn("Action-API-NuGet-Regression");

// default targets
RunTarget(target);