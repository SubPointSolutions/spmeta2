
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
                'Assemblies35'= @();
				'Assemblies40'= @();
                'Assemblies45'= @();
                'Dependencies'= @();
				"Id" = "";
				"Version" = "";
				"Description" = "";
				"ReleaseNotes" = "";
				"Summary" = "";
				"RequireLicenseAcceptance" = $false;
				"ProjectUrl" = "https://github.com/SubPointSolutions/spmeta2";
				"IconUrl" = "https://raw.githubusercontent.com/SubPointSolutions/spmeta2/dev/SPMeta2/SPMeta2.Dependencies/Images/SPMeta2_64_64.png";
				"Tags" = "SharePoint SP2013 SP2010 O365 Office365 Office365Dev Provision SPMeta2";
			}

	$object = New-Object –TypeName PSObject –Prop $properties

	return $object;
}

function GetNugetExePath
{
	$currentDir = GetCurrentDirectory
	$nugetPath = $currentDir + "/tools/Nuget/NuGet.exe"

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
	return $g_solutionDirectory;
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


    if($g_hardcoreVersion -ne "" -and $g_hardcoreVersion -ne $null) {
        $result = $g_hardcoreVersion;
    }
	
	return $result;
}

function CreatePackage($package, $spRuntime) {
	
	Write-BInfo "Creating package with ID: [$($package.Id)] Runtime:[$($spRuntime)]"

    if(! $spRuntime) {
        $package.Id = $package.Id 
    } else {
        $package.Id = $package.Id + "-v" + $spRuntime
    }

	$packageName  = $package.Id;
	$version = $package.Version;
	#$asm = $package.Assemblies;

	$currentDir = GetCurrentDirectory
	$packagesDirName = GetPackagesDirectory
		
	$packagesFolder = [System.IO.Path]::Combine($currentDir, $packagesDirName)
	$versionDirFolder = [System.IO.Path]::Combine($packagesFolder, $version)

	$targetFolder = [System.IO.Path]::Combine($versionDirFolder, $packageName)

    $targetFolder35 = [System.IO.Path]::Combine($targetFolder, "net35")    
    $targetFolder40 = [System.IO.Path]::Combine($targetFolder, "net40")    
    $targetFolder45 = [System.IO.Path]::Combine($targetFolder, "net45")
    

	$dir = [System.IO.Directory]::CreateDirectory($targetFolder);
    
    $dir = [System.IO.Directory]::CreateDirectory($targetFolder35);
    $dir = [System.IO.Directory]::CreateDirectory($targetFolder40);
    $dir = [System.IO.Directory]::CreateDirectory($targetFolder45);
	
	$solutionDirectory = GetSolutionDirectory
    
    if($g_is13 -eq $false) {

        if($spRuntime -eq "16") {

            Write-BInfo "`tv12, reverting spRuntime 16 -> 365" -fore Gray
            $spRuntime = "365"
        }

        if($spRuntime -eq "") {

            Write-BInfo "`tv12, reverting spRuntime '' -> 15" -fore Gray
            $spRuntime = "15"
        }
    }


    if($package.Assemblies35.Count -gt 0) {

        Write-BInfo "`tProcessing 35 assemblies" -ForegroundColor Yellow
	    foreach($fileName in $package.Assemblies35) {

		    $name = [System.IO.Path]::GetFileNameWithoutExtension($fileName)
    
            $targetOutputFolder = "bin\debug35"    

            if($spRuntime -ne $null) {
                $targetOutputFolder += ("-" + $spRuntime)
            }
            Write-BInfo "`tTarget output folder: [$targetOutputFolder]" -fore gray

		    $projectFolderName = $name
		    $projectDirectory = [System.IO.Path]::Combine($solutionDirectory, $projectFolderName)
            $projectDirectory = [System.IO.Path]::Combine($projectDirectory, $targetOutputFolder)

		    $files = Get-ChildItem $projectDirectory -Recurse -Include "$fileName"

		    if($files.Length -gt 0) {
			    $targetFile = $files[0];

			    Write-BInfo $targetFile -ForegroundColor Green
                Copy-Item $targetFile $targetFolder35  -Force 
		    } else {
                throw "[35] Can't find file [$fileName] in folder:[$projectDirectory] "
            }
	    }

    } 

    if($package.Assemblies45.Count -gt 0) {

	        Write-BInfo "`tProcessing 45 assemblies" -ForegroundColor Yellow
	        foreach($fileName in $package.Assemblies45) {

		        $name = [System.IO.Path]::GetFileNameWithoutExtension($fileName)
    
                $targetOutputFolder = "bin\debug45"    

                if($spRuntime -ne $null) {
                    $targetOutputFolder += ("-" + $spRuntime)
                }
                Write-BInfo "`tTarget output folder: [$targetOutputFolder]" -fore gray

		        $projectFolderName = $name
		        $projectDirectory = [System.IO.Path]::Combine($solutionDirectory, $projectFolderName)
                $projectDirectory = [System.IO.Path]::Combine($projectDirectory, $targetOutputFolder)

		        $files = Get-ChildItem $projectDirectory -Recurse -Include "$fileName"

		        if($files.Length -gt 0) {
			        $targetFile = $files[0];

			        Write-BInfo $targetFile -ForegroundColor Green
                    Copy-Item $targetFile $targetFolder45  -Force 
    	        } else {
                    throw "[45] Can't find file [$fileName] in folder:[$projectDirectory]"
                }
	        }
    }

    if($package.Assemblies40.Count -gt 0) {

	    Write-BInfo "`tProcessing 40 assemblies" -ForegroundColor Yellow
	    foreach($fileName in $package.Assemblies40) {

		    $name = [System.IO.Path]::GetFileNameWithoutExtension($fileName)

            $targetOutputFolder = "bin\debug40"    
        
            if($spRuntime -ne $null) {
                $targetOutputFolder += ("-" + $spRuntime)
            }
            Write-BInfo "`tTarget output folder: [$targetOutputFolder]" -fore gray

		    $projectFolderName = $name
		    $projectDirectory = [System.IO.Path]::Combine($solutionDirectory, $projectFolderName)
		    $projectDirectory = [System.IO.Path]::Combine($projectDirectory, $targetOutputFolder)

		    $files = Get-ChildItem $projectDirectory -Recurse -Include "$fileName"

		    if($files.Length -gt 0) {
			    $targetFile = $files[0];

			    Write-BInfo $targetFile -ForegroundColor Green
                Copy-Item $targetFile $targetFolder40  -Force
		    } 
            else {
                   throw "[40] Can't find file [$fileName] in folder:[$projectDirectory] "
            }
	    }
    }

	CreateNugetSpec $package $targetFolder

    ""
    ""
}

