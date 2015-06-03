clscd "$PSScriptRoot"$PSScriptRoot. "$PSScriptRoot\_config.ps1"
. "$PSScriptRoot\_sys.common.ps1"

Write-Host "Configuring M2 test environment with the following settings:" -fore Green
M2ShowSettings $g_M2TestEnvironment

# CSOM, SSOM, O365
SetupSPMeta2RegressionTestEnvironment "SSOM"
#SetupSPMeta2RegressionTestEnvironment "CSOM"
#SetupSPMeta2RegressionTestEnvironment "O365"