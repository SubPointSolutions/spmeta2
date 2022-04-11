#region utils

function Check-AssemblyBaseline($assemblyFileName, $runtime, $assemblyFilePath, $baselines, $netRuntime) {

    if($baselines -eq $null) {
        throw "baselines is null"
    }

    # ensure Mono.Cecil
    $assemblies = @(
        "../SPMeta2.Dependencies/Mono.Cecil/Mono.Cecil.0.9.6.1/Mono.Cecil.dll",
        "../SPMeta2.Dependencies/Mono.Cecil/Mono.Cecil.0.9.6.1/Mono.Cecil.Mdb.dll",
        "../SPMeta2.Dependencies/Mono.Cecil/Mono.Cecil.0.9.6.1/Mono.Cecil.Pdb.dll",
        "../SPMeta2.Dependencies/Mono.Cecil/Mono.Cecil.0.9.6.1/Mono.Cecil.Rocks.dll"
    )

    $currentPath =  Get-ScriptDirectory

    foreach($path in $assemblies) {
        $fullPath = [System.IO.Path]::Combine( $currentPath, $path)
        [System.Reflection.Assembly]::LoadFile($fullPath) |out-null
    }

    # load the baseline
    $baseline = [xml](Get-Content "m2.buildbaseline.xml")
    $customBaseline = $baselines.Assemblies `
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

function Get-TimeStamp() {
    return $(get-date -f "yyyy-MM-dd HH:mm:ss") 
}

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

function Write-BInfo($msg, $fore = 'green') {
    
    $stamp = Get-TimeStamp

    if([string]::IsNullOrEmpty($msg) -eq $false) {
        $msg = "[$stamp] [INFO] $msg"
    } else {
        $msg = "[$stamp] [INFO]"
    }

    Write-Host $msg -fore $fore
}

function Get-EnvironmentVariable($name) {
	
	$result = [System.Environment]::GetEnvironmentVariable($name) 
	if($result -ne $null) { return $result; }

	$result = [System.Environment]::GetEnvironmentVariable($name, "Machine") 
	if($result -ne $null) { return $result; }

	$result = [System.Environment]::GetEnvironmentVariable($name, "Process") 
	if($result -ne $null) { return $result; }

	$result = [System.Environment]::GetEnvironmentVariable($name, "User") 
	if($result -ne $null) { return $result; }

	return $null
}

# https://www.appveyor.com/docs/build-phase
$g_isAppVeyor = ((Get-EnvironmentVariable "APPVEYOR") -ne $null)

#endregion