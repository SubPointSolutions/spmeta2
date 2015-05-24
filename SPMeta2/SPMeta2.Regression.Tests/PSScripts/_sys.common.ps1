. "$PSScriptRoot\_config.ps1"

Write-Host "Loading SharePoint API" -fore Green
$ver = $host | select version
if ($ver.Version.Major -gt 1) {$host.Runspace.ThreadOptions = "ReuseThread"} 
if ((Get-PSSnapin "Microsoft.SharePoint.PowerShell" -ErrorAction SilentlyContinue) -eq $null) {
	Add-PSSnapin "Microsoft.SharePoint.PowerShell"
}


function M2ShowSettings($obj) {

    $props = $obj  | Get-Member -type NoteProperty 

    foreach($prop in $props) {
        $value=$obj."$($prop.Name)"
        Write-Host "`t[$($prop.Name)]:`t[$value]" -fore Gray
    }
}

#script setting
$g_script_RecreateWebApp = $g_M2WebAppSettings.ShouldRecreateWebApplicaiton
$g_script_RecreateSiteCollections = $g_M2WebAppSettings.ShouldRecreateSiteCollection

#global settings
$g_port = $g_M2WebAppSettings.WebApplicationPort

$g_machineName = $g_M2WebAppSettings.MachineName
$g_DomainName = $g_M2WebAppSettings.DomainName
$g_webAppDatabaseServerName = $g_M2WebAppSettings.SqlServerMachineName

# web app settings

	$g_webAppUrl = $g_machineName + ":" + $g_port;

	$g_webAppName = "SPMeta2 Sandbox - $g_port"
	$g_webAppPool = "$g_machineName - $g_port"
	$g_webAppHostHeader = $g_machineName
	$g_webAppPort = $g_port

	# should be managed account
	$g_webAppAppPoolAccount =  $g_M2WebAppSettings.WebApplicationPoolAccountName

	#$g_webAppDatabaseServerName = $g_machineName
	$g_webAppDatabaseName = "WSS_Content_$g_port"

# site settings

	$g_siteCollectionOwner = $g_M2WebAppSettings.SiteCollectionOwner 
	
    $g_siteCollectionTemplate = $g_M2WebAppSettings.SiteCollectionTemplate
	$g_siteCollectionLanguage = $g_M2WebAppSettings.SiteCollectionLCID
	
	$g_siteUrls = @()
	$g_siteUrls += ("http://" + $g_machineName + ":" + $g_port);
	$g_siteUrls += ("http://" + $g_machineName + ":" + $g_port + "/sites/m2");

# subweb settings

	$g_webTemplate = "STS#0"
	

	$g_siteRelativeWebUrls = @()
	$g_siteRelativeWebUrls += "/first"
	$g_siteRelativeWebUrls += "/first/second"
	$g_siteRelativeWebUrls += "/first/second/third"


# creates a new web app
function CreateWebApplication() 
{
	$WebAppName = $g_webAppName
	$WebAppAppPool = $g_webAppPool
   
	$WebAppHostHeader = $g_webAppHostHeader
	$WebAppPort = $g_webAppPort
	
	$WebAppAppPoolAccount = $g_webAppAppPoolAccount
	$WebAppDatabaseName = $g_webAppDatabaseName
	$WebAppDatabaseServer = $g_webAppDatabaseServerName

    $WebAppName
    $WebAppPort
    $WebAppHostHeader
    $WebAppAppPool 
    (Get-SPManagedAccount $WebAppAppPoolAccount)
    $WebAppDatabaseName
    $WebAppDatabaseServer 

	New-SPWebApplication -Name $WebAppName -Port $WebAppPort -HostHeader $WebAppHostHeader -URL $WebAppHostHeader -ApplicationPool $WebAppAppPool -ApplicationPoolAccount (Get-SPManagedAccount $WebAppAppPoolAccount) -DatabaseName $WebAppDatabaseName -DatabaseServer $WebAppDatabaseServer 
}

