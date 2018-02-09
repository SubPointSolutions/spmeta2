$ErrorActionPreference = "Stop"

$coreScript = "$($PSScriptRoot)/local.config.ps1"
if(!(Test-Path $coreScript)) { throw "Cannot find core script: $coreScript"}
. $coreScript

$api = "O365v16"
$configName = "Default.SharePointOnline"

Apply-M2RegressionSettings -config $spMeta2Config `
                -configName $configName `
                -api $api `
                -skipTestData $false `
                -skipSPWebApp $true `
                -skipSPPostCheck $true `
                -skipSPServicesFix $true `
                -skipEnvVariables $false `
                -deleteSPWebApp $false 