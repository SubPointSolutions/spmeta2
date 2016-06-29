cls

#region utils

function Get-ScriptDirectory
{
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value;
    if($Invocation.PSScriptRoot)
    {
        $Invocation.PSScriptRoot;
    }
    Elseif($Invocation.MyCommand.Path)
    {
        Split-Path $Invocation.MyCommand.Path
    }
    else
    {
        $Invocation.InvocationName.Substring(0,$Invocation.InvocationName.LastIndexOf("\"));
    }
}


function CheckBaselineConfig($buildProfile) {
    
    $configuration = $buildProfile.Configuration
    $runtime = $buildProfile.Runtime

    if($buildProfile.CheckBaseline -eq $true) {

        if($configuration -eq $null ) {
            throw "configuration is null"
        }

        if($runtime -eq $null) {
            throw "runtime is null"
        }

        return $true
    }

    return $false
}

function BuildProfile($buildProfile) {
    
    $checkBaseline = CheckBaselineConfig $buildProfile

    foreach($projectName in $buildProfile.ProjectNames) {
    
        # build

        $index = $buildProfile.ProjectNames.IndexOf($projectName) + 1
        $count = $buildProfile.ProjectNames.Count

        Write-BVerbose "`t[$index/$count] Profile:[$($buildProfile.Name)] Project name:[$projectName]" -fore Gray
        Write-BVerbose "`tParams: [$($buildProfile.BuildParams)]" -fore Gray
        
        # always set the solution dir
        
        # build.v12.ps1 cannot build the solution on win10 #815
        # https://github.com/SubPointSolutions/spmeta2/issues/815
        
        # MSBuild does not define the SolutionDir property so you'll need to manually specify it:
        #http://stackoverflow.com/questions/13628319/cant-build-our-solution-with-msbuild-it-cant-find-our-targets-files

        $solutionDirectory = [System.IO.Path]::GetFullPath("$solutionRootPath\..\")
        $projectPath = [System.IO.Path]::GetFullPath("$solutionRootPath\$projectName\$projectName.csproj")

        $buildParams = "";        
        
        $configuration = $buildProfile.Configuration
        $runtime = $buildProfile.Runtime

        if($runtime -ne $null) {
            $buildParams += (" /p:spRuntime=$runtime" )
        }

        if($configuration -ne $null) {
            $buildParams += (" /p:Configuration=$configuration" )
        }

        $buildParams += " $($buildProfile.BuildParams)"
        Write-BVerbose "Building with params:[$buildParams]"
        & $msbuild_path "/p:SolutionDir=""$solutionDirectory"" ""$projectPath"" $buildParams " 

		if (! $?) { 
		
            Write-BError "`t[M2 Build] There was an error building profile:[$($buildProfile.Name)]" -fore red
            Write-BError "`t[M2 Build] Expanding params:" -fore Red
                                    
            foreach($key in $buildProfile.Keys) {
                Write-BError "`t$key":[$( $buildProfile[$key])] -fore Red
            }

			throw "`t[M2 Build] !!! Build faild on profile:[$($buildProfile.Name)]. Please check output early to check the details. !!!" 
		}

        if($checkBaseline -eq $true)
        {
            $assemblyFileName = $projectName + ".dll"

            $projectFolder = [system.io.path]::GetDirectoryName($projectPath)
        
            $assemblyDirectory = $projectFolder  + "/bin/$configuration-$runtime/" 

            if($assemblyFileName -eq "SPMeta2.dll" -or $assemblyFileName -eq "SPMeta2.Standard.dll") {
                $assemblyDirectory = $projectFolder  + "/bin/$configuration/" 
            }

            $assemblyPath = $assemblyDirectory  + "/$assemblyFileName" 

            # loading a "tmp" assembly to avoid locking due the next build
            $tmpPath = $env:TEMP 
            $assemblyTemporaryDirectory = [System.IO.Path]::Combine($tmpPath, "m2.build.tmp")

            [System.IO.Directory]::CreateDirectory($assemblyTemporaryDirectory) | out-null;
            $assemblyTmpPath = [System.IO.Path]::Combine($assemblyTemporaryDirectory, [guid]::NewGuid().ToString("N") + ".dll")
            Copy-Item $assemblyPath $assemblyTmpPath -Force -Confirm:$false
            
            $assemblyFileName = $projectName + ".dll"

            # Check-AssemblyBaseline($assemblyFileName, $runtime, $assemblyTmpPath, $baselines) {
            Check-AssemblyBaseline $assemblyFileName $runtime $assemblyTmpPath $g_buildBaseline
        }           
    }
}

#endregion

#region default values / profiles

$currentPath =  Get-ScriptDirectory

cd "$currentPath"

. "$currentPath\_m2.core.ps1"
. "$currentPath\_m2.baseline.ps1"

$solutionRootPath =  "$currentPath\..\"
$msbuild_path = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"

$defaultProjects = @("SPMeta2", "SPMeta2.Standard", "SPMeta2.SSOM", "SPMeta2.SSOM.Standard", "SPMeta2.CSOM", "SPMeta2.CSOM.Standard" )
#$defaultProjects = @( "SPMeta2", "SPMeta2.Standard", "SPMeta2.SSOM")
$o365Projects = @("SPMeta2", "SPMeta2.Standard", "SPMeta2.CSOM", "SPMeta2.CSOM.Standard" )

# https://msdn.microsoft.com/en-us/library/ms164311.aspx
$defaultBuildParams = " /t:Clean,Rebuild /p:Platform=AnyCPU /p:WarningLevel=0 /verbosity:quiet /clp:ErrorsOnly /nologo"

$isAppVeyor = $g_isAppVeyor

if($isAppVeyor -eq $true) {
    $defaultBuildParams += " /logger:""C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll"""
} else {
    $defaultBuildParams += " "
}

#endregion

$buildProfiles =  @()

$build14 = $true
$build15 = $true
$build16 = $true
$build365 = $true

if($build14 -eq $true) {

    $buildProfiles += @{
        "Name"  = "M2 SP2010 - NET35";
        "ProjectNames" = $defaultProjects
        
        "CheckBaseline" = $true;
        "Runtime" = "14";
        "Configuration" = "Debug35";
        
        #"BuildParams" = ("/p:spRuntime=14 /p:Configuration=Debug35 /p:DefineConstants=NET35 " + $defaultBuildParams);
        "BuildParams" = ("/p:DefineConstants=NET35 " + $defaultBuildParams);
    }
}

if($build15 -eq $true) {

    $buildProfiles += @{
        "Name"  = "M2 SP2013 NET40";
        "ProjectNames" = $defaultProjects

        "CheckBaseline" = $true;
        "Runtime" = "15";
        "Configuration" = "Debug40";

        #"BuildParams" = ("/p:spRuntime=15 /p:Configuration=Debug40 /p:DefineConstants=NET40 " + $defaultBuildParams);
        "BuildParams" = ("/p:DefineConstants=NET40 " + $defaultBuildParams);
    }

    $buildProfiles += @{
        "Name"  = "M2 SP2013 NET45";
        "ProjectNames" = $defaultProjects

        "CheckBaseline" = $true;
        "Runtime" = "15";
        "Configuration" = "Debug45";

        #"BuildParams" = ("/p:spRuntime=15 /p:Configuration=Debug45 /p:DefineConstants=NET45 " + $defaultBuildParams);
        "BuildParams" = ("/p:DefineConstants=NET45 " + $defaultBuildParams);
    }
}

if($build16 -eq $true) {

    <#
    $buildProfiles += @{
        "Name"  = "M2 SP2016 NET40";
        "ProjectNames" = $defaultProjects
        "BuildParams" = ("/p:spRuntime=16 /p:Configuration=Debug40 /p:DefineConstants=NET40 " + $defaultBuildParams);
    }
    #>

    $buildProfiles += @{
        "Name"  = "M2 SP2016 NET45";
        "ProjectNames" = $defaultProjects

        "CheckBaseline" = $true;
        "Runtime" = "16";
        "Configuration" = "Debug45";

        #"BuildParams" = ("/p:spRuntime=16 /p:Configuration=Debug45 /p:DefineConstants=NET45 " + $defaultBuildParams);
        "BuildParams" = ("/p:DefineConstants=NET45 " + $defaultBuildParams);
    }
}

if($build365 -eq $true) {

    $buildProfiles += @{
        "Name"  = "M2 O365 NET40";
        "ProjectNames" = $o365Projects; 

        "CheckBaseline" = $true;
        "Runtime" = "365";
        "Configuration" = "Debug40";

        #"BuildParams" = ("/p:spRuntime=365 /p:Configuration=Debug40 /p:DefineConstants=NET40 " + $defaultBuildParams);
        "BuildParams" = ("/p:DefineConstants=NET40 " + $defaultBuildParams);
    }

    $buildProfiles += @{
        "Name"  = "M2 O365 NET45";
        "ProjectNames" = $o365Projects; 

        "CheckBaseline" = $true;
        "Runtime" = "365";
        "Configuration" = "Debug45";

        #"BuildParams" = ("/p:spRuntime=365 /p:Configuration=Debug45 /p:DefineConstants=NET45 " + $defaultBuildParams);
        "BuildParams" = ("/p:DefineConstants=NET45 " + $defaultBuildParams);
    }
}

$buildProfiles += @{
        "Name"  = "Test containers NET45";
        "ProjectNames" = @('SPMeta2.Containers.CSOM', 'SPMeta2.Containers.O365', 'SPMeta2.Containers.O365v16', 'SPMeta2.Containers.SSOM'); 
        
        "CheckBaseline" = $false;
        "Configuration" = "Debug";
        
        #"BuildParams" = (" /p:Configuration=Debug /p:DefineConstants=NET45 " + $defaultBuildParams);
        "BuildParams" = ("/p:DefineConstants=NET45 " + $defaultBuildParams);
}

$buildProfiles += @{
        "Name"  = "Tests NET45";
        "ProjectNames" = @('SPMeta2.Regression.Tests', 'SPMeta2.Regression.Impl.Tests'); 

        "CheckBaseline" = $false;
        "Configuration" = "Debug"

        #"BuildParams" = (" /p:Configuration=Debug /p:DefineConstants=NET45 " + $defaultBuildParams);
        "BuildParams" = (" /p:DefineConstants=NET45 " + $defaultBuildParams);
}

foreach($buildProfile in $buildProfiles) {

    $index = $buildProfiles.IndexOf($buildProfile) + 1
    $count = $buildProfiles.Count

    Write-BInfo "[M2 Build] [$index/$count] Building profile [$($buildProfile.Name)]" -fore Green

    BuildProfile $buildProfile	
}