# ensures security groups on the site
function EnsureSecurityGroup($web, $groupName) {

    $group = $web.SiteGroups[$groupName];

    if($group -eq $null) {
        $web.SiteGroups.Add($groupName, $web.Site.Owner, $web.Site.Owner, $groupName)
        $group = $web.SiteGroups[$groupName];
    }

    return $group
}

# ensures a web app
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

function EnableFeature($web, $id) {

	$f = $web.Features[[guid]$id]

	if($f -eq $null) {
		Enable-SPFeature -Identity  $id -url $web.Url -Confirm:$false -force
	}
}

function DisableFeature($web, $id) {

	$f = $web.Features[[guid]$id]

	if($f -ne $null) {
		Disable-SPFeature -Identity  $id -url $web.Url -Confirm:$false -force
	}
}

function EnsureSiteCollectionAdministrators($site, $logins) {

    foreach($login in $logins) {
        
        Write-Host "`tEnsuring site admin: $login" -ForegroundColor Gray;
      
        $user = $site.RootWeb.EnsureUser($login);
        if($user.IsSiteAdmin -ne $true)
        {
            $user.IsSiteAdmin = $true;
            $user.Update();
            #Write-Host "User is now site collection admin for $site" -ForegroundColor Green;
        }
        else
        {
            #Write-Host "User is already site collection admin for $site" -ForegroundColor DarkYellow;
        }
 
        #Write-Host "Current Site Collection Admins for site: " $site.Url " " $site.RootWeb.SiteAdministrators;
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
        
        EnsureSiteCollectionAdministrators $site $g_M2WebAppSettings.SiteCollectionAdministrators
        EnsureAssociatedGroups $site.RootWeb

		$webUrls = $g_siteRelativeWebUrls
		$rootWeb  = $site.RootWeb

        # disabling MDS feature    
		DisableFeature  $rootWeb "87294c72-f260-42f3-a41b-981a2ffce37a"
        # eabling wiki page lib
        EnableFeature   $rootWeb "00bfea71-d8fe-4fec-8dad-01c19a6e4053"

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

             Write-Host "Ensuring Associated groups..."

             if($web.HasUniqueRoleAssignments -eq $false) {
                $web.RoleDefinitions.BreakInheritance($true,$true);
             }

             EnsureAssociatedGroups $web

			 DisableFeature $web "87294c72-f260-42f3-a41b-981a2ffce37a"
             EnableFeature   $web "00bfea71-d8fe-4fec-8dad-01c19a6e4053"
		   
		}

		Write-Host ""
	}    
}

function EnsureAssociatedGroups($web) {

            $assOwnerGroup = EnsureSecurityGroup $web "SPMeta2 AssociatedOwnerGroup"
             $web.AssociatedOwnerGroup = $assOwnerGroup

             $assMemberGroup = EnsureSecurityGroup $web "SPMeta2 AssociatedMemberGroup"
             $web.AssociatedMemberGroup  = $assMemberGroup

             $assVisitorGroup = EnsureSecurityGroup $web "SPMeta2 AssociatedVisitorGroup"
             $web.AssociatedVisitorGroup  = $assVisitorGroup

             $web.Update()

}

# test environment settings

$envType = $g_M2TestEnvironment.EnvironmentType
$o365RuntimePath = "$PSScriptRoot\..\..\SPMeta2.Dependencies\SharePoint\SP2013 - 15.0.4569.1000\CSOM"

$o365_UserName = $g_M2TestEnvironment.O365UserName
$o365_UserPassword = $g_M2TestEnvironment.O365UserPassword

$serverName = $g_M2WebAppSettings.MachineName
$sqlServerName = $g_M2WebAppSettings.SqlServerMachineName

$serverName = [Environment]::MachineName