function CreateNugetSpec($package, $targetFolder) {
	
	$packageName = $package.Id;
	$version = $package.Version;
	
    #$asm = $package.Assemblies40;
    #$asm = $package.Assemblies45;
	
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
	$metadata.Element("{http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd}iconUrl").Value = $package.IconUrl;	 
	$metadata.Element("{http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd}requireLicenseAcceptance").Value = $package.RequireLicenseAcceptance.ToString().ToLower();	 

    $metadata.Element("{http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd}releaseNotes").Value = $g_releaseNotes ;	 

	$files = $specXml.FirstNode.Element("{http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd}files");
	$dependencies = $metadata.Element("{http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd}dependencies");
	$dependenciesGroup = $dependencies.Element("{http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd}group");

    foreach($file in $package.Assemblies35) {
		[System.Xml.Linq.XElement] $asmNode = New-Object -TypeName System.Xml.Linq.XElement -ArgumentList "file", $null

		$asmNode.SetAttributeValue("src", "net35\$file");
		$asmNode.SetAttributeValue("target","lib\net35\$file");
		
		$files.Add($asmNode)

        # XML


	}

	foreach($file in $package.Assemblies45) {
		[System.Xml.Linq.XElement] $asmNode = New-Object -TypeName System.Xml.Linq.XElement -ArgumentList "file", $null

		$asmNode.SetAttributeValue("src", "net45\$file");
		$asmNode.SetAttributeValue("target","lib\net45\$file");
		
		$files.Add($asmNode)
	}

    foreach($file in $package.Assemblies40) {
		[System.Xml.Linq.XElement] $asmNode = New-Object -TypeName System.Xml.Linq.XElement -ArgumentList "file", $null

		$asmNode.SetAttributeValue("src", "net40\$file");
		$asmNode.SetAttributeValue("target","lib\net40\$file");
		
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

	$nugetFolder = [System.IO.Path]::Combine($currentDir, ("packages" + $g_hardcoreVersion))
	$nugetFilePath =[System.IO.Path]::Combine($nugetFolder, $nupkgFileName)

	[System.IO.Directory]::CreateDirectory($nugetFolder) | out-null

	Write-BInfo "Building NuGet package:[$specFullFilePath]"
	& $nugetPath "Pack" $specFullFilePath -OutputDirectory $nugetFolder -Verbosity $g_Verbosity

	if (! $?)
	{ 
		Write-BError "`t[M2 NuGet Publishing] There was an error creating NuGet package:[$nupkgFileName]" -fore red
		Write-BError "Spec:[$specFullFilePath]"
		Write-BError "File:[$nugetFilePath]"
	} 
	else
	{
		Write-BInfo "NuGet package was succesfully created!"
		Write-BInfo "Spec:[$specFullFilePath]"
		Write-BInfo "File:[$nugetFilePath]"
	}

	# baseline check!
	# unpack NUuGet package, get all m2 assemblies and check the baseline
	# $nupkgFileName
    $packageName = [System.IO.Path]::GetFileNameWithoutExtension($nugetFilePath)
    
    $tmpPath = $env:TEMP 
    $assemblyTemporaryDirectory = [System.IO.Path]::Combine($tmpPath, "m2.nuget.tmp")

    [System.IO.Directory]::CreateDirectory($assemblyTemporaryDirectory) | out-null;
    $assemblyTmpPath = [System.IO.Path]::Combine($assemblyTemporaryDirectory, [guid]::NewGuid().ToString("N") + ".dll")
    Copy-Item $nugetFilePath $assemblyTmpPath -Force -Confirm:$false
            
    #tmp folder
    $tmpNuGetPackageFolder = [System.IO.Path]::Combine($assemblyTemporaryDirectory,$packageName + ([guid]::NewGuid().ToString("N")) )
    [System.IO.Directory]::CreateDirectory($tmpNuGetPackageFolder) | out-null;

    # unpack
    Write-BInfo "Unpacking NuGet package for baseline check:[$tmpNuGetPackageFolder]"
    Add-Type -assembly "system.io.compression.filesystem" | out-null
    [io.compression.zipfile]::ExtractToDirectory($nugetFilePath, $tmpNuGetPackageFolder)

    $libFolder = [System.IO.Path]::Combine($tmpNuGetPackageFolder, "lib")
    $netProfileFolders = Get-ChildItem $libFolder

    foreach($netProfileFolder in $netProfileFolders) {
        Write-BInfo "Checking baseline for net folder:[$netProfileFolder]"

        $assemblies = Get-ChildItem $netProfileFolder.FullName -Filter "*.dll"

        foreach($assembyPath in $assemblies) {
            
            $assembyFullPath = $assembyPath.FullName
            $assembyFullName = $assembyPath.Name

            Write-BVerbose "Checking baseline for assembly:[$assembyFullPath]"

            $runtime = "15"
            $netRuntime = "v3.5"

            if($netProfileFolder.Name -eq "net35") {
                Write-BVerbose "Handling net35 folder"
                $runtime = "14"
                $netRuntime = "v3.5"
            }

            if($netProfileFolder.Name -eq "net40") {
                Write-BVerbose "Handling net40 folder"
                $runtime = "15"
                $netRuntime = "v4.0"
            }

            if($netProfileFolder.Name -eq "net45") {
                Write-BVerbose "Handling net45 folder"
                $runtime = "15"
                $netRuntime = "v4.5"
            }

            # is vXX version? SPMeta2.CSOM.Foundation-v14.1.2.65-
            if($packageName.Contains("-v") -eq $true) {
                Write-BVerbose "vXX package:[$packageName]"
                $packageRuntimeversion = (($packageName -split "-v")[1]).Split('.')[0]

                if($runtime -eq "16") {
                    $runtime = "365"
                }

                Write-BVerbose "Runtime version:[$packageRuntimeversion]"
                $runtime = $packageRuntimeversion               
            }

            Check-AssemblyBaseline $assembyFullName $runtime $assembyFullPath $g_buildBaseline $netRuntime
        }
    }    

	if($g_shouldPublish -eq $true) {
	
		Write-BInfo "Publishing NuGet package [$nupkgFileName] "
		Write-BInfo "`tSpec:[$specFullFilePath]"
		Write-BInfo "`tFile:[$nugetFilePath]"
		
		if($g_SourceUrl -ne $null) {
			Write-BInfo "`tSource:[$g_SourceUrl]"
			& $nugetPath "Push" $nugetFilePath $g_apiKey -Verbosity $g_Verbosity -Source $g_SourceUrl
		} else{
			Write-BInfo "`tSource:[no source, pushing to the public gallery]"
			& $nugetPath "Push" $nugetFilePath $g_apiKey -Verbosity $g_Verbosity 
		}
		
		if (! $?) 
		{ 
			Write-BError "`t[M2 NuGet Publishing] There was an error publishing NuGet package:[$nupkgFileName]" -fore red
			Write-BError "Spec:[$specFullFilePath]"
			Write-BError "File:[$nugetFilePath]"

			throw "`t[M2 NuGet Publishing] !!! NuGet failed on:[$specFullFilePath]. !!!" 
		} 
		else 
		{
			Write-BInfo "NuGet package was succesfully published!"
			Write-BInfo "Spec:[$specFullFilePath]"
			Write-BInfo "File:[$nugetFilePath]"
		}
	} 
}

function CreateSPMeta2CorePackage($version, $spRuntime) {
	
	$asm = @()

	$asm += "SPMeta2.dll";
    $asm += "SPMeta2.xml";
	#$asm += "SPMeta2.Common.dll";
	#$asm += "SPMeta2.Syntax.Default.dll";
    #$asm += "SPMeta2.Syntax.Default.xml";

	$package = GetPackagePrototype

    $package.Assemblies35 = $asm;
	$package.Assemblies45 = $asm;
    $package.Assemblies40 = $asm;

	$package.Version = $version;
	$package.Id =  "SPMeta2.Core"

	$package.Description = "SPMeta2 common infrastructure. Provides artifact definitions for SharePoint Foundation, models and handlers, artifact model tree construction, base syntax, validation and common set of the services.";
	$package.Summary = "SPMeta2 common infrastructure."
	
	CreatePackage $package $spRuntime
}

function CreateSPMeta2CoreStandardPackage($version) {
	
	$asm = @()

	$asm += "SPMeta2.Standard.dll";
    $asm += "SPMeta2.Standard.xml";

	$package = GetPackagePrototype

    $package.Assemblies35 = $asm;
	$package.Assemblies45 = $asm;
    $package.Assemblies40 = $asm;

	$package.Version = $version;
	$package.Id =  "SPMeta2.Core.Standard"

	$package.Description = "SPMeta2 common infrastructure. Provides artifact definitions for SharePoint Standard+, models and handlers, artifact model tree construction, base syntax, validation and common set of the services.";
	$package.Summary = "SPMeta2 common standard infrastructure."
	
	$spMetaCore = GetDependencyPrototype
	$spMetaCore.Id = "SPMeta2.Core"
	$spMetaCore.Version = $version

	$package.Dependencies += $spMetaCore

	CreatePackage $package	
}

function CreateSPMeta2CSOMFoundationPackage($version, $spRuntime) {

	$package = GetPackagePrototype

	$package.Version = $version;
	$package.Id =  "SPMeta2.CSOM.Foundation"

	$package.Description = "SPMeta2 CSOM provision implementation for SharePoint Foundation.";
	$package.Summary = "SPMeta2 CSOM provision implementation for SharePoint Foundation.";

    if($spRuntime -eq $null -or $spRuntime -eq "" -or ($spRuntime -eq "15") -or ($spRuntime -eq "16") -or ($spRuntime -eq "365")) {

	    $package.Assemblies45 += "SPMeta2.CSOM.dll";
        $package.Assemblies45 += "SPMeta2.CSOM.xml";

	    #$package.Assemblies45 += "SPMeta2.CSOM.Behaviours.dll";
        #$package.Assemblies45 += "SPMeta2.CSOM.Behaviours.xml";

        $package.Assemblies40 += "SPMeta2.CSOM.dll";
        $package.Assemblies40 += "SPMeta2.CSOM.xml";

        #$package.Assemblies40 += "SPMeta2.CSOM.Behaviours.dll";
        #$package.Assemblies40 += "SPMeta2.CSOM.Behaviours.xml";
    }

    if($spRuntime -eq "14") {
        
        $package.Assemblies35 += "SPMeta2.CSOM.dll";
        $package.Assemblies35 += "SPMeta2.CSOM.xml";

    }

	$spMetaCore = GetDependencyPrototype
	$spMetaCore.Id = "SPMeta2.Core"
	$spMetaCore.Version = $version

	$package.Dependencies += $spMetaCore

	CreatePackage $package $spRuntime
}

function CreateSPMeta2CSOMStandardPackage($versionm, $spRuntime) {
	
	$package = GetPackagePrototype

	$package.Version = $version;
	$package.Id = "SPMeta2.CSOM.Standard"

	$package.Description = "SPMeta2 CSOM provision implementation for SharePoint Standard.";
	$package.Summary = "SPMeta2 CSOM provision implementation for SharePoint Standard.";

    if($spRuntime -eq $null  -or  $spRuntime -eq "" -or ($spRuntime -eq "15") -or ($spRuntime -eq "16") -or ($spRuntime -eq "365")) {

	    $package.Assemblies45 += "SPMeta2.CSOM.Standard.dll";
        $package.Assemblies45 += "SPMeta2.CSOM.Standard.xml";

	    #$package.Assemblies45 += "SPMeta2.CSOM.Standard.Behaviours.dll";
        #$package.Assemblies45 += "SPMeta2.CSOM.Standard.Behaviours.xml";

        $package.Assemblies40 += "SPMeta2.CSOM.Standard.dll";
        $package.Assemblies40 += "SPMeta2.CSOM.Standard.xml";

	    #$package.Assemblies40 += "SPMeta2.CSOM.Standard.Behaviours.dll";
        #$package.Assemblies40 += "SPMeta2.CSOM.Standard.Behaviours.xml";
    }

    if($spRuntime -eq "14") {
        
        $package.Assemblies35 += "SPMeta2.CSOM.Standard.dll";
        $package.Assemblies35 += "SPMeta2.CSOM.Standard.xml";

    }

	$spMetaCore = GetDependencyPrototype
	$spMetaCore.Id = "SPMeta2.Core"
	$spMetaCore.Version = $version

	$package.Dependencies += $spMetaCore

	$spMetaCoreStandard = GetDependencyPrototype
	$spMetaCoreStandard.Id = "SPMeta2.Core.Standard"
	$spMetaCoreStandard.Version = $version

	$package.Dependencies += $spMetaCoreStandard

	$spMetaCSOM = GetDependencyPrototype
	$spMetaCSOM.Id = "SPMeta2.CSOM.Foundation"
	$spMetaCSOM.Version = $version
	
    if($spRuntime -eq "14") {
        $spMetaCSOM.Id = $spMetaCSOM.Id + "-v14"
    }

    if($spRuntime -eq "16") {
        $spMetaCSOM.Id = $spMetaCSOM.Id + "-v16"
    }

    if($spRuntime -eq "365") {
        $spMetaCSOM.Id = $spMetaCSOM.Id + "-v365"
    }


	$package.Dependencies += $spMetaCSOM

	CreatePackage $package $spRuntime
}

function CreateSPMeta2SSOMFoundationPackage($version, $spRuntime) {
	
	$package = GetPackagePrototype

	$package.Version = $version;
	$package.Id = "SPMeta2.SSOM.Foundation"

	$package.Description = "SPMeta2 SSOM provision implementation for SharePoint Foundation.";
	$package.Summary = "SPMeta2 SSOM provision implementation for SharePoint Foundation.";

    if($spRuntime -eq $null  -or  $spRuntime -eq "" -or ($spRuntime -eq "15")) {

	    $package.Assemblies45 += "SPMeta2.SSOM.dll";
        $package.Assemblies45 += "SPMeta2.SSOM.xml";

	    #$package.Assemblies45 += "SPMeta2.SSOM.Behaviours.xml";
        #$package.Assemblies45 += "SPMeta2.SSOM.Behaviours.dll";

	    $package.Assemblies40 += "SPMeta2.SSOM.dll";
        $package.Assemblies40 += "SPMeta2.SSOM.xml";
	    
        #$package.Assemblies40 += "SPMeta2.SSOM.Behaviours.dll";
        #$package.Assemblies40 += "SPMeta2.SSOM.Behaviours.xml";
    }

    if($spRuntime -eq "14") {

	    $package.Assemblies35 += "SPMeta2.SSOM.dll";
        $package.Assemblies35 += "SPMeta2.SSOM.xml";

	    #$package.Assemblies35 += "SPMeta2.SSOM.Behaviours.dll";
        #$package.Assemblies35 += "SPMeta2.SSOM.Behaviours.xml";
    }

	$spMetaCore = GetDependencyPrototype
	$spMetaCore.Id = "SPMeta2.Core"
	$spMetaCore.Version = $version

	$package.Dependencies += $spMetaCore
	
	CreatePackage $package $spRuntime
}

function CreateSPMeta2SSOMStandardPackage($version, $spRuntime) {
	
	$package = GetPackagePrototype

	$package.Version = $version;
	$package.Id = "SPMeta2.SSOM.Standard"

	$package.Description = "SPMeta2 SSOM provision implementation for SharePoint Standard.";
	$package.Summary = "SPMeta2 SSOM provision implementation for SharePoint Standard.";

    if($spRuntime -eq $null  -or  $spRuntime -eq "" -or ($spRuntime -eq "15")) {

	    $package.Assemblies45 += "SPMeta2.SSOM.Standard.dll";
        $package.Assemblies45 += "SPMeta2.SSOM.Standard.xml";

	    $package.Assemblies40 += "SPMeta2.SSOM.Standard.dll";
        $package.Assemblies40 += "SPMeta2.SSOM.Standard.xml";
    }

    if($spRuntime -eq "14") {

	    $package.Assemblies35 += "SPMeta2.SSOM.Standard.dll";
        $package.Assemblies35 += "SPMeta2.SSOM.Standard.xml";
    }

	$spMetaCore = GetDependencyPrototype
	$spMetaCore.Id = "SPMeta2.Core"
	$spMetaCore.Version = $version

	$package.Dependencies += $spMetaCore

	$spMetaStandard = GetDependencyPrototype
	$spMetaStandard.Id = "SPMeta2.Core.Standard"
	$spMetaStandard.Version = $version

	$package.Dependencies += $spMetaStandard


	$spMetaFoundation = GetDependencyPrototype
	$spMetaFoundation.Id = "SPMeta2.SSOM.Foundation"
	$spMetaFoundation.Version = $version

    if($spRuntime -eq "14") {
        $spMetaFoundation.Id = $spMetaFoundation.Id + "-v14"
    }
    
	$package.Dependencies += $spMetaFoundation
	
	CreatePackage $package $spRuntime
}

function CreateSPMeta2RegressionPackage($version) {
	
	$package = GetPackagePrototype

	$package.Version = $version;
	$package.Id = "SPMeta2.Regression.Core"

	$package.Description = "SPMeta2 common regression package. Proivides core services for regression tests.";
	$package.Summary = "SPMeta2 common regression package.";

	$package.Assemblies45 += "SPMeta2.Regression.dll";
	$package.Assemblies45 += "SPMeta2.Regression.Common.dll";

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

	$package.Assemblies45 += "SPMeta2.Regression.CSOM.dll";

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

	$package.Assemblies45 += "SPMeta2.Regression.SSOM.dll";

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

	$package.Description = "SPMeta2 validation package. Proivides services for model validation - properties, collections and relationships.";
	$package.Summary = "SPMeta2 validation package";

    $package.Assemblies35 += "SPMeta2.Validation.dll";
    $package.Assemblies35 += "SPMeta2.Validation.xml";

	$package.Assemblies45 += "SPMeta2.Validation.dll";
    $package.Assemblies45 += "SPMeta2.Validation.xml";

	$package.Assemblies40 += "SPMeta2.Validation.dll";
    $package.Assemblies40 += "SPMeta2.Validation.xml";

	$spMetaCore = GetDependencyPrototype
	$spMetaCore.Id = "SPMeta2.Core"
	$spMetaCore.Version = $version

	$package.Dependencies += $spMetaCore
	
	CreatePackage $package
}





