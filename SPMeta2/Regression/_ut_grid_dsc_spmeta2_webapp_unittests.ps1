cls

$ScriptDirectory = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
. "$ScriptDirectory/_helpers.ps1"

$o365RuntimePath = "$PSScriptRoot\..\SPMeta2.Dependencies\SharePoint\SP2013 - 15.0.4420.1017\CSOM"
        Write-Host "Loading SharePoint CSOM API" -fore Green

        $files = [System.IO.Directory]::GetFiles($o365RuntimePath, "*.dll")

        foreach($filePath in $files) {
            Write-Host "`tLoading assembly: [$filePath]"
            $a = [System.Reflection.Assembly]::LoadFile($filePath)
        }

Configuration SPMeta2_UnitTestSettings_Clean
{
    param (
        [Parameter(Mandatory=$true)] [ValidateNotNullorEmpty()] [string] $NodeName,
        [Parameter(Mandatory=$true)] [ValidateNotNullorEmpty()] [PSCredential] $RunAsCredential
    )

    $vars = @(
            "SPMeta2_CSOM_SiteUrls"
            "SPMeta2_CSOM_WebUrls"

            "SPMeta2_SSOM_WebApplicationUrls"
            "SPMeta2_SSOM_SiteUrls"
            "SPMeta2_SSOM_WebUrls"

            "SPMeta2_O365_SiteUrls"
            "SPMeta2_O365_WebUrls"
            "SPMeta2_O365_UserName"
            "SPMeta2_O365_Password"

            "SPMeta2_DefaultSqlServerName"

            "SPMeta2_DefaultTaxonomyStoreId"
            "SPMeta2_DefaultTaxonomyStoreName"

            "SPMeta2_DefaultTestUserLogins",
            "SPMeta2_DefaultTestDomainUserEmails"

            "SPMeta2_DefaultTestADGroups"

            "SPMeta2_RunnerProvisionMode"
        );

    Node $NodeName {

        foreach($var in $vars) {

            $env_resource_name = $var.Replace(".", "_")

            Environment ($env_resource_name + "_Remove") {
                Name = $env_resource_name 
                Ensure = 'Absent'
            }
        }
       
    }   
}


