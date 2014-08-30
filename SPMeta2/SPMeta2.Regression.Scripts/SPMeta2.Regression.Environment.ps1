
[Environment]::SetEnvironmentVariable("SPMeta2_SSOM_WebApplicationUrl", "", "Machine")
[Environment]::SetEnvironmentVariable("SPMeta2_SSOM_SiteUrl", "", "Machine")
 

[Environment]::SetEnvironmentVariable("SPMeta2_O365_SiteUrl", "", "Machine")
[Environment]::SetEnvironmentVariable("SPMeta2_O365_UserName", "", "Machine")
[Environment]::SetEnvironmentVariable("SPMeta2_O365_Password", "", "Machine")

$runners = @()
$runners += "SPMeta2.Regression.Runners.SSOM.dll"
$runners += "SPMeta2.Regression.Runners.O365.dll"

[Environment]::SetEnvironmentVariable("SPMeta2_RunnerLibraries", [string]::Join(",", $runners), "Machine")
