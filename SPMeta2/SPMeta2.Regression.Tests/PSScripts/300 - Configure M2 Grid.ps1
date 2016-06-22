cls

$s_runnerLibrary = "SPMeta2.Containers.CSOM.dll"
#$s_runnerLibrary = "SPMeta2.Containers.SSOM.dll"

$s_username = [System.Environment]::GetEnvironmentVariable("SPMeta2_Grid_UserName", "Machine")
$s_password = [System.Environment]::GetEnvironmentVariable("SPMeta2_Grid_UserPassword", "Machine") 
$s_nodeList = [System.Environment]::GetEnvironmentVariable("SPMeta2_Grid_Nodes", "Machine") 

$s_passwordSecure = $s_password  | convertto-securestring -Force -AsPlainText

$s_cred = new-object -typename System.Management.Automation.PSCredential `
                     -argumentlist $s_username, $s_passwordSecure

$s_nodes = $s_nodeList.Split(',')

Configuration M2GridRunnerLibrariesClean {
    
    param(
       $nodeName,
       $runnerLibraries
    )

    Import-DscResource –ModuleName 'PSDesiredStateConfiguration'

    Node $nodeName {

        Environment RunnerLibraryRemove {
            Name = "SPMeta2_RunnerLibraries"
            Ensure = 'Absent'
        }
    }

    Node $nodeName {

        Environment SPMeta2_SSOM_WebApplicationUrlsRemove {
            Name = "SPMeta2_SSOM_WebApplicationUrls"
            Ensure = 'Absent'
        }
    }
}

Configuration M2GridRunnerLibraries {
    
    param(
       $nodeName,
       $runnerLibraries,
       $webAppUrls
    )

    Import-DscResource –ModuleName 'PSDesiredStateConfiguration'

    Node $nodeName {

        Environment RunnerLibrary {
            Name = "SPMeta2_RunnerLibraries"
            Ensure = 'Present'
            Value = $runnerLibraries
        }
    }
    
    Node $nodeName {

        Environment SPMeta2_SSOM_WebApplicationUrls {
            Name = "SPMeta2_SSOM_WebApplicationUrls"
            Ensure = 'Present'
            Value = $webAppUrls
        }
    }
}

Remove-item M2GridRunnerLibrariesClean -Force -Recurse -Confirm:$false
Remove-item M2GridRunnerLibraries -Force -Recurse -Confirm:$false

foreach($node in $s_nodes) {
    
    Write-Host "Compiling DSC for node [$node] and library:[$s_runnerLibrary]" -fore green
    
    M2GridRunnerLibrariesClean -nodeName $node  `
                               -runnerLibraries $s_runnerLibrary | out-null

    M2GridRunnerLibraries -nodeName $node  `
                          -webAppUrls ("http://" + $node + ":31417") `
                          -runnerLibraries $s_runnerLibrary | out-null
}

Write-Host "Applying DSC with library:[$s_runnerLibrary ]" -fore green

Start-DscConfiguration M2GridRunnerLibrariesClean -Credential $s_cred `
                           -Wait `
                           -Verbose

Start-DscConfiguration M2GridRunnerLibraries -Credential $s_cred `
                           -Wait `
                           -Verbose