function SetSqlServerName($name)
{
    SetEnvironmentVar "SPMeta2_DefaultSqlServerName" $name
}

function SetTestOnPremTestLogins()
{
    $logins = $g_M2TestEnvironment.OnPremTestActiveDirectoryLogins
    $loginsString = [string]::Join(",", $logins);

    SetEnvironmentVar "SPMeta2_DefaultTestUserLogins" $loginsString
}

function SetTestOnPremTestADGroups()
{
    $groups = $g_M2TestEnvironment.OnPremTestActiveDirectoryGroups
    $groupsString = [string]::Join(",", $groups);

    SetEnvironmentVar "SPMeta2_DefaultTestADGroups" $groupsString
}

function SetEnvironmentVar($name, $value) {

    Write-Host "`tSetting [$name] - [$value]" -fore Gray
    [Environment]::SetEnvironmentVariable($name, $value, "Machine")
}

function SetSSOMManagedMetadataApplicationParams($siteUrl) {

    $session = Get-SPTaxonomySession -Site $siteUrl
    $store = $session.DefaultSiteCollectionTermStore;
    
    Write-Host "Setting up taxonomy store vars" -fore Yellow

    SetEnvironmentVar "SPMeta2_DefaultTaxonomyStoreId"  $store.Id
    SetEnvironmentVar "SPMeta2_DefaultTaxonomyStoreName"  $store.Name
}

function SetupSSOMEnvironment() {
    
    Write-Host "Setting up SSOM environment" -fore Yellow


    SetTestOnPremTestLogins
    SetTestOnPremTestADGroups

    SetEnvironmentVar "SPMeta2_RunnerLibraries" "SPMeta2.Containers.SSOM.dll"
    SetSqlServerName $sqlServerName

	$webAppUrls = $g_M2TestEnvironment.SSOMWebApplicationUrls
	$siteUrls = $g_M2TestEnvironment.SSOMSiteUrls
    $webUrls = $g_M2TestEnvironment.SSOMWebUrls	
	
    SetSSOMManagedMetadataApplicationParams $siteUrls[0]
	 
	$webAppUrlsValue = $webAppUrls -join ','
	$siteUrlsValue =  $siteUrls -join ','
	$webUrlsValue = $webUrls -join ','
    
    SetEnvironmentVar "SPMeta2_SSOM_WebApplicationUrls" $webAppUrlsValue 
    SetEnvironmentVar "SPMeta2_SSOM_SiteUrls" $siteUrlsValue 
    SetEnvironmentVar "SPMeta2_SSOM_WebUrls" $webUrlsValue 
}

function SetupCSOMEnvironment() {


    Write-Host "Setting up CSOM environment" -fore Yellow
    
    SetTestOnPremTestLogins
    SetTestOnPremTestADGroups

    SetEnvironmentVar "SPMeta2_RunnerLibraries" "SPMeta2.Containers.CSOM.dll"
    SetSqlServerName $sqlServerName

	$webAppUrls = $g_M2TestEnvironment.CSOMWebApplicationUrls
	$siteUrls = $g_M2TestEnvironment.CSOMSiteUrls
    $webUrls = $g_M2TestEnvironment.CSOMWebUrls	

    SetSSOMManagedMetadataApplicationParams $siteUrls[0]
	 
	$webAppUrlsValue = $webAppUrls -join ','
	$siteUrlsValue =  $siteUrls -join ','
	$webUrlsValue = $webUrls -join ','
    
    SetEnvironmentVar "SPMeta2_CSOM_SiteUrls" $siteUrlsValue
    SetEnvironmentVar "SPMeta2_CSOM_WebUrls" $webUrlsValue
    SetEnvironmentVar "SPMeta2_CSOM_UserName" ""
    SetEnvironmentVar "SPMeta2_CSOM_Password" ""
}

