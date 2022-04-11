#addin nuget:https://www.nuget.org/api/v2/?package=Cake.Powershell&Version=0.2.9

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////


var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");

Task("Docs-Publishing")
    .Does(() =>
{
    var projectName = "SPMeta2";
    var projectDocsFolder = "SPMeta2";

    var environmentVariables = new [] {
        "subpointsolutions-docs-username",
        "subpointsolutions-docs-userpassword",
    };

    foreach(var name in environmentVariables)
    {
        Information(string.Format("HasEnvironmentVariable - [{0}]", name));
        if(!HasEnvironmentVariable(name)) {
            Information(string.Format("Cannot find environment variable:[{0}]", name));
            throw new ArgumentException(string.Format("Cannot find environment variable:[{0}]", name));
        }
    }

     var docsRepoUserName = EnvironmentVariable("subpointsolutions-docs-username");
	 var docsRepoUserPassword = EnvironmentVariable("subpointsolutions-docs-userpassword");

     var docsRepoFolder = string.Format(@"{0}/m2",  "c:/__m2");
     var docsRepoUrl = @"https://github.com/SubPointSolutions/subpointsolutions-docs";
     var docsRepoPushUrl = string.Format(@"https://{0}:{1}@github.com/SubPointSolutions/subpointsolutions-docs", docsRepoUserName, docsRepoUserPassword);

     var srcDocsPath = System.IO.Path.GetFullPath(string.Format(@"./../SubPointSolutions.Docs/Views/{0}", projectDocsFolder));
     var dstDocsPath = string.Format(@"{0}/subpointsolutions-docs/SubPointSolutions.Docs/Views", docsRepoFolder);

     var srcSamplesPath = System.IO.Path.GetFullPath(string.Format(@"./../SubPointSolutions.Docs/Code/Samples/m2Samples.cs"));
     var dstSamplesPath = string.Format(@"{0}/subpointsolutions-docs/SubPointSolutions.Docs/Code/Samples", docsRepoFolder);

     var commitName = string.Format(@"{0} - CI docs update {1}", projectName, DateTime.Now.ToString("yyyyMMdd_HHmmssfff"));

     Information(string.Format("Merging documentation wiht commit:[{0}]", commitName));

     Information(string.Format("Cloning repo [{0}] to [{1}]", docsRepoUrl, docsRepoFolder));

     if(!System.IO.Directory.Exists(docsRepoFolder))
     {   
        System.IO.Directory.CreateDirectory(docsRepoFolder);   

        var cloneCmd = new []{
            string.Format("cd '{0}'", docsRepoFolder),
            string.Format("git clone -b wyam-dev {0}", docsRepoUrl)
        };

        StartPowershellScript(string.Join(Environment.NewLine, cloneCmd));  
     }                            

     docsRepoFolder = docsRepoFolder + "/subpointsolutions-docs"; 

     Information(string.Format("Checkout..."));
     var checkoutCmd = new []{
            string.Format("cd '{0}'", docsRepoFolder),
            string.Format("git checkout wyam-dev"),
            string.Format("git pull")
      };

      StartPowershellScript(string.Join(Environment.NewLine, checkoutCmd));  

      Information(string.Format("Merge and commit..."));
      var mergeCmd = new []{
            string.Format("cd '{0}'", docsRepoFolder),
            string.Format("copy-item  '{0}' '{1}' -Recurse -Force", srcDocsPath,  dstDocsPath),
            string.Format("copy-item  '{0}' '{1}' -Recurse -Force", srcSamplesPath,  dstSamplesPath),
            string.Format("git add *.md"),
            string.Format("git add *.cs"),
            string.Format("git commit -m '{0}'", commitName),
      };

      StartPowershellScript(string.Join(Environment.NewLine, mergeCmd)); 

      Information(string.Format("Push to the main repo..."));
      var pushCmd = new []{
            string.Format("cd '{0}'", docsRepoFolder),
            string.Format("git config http.sslVerify false"),
            string.Format("git push {0}", docsRepoPushUrl)
      };

      var res = StartPowershellScript(string.Join(Environment.NewLine, pushCmd), new PowershellSettings()
      {
            LogOutput = false,
            OutputToAppConsole  = false
      });

      Information(string.Format("Completed docs merge.")); 
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default-Docs")
    .IsDependentOn("Docs-Publishing");

Task("Default-Appveyor")
    .IsDependentOn("Docs-Publishing");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);