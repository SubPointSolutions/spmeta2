param 
(
	 [bool]$dropWebApp = $false,
	 [bool]$dropSite = $false
)

cls

Write-Host "Loading SharePoint API"

$ver = $host | select version
if ($ver.Version.Major -gt 1) {$host.Runspace.ThreadOptions = "ReuseThread"} 
if ((Get-PSSnapin "Microsoft.SharePoint.PowerShell" -ErrorAction SilentlyContinue) -eq $null) {
	Add-PSSnapin "Microsoft.SharePoint.PowerShell"
}

#script setting
$g_script_RecreateWebApp = $dropWebApp
$g_script_RecreateSiteCollections = $dropSite

#global settings
$g_port = "31415"

$g_machineName = [Environment]::MachineName
$g_DomainName = [Environment]::UserDomainName
$g_UserName = [Environment]::UserName

# web app settings

	$g_webAppUrl = $g_machineName + ":" + $g_port;

	$g_webAppName = "SPMeta2 Sandbox - $g_port"
	$g_webAppPool = "$g_machineName - $g_port"
	$g_webAppHostHeader = $g_machineName
	$g_webAppPort = $g_port

	# should be managed account
	$g_webAppAppPoolAccount = "$g_DomainName\sp-farm"

	$g_webAppDatabaseServerName = $g_machineName
	$g_webAppDatabaseName = "WSS_Content_$g_port"

# site settings

	$g_siteCollectionOwner = [Environment]::UserName
	$g_siteCollectionTemplate = "STS#0"
	$g_siteCollectionLanguage = "1033"
	
	$g_siteUrls = @()
	$g_siteUrls += ("http://" + $g_machineName + ":" + $g_port);
	$g_siteUrls += ("http://" + $g_machineName + ":" + $g_port + "/sites/spmeta2");

# subweb settings

	$g_webTemplate = "STS#0"
	

	$g_siteRelativeWebUrls = @()
	$g_siteRelativeWebUrls += "/first"
	$g_siteRelativeWebUrls += "/first/second"
	$g_siteRelativeWebUrls += "/first/second/third"


function CreateWebApplication() 
{
	$WebAppName = $g_webAppName
	$WebAppAppPool = $g_webAppPool
   
	$WebAppHostHeader = $g_webAppHostHeader
	$WebAppPort = $g_webAppPort
	
	$WebAppAppPoolAccount = $g_webAppAppPoolAccount
	$WebAppDatabaseName = $g_webAppDatabaseName
	$WebAppDatabaseServer = $g_webAppDatabaseServerName

	New-SPWebApplication -Name $WebAppName -Port $WebAppPort -HostHeader $WebAppHostHeader -URL $WebAppHostHeader -ApplicationPool $WebAppAppPool -ApplicationPoolAccount (Get-SPManagedAccount $WebAppAppPoolAccount) -DatabaseName $WebAppDatabaseName -DatabaseServer $WebAppDatabaseServer
}

function EnsureSPMeta2SandboxWebApplication($recreate) {
 
	$webApp = GetSPMeta2SandboxWebApp $g_webAppUrl

	if($webApp -eq $null) {
	 
		Write-Host "Cannot find web application. at URL:[$g_webAppUrl]. Creating one." -fore yellow
		
		CreateWebApplication

	} else {

		Write-Host "Web application at URL:[$g_webAppUrl] exists." -fore green

		if($recreate -eq $true) {
			if($webApp -ne $null) {
			
				Write-Host "Deleting web application at URL:[$g_webAppUrl]." -fore yellow
				Remove-SPWebApplication $webApp -Confirm:$false

				CreateWebApplication
			}
		}
		
	}
}

function GetSPMeta2SandboxWebApp($url) {
	return Get-SPWebApplication | Where-Object { $_.Url.ToUpper().Contains($url.ToUpper()) -eq $true }
}

function LookupWebSite($url)
{
	 $webApp = GetSPMeta2SandboxWebApp $g_webAppUrl

	 return $webApp.Sites | Where-Object { $_.Url.ToUpper().EndsWith($url.ToUpper()) -eq $true }
}

function LookupWeb($site, $url) {

	$web = $site.AllWebs | Where-Object { $_.Url.ToUpper().EndsWith($url.ToUpper()) -eq $true }

	return $web 
}

function CreateWeb( $url) {
 
   $web = New-SPWeb  $url -Template $g_webTemplate

   return $web
}

function CreateSiteCollection($url) {
	
	$SiteCollectionName = "Root"
	$SiteCollectionURL = $url
	$SiteCollectionTemplate = $g_siteCollectionTemplate
	$SiteCollectionLanguage = $g_siteCollectionLanguage
	$SiteCollectionOwner = $g_siteCollectionOwner
 
	$site =  New-SPSite -URL $SiteCollectionURL -OwnerAlias $SiteCollectionOwner -Language $SiteCollectionLanguage -Template $SiteCollectionTemplate -Name $SiteCollectionName
	
	return $site;
}

function DisableFeature($web, $id) {

	$f = $web.Features[[guid]$id]

	if($f -ne $null) {
		Disable-SPFeature -Identity  $id -url $web.Url -Confirm:$false -force
	}
}

function EnsureSPMeta2SandboxSiteCollections($recreate) {
	
	Write-Host ""
	Write-Host "Ensuring sandbox site collections" -fore Green
   
	$siteUrls = $g_siteUrls

	foreach($url in $siteUrls) {

		$site = LookupWebSite $url
   
		if($recreate -eq $true) {
	
			if(  $site -ne $null) {

				Write-Host "Deleting site collection [$url]" -fore DarkYellow

				$siteId =  $site.ID

				Remove-spsite   $siteId  -Confirm:$false 
				#Remove-SPDeletedSite -Identity  $siteId  -Confirm:$false 

				$site  = $null
			}
		}

		if($site -eq $null) {
			
			Write-Host "Site [$url] does not exist." -fore Gray
			Write-Host "Creating site: [$url]" -fore DarkYellow
			
			$site = CreateSiteCollection $url               
		} else { 

			Write-Host "Site [$url] exists." -fore Gray

		}

		$webUrls = $g_siteRelativeWebUrls
		$rootWeb  = $site.RootWeb

        $rootWeb.EnsureUser($g_DomainName + "\" + $g_UserName)
        $rootWeb.CreateDefaultAssociatedGroups($g_DomainName + "\" + $g_UserName, "", "")

	    # MDS
		DisableFeature  $rootWeb "87294c72-f260-42f3-a41b-981a2ffce37a"

		foreach($webUrl in $webUrls) {

			$fullwebUrl = $url + $webUrl

			$web = LookupWeb $site $fullwebUrl 
		
			if($web -eq $null) {
			
				Write-Host "`tWeb [$fullwebUrl] does not exist." -fore Gray
				Write-Host "`tCreating web: [$fullwebUrl]" -fore DarkYellow
				
				$web = CreateWeb $fullwebUrl    
		
			} else {
				
				Write-Host "`tWeb [$fullwebUrl] exists." -fore Gray

			}

			# MDS
			DisableFeature $web "87294c72-f260-42f3-a41b-981a2ffce37a"
		   
		}

		Write-Host ""
	}    
}

EnsureSPMeta2SandboxWebApplication $g_script_RecreateWebApp
EnsureSPMeta2SandboxSiteCollections $g_script_RecreateSiteCollections

EnsureSPMeta2SandboxWebApplication 
EnsureSPMeta2SandboxSiteCollections 