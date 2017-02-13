cls

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

$currentDir = Get-ScriptDirectory

cd "$currentDir"

. "$currentDir\_m2.core.ps1"
. "$currentDir\_m2.baseline.ps1"
. "$currentDir\_m2.nuget.core.ps1"

# global 'g_' variables to be used across the board

# should publish to NuGet?
$g_shouldPublish = $false

# should use daily-based version
$g_useDayVersion = $false

# use M2 v12 or v13 packaging
$g_is13 = $false

# M2 NuGet package version, release noted URL and target solution directory
$g_hardcoreVersionBase = "1.2.105";
$g_hardcoreVersion = "$g_hardcoreVersionBase-beta1";

$g_releaseNotes = "https://github.com/SubPointSolutions/spmeta2/releases/tag/$g_hardcoreVersion";
$g_solutionDirectory = [System.IO.Path]::GetFullPath("$currentDir\..\..\SPMeta2")

$g_Verbosity = 'quiet'

#by default to the public repo
$g_SourceUrl = $null
$g_apiKey = $null

if($g_isAppVeyor -eq $true) 
{
	Write-BInfo "AppVeyor build"

	# setting up everything for staging build
	
	$g_apiKey = Get-EnvironmentVariable "SPMeta2_NuGet_Staging_APIKey"
	$g_SourceUrl = "https://www.myget.org/F/subpointsolutions-staging/api/v2/package"
	
	$date = get-date
	$stamp = ( $date.ToString("yy") + $date.DayOfYear.ToString("000") + $date.ToString("HHmm"))
	$g_hardcoreVersion = ($g_hardcoreVersionBase + "-alpha" + $stamp)

	# appveyor path to build
	$g_solutionDirectory = "c:\prj\m2\SPMeta2"
}
else 
{
	Write-BInfo "Local build"

	# setting for user-efined build
	$g_apiKey = "YOUR API KEY"
	$g_SourceUrl = "YOUR SOURCE KEY"
	
	$date = get-date
	$stamp = ( $date.ToString("yy") + $date.DayOfYear.ToString("000") + $date.ToString("HHmm"))
	#$g_hardcoreVersion = $g_hardcoreVersionBase + "-alpha" + $stamp

	#$g_solutionDirectory = "c:\prj\m2\SPMeta2"
}

Write-BInfo "g_solutionDirectory:[$g_solutionDirectory]"
Write-BInfo "g_hardcoreVersion:[$g_hardcoreVersion]"
Write-BInfo "g_SourceUrl:[$g_SourceUrl]"

# override 'g_' here if any

function CreateSPMeta2Packages() {
	
	$version = GetPackageVersion 1 0 $g_useDayVersion

	Write-BInfo "Creating packages for version [$version]"

	# Core & Core.Standard
	Write-BInfo "Creating SPMeta2.Core package"
	CreateSPMeta2CorePackage $version

	Write-BInfo "Creating SPMeta2.Core.Standard package"
	CreateSPMeta2CoreStandardPackage $version 

    #CSOM
	Write-BInfo "Creating SPMeta2.CSOM.Foundation package"
    
    if($g_is13 -eq $true) {
    
        CreateSPMeta2CSOMFoundationPackage $version "14"
	    CreateSPMeta2CSOMFoundationPackage $version "15"
        CreateSPMeta2CSOMFoundationPackage $version "16"
        CreateSPMeta2CSOMFoundationPackage $version "365"

    }
    else {

        CreateSPMeta2CSOMFoundationPackage $version "14"
	    CreateSPMeta2CSOMFoundationPackage $version ""
        CreateSPMeta2CSOMFoundationPackage $version "16"
    }

	Write-BInfo "Creating SPMeta2.CSOM.Standard package"

    if($g_is13 -eq $true) {

        CreateSPMeta2CSOMStandardPackage $version "14"
	    CreateSPMeta2CSOMStandardPackage $version "15"
        CreateSPMeta2CSOMStandardPackage $version "16"
        CreateSPMeta2CSOMStandardPackage $version "365"

    } else {

        CreateSPMeta2CSOMStandardPackage $version "14"
	    CreateSPMeta2CSOMStandardPackage $version ""
        CreateSPMeta2CSOMStandardPackage $version "16"
    }

    #SSOM
	Write-BInfo "Creating SPMeta2.SSOM.Foundation package"

    if($g_is13 -eq $true) {

        CreateSPMeta2SSOMFoundationPackage $version "14"	
        CreateSPMeta2SSOMFoundationPackage $version "15"
        CreateSPMeta2SSOMFoundationPackage $version "16"
    } else {
        
        CreateSPMeta2SSOMFoundationPackage $version "14"	
        CreateSPMeta2SSOMFoundationPackage $version ""
        #CreateSPMeta2SSOMFoundationPackage $version "16"
    }

    Write-BInfo "Creating SPMeta2.SSOM.Standard package"

    if($g_is13 -eq $true) {
        CreateSPMeta2SSOMStandardPackage $version "14"	
        CreateSPMeta2SSOMStandardPackage $version "15"
        CreateSPMeta2SSOMStandardPackage $version "16"
    } else {
        CreateSPMeta2SSOMStandardPackage $version "14"	
        CreateSPMeta2SSOMStandardPackage $version ""
        #CreateSPMeta2SSOMStandardPackage $version "16"
    }
}

CreateSPMeta2Packages