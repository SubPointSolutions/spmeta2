
function GetPackagesDirectory
{
	return "packages";
}

function GetDependencyPrototype() {
	
	$properties = @{
				'Id'= "";
                'Version'= "";
			}
	$object = New-Object –TypeName PSObject –Prop $properties

	return $object;

}

function GetPackagePrototype() {
	
	$properties = @{
				'Assemblies'= @();
                'Dependencies'= @();
				"Id" = "";
				"Version" = "";
				"Description" = "";
				"ReleaseNotes" = "";
				"Summary" = "";
				"ProjectUrl" = "https://github.com/SubPointSolutions/spmeta2";
				"Tags" = "SharePoint SP2013 SP2010 O365 Office365 Office365Dev Provision SPMeta2";
			}

	$object = New-Object –TypeName PSObject –Prop $properties

	return $object;
}

function GetNugetExePath
{
	$currentDir = GetCurrentDirectory
	$nugetPath = $currentDir + "/_3rd party/Nuget/NuGet.exe"

	return $nugetPath
}

function GetNugetSpecTemplate() {
	$currentDir = GetCurrentDirectory
	$spec = $currentDir + "/specs/SpecTemplate.nuspec"

	return $spec
}

function GetNugetSpecTemplateXml() {
	$specPath = GetNugetSpecTemplate
	[xml]$xml = Get-Content $specPath
	
	return $xml;
}

function GetSolutionDirectory
{
	$dir = GetCurrentDirectory

	$dir += "\..\"

	return $dir;
}

function GetCurrentDirectory
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

function GetPackageVersion($major, $minor, $useBuildTime) {
	
	$date = [System.DateTime]::Now;
	$result = "$major.$minor." + $date.DayOfYear;

	if($useBuildTime -eq $true) {
		$result += "." + $date.Hour.ToString("00") + $date.Minute.ToString("00");
	}
	
	return $result;
}

function CreatePackage($package) {
	
	Write-Host $package.Id

	$packageName  = $package.Id;
	$version = $package.Version;
	$asm = $package.Assemblies;

	$currentDir = GetCurrentDirectory
	$packagesDirName = GetPackagesDirectory
		
	$packagesFolder = [System.IO.Path]::Combine($currentDir, $packagesDirName)
	$versionDirFolder = [System.IO.Path]::Combine($packagesFolder, $version)

	$targetFolder = [System.IO.Path]::Combine($versionDirFolder, $packageName)
	$dir = [System.IO.Directory]::CreateDirectory($targetFolder);
	
	$solutionDirectory = GetSolutionDirectory

	foreach($fileName in $asm) {

		$name = [System.IO.Path]::GetFileNameWithoutExtension($fileName)

		$projectFolderName = $name
		$projectDirectory = [System.IO.Path]::Combine($solutionDirectory, $projectFolderName)
		$projectDirectory = [System.IO.Path]::Combine($projectDirectory, "bin\debug")

		$files = Get-ChildItem $projectDirectory -Recurse -Include "$fileName"

		if($files.Length -gt 0) {
			$targetFile = $files[0];

			Copy-Item $targetFile $targetFolder -Force 
		}
	}
	 
	CreateNugetSpec $package $targetFolder
}

function CreateNugetSpec($package, $targetFolder) {
	
	$packageName = $package.Id;
	$version = $package.Version;
	$asm = $package.Assemblies;
	
	$specXml = GetNugetSpecTemplateXml

	[Reflection.Assembly]::LoadWithpartialName("System.Xml.Linq") | Out-Null
	$specXml = [System.Xml.Linq.XDocument]::Parse($specXml.OuterXml)

	$metadata = $specXml.FirstNode.Element("{http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd}metadata");
			
	$metadata.Element("{http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd}id").Value = $packageName;	 
	$metadata.Element("{http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd}version").Value = $version;	 
	$metadata.Element("{http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd}title").Value = $packageName;	 
	$metadata.Element("{http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd}authors").Value = "SubPoint Solutions";	 
	$metadata.Element("{http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd}owners").Value = "SubPoint Solutions";	 
	$metadata.Element("{http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd}tags").Value = $package.Tags;	 
	$metadata.Element("{http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd}summary").Value = $package.Summary;	 
	$metadata.Element("{http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd}description").Value = $package.Description;	 
	$metadata.Element("{http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd}projectUrl").Value = $package.ProjectUrl;	 

	$files = $specXml.FirstNode.Element("{http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd}files");
	$dependencies = $metadata.Element("{http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd}dependencies");
	$dependenciesGroup = $dependencies.Element("{http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd}group");

	foreach($file in $asm) {
		[System.Xml.Linq.XElement] $asmNode = New-Object -TypeName System.Xml.Linq.XElement -ArgumentList "file", $null

		$asmNode.SetAttributeValue("src", $file);
		$asmNode.SetAttributeValue("target","lib\$file");
		
		$files.Add($asmNode)
	}

	#if($package.Dependencies.Count -gt  0) {

		foreach($dep in $package.Dependencies) {
			[System.Xml.Linq.XElement] $depNode = New-Object -TypeName System.Xml.Linq.XElement -ArgumentList "dependency", $null
			
			$depNode.SetAttributeValue("id", $dep.Id);
			$depNode.SetAttributeValue("version",$dep.Version);
		
			$dependenciesGroup.Add($depNode)
		}
	#}

	$spesFileName = "$packageName.$version.nuspec";
	$nupkgFileName = "$packageName.$version.nupkg";

	$specFullFilePath = [System.IO.Path]::Combine($targetFolder, $spesFileName);

	$specXml.Save($specFullFilePath)

	$nugetPath = GetNugetExePath
	
	$currentDir = GetCurrentDirectory
	$nugetFolder = [System.IO.Path]::Combine($currentDir, "nuget")

	$nugetFilePath =[System.IO.Path]::Combine($nugetFolder, $nupkgFileName)

	& $nugetPath "Pack" $specFullFilePath -OutputDirectory $nugetFolder

	Write-Host $nugetFilePath

	if($g_shouldPublish -eq $true) {
		& $nugetPath "Push" $nugetFilePath $g_apiKey
	}

}

