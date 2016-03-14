cls

#region  utils

function Get-TimeStamp() {
    return $(get-date -f "yyyy-MM-dd HH:mm:ss") 
}

function Write-Error($msg, $fore = 'red') {
    
    $stamp = Get-TimeStamp

    if([string]::IsNullOrEmpty($msg) -eq $false) {
        $msg = "[$stamp] [ERROR] $msg"
    } else {
        $msg = "[$stamp] [ERROR]"
    }

    Write-Host $msg -fore $fore
}

function Write-Verbose($msg, $fore = 'gray') {
    
    $stamp = Get-TimeStamp

    if([string]::IsNullOrEmpty($msg) -eq $false) {
        $msg = "[$stamp] [VERBOSE] $msg"
    } else {
        $msg = "[$stamp] [VERBOSE]"
    }

    Write-Host $msg -fore $fore
}

function Write-Info($msg, $fore = 'green') {
    
    $stamp = Get-TimeStamp

    if([string]::IsNullOrEmpty($msg) -eq $false) {
        $msg = "[$stamp] [INFO] $msg"
    } else {
        $msg = "[$stamp] [INFO]"
    }

    Write-Host $msg -fore $fore
}


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

    foreach($projectName in $buildProfile.ProjectNames) {
        
        $index = $buildProfile.ProjectNames.IndexOf($projectName) + 1
        $count = $buildProfile.ProjectNames.Count

        Write-Verbose "`t[$index/$count] Profile:[$($buildProfile.Name)] Project name:[$projectName]" -fore Gray
        Write-Verbose "`tParams: [$($buildProfile.BuildParams)]" -fore Gray
     
        & $msbuild_path """$solutionRootPath\$projectName\$projectName.csproj"" $($buildProfile.BuildParams) " 

		if (! $?) { 
		
            Write-Error "`t[M2 Build] There was an error building profile:[$($buildProfile.Name)]" -fore red
            Write-Error "`t[M2 Build] Expanding params:" -fore Red
                                    
            foreach($key in $buildProfile.Keys) {
                Write-Error "`t$key":[$( $buildProfile[$key])] -fore Red
            }

			throw "`t[M2 Build] !!! Build faild on profile:[$($buildProfile.Name)]. Please check output early to check the details. !!!" 
		}
    }
}

#endregion

#region default values / profiles

$defaultProjects = @("SPMeta2", "SPMeta2.Standard", "SPMeta2.SSOM", "SPMeta2.SSOM.Standard", "SPMeta2.CSOM", "SPMeta2.CSOM.Standard" )
#$defaultProjects = @( "SPMeta2", "SPMeta2.Standard", "SPMeta2.SSOM")
$o365Projects = @("SPMeta2", "SPMeta2.Standard", "SPMeta2.CSOM", "SPMeta2.CSOM.Standard" )

# https://msdn.microsoft.com/en-us/library/ms164311.aspx
$verbosity = "quiet" 
$defaultBuildParams = " /t:Clean,Rebuild /p:Platform=AnyCPU /p:WarningLevel=0 /verbosity:$verbosity /clp:ErrorsOnly /nologo"

# https://www.appveyor.com/docs/build-phase
if( [System.Environment]::GetEnvironmentVariable("APPVEYOR") -ne $null) {
    $defaultBuildParams += " /logger:""C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll"""
}

$currentPath =  Get-ScriptDirectory
$solutionRootPath =  "$currentPath\..\"

$msbuild_path = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"

#endregion


$buildProfiles =  @()

$build14 = $true
$build15 = $false
$build16 = $false
$build365 = $false

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

    $index = $buildProfiles.IndexOf($buildProfile) + 1
    $count = $buildProfiles.Count

    Write-Info "[M2 Build] [$index/$count] Building profile [$($buildProfile.Name)]" -fore Green

    BuildProfile $buildProfile	
}