function InitO365Runtime() {
    $files = [System.IO.Directory]::GetFiles($o365RuntimePath, "*.dll")

    foreach($filePath in $files) {
        Write-Host "`tLoading assembly: [$filePath]"
        $a = [System.Reflection.Assembly]::LoadFile($filePath)
    }
}

function SetO365MManagedMetadataApplicationParams($siteUrl) {

    InitO365Runtime

    $secO365_UserPassword = ConvertTo-SecureString $o365_UserPassword -AsPlainText -Force

    $context = New-Object Microsoft.SharePoint.Client.ClientContext($siteUrl)
    $credentials = New-Object Microsoft.SharePoint.Client.SharePointOnlineCredentials($o365_UserName, $secO365_UserPassword)
    
    $context.Credentials = $credentials

    $taxSession = [Microsoft.SharePoint.Client.Taxonomy.TaxonomySession]::GetTaxonomySession($context)
    $store = $taxSession.GetDefaultSiteCollectionTermStore();

    $context.Load($store)
    $context.ExecuteQuery()

    Write-Host "Setting up taxonomy store vars" -fore Yellow

    SetEnvironmentVar "SPMeta2_DefaultTaxonomyStoreId"  $store.Id
    SetEnvironmentVar "SPMeta2_DefaultTaxonomyStoreName"  $store.Name    
}

function SetupO365v16Environment() {

    Write-Host "Setting up O365 environment" -fore Yellow

    SetEnvironmentVar "SPMeta2_RunnerLibraries" "SPMeta2.Containers.O365v16.dll"

    $o365_siteUrls = $g_M2TestEnvironment.O365SiteUrls
    $o365_webUrls = $g_M2TestEnvironment.O365WebUrls

    SetO365MManagedMetadataApplicationParams $o365_siteUrls[0]

	$o365_siteUrlsValue =   $o365_siteUrls -join ','
	$o365_webUrlsValue =  $o365_webUrls -join ','


    SetEnvironmentVar "SPMeta2_O365_SiteUrls" $o365_siteUrlsValue
    SetEnvironmentVar "SPMeta2_O365_WebUrls" $o365_webUrlsValue
    SetEnvironmentVar "SPMeta2_O365_UserName" $g_M2TestEnvironment.O365UserName
    SetEnvironmentVar "SPMeta2_O365_Password" $g_M2TestEnvironment.O365UserPassword
}

function SetupO365Environment() {

    Write-Host "Setting up O365 environment" -fore Yellow

    SetEnvironmentVar "SPMeta2_RunnerLibraries" "SPMeta2.Containers.O365.dll"

    $o365_siteUrls = $g_M2TestEnvironment.O365SiteUrls
    $o365_webUrls = $g_M2TestEnvironment.O365WebUrls

    SetO365MManagedMetadataApplicationParams $o365_siteUrls[0]

	$o365_siteUrlsValue =   $o365_siteUrls -join ','
	$o365_webUrlsValue =  $o365_webUrls -join ','

    SetEnvironmentVar "SPMeta2_O365_SiteUrls" $o365_siteUrlsValue
    SetEnvironmentVar "SPMeta2_O365_WebUrls" $o365_webUrlsValue
    SetEnvironmentVar "SPMeta2_O365_UserName" $g_M2TestEnvironment.O365UserName
    SetEnvironmentVar "SPMeta2_O365_Password" $g_M2TestEnvironment.O365UserPassword
}

function SetupSPMeta2RegressionTestEnvironment($envType) {

    if($envType -eq $null) {
        $envType = $g_M2TestEnvironment.EnvironmentType
    }

    Write-Host "Setting up [$envType] environment." -fore Green

    switch($envType)
    {
        "SSOM" {
            SetupSSOMEnvironment
        }

        "CSOM" {
            SetupCSOMEnvironment
        }

        "O365" {
            SetupO365Environment
        }

        "O365v16" {
            SetupO365v16Environment
        }
    }

    Write-Host "Setting up [$envType] environment completed." -fore Green
}
