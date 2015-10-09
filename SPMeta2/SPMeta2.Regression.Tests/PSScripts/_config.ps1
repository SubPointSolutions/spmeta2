Add-Type -TypeDefinition @"
       public enum EnvironmentType
       {
          SSOM,
          CSOM,
          O365,
          O365v16
       }
"@ 

# defines configs for new 'sandbox' web application 
$g_M2WebAppSettings = New-Object PSObject -Property @{   
    
    # -- GLOBAL SETTINGS --
    # current machine/domain name
    MachineName = [Environment]::MachineName
    DomainName = [Environment]::UserDomainName

    # sql server address
    SqlServerMachineName = [Environment]::MachineName

    # -- WEB APP SETTINGS --
    # should web app or site be recreated

    ShouldRecreateWebApplicaiton = $true                 
    
    # port and ap account for the web app
    WebApplicationPort = 31415
    WebApplicationPoolAccountName = "$([Environment]::UserDomainName)\sp_farm"

    
    # -- SITE COLLECTION SETTINGS --
    # should site colleciton be recreated    

    ShouldRecreateSiteCollection = $true       
    SiteCollectionTemplate = "BLANKINTERNET#0"
    SiteCollectionLCID = 1033
    SiteCollectionOwner = "$([Environment]::UserDomainName)\$([Environment]::UserName)"
    SiteCollectionAdministrators = @(
        "$([Environment]::UserDomainName)\administrator",
        "$([Environment]::UserDomainName)\sp_admin",
        "$([Environment]::UserDomainName)\sp_farm"
    )
}         

# defines config for the regression test 
$g_M2TestEnvironment = New-Object PSObject -Property @{   

    # -- GLOBAL SETTINGS --
    # current environment type
    # CSOM, SSOM or O365
    EnvironmentType = "SSOM"

   #O365 specific settings
    O365UserName = "support@subpointsolutions.com"
    O365UserPassword = "sbhsbhu8wxvKQsu"

    O365SiteUrls = @(
        "https://subpointsolutions.sharepoint.com/sites/ci-10"
    )

    O365WebUrls = @(
        "https://subpointsolutions.sharepoint.com/sites/ci-10"
    )
   
   RandomBalancedUrls = @( 
	#	"https://subpointsolutions.sharepoint.com/sites/ci-10"
	#	"https://subpointsolutions.sharepoint.com/sites/ci-11"
	#	"https://subpointsolutions.sharepoint.com/sites/ci-12"
	#	"https://subpointsolutions.sharepoint.com/sites/ci-13"
	#	"https://subpointsolutions.sharepoint.com/sites/ci-14"
	#	"https://subpointsolutions.sharepoint.com/sites/ci-15"
	 )

	O365DefaultTestUserLogins = @(
		"ci@subpointsolutions.com"
	)


    # CSOM specific settings
    CSOMWebApplicationUrls = @(
        "http://$([Environment]::MachineName):$($g_M2WebAppSettings.WebApplicationPort)"
        #"http://$([Environment]::MachineName)"
        #"http://portal"
    )

    CSOMSiteUrls = @(
        "http://$([Environment]::MachineName):$($g_M2WebAppSettings.WebApplicationPort)"
        #"http://$([Environment]::MachineName)"
        #"http://portal"
    )

    CSOMWebUrls = @(
        "http://$([Environment]::MachineName):$($g_M2WebAppSettings.WebApplicationPort)"
        #"http://$([Environment]::MachineName)"
        #"http://portal"
    )

    # SSOM specific settings
    SSOMWebApplicationUrls = @(
        "http://$([Environment]::MachineName):$($g_M2WebAppSettings.WebApplicationPort)"
    )

    SSOMSiteUrls = @(
        "http://$([Environment]::MachineName):$($g_M2WebAppSettings.WebApplicationPort)"
    )

    SSOMWebUrls = @(
        "http://$([Environment]::MachineName):$($g_M2WebAppSettings.WebApplicationPort)"
    )

    OO365TestLogins = @(
        ""
    )

    OnPremTestActiveDirectoryLogins = @( 
        "$([Environment]::UserDomainName)\administrator",
        "$([Environment]::UserDomainName)\sp_admin",
        "$([Environment]::UserDomainName)\sp_farm"
    )

    OnPremTestActiveDirectoryGroups = @(
        "$([Environment]::UserDomainName)\test-g1"
    )
}