function CreateSPMeta2CorePackage($version) {
	
	$asm = @()

	$asm += "SPMeta2.dll";
	$asm += "SPMeta2.Common.dll";
	$asm += "SPMeta2.Syntax.Default.dll";

	$package = GetPackagePrototype

	$package.Assemblies = $asm;
	$package.Version = $version;
	$package.Id =  "SPMeta2.Core"

	$package.Description = "SPMeta2 common infrastructure. Provides model definitions, model handlers, model tree construction and base syntax.";
	$package.Summary = "SPMeta2 common infrastructure."
	
	CreatePackage $package	
}

function CreateSPMeta2CSOMFoundationPackage($version) {

	$package = GetPackagePrototype

	$package.Version = $version;
	$package.Id =  "SPMeta2.CSOM.Foundation"

	$package.Description = "SPMeta2 CSOM provision implementation for SharePoint Foundation.";
	$package.Summary = "SPMeta2 CSOM provision implementation for SharePoint Foundation.";

	$package.Assemblies += "SPMeta2.CSOM.dll";
	$package.Assemblies += "SPMeta2.CSOM.Behaviours.dll";

	$spMetaCore = GetDependencyPrototype
	$spMetaCore.Id = "SPMeta2.Core"
	$spMetaCore.Version = $version

	$package.Dependencies += $spMetaCore

	CreatePackage $package
}

function CreateSPMeta2CSOMStandardPackage($version) {
	
	$package = GetPackagePrototype

	$package.Version = $version;
	$package.Id = "SPMeta2.CSOM.Standard"

	$package.Description = "SPMeta2 CSOM provision implementation for SharePoint Standard.";
	$package.Summary = "SPMeta2 CSOM provision implementation for SharePoint Standard.";

	$package.Assemblies += "SPMeta2.CSOM.Standard.dll";
	$package.Assemblies += "SPMeta2.CSOM.Standard.Behaviours.dll";
	$package.Assemblies += "SPMeta2.CSOM.Standard.Behaviours.dll";

	$spMetaCore = GetDependencyPrototype
	$spMetaCore.Id = "SPMeta2.Core"
	$spMetaCore.Version = $version

	$package.Dependencies += $spMetaCore

	$spMetaCSOM = GetDependencyPrototype
	$spMetaCSOM.Id = "SPMeta2.CSOM.Foundation"
	$spMetaCSOM.Version = $version
	
	$package.Dependencies += $spMetaCSOM

	CreatePackage $package
}

function CreateSPMeta2SSOMFoundationPackage($version) {
	
	$package = GetPackagePrototype

	$package.Version = $version;
	$package.Id = "SPMeta2.SSOM.Foundation"

	$package.Description = "SPMeta2 SSOM provision implementation for SharePoint Foundation.";
	$package.Summary = "SPMeta2 SSOM provision implementation for SharePoint Foundation.";

	$package.Assemblies += "SPMeta2.SSOM.dll";
	$package.Assemblies += "SPMeta2.SSOM.Behaviours.dll";

	$spMetaCore = GetDependencyPrototype
	$spMetaCore.Id = "SPMeta2.Core"
	$spMetaCore.Version = $version

	$package.Dependencies += $spMetaCore
	
	CreatePackage $package
}

