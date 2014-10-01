
function SPM2_SetRegressionEnvironment(
	$runnerAssemblies,
	
	$ssomWebAppUrl,
	$ssomSiteUrl,

	$o365SiteUrl,
	$o365UserName,
	$o365UserPassword	
) {

	Write-Host "Setting up SPmeta regression environment" -ForegroundColor Green

	Write-Host "`tSetting up Web Application url to [$ssomWebAppUrl]" -ForegroundColor Yellow
	[Environment]::SetEnvironmentVariable("SPMeta2_SSOM_WebApplicationUrl", $ssomWebAppUrl, "Machine")

	Write-Host "`tSetting up Site url to [$ssomSiteUrl]" -ForegroundColor Yellow
	[Environment]::SetEnvironmentVariable("SPMeta2_SSOM_SiteUrl", $ssomSiteUrl, "Machine")
 
	Write-Host ""

	Write-Host "`tSetting up O365 site url to [$o365SiteUrl]" -ForegroundColor Yellow
	[Environment]::SetEnvironmentVariable("SPMeta2_O365_SiteUrl", $o365SiteUrl, "Machine")
	Write-Host "`tSetting up O365 user name to [$o365UserName]" -ForegroundColor Yellow
	[Environment]::SetEnvironmentVariable("SPMeta2_O365_UserName", $o365UserName, "Machine")
	Write-Host "`tSetting up O365 user password to [$o365UserPassword]" -ForegroundColor Yellow
	[Environment]::SetEnvironmentVariable("SPMeta2_O365_Password", $o365UserPassword, "Machine")

	$runnerString = [string]::Join(",", $runnerAssemblies)	
	Write-Host "`tSetting up runner assemblies to [$runnerString]" -ForegroundColor Yellow
		
	[Environment]::SetEnvironmentVariable("SPMeta2_RunnerLibraries", $runnerString, "Machine")

	Write-Host "Completed" -ForegroundColor Green
}