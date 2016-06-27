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
     
            # load the baseline
            $baseline = [xml](Get-Content "m2.buildbaseline.xml")
            $customBaseline = $g_buildBaseline.Assemblies `
                                        | Where-Object { ($_.AssemblyFileName -eq $assemblyFileName) -and ($_.Runtime -eq $runtime) }

            if($baseline -eq $null) {
                throw "Cannot load baseline from m2.buildbaseline.xml"
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
                Write-BVerbose " - DefinitionTypeFullNames: [$($targetBaseline.DefinitionTypeFullNames.ChildNodes.Count)]"
                Write-BVerbose " - ModelHandlerTypeFullNames: [$($targetBaseline.ModelHandlerTypeFullNames.ChildNodes.Count)]"
            }

            # file check
            $projectFolder = [system.io.path]::GetDirectoryName($projectPath)
        
            $assemblyDirectory = $projectFolder  + "/bin/$configuration-$runtime/" 
            $assemblyPath = $assemblyDirectory  + "/$assemblyFileName" 
        
            if([system.io.file]::Exists($assemblyPath) -eq $false) {
                Write-BError ("Cannot find file:[$assemblyPath]")
                throw "Cannot find file:[$assemblyPath]"
            }

            # baseline check
            $tmpPath = $env:TEMP 
            $assemblyTemporaryDirectory = [System.IO.Path]::Combine($tmpPath, "m2.build.tmp")

            #[System.IO.Directory]::CreateDirectory($assemblyTemporaryDirectory) | out-null;
            #$assemblyTmpPath = [System.IO.Path]::Combine($assemblyTemporaryDirectory, [guid]::NewGuid().ToString("N") + ".dll")
            
            # should be in the same folder to be able to loas refs
            $assemblyTmpPath = [System.IO.Path]::Combine($assemblyDirectory, [guid]::NewGuid().ToString("N") + ".dll")
            Copy-Item $assemblyPath $assemblyTmpPath -Force -Confirm:$false

            [System.Reflection.Assembly]$assembly = [System.Reflection.Assembly]::LoadFile($assemblyTmpPath)

            $refs = $assembly.GetReferencedAssemblies()
            $assemblyRefs = @()

            
            foreach($ref in $refs) {

                #if(($ref.Name.StartsWith("Microsoft.SharePoint") -eq $true) -or ($ref.Name.StartsWith("SPMeta2") -eq $true)) {
                if(($ref.Name.StartsWith("Microsoft.SharePoint") -eq $true) ) {

                    $name = ($ref.Name + ".dll")
                    $assemblyRefsPaths = [System.IO.Directory]::GetFiles($assemblyDirectory, $name);

                    if($assemblyRefsPaths -ne $null -and $assemblyRefsPaths.Count -gt 0) {
                        $assemblyRefs += $assemblyRefsPaths[0]
                    }
                }
            }
            

            foreach($assemblyRef in $assemblyRefs) {

                Write-BVerbose "Copying ref assembly:[$assemblyRef]"
                [System.IO.Directory]::CreateDirectory($assemblyTemporaryDirectory) | out-null;
                $assemblyTmpPath = [System.IO.Path]::Combine($assemblyTemporaryDirectory, [guid]::NewGuid().ToString("N") + ".dll")
            
                $assemblyTmpPath = [System.IO.Path]::Combine($assemblyDirectory, [guid]::NewGuid().ToString("N") + ".dll")
                Copy-Item $assemblyRef $assemblyTmpPath -Force -Confirm:$false

                Write-BVerbose "Loading ref assembly:[$assemblyTmpPath]"
                $asm = [System.Reflection.Assembly]::LoadFile($assemblyTmpPath) 
                if($asm -eq $null) {
                    throw ("Cannot load ref asembly:[$assemblyTmpPath)]")
                }
            } 

            if($assembly -eq $null) {
                throw "Cannot load assembly from file:[$assemblyTmpPath]"
            } else {
                Write-BVerbose "Loaded TMP assembly from file:[$assemblyTmpPath]"
            }
        
            $allTypes = $null
            try {
                $allTypes = $assembly.GetTypes() 
            } catch {
                
                $_.Exception
                $_.Exception.InnerException.LoaderExceptions
                #$_.Exception.LoaderExceptions[0]
            }

            if($allTypes -eq $null) {
                Write-BError "Cannot load types from assembly:[$assemblyTmpPath]"
                throw ("Cannot load types from assembly:[$assemblyTmpPath]")
            }
            else {
                Write-BVerbose "Found [$($allTypes.Count)] types"

                foreach($type in $allTypes ) {
                   # Write-BVerbose "`t[$($type.AssemblyQualifiedName)]"
                }
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

        "CheckBaseline" = $false;
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