Write-Host "Running regression on NuGet packages" -fore Green

# additional assemblies

#   $assemblies = @(
#         "../SPMeta2.Dependencies/Mono.Cecil/Mono.Cecil.0.9.6.1/Mono.Cecil.dll",
#         "../SPMeta2.Dependencies/Mono.Cecil/Mono.Cecil.0.9.6.1/Mono.Cecil.Mdb.dll",
#         "../SPMeta2.Dependencies/Mono.Cecil/Mono.Cecil.0.9.6.1/Mono.Cecil.Pdb.dll",
#         "../SPMeta2.Dependencies/Mono.Cecil/Mono.Cecil.0.9.6.1/Mono.Cecil.Rocks.dll"
#     )



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

$g_buildBaseline = @{

	Assemblies = @(
		
        #region SPMeta2.dll
		@{
            AssemblyFileName = "SPMeta2.dll";
			Runtime = "14";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

		@{
            AssemblyFileName = "SPMeta2.dll";
			Runtime = "15";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

		@{
            AssemblyFileName = "SPMeta2.dll";
			Runtime = "16";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

		@{
            AssemblyFileName = "SPMeta2.dll";
			Runtime = "365";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

        #end region

        # SPMeta2.Standard
        @{
            AssemblyFileName = "SPMeta2.Standard.dll";
			Runtime = "14";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

        @{
            AssemblyFileName = "SPMeta2.Standard.dll";
			Runtime = "15";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

        @{
            AssemblyFileName = "SPMeta2.Standard.dll";
			Runtime = "16";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},
        
        @{
            AssemblyFileName = "SPMeta2.Standard.dll";
			Runtime = "365";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

        # SPMeta2.SSOM.dll
		@{
            AssemblyFileName = "SPMeta2.SSOM.dll";
			Runtime = "14";
            
			ExcludedHandlers = @(
			    "SPMeta2.SSOM.ModelHandlers.ComposedLookItemLinkModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a",
                
                "SPMeta2.SSOM.ModelHandlers.AppModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.ModelHandlers.AppPrincipalModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                
                "SPMeta2.SSOM.ModelHandlers.Fields.GeolocationFieldModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.ModelHandlers.Fields.OutcomeChoiceFieldModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                
                "SPMeta2.SSOM.ModelHandlers.InformationRightsManagementSettingsModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.ModelHandlers.ComposedLookItemModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                
                "SPMeta2.SSOM.ModelHandlers.SP2013WorkflowDefinitionHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.ModelHandlers.SP2013WorkflowSubscriptionDefinitionModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                
                "SPMeta2.SSOM.ModelHandlers.Webparts.BlogLinksWebPartModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.ModelHandlers.Webparts.ClientWebPartModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.ModelHandlers.Webparts.GettingStartedWebPartModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.ModelHandlers.Webparts.PictureLibrarySlideshowWebPartModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.ModelHandlers.Webparts.SPTimelineWebPartModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.ModelHandlers.Webparts.ScriptEditorWebPartModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
			);

			ExcludedDefinitions =  @(
			
			);
		},
        
        @{
            AssemblyFileName = "SPMeta2.SSOM.dll";
			Runtime = "15";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

        @{
            AssemblyFileName = "SPMeta2.SSOM.dll";
			Runtime = "16";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

        # SPMeta2.SSOM.Standard.dll
		@{
            AssemblyFileName = "SPMeta2.SSOM.Standard.dll";
			Runtime = "14";
            
			ExcludedHandlers = @(
                "SPMeta2.SSOM.Standard.ModelHandlers.ManagedPropertyModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.Standard.ModelHandlers.ImageRenditionModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                
                "SPMeta2.SSOM.Standard.ModelHandlers.SearchResultModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.Standard.ModelHandlers.SearchConfigurationModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.SSOM.Standard.ModelHandlers.WebNavigationSettingsModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                
                "SPMeta2.SSOM.Standard.ModelHandlers.Webparts.CommunityAdminWebPartModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.Standard.ModelHandlers.Webparts.CommunityJoinWebPartModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.Standard.ModelHandlers.Webparts.ContentBySearchWebPartModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.SSOM.Standard.ModelHandlers.Webparts.MyMembershipWebPartModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.Standard.ModelHandlers.Webparts.RefinementScriptWebPartModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.Standard.ModelHandlers.Webparts.ResultScriptWebPartModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.SSOM.Standard.ModelHandlers.Webparts.SearchBoxScriptWebPartModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.Standard.ModelHandlers.Webparts.SearchNavigationWebPartModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                
                "SPMeta2.SSOM.Standard.ModelHandlers.Webparts.SiteFeedWebPartModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.SSOM.Standard.ModelHandlers.Webparts.ProjectSummaryWebPartModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
				"SPMeta2.SSOM.Standard.ModelHandlers.DesignPackageModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
			);

			ExcludedDefinitions =  @(
			
			);
		},

        @{
            AssemblyFileName = "SPMeta2.SSOM.Standard.dll";
			Runtime = "15";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

        @{
            AssemblyFileName = "SPMeta2.SSOM.Standard.dll";
			Runtime = "16";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

        # SPMeta2.CSOM.dll
        @{
            AssemblyFileName = "SPMeta2.CSOM.dll";
			Runtime = "14";
            
			ExcludedHandlers = @(
			    "SPMeta2.CSOM.ModelHandlers.AppModelHandler, SPMeta2.CSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.CSOM.ModelHandlers.AppPrincipalModelHandler, SPMeta2.CSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.CSOM.ModelHandlers.ClearRecycleBinModelHandler, SPMeta2.CSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.CSOM.ModelHandlers.ComposedLookItemLinkModelHandler, SPMeta2.CSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.CSOM.ModelHandlers.EventReceiverModelHandler, SPMeta2.CSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.CSOM.ModelHandlers.Fields.OutcomeChoiceFieldModelHandler, SPMeta2.CSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.CSOM.ModelHandlers.Fields.GeolocationFieldModelHandler, SPMeta2.CSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.CSOM.ModelHandlers.InformationRightsManagementSettingsModelHandler, SPMeta2.CSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.CSOM.ModelHandlers.RegionalSettingsModelHandler, SPMeta2.CSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.CSOM.ModelHandlers.SP2013WorkflowSubscriptionDefinitionModelHandler, SPMeta2.CSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.CSOM.ModelHandlers.SupportedUICultureModelHandler, SPMeta2.CSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.CSOM.ModelHandlers.SP2013WorkflowDefinitionHandler, SPMeta2.CSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
			);

			ExcludedDefinitions =  @(
			
			);
		},

        @{
            AssemblyFileName = "SPMeta2.CSOM.dll";
			Runtime = "15";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

        
        @{
            AssemblyFileName = "SPMeta2.CSOM.dll";
			Runtime = "16";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

        
        @{
            AssemblyFileName = "SPMeta2.CSOM.dll";
			Runtime = "365";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

        # SPMeta2.CSOM.Standard.dll
        @{
            AssemblyFileName = "SPMeta2.CSOM.Standard.dll";
			Runtime = "14";
            
			ExcludedHandlers = @(
			    "SPMeta2.CSOM.Standard.ModelHandlers.Fields.TaxonomyFieldModelHandler, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                
                "SPMeta2.CSOM.Standard.ModelHandlers.ImageRenditionModelHandler, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.CSOM.Standard.ModelHandlers.PageLayoutAndSiteTemplateSettingsModelHandler, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.CSOM.Standard.ModelHandlers.SearchConfigurationModelHandler, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                
                "SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy.TaxonomyGroupModelHandler, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy.TaxonomyTermLabelModelHandler, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy.TaxonomyTermModelHandler, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy.TaxonomyTermSetModelHandler, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy.TaxonomyTermStoreModelHandler, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.CSOM.Standard.ModelHandlers.WebNavigationSettingsModelHandler, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.CSOM.Standard.ModelHandlers.TermGroupModelHost, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.CSOM.Standard.ModelHandlers.TermModelHost, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.CSOM.Standard.ModelHandlers.TermSetModelHost, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.CSOM.Standard.ModelHandlers.TermStoreModelHost, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.CSOM.Standard.ModelHandlers.SandboxSolutionModelHandler, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
				"SPMeta2.CSOM.Standard.ModelHandlers.DocumentSetModelHandler, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.CSOM.Standard.ModelHandlers.DesignPackageModelHandler, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

			);

			ExcludedDefinitions =  @(
			
			);
		},

         @{
            AssemblyFileName = "SPMeta2.CSOM.Standard.dll";
			Runtime = "15";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

        
        @{
            AssemblyFileName = "SPMeta2.CSOM.Standard.dll";
			Runtime = "16";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

        
        @{
            AssemblyFileName = "SPMeta2.CSOM.Standard.dll";
			Runtime = "365";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		}
	)
}

$currentDirPath = Get-ScriptDirectory

$defaultBaselineXmlFile = ""
$defautPackagedFolder = "build-artifact-nuget-packages"

$expectedNuGetPackagesCount = 12

function EnsureDefaultBaselinePath() {
    $currentDirPath = Get-ScriptDirectory
    $script:defaultBaselineXmlFile = [System.IO.Path]::GetFullPath( [System.IO.Path]::Combine( $currentDirPath, "../../SPMeta2.Build/m2.buildbaseline.xml") )
}

EnsureDefaultBaselinePath

function Write-BError($msg, $fore = 'red') {
    
    $stamp = Get-TimeStamp

    if([string]::IsNullOrEmpty($msg) -eq $false) {
        $msg = "[$stamp] [ERROR] $msg"
    } else {
        $msg = "[$stamp] [ERROR]"
    }

    Write-Host $msg -fore $fore
}

function Write-BVerbose($msg, $fore = 'gray') {
    
    $stamp = Get-TimeStamp

    if([string]::IsNullOrEmpty($msg) -eq $false) {
        $msg = "[$stamp] [Verbose] $msg"
    } else {
        $msg = "[$stamp] [Verbose]"
    }

    Write-Host $msg -fore $fore
}

function Get-TimeStamp() {
    return $(get-date -f "yyyy-MM-dd HH:mm:ss") 
}


function Write-BInfo($msg, $fore = 'green') {
    
    $stamp = Get-TimeStamp

    if([string]::IsNullOrEmpty($msg) -eq $false) {
        $msg = "[$stamp] [INFO] $msg"
    } else {
        $msg = "[$stamp] [INFO]"
    }

    Write-Host $msg -fore $fore
}

function Check-PackageBaseline($nugetFilePath) {

    if( !(Test-Path $nugetFilePath) ) {
        throw "Cannot find file:[$nugetFilePath]"
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
}

function Check-AssemblyBaseline($assemblyFileName, $runtime, $assemblyFilePath, $baselines, $netRuntime) {

    if($baselines -eq $null) {
        throw "baselines is null"
    }

    # ensure Mono.Cecil
    $assemblies = @(
        "../../SPMeta2.Dependencies/Mono.Cecil/Mono.Cecil.0.9.6.1/Mono.Cecil.dll",
        "../../SPMeta2.Dependencies/Mono.Cecil/Mono.Cecil.0.9.6.1/Mono.Cecil.Mdb.dll",
        "../../SPMeta2.Dependencies/Mono.Cecil/Mono.Cecil.0.9.6.1/Mono.Cecil.Pdb.dll",
        "../../SPMeta2.Dependencies/Mono.Cecil/Mono.Cecil.0.9.6.1/Mono.Cecil.Rocks.dll"
    )

    $currentPath =  Get-ScriptDirectory

    
    foreach($path in $assemblies) {
        
        $fullPath = [System.IO.Path]::GetFullPath( [System.IO.Path]::Combine( $currentPath, $path) );

        Write-BInfo("Loading assembly:[$fullPath]")    

        if(!(Test-Path($fullPath))) {
            throw "Cannot find path:[$fullPath]"
        }

        [System.Reflection.Assembly]::LoadFile($fullPath) |out-null
    }

    # load the baseline
    $baseline = [xml](Get-Content $defaultBaselineXmlFile )
    $customBaseline = $baselines.Assemblies `
                                  | Where-Object { ($_.AssemblyFileName -eq $assemblyFileName) -and ($_.Runtime -eq $runtime) }

    if($baseline -eq $null) {
        throw "Cannot load baseline from [$defaultBaselineXmlFile ]"
    }

    if($customBaseline -eq $null) {
      #$g_buildBaseline.Assemblies
     throw "Cannot find custom baseline form assembly [$assemblyFileName] and runtime [$runtime]"
    }

   
        
    $targetBaseline = $baseline.ArrayOfBuildBaseline.BuildBaseline `
                                    | Where-Object { $_.AssemblyFileName -eq $assemblyFileName }

    if($targetBaseline -eq $null) {
        $projectName
        throw ("Cannot find baseline for project:[$projectName]")
    } else {

        Write-BVerbose "Target baseline: [$($targetBaseline.AssemblyFileName)]"
        Write-BVerbose "Custom baseline runtime: [$($customBaseline.Runtime)]"

        Write-BVerbose " - DefinitionTypeFullNames: [$($targetBaseline.DefinitionTypeFullNames.ChildNodes.Count)]"
        Write-BVerbose " - ModelHandlerTypeFullNames: [$($targetBaseline.ModelHandlerTypeFullNames.ChildNodes.Count)]"
    }

    $assembly = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($assemblyFilePath);
    $typeReferences = $assembly.MainModule.GetTypes();

    if($netRuntime -ne $null) {
        Write-BVerbose "Checking expecting .NET runtime:[$netRuntime]"

        $targetFrameworkAttr = $assembly.CustomAttributes | where-object { $_.AttributeType.Name -eq "TargetFrameworkAttribute" }

        $framework = "v3.5"

        if($targetFrameworkAttr -eq $null) {
            # they don't have TargetFrameworkAttribute attr
            $framework = "v3.5"
            #throw "Cannot find TargetFrameworkAttribute for assembly:[$assemblyFilePath]"
        } else {
            $targetFrameworkValue = $targetFrameworkAttr.ConstructorArguments[0].Value;
            $framework = $targetFrameworkValue.ToString().Split(',')[1].Split('=')[1];
        }

        if($framework -ne $netRuntime) {
            throw "[FAILED]: expecting NET framework [$netRuntime] and it was:[$framework]"
        } else {
            Write-BVerbose "[+]: expecting NET framework [$netRuntime] and it was:[$framework]"
        }

    } else {
        Write-BVerbose "Skipping expecting .NET runtime check"
    }

    $allTypes = @()

    foreach($type in $typeReferences)
    {
        $allTypes += @{
            AssemblyQualifiedName = ($type.FullName + ", " +  $assembly.Name)
        };
    }

    foreach($type in $allTypes){
        #Write-BVerbose $type.AssemblyQualifiedName
    }

            Write-BInfo "Checking definition types..."
            foreach($typeNode in $targetBaseline.DefinitionTypeFullNames.ChildNodes) {
               $typeFullName = $typeNode.InnerText;

               $type = $allTypes | where-object { $_.AssemblyQualifiedName -eq $typeFullName };
               $hasType = $type -ne $null

               if($hasType -eq $true)
               {
                    #Write-BVerbose "[+] Found type:[$typeFullName]"
               } else {

                    Write-BVerbose "[-] CANNOT find type:[$typeFullName]"
                    throw ("[-] CANNOT find type:[$typeFullName]")
               }
           
            }

            Write-BInfo "Checking model handler types..."
            foreach($typeNode in $targetBaseline.ModelHandlerTypeFullNames.ChildNodes) {
               $typeFullName = $typeNode.InnerText;

               #$assembly.GetTypes()

               $type = $allTypes | where-object { $_.AssemblyQualifiedName -eq $typeFullName };
               $hasType = $type -ne $null

               if($hasType -eq $true)
               {
                    #Write-BVerbose "[+] Found type:[$typeFullName]"
               } else {

                    # is special type?
                    $isExcludedModelHandler = $customBaseline.ExcludedHandlers.Contains($typeFullName);
                
                    if($isExcludedModelHandler -eq $true) {
                        Write-BInfo "[!] Excluded handler for runtime [$($customBaseline.Runtime)] and assembly [$($customBaseline.AssemblyFileName)] Type:[$typeFullName]"
                    }
                    else {
                        Write-BVerbose "[-] CANNOT find type:[$typeFullName]"
                        throw ("[-] CANNOT find type:[$typeFullName]")
                    }
               }
           
            }

}

Describe "regression.nuget" {
  
    # generic
    It "Expect [$expectedNuGetPackagesCount] NuGet packages" {
        
        $files = Get-ChildItem -Path $defautPackagedFolder -Filter "*.nupkg"
        $filesCount = $files.Count   
        
        $filesCount | Should Be $expectedNuGetPackagesCount 
    }

    #NET version within folders
    It "Packages baseline" {
        
        $files = Get-ChildItem -Path $defautPackagedFolder -Filter "*.nupkg"
        $filesCount = $files.Count
        
        foreach($file in $files) {
            Check-PackageBaseline $file.FullName
        }
    }
}