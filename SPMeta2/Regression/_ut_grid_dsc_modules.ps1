cls

$ScriptDirectory = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
. "$ScriptDirectory/_helpers.ps1"

configuration SPMeta2_DSCModules {

    param (
        [Parameter(Mandatory=$true)] [ValidateNotNullorEmpty()] [string] $NodeName,
        [Parameter(Mandatory=$true)] [ValidateNotNullorEmpty()] [PSCredential] $RunAsCredential
    )

    Import-DscResource -ModuleName PowerShellModule -ModuleVersion 0.3

    Node $NodeName {

        PSModuleResource SharePointDSC {

            Module_Name = "SharePointDSC"
            RequiredVersion = "1.6.0.0"
            PsDscRunAsCredential = $RunAsCredential
        }

        PSModuleResource PowershellYaml {

            Module_Name = "powershell-yaml"
            RequiredVersion = "0.1"
            PsDscRunAsCredential = $RunAsCredential
            DependsOn = "[PSModuleResource]SharePointDSC"
        }

        PSModuleResource PSParallel {

            Module_Name = "PSParallel"
            RequiredVersion = "2.2.2"
            PsDscRunAsCredential = $RunAsCredential
            DependsOn = "[PSModuleResource]PowershellYaml"
        }
    }
}

Apply-Dsc-Configuration -name SPMeta2_DSCModules `
                        -nodeNames $dsc_nodeNames `
                        -isVerbose $true