function CreateSPMeta2RegressionPackage($version) {
	
	$package = GetPackagePrototype

	$package.Version = $version;
	$package.Id = "SPMeta2.Regression.Core"

	$package.Description = "SPMeta2 common regression package. Proivides core services for regression tests.";
	$package.Summary = "SPMeta2 common regression package.";

	$package.Assemblies += "SPMeta2.Regression.dll";
	$package.Assemblies += "SPMeta2.Regression.Common.dll";

	$spMetaCore = GetDependencyPrototype
	$spMetaCore.Id = "SPMeta2.Core"
	$spMetaCore.Version = $version

	$package.Dependencies += $spMetaCore

	CreatePackage $package
}

function CreateSPMeta2RegressionCSOMPackage($version) {
	
	$package = GetPackagePrototype

	$package.Version = $version;
	$package.Id = "SPMeta2.Regression.CSOM"

	$package.Description = "SPMeta2 CSOM regression package. Proivides CSOM support for regression tests.";
	$package.Summary = "SPMeta2 CSOM regression package.";

	$package.Assemblies += "SPMeta2.Regression.CSOM.dll";

	$spMetaCore = GetDependencyPrototype
	$spMetaCore.Id = "SPMeta2.Core"
	$spMetaCore.Version = $version

	$package.Dependencies += $spMetaCore

	$spMetaCSOM = GetDependencyPrototype
	$spMetaCSOM.Id = "SPMeta2.CSOM.Foundation"
	$spMetaCSOM.Version = $version

	$package.Dependencies += $spMetaCSOM
		
	$spMetaRegressionCore = GetDependencyPrototype
	$spMetaRegressionCore.Id = "SPMeta2.Regression.Core"
	$spMetaRegressionCore.Version = $version

	$package.Dependencies += $spMetaRegressionCore
	
	CreatePackage $package
}

function CreateSPMeta2RegressionSSOMPackage($version) {
	
	$package = GetPackagePrototype

	$package.Version = $version;
	$package.Id = "SPMeta2.Regression.SSOM"

	$package.Description = "SPMeta2 SSOM regression package. Proivides SSOM support for regression tests.";
	$package.Summary = "SPMeta2 SSOM regression package.";

	$package.Assemblies += "SPMeta2.Regression.SSOM.dll";

	$spMetaCore = GetDependencyPrototype
	$spMetaCore.Id = "SPMeta2.Core"
	$spMetaCore.Version = $version

	$package.Dependencies += $spMetaCore

	$spMetaCSOM = GetDependencyPrototype
	$spMetaCSOM.Id = "SPMeta2.SSOM.Foundation"
	$spMetaCSOM.Version = $version

	$package.Dependencies += $spMetaCSOM

	$spMetaRegressionCore = GetDependencyPrototype
	$spMetaRegressionCore.Id = "SPMeta2.Regression.Core"
	$spMetaRegressionCore.Version = $version

	$package.Dependencies += $spMetaRegressionCore
	
	CreatePackage $package
}

function CreateSPMeta2Validation($version) {
	
	$package = GetPackagePrototype

	$package.Version = $version;
	$package.Id = "SPMeta2.Validation"

	$package.Description = "SPMeta2 validation package. Proivides services for model validation - propeties, collections and relationships.";
	$package.Summary = "SPMeta2 validation package";

	$package.Assemblies += "SPMeta2.Validation.dll";

	$spMetaCore = GetDependencyPrototype
	$spMetaCore.Id = "SPMeta2.Core"
	$spMetaCore.Version = $version

	$package.Dependencies += $spMetaCore
	
	CreatePackage $package
}

function CreateSPMeta2Packages() {
	
	$version = GetPackageVersion 1 0 $g_useDayVersion

	Write-Host "Creating packages for version [$version]"

	Write-Host "Creating SPMeta2.Core package"
	CreateSPMeta2CorePackage $version

	Write-Host "Creating SPMeta2.CSOM.Foundation package"
	CreateSPMeta2CSOMFoundationPackage $version

	Write-Host "Creating SPMeta2.CSOM.Standard package"
	CreateSPMeta2CSOMStandardPackage $version

	Write-Host "Creating SPMeta2.SSOM.Foundation package"
	CreateSPMeta2SSOMFoundationPackage $version

	Write-Host "Creating SPMeta2.Regression.Core package"
	CreateSPMeta2RegressionPackage $version

	Write-Host "Creating SPMeta2.Regression.CSOM package"
	CreateSPMeta2RegressionCSOMPackage $version

	Write-Host "Creating SPMeta2.Regression.SSOM package"
	CreateSPMeta2RegressionSSOMPackage $version	

	Write-Host "Creating SPMeta2.Validation package"
	CreateSPMeta2Validation $version	
}
