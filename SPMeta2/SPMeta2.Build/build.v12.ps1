cls

#region  utils

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

function BuildProfile($buildProfile) {
    
    $configuration = $buildProfile.Configuration

    foreach($projectName in $buildProfiles.ProjectNames) {
        
        $buildProfile.BuildParams
     
        & $msbuild_path """$solutionRootPath\$projectName\$projectName.csproj"" $($buildProfile.BuildParams)"
    }

}

#endregion

#region default values / profiles

$defaultProjects = @("SPMeta2", "SPMeta2.Standard", "SPMeta2.SSOM", "SPMeta2.SSOM.Standard", "SPMeta2.CSOM", "SPMeta2.CSOM.Standard" )
#$defaultProjects = @( "SPMeta2", "SPMeta2.Standard", "SPMeta2.SSOM")
$o365Projects = @("SPMeta2", "SPMeta2.Standard", "SPMeta2.CSOM", "SPMeta2.CSOM.Standard" )

$verbosity = "minimal"
$defaultBuildParams = " /t:Clean,Rebuild /p:Platform=AnyCPU /p:WarningLevel=0 /verbosity:$verbosity"

$currentPath =  Get-ScriptDirectory
$solutionRootPath =  "$currentPath\..\"

$msbuild_path = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"

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
        "BuildParams" = ("/p:spRuntime=14 /p:Configuration=Debug35 /p:DefineConstants=NET35 " + $defaultBuildParams);
    }
}

if($build15 -eq $true) {

    $buildProfiles += @{
        "Name"  = "M2 SP2013 NET40";
        "ProjectNames" = $defaultProjects
        "BuildParams" = ("/p:spRuntime=15 /p:Configuration=Debug40 /p:DefineConstants=NET40 " + $defaultBuildParams);
    }

    $buildProfiles += @{
        "Name"  = "M2 SP2013 NET45";
        "ProjectNames" = $defaultProjects
        "BuildParams" = ("/p:spRuntime=15 /p:Configuration=Debug45 /p:DefineConstants=NET45 " + $defaultBuildParams);
    }
}

if($build16 -eq $true) {

    $buildProfiles += @{
        "Name"  = "M2 SP2016 NET40";
        "ProjectNames" = $defaultProjects
        "BuildParams" = ("/p:spRuntime=16 /p:Configuration=Debug40 /p:DefineConstants=NET40 " + $defaultBuildParams);
    }

    $buildProfiles += @{
        "Name"  = "M2 SP2016 NET45";
        "ProjectNames" = $defaultProjects
        "BuildParams" = ("/p:spRuntime=16 /p:Configuration=Debug45 /p:DefineConstants=NET45 " + $defaultBuildParams);
    }
}

if($build365 -eq $true) {

    $buildProfiles += @{
        "Name"  = "M2 O365 NET40";
        "ProjectNames" = $o365Projects; 
        "BuildParams" = ("/p:spRuntime=365 /p:Configuration=Debug40 /p:DefineConstants=NET40 " + $defaultBuildParams);
    }

    $buildProfiles += @{
        "Name"  = "M2 O365 NET45";
        "ProjectNames" = $o365Projects; 
        "BuildParams" = ("/p:spRuntime=365 /p:Configuration=Debug45 /p:DefineConstants=NET45 " + $defaultBuildParams);
    }
}

foreach($buildProfile in $buildProfiles) {

    Write-Host "Building profile [$($buildProfile.Name)]"

    BuildProfile $buildProfile
}





