clscd "$PSScriptRoot". "$PSScriptRoot\_config.ps1"
. "$PSScriptRoot\_sys.common.ps1"

Write-Host "Ensuring M2 test web application with the following settings:" -fore Green
M2ShowSettings $g_M2WebAppSettings

Write-Host "Ensuring M2 test web application with the following settings:" -fore Green

$g_M2WebAppSettings.ShouldRecreateWebApplicaiton = $true
$g_M2WebAppSettings.ShouldRecreateSiteCollection = $true

EnsureSPMeta2SandboxWebApplication $g_M2WebAppSettings.ShouldRecreateWebApplicaiton
EnsureSPMeta2SandboxSiteCollections $g_M2WebAppSettings.ShouldRecreateSiteCollection