Configuration SPMeta2_UnitTestSettings
{
    param (
        [Parameter(Mandatory=$true)] [ValidateNotNullorEmpty()] [string] $NodeName,
        [Parameter(Mandatory=$true)] [ValidateNotNullorEmpty()] [PSCredential] $RunAsCredential
    )

    Import-DscResource -ModuleName SharePointDsc -ModuleVersion 1.6.0.0
 
    $config = $ConfigurationData.AllNodes
    $objectModels = $config.ObjectModels

    # SetEnvironmentVar "SPMeta2_RunnerLibraries" "SPMeta2.Containers.O365v16.dll"

    $r_runnerLibraries = @();
    
    $onprem = $false
    $online = $false

    foreach($objectModel in $objectModels)
    {
        if("SSOM" -eq $objectModel) {
            $r_runnerLibraries += "SPMeta2.Containers.SSOM.dll";
            $onprem = $true
        }

        if("CSOM" -eq $objectModel) {
            $r_runnerLibraries += "SPMeta2.Containers.CSOM.dll";
            $onprem = $true
        }

        if("O365" -eq $objectModel) {
            $r_runnerLibraries += "SPMeta2.Containers.O365.dll";
            $online = $true
        }

        if("O365v16" -eq $objectModel) {
            $r_runnerLibraries += "SPMeta2.Containers.O365v16.dll";
            $online = $true
        }
    }

    

    $env_vars = @()

    $env_vars += @{
        Name = "SPMeta2_RunnerLibraries"
        Value = [string]::Join(',', $r_runnerLibraries)
    }

    $webApp_Url = "http://$NodeName" + ':' + $config.WebAppPort

    # onprem
    
    $taxonomyStoreName = "";
    $taxonomyStoreId = "";

    if($onprem -eq $true) {

        $siteUrl = $config.SiteCollectionUrls 

        if( ($siteUrl -is [System.Object[]]) -eq $true)
        {
            $siteUrl = $siteUrl[0]
        }

        if($siteUrl.StartsWith("http") -eq $false)
        {
            $siteUrl = $webApp_Url.TrimEnd('/') + $siteUrl
        }

        Write-Host "Fetching default taxoomy store for SharePoint:[$siteUrl]" -fore Green

        $o365_UserName = $config.OnlineUserName
        $secO365_UserPassword = ConvertTo-SecureString $config.OnlineUserPassword -AsPlainText -Force

        $context = New-Object Microsoft.SharePoint.Client.ClientContext($siteUrl)

        $taxSession = [Microsoft.SharePoint.Client.Taxonomy.TaxonomySession]::GetTaxonomySession($context)
        $store = $taxSession.GetDefaultSiteCollectionTermStore();

        $context.Load($store)
        $context.ExecuteQuery()

        Write-Host "Setting up taxonomy store vars" -fore Yellow

        $taxonomyStoreName =  $store.Name
        $taxonomyStoreId = $store.Id

        Write-Host "`tName:[$taxonomyStoreName]" -fore Gray
        Write-Host "`tId:[$taxonomyStoreId]" -fore Gray

        $env_vars += @{
            Name = "SPMeta2_DefaultTaxonomyStoreId"
            Value = $taxonomyStoreId
        }

        $env_vars += @{
            Name = "SPMeta2_DefaultTaxonomyStoreName"
            Value = $taxonomyStoreName
        }
    }

    if($online -eq $true) {

        $siteUrl = $config.OnlineSiteCollectionUrls;

        Write-Host "Fetching default taxoomy store for O365: [$siteUrl]" -fore Green

        $o365_UserName = $config.OnlineUserName
        $secO365_UserPassword = ConvertTo-SecureString $config.OnlineUserPassword -AsPlainText -Force

        $context = New-Object Microsoft.SharePoint.Client.ClientContext($siteUrl)
        $credentials = New-Object Microsoft.SharePoint.Client.SharePointOnlineCredentials($o365_UserName, $secO365_UserPassword)
    
        $context.Credentials = $credentials

        $taxSession = [Microsoft.SharePoint.Client.Taxonomy.TaxonomySession]::GetTaxonomySession($context)
        $store = $taxSession.GetDefaultSiteCollectionTermStore();

        $context.Load($store)
        $context.ExecuteQuery()

        Write-Host "Setting up taxonomy store vars" -fore Yellow

        $taxonomyStoreName =  $store.Name
        $taxonomyStoreId = $store.Id

        Write-Host "`tName:[$taxonomyStoreName]" -fore Gray
        Write-Host "`tId:[$taxonomyStoreId]" -fore Gray

        $env_vars += @{
            Name = "SPMeta2_DefaultTaxonomyStoreId"
            Value = $taxonomyStoreId
        }

        $env_vars += @{
            Name = "SPMeta2_DefaultTaxonomyStoreName"
            Value = $taxonomyStoreName
        }
    }
    
    $env_vars += @{
        Name = "SPMeta2_SSOM_WebApplicationUrls"
        Value = $webApp_Url
    }

    $env_vars += @{
        Name = "SPMeta2_SSOM_SiteUrls"
        Value = [string]::Join(",", ($config.SiteCollectionUrls | Foreach { [string]($webApp_Url.TrimEnd('/') + $_ ) } ))
    }

    $env_vars += @{
        Name = "SPMeta2_SSOM_WebUrls"
        Value = [string]::Join(",", ($config.WebUrls | Foreach { [string]($webApp_Url.TrimEnd('/') + $_ ) } ))
    }    

    $env_vars += @{
        Name = "SPMeta2_CSOM_SiteUrls"
        Value = [string]::Join(",", ($config.SiteCollectionUrls | Foreach { [string]($webApp_Url.TrimEnd('/') + $_ ) } ))
    }

    $env_vars += @{
        Name = "SPMeta2_CSOM_WebUrls"
        Value = [string]::Join(",", ($config.WebUrls | Foreach { [string]($webApp_Url.TrimEnd('/') + $_ ) } ))
    }

    # online
    $env_vars += @{
        Name = "SPMeta2_O365_SiteUrls"
        Value = [string]::Join(",", ($config.OnlineSiteCollectionUrls | Foreach { [string]$_  } ) ) 
    }

    $env_vars += @{
        Name = "SPMeta2_O365_WebUrls"
        Value = [string]::Join(",", ($config.OnlineWebUrls | Foreach { [string]$_  } ) ) 
    }

    $env_vars += @{
        Name = "SPMeta2_O365_UserName"
        Value =  ($config.OnlineUserName) 
    }


    $env_vars += @{
        Name = "SPMeta2_O365_Password"
        Value =  ($config.OnlineUserPassword) 
    }

    $env_vars += @{
        Name = "SPMeta2_DefaultSqlServerName"
        Value =  $NodeName
    }

    $env_vars += @{
        Name = "SPMeta2_O365_DefaultTestUserLogins"
        Value = [string]::Join(",", ($config.TestUserLogins | Foreach { [string]$_  } ) ) 
    }

    $env_vars += @{
        Name = "SPMeta2_DefaultTestUserLogins"
        Value = [string]::Join(",", ($config.TestUserLogins | Foreach { [string]$_  } ) ) 
    }

    $env_vars += @{
        Name = "SPMeta2_DefaultTestDomainUserEmails"
        Value = [string]::Join(",", ($config.TestDomainUserEmails | Foreach { [string]$_  } ) ) 
    }

    $env_vars += @{
        Name = "SPMeta2_DefaultTestADGroups"
        Value = [string]::Join(",", ($config.TestActiveDirectoryGroups | Foreach { [string]$_  } ) ) 
    }

    Node $NodeName {
        
        foreach($var in $env_vars) {

            $env_resource_name = $var.Name.Replace(".", "_")

            Environment ($env_resource_name) {
                Name = $var.Name
                Ensure = 'Present'
                Value = $var.Value
            }
        }
    }   
}

$config = @{
        AllNodes = @(
                @{
                    ObjectModels = $regression_config.ObjectModels

                    TestUserLogins = $regression_config.TestData.UserLogins
                    TestActiveDirectoryGroups = $regression_config.TestData.ActiveDirectoryGroups
                    TestDomainUserEmails = $regression_config.TestData.DomainUserEmails

                    WebAppPort =  $env_config.SharePoint.WebApp.Port
                    
                    SiteCollectionUrls = $env_config.SharePoint.SiteCollection.Urls
                    WebUrls = $env_config.SharePoint.Web.Urls

                    OnlineSiteCollectionUrls = $env_config.SharePointOnline.SiteCollection.Urls
                    OnlineWebUrls = $env_config.SharePointOnline.Web.Urls
                    OnlineUserName = $env_config.SharePointOnline.UserName
                    OnlineUserPassword = $env_config.SharePointOnline.UserPassword
                }
            )
    }

<#
Apply-Dsc-Configuration -name SPMeta2_UnitTestSettings_Clean `
                        -nodeNames $dsc_nodeNames `
                        -isVerbose $true `
                        -config $config
#>

Apply-Dsc-Configuration -name SPMeta2_UnitTestSettings `
                        -nodeNames $dsc_nodeNames `
                        -isVerbose $true `
                        -config $config