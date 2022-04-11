# helpers - end
function Log-M2Message($message, $level, $fore) {

	if($fore -eq $null) {
		$fore = "green"	
	}

    $stamp = $(get-date -f "MM-dd-yyyy HH:mm:ss.fff")
    $logMessage = "SPMETA2: $stamp : $level : $([environment]::UserDomainName)/$([environment]::UserName) : $message"

    Write-Host $logMessage -ForegroundColor $fore
}
function Log-M2InfoMessage($message) {
    Log-M2Message "$message" "INFO" "Green"
}
function Log-M2DebugMessage($message) {
    Log-M2Message "`t$message" "DEBUG" "Gray"
}
function Install-M2PSModules {

    Param(
        [Parameter(Mandatory=$True)]
        $packages
    )

    foreach($package in $packages ) {

        Log-M2InfoMessage "`tensuring package: $($package.Id) $($package.Version)"

        if ([System.String]::IsNullOrEmpty($package["Version"]) -eq $true) {
            Install-Module -Name $package["Id"];
        } else {
            Install-Module -Name $package["Id"] -RequiredVersion $package["Version"];
        }       
    }
} 
# helpers - end

function Ensure-M2PSModules {

    Log-M2InfoMessage "Ensuring required PowerShell modules..."

	Install-M2PSModules @(
		# using 1.6.0.0 due to NTLM auth issue in the recent module
        @{ Id = "sharepointdsc"; Version = "1.9.0.0" }
        @{ Id = "xActiveDirectory"; Version = "" }
	)
}

function Get-M2ScriptDirectory
{
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value;
    if($Invocation.PSScriptRoot)
    {
        $Invocation.PSScriptRoot;
    }
    Elseif($Invocation.MyCommand.Path)
    {
        Split-Path $Invocation.MyCommand.Path
    }
    else
    {
        $Invocation.InvocationName.Substring(0,$Invocation.InvocationName.LastIndexOf("\"));
    }
}

function Get-M2O365Account {
    Param(
        [Parameter(Mandatory=$True)]
        $config
    )

    $account =  $config.Accounts.O365Account
    $creds = New-CredsFromString $account.UserName $account.UserPassword

    return $creds
}

function Get-M2LocalRunAsAccount {
    Param(
        [Parameter(Mandatory=$True)]
        $config
    )

    $account =  $config.Configuration.Accounts.RunAsAccount
    Check-NullOrEmpty $account "Cannot find RunAsAccount"

    $creds = New-CredsFromString $account.UserName $account.UserPassword

    return $creds
}

function Get-M2SPWebPoolManagedAccount {
    Param(
        [Parameter(Mandatory=$True)]
        $config
    )

    $account =  $config.Configuration.Accounts.SPWebPoolManagedAccount
    Check-NullOrEmpty $account "Cannot find SPWebPoolManagedAccount"

    $creds = New-CredsFromString $account.UserName  $account.UserPassword

    return $creds
}

function Check-NullOrEmpty {
     Param(
        [Parameter(Mandatory=$True)]
        [AllowNull()]
        $data,

        [Parameter(Mandatory=$True)]
        $message
    )

      if($data -eq $null) {
        throw $message
    }
}

function Get-M2SPSetupAccount {
    Param(
        [Parameter(Mandatory=$True)]
        $config
    )

    $account = $config.Configuration.Accounts.SPWebPoolManagedAccount
    Check-NullOrEmpty $account "Cannot find SPWebPoolManagedAccount"

    $creds = New-CredsFromString $account.UserName  $account.UserPassword

    return $creds
}

function New-CredsFromString($name, $pass) {
    $name = Eval-M2string $name
    $pass = Eval-M2string $pass
    
    Log-M2InfoMessage "RunAs account name: $name"
    Log-M2InfoMessage "RunAs account pass: $pass"

    $userPass   = ConvertTo-SecureString $pass -AsPlainText -Force
    $userCreds  = New-Object System.Management.Automation.PSCredential($name, $userPass)
    
    return $userCreds
}

function Get-M2RegressionLibraries($objectModels) {


}

function Get-M2RegressionLibraries($objectModels) {

    $result = @()

    foreach($objectModel in $objectModels)
    {
        if("SSOM" -eq $objectModel) {
            $result += "SPMeta2.Containers.SSOM.dll";
        }
        elseif("CSOM" -eq $objectModel) {
            $result += "SPMeta2.Containers.CSOM.dll";
        }
        elseif("O365" -eq $objectModel) {
            $result += "SPMeta2.Containers.O365.dll";
        }
        elseif("O365v16" -eq $objectModel) {
            $result += "SPMeta2.Containers.O365v16.dll";
        }
        else {
            throw "Unknown API value: $objectModel - use SSOM, CSOM, O365 or O365v16"
        }
    }        

    return [string]::Join(',', $result)
}
function Get-M2WebAppUrl {

     Param(
        [Parameter(Mandatory=$True)]
        $config
    )

    $spConfig = $config.Configuration.SharePoint
    Check-NullOrEmpty $spConfig "Cannot find SharePoint config"

    $webAppConfig = $spConfig.WebApplication
    Check-NullOrEmpty $webAppConfig "Cannot find SharePoint web app config"

    $computerName = $env:computerName
    $webAppUrl = "http://$computerName" + ':' + $webAppConfig.Port

    if( $webAppConfig.IsHttps -eq $true) {
        $webAppUrl = "https://$computerName" + ':' + $webAppConfig.Port
    }

    return $webAppUrl
}

function Get-M2SharePointServerConfig{
     Param(
        [Parameter(Mandatory=$True)]
        $config
    )

    $spConfig = $config.Configuration.SharePoint
    Check-NullOrEmpty $spConfig "Cannot find SharePoint config"

    $webAppConfig = $spConfig.WebApplication
    Check-NullOrEmpty $webAppConfig "Cannot find SharePoint web app config"

    $siteCollectionUrls = $spConfig.SiteCollectionUrls
    Check-NullOrEmpty $siteCollectionUrls "Cannot find SharePoint site colection urls"

    $webUrls = $spConfig.WebUrls
    Check-NullOrEmpty $webUrls "Cannot find SharePoint web urls"

    return $spConfig
}

function Get-CSOMSiteUrls {
    Param(
        [Parameter(Mandatory=$True)]
        $config
    )

    $spConfig = Get-M2SharePointServerConfig $config

    $webAppConfig = $spConfig.WebApplication
    $siteCollectionUrls = $spConfig.SiteCollectionUrls
    $webUrls = $spConfig.WebUrls

    $computerName = $env:computerName
    $webAppUrl = "http://$computerName" + ':' + $webAppConfig.Port

    if( $webAppConfig.IsHttps -eq $true) {
        $webAppUrl = "https://$computerName" + ':' + $webAppConfig.Port
    }

    [string]::Join(",", ($siteCollectionUrls | Foreach { [string]($webAppUrl.TrimEnd('/') + $_ ) } ))
}

function Get-CSOMWebUrls {
    Param(
        [Parameter(Mandatory=$True)]
        $config
    )

    $spConfig = Get-M2SharePointServerConfig $config

    $webAppConfig = $spConfig.WebApplication
    $siteCollectionUrls = $spConfig.SiteCollectionUrls
    $webUrls = $spConfig.WebUrls

    $computerName = $env:computerName
    $webAppUrl = "http://$computerName" + ':' + $webAppConfig.Port

    if( $webAppConfig.IsHttps -eq $true) {
        $webAppUrl = "https://$computerName" + ':' + $webAppConfig.Port
    }

    [string]::Join(",", ($webUrls | Foreach { [string]($webAppUrl.TrimEnd('/') + $_ ) } ))
}

function Get-O365SiteUrls {
    Param(
        [Parameter(Mandatory=$True)]
        $config
    )

    $spConfig = $config.Configuration.SharePoint
    Check-NullOrEmpty $spConfig "Cannot find O365 condfig"

    $siteUrls = $spConfig.SiteCollectionUrls
    Check-NullOrEmpty $siteUrls "Cannot find O365 site colection urls"

    return [string]::Join(",", ($siteUrls))
}

function Get-O365WebUrls {
    Param(
        [Parameter(Mandatory=$True)]
        $config
    )

    $spConfig = $config.Configuration.SharePoint
    Check-NullOrEmpty $spConfig "Cannot find O365 condfig"

    $webUrls = $spConfig.WebUrls
    Check-NullOrEmpty $webUrls "Cannot find O365 web urls"

    return [string]::Join(",", ($webUrls ))
}

function Get-O365UserName {
    Param(
        [Parameter(Mandatory=$True)]
        $config
    )

    $spConfig = $config.Configuration.Accounts.RunAsAccount
    return $spConfig.UserName
}


function Get-O365UserPassword {
    Param(
        [Parameter(Mandatory=$True)]
        $config
    )

    $spConfig = $config.Configuration.Accounts.RunAsAccount
    return $spConfig.UserPassword
}

function Get-SSOMWebAppUrls {

    Param(
        [Parameter(Mandatory=$True)]
        $config
    )
 
    $spConfig = $config.Configuration.SharePoint

    $webAppConfig = $spConfig.WebApplication
    $siteCollectionConfig = $spConfig.SiteCollection
    $webConfig = $spConfig.Web

    $computerName = $env:computerName
    $webAppUrl = "http://$computerName" + ':' + $webAppConfig.Port

    if( $webAppConfig.IsHttps -eq $true) {
        $webAppUrl = "https://$computerName" + ':' + $webAppConfig.Port
    }

    [string]::Join(",", $webAppUrl )
}

function Ensure-M2EnvironmentVariables {
    Param(
        
        [Parameter(Mandatory=$True)]
        [String]$api,

        [Parameter(Mandatory=$True)]
        $config
    )

    $isOnPrem = (Is-OnlineApi $api) -eq $false
    $isOnline = ($isOnPrem -eq $false) 

    Log-M2InfoMessage " configuring environment variables, api: $api, isOnPrem: $isOnPrem"

    $variables = Get-M2Variables

    # api 
    Log-M2DebugMessage "SPMeta2_RunnerLibraries..."
    $variables["SPMeta2_RunnerLibraries"] = (Get-M2RegressionLibraries @($api))

    # CSOM/SSOM
    
    if($isOnPrem -eq $true) {

        Log-M2DebugMessage "CSOM settings..."
        $variables["SPMeta2_CSOM_SiteUrls"] = (Get-CSOMSiteUrls $config)
        $variables["SPMeta2_CSOM_WebUrls"]  = (Get-CSOMWebUrls  $config)
        
        Log-M2DebugMessage "SSOM settings..."
        $variables["SPMeta2_SSOM_WebApplicationUrls"] = (Get-SSOMWebAppUrls $config)
        $variables["SPMeta2_SSOM_SiteUrls"] = (Get-CSOMSiteUrls $config)
        $variables["SPMeta2_SSOM_WebUrls"]  = (Get-CSOMWebUrls  $config)
    }

    # 365 
    if($isOnline -eq $true) {
        Log-M2DebugMessage "O365 settings..."
        $variables["SPMeta2_O365_SiteUrls"]  = (Get-O365SiteUrls $config)
        $variables["SPMeta2_O365_WebUrls"]   = (Get-O365WebUrls  $config)
        $variables["SPMeta2_O365_UserName"]  = (Get-O365UserName     $config)
        $variables["SPMeta2_O365_Password"]  = (Get-O365UserPassword $config)
    }

    # domain settings
    if($isOnPrem -eq $true) {
        Log-M2DebugMessage "Local domain test date settings..."
        $variables["SPMeta2_DefaultTestUserLogins"] = [String]::Join(",", (Get-M2DomainTestUsers -config $config))
        $variables["SPMeta2_DefaultTestADGroups"]    = [String]::Join(",", (Get-M2DomainTestGroups -config $config))
        $variables["SPMeta2_DefaultTestDomainUserEmails"]    = [String]::Join(",", (Get-M2DomainTestUserEmails -config $config))
    }

    # updating O365 users/groups
    if ($isOnPrem -eq $false) {
        Log-M2DebugMessage "Overriding test user logins for SharePoint online..."
        $variables["SPMeta2_DefaultTestUserLogins"] = [String]::Join(",", (Get-M2O365TestUsers -config $config))
    } 

    $taxonomyServiceDetails = Get-M2DefaultTaxonomyDetails $api $config

    # additional settings, taxonomy relates tests
    $variables["SPMeta2_DefaultTaxonomyStoreId"]   = $taxonomyServiceDetails["TermStoreId"]
    $variables["SPMeta2_DefaultTaxonomyStoreName"] = $taxonomyServiceDetails["TermStoreName"]

    $config = @{
        AllNodes = @(
            @{
                NodeName = 'localhost'
                PSDscAllowPlainTextPassword = $true

                Variables = $variables

                RunAsAccount =  $runAsAccount
            }
        )
    }
    
    Apply-M2DSC "SPMeta2_CleanEnvironmentVariablesConfiguration" $config  $true
    Apply-M2DSC "SPMeta2_EnvironmentVariablesConfiguration" $config  $true
}

function Get-M2DomainTestGroups
{
     Param(
        [Parameter(Mandatory=$True)]
        $config
    )    

    $data = Get-M2LocalGroups $config
    $result = @()

    foreach($user in $data)  {
        $result += $user.GroupName
    }

    return $result
}

function Get-M2DomainTestUserEmails
{
    Param(
        [Parameter(Mandatory=$True)]
        $config
    )    

    $data = Get-M2LocalUsers $config
    $result = @()

    foreach($user in $data)  {
        $result += $user.EmailAddress
    }

    return $result
}

function Get-M2O365TestUsers
{
    Param(
        [Parameter(Mandatory=$True)]
        $config
    )    

    $data =Get-M2OnlineUsers $config
    $result = @()

    foreach($user in $data)  {
        $result += $user.UserName
    }

    return $result
}

function Get-M2DomainTestUsers
{
    Param(
        [Parameter(Mandatory=$True)]
        $config
    )    

    $result = @()
    $data = Get-M2LocalUsers   $config
    
    foreach($user in $data)  {
        $result += $user.UserName
    }

    return $result
}

function Get-M2LocalUsers() {

    Param(
        [Parameter(Mandatory=$True)]
        $config
    )

    $result = @()
    
    $data = $config.Configuration.Domain.DomainUsers

    if($data -eq $null -or $data.Count -eq 0) {
        throw "Cannot find local users"
    }

    foreach($user in $data) {
        $result += @{ 
            "DomainName"   =  Eval-M2String $user.DomainName
            "UserName"     =  Eval-M2String ($user.UserName.Split("\\")[1])
            "EmailAddress" =  Eval-M2String $user.EmailAddress
        }
    }

    return $result
}


function Get-M2OnlineUsers() {

    Param(
        [Parameter(Mandatory=$True)]
        $config
    )

    $result = @()
    
    $data = $config.Configuration.Domain.Users

    if($data -eq $null -or $data.Count -eq 0) {
        throw "Cannot online local users"
    }

    foreach($user in $data) {
        $result += @{ 
            "UserName"     =  Eval-M2String ($user.UserName.Split("\\")[1])
            "EmailAddress" =  Eval-M2String $user.EmailAddress
        }
    }

    return $result
}


function Get-M2LocalGroups() {

    Param(
        [Parameter(Mandatory=$True)]
        $config
    )

    $result = @()

    $data = $config.Configuration.Domain.DomainGroups

    if($data -eq $null -or $data.Count -eq 0) {
        throw "Cannot find local groups"
    }

    foreach($group in $data) {
        $result += @{ 
            "GroupName" =  Eval-M2String $group.GroupName
            "Category"  =  Eval-M2String $group.Category
        }
    }

    return $result
}

function Ensure-M2TestData {

    Param(
        
        [Parameter(Mandatory=$True)]
        [String]$api,

        [Parameter(Mandatory=$True)]
        $config
    )

    Log-M2InfoMessage "- configuring integration tests data..."
    
    $isOnPrem = (Is-OnlineApi $api) -eq $false

    if( $isOnPrem -eq  $true) {
        Log-M2InfoMessage "- SharePoint Server environment detected..."
        
        $runAsAccount =  (Get-M2LocalRunAsAccount $config)
        
        $dscDomainUsers  = Get-M2LocalUsers $config
        $dscDomainGroups = Get-M2LocalGroups $config

        $dscConfig = @{
            AllNodes = @(
                @{
                    NodeName = 'localhost'
                    PSDscAllowPlainTextPassword = $true

                    Users  = $dscDomainUsers
                    Groups = $dscDomainGroups

                    RunAsAccount =  $runAsAccount
                }
            )
        }
        
        Apply-M2DSC "SPMeta2_DomainConfiguration" $dscConfig  $true
    } else {
        Log-M2InfoMessage "- SharePoint online environment detected..."
    }       
}

function Ensure-M2PostSharePointSettings {
    Param(
        
        [Parameter(Mandatory=$True)]
        [String]$api,

        [Parameter(Mandatory=$True)]
        $config
    )

    Log-M2InfoMessage "- configuring post-SharePoint settings..."
    Apply-M2DSC "SharePoint16_FarmTuning" $null $true
}


function Ensure-M2SharePointSettings {

    Param(
        
        [Parameter(Mandatory=$True)]
        [String]$api,

        [Parameter(Mandatory=$True)]
        $config,

        [Parameter(Mandatory=$False)]
        $deleteSPWebApp = $false
    )

    Log-M2InfoMessage "- configuring SharePoint settings..."

    $runAsAccount       = Get-M2LocalRunAsAccount  $config
    $spWebPollAccount   = Get-M2SPWebPoolManagedAccount  $config
    $spSetupAccount     = Get-M2SPSetupAccount  $config

    $spData = $config.Configuration.SharePoint
    Check-NullOrEmpty $spData 'Cannot find SharePoint related settings'

    $webAppPort = $spData.WebApplication.Port 
    Check-NullOrEmpty $webAppPort "Cannot find SharePoint web app 'Port' setting"

    $isHttps    = $spData.WebApplication.IsHttps 
    Check-NullOrEmpty $isHttps "Cannot find SharePoint web app 'IsHttps' setting"

    $siteCollectionUrls = $spData.SiteCollectionUrls
    Check-NullOrEmpty $siteCollectionUrls "Cannot find SharePoint 'SiteCollectionUrls' setting"

    $config = @{
        AllNodes = @(
            @{
                NodeName = 'localhost'
                PSDscAllowPlainTextPassword = $true

                RunAsAccount            =  $runAsAccount
                WebPoolManagedAccount   =  $spWebPollAccount
                SPSetupAccount          =  $spSetupAccount

                DeleteWebApplication = $deleteSPWebApp

                WebAppPort = $webAppPort 
                IsHttps = $isHttps 
                
                SiteCollectionUrls = $siteCollectionUrls
            }
        )
    }
    
    Apply-M2DSC "SPMeta2_SharePointConfiguration" $config  $true
}

function Eval-M2String($value) {
    return ($ExecutionContext.InvokeCommand.ExpandString($value))
}

function Get-M2DSCConfig {
    return @{
        AllNodes = @(
            @{
                NodeName = 'localhost'
                PSDscAllowPlainTextPassword = $true
            }
        )
    }
}

function Ensure-M2Folder($path) {
    New-Item -ItemType Directory -Force -Path $path | out-null
}

function Get-M2Variables() {
     $result = @{
        # api to test with
        "SPMeta2_RunnerLibraries" = $null;

        # csom
        "SPMeta2_CSOM_SiteUrls" = $null;
        "SPMeta2_CSOM_WebUrls" = $null;
        # ssom
        "SPMeta2_SSOM_WebApplicationUrls" = $null;
        "SPMeta2_SSOM_SiteUrls" = $null;
        "SPMeta2_SSOM_WebUrls" = $null;

        # o365
        "SPMeta2_O365_SiteUrls" = $null;
        "SPMeta2_O365_WebUrls" = $null;
        "SPMeta2_O365_UserName" = $null;
        "SPMeta2_O365_Password" = $null;
        "SPMeta2_O365_DefaultTestUserLogins" = $null;

        # additional settings, content Db creation test
        "SPMeta2_DefaultSqlServerName" = $null;

        # additional settings, taxonomy relates tests
        "SPMeta2_DefaultTaxonomyStoreId" = $null;
        "SPMeta2_DefaultTaxonomyStoreName" = $null;

        # additional settings, security related tests
        "SPMeta2_DefaultTestUserLogins" = $null;
        "SPMeta2_DefaultTestDomainUserEmails" = $null;
        "SPMeta2_DefaultTestADGroups" = $null;

        # additional settings, incremental provision mode and tests
        "SPMeta2_RunnerProvisionMode" = $null;
    }

    return $result
}

function Apply-M2DSC {
    Param(
        [Parameter(Mandatory=$True)]
        $name,
        
        [Parameter(Mandatory=$False)]
        $config = $null,

        [Parameter(Mandatory=$False)]
        $expectInDesiredState = $null
    )

    $skipDscCheck = $false

    if($config -eq $null) {
        $config = Get-M2DSCConfig
    }

    $dscFolder = "C:\_spmeta2_dsc"
    $dscConfigFolder = [System.IO.Path]::Combine($dscFolder, $name)

    Log-M2InfoMessage "Ensuring folder: $dscConfigFolder for config: $name"
    Ensure-M2Folder $dscFolder

    Log-M2InfoMessage "Clearing previous configuration: $name"
    if(Test-Path $dscConfigFolder) {
        Remove-Item $dscConfigFolder -Recurse -Force
    }

    Log-M2InfoMessage "Compiling new configuration: $name to folder: $dscConfigFolder"
    . $name -ConfigurationData $config -OutputPath $dscConfigFolder

    Log-M2InfoMessage "Starting configuration: $name from path: $dscConfigFolder"
    Start-DscConfiguration -Path $dscConfigFolder -Force -Wait -Verbose

    $result = $null

    if($skipDscCheck -eq $false) {

        Log-M2InfoMessage "Testing configuration: $name"
        $result = Test-DscConfiguration -Path $dscConfigFolder

        if($expectInDesiredState -eq $true) {
            Log-M2InfoMessage "Expecting DSC [$name] in a desired state"

            if($result.InDesiredState -ne $true) {
                $message = "DSC: $name - is NOT in a desired state: $result"
                Log-M2InfoMessage $message

                if ($result.ResourcesNotInDesiredState -ne $null) {
                    foreach($resource in $result.ResourcesNotInDesiredState) {
                        Log-M2InfoMessage $resource
                    }
                }

                throw $message
            } else {
                $message = "DSC: $name is in a desired state: $result"
                Log-M2InfoMessage $message
            }
        } else {
            Log-M2InfoMessage "No check for DSC [$name] is done. Skipping."
        }
    } else {
        Log-M2InfoMessage "Skipping testing configuration: $name"
    }

    return  $result
}

function Is-OnlineApi {

    Param(
        [Parameter(Mandatory=$True)]
        [String]$api 
    )

    return ( $api.Contains("O365") -eq $true )
}

function Get-M2DefaultTaxonomyDetails {
    
	Param(
        [Parameter(Mandatory=$True)]
        [String]$api = "CSOM",

        [Parameter(Mandatory=$True)]
        $config 
    )        

    Log-M2InfoMessage "Loading SharePoint helpers..."

    . "$(Get-M2ScriptDirectory)\_spmeta2.sp.ps1"

    Log-M2InfoMessage "Ensuring PSSnapin: SharePoint..."
    Ensure-SPPSSnapin 

    $o365RuntimePath = "$PSScriptRoot\..\..\SPMeta2.Dependencies\SharePoint\SP2013 - 15.0.4420.1017\CSOM"
    Log-M2DebugMessage "Loading SharePoint client assemblies from:"
    Log-M2InfoMessage " - $o365RuntimePath"
    
    Ensure-SPAssemblies $o365RuntimePath
    
    $isOnPrem = (Is-OnlineApi $api) -eq $false
    $result =  $()
    
    if($isOnPrem -eq $true) {
        Log-M2InfoMessage "Detected SharePoint Onprem with api: $api"

        $spConfig = Get-M2SharePointServerConfig $config

        $webAppConfig = $spConfig.WebApplication
        $siteCollectionUrls = $spConfig.SiteCollectionUrls
        $webUrls = $spConfig.WebUrls

        $siteUrl = $siteCollectionUrls

        Log-M2InfoMessage " - initial siteurl: $siteUrl"
        if( $siteUrl.Count -gt 0 ) {
            $siteUrl = $siteUrl[0]
        }

        Log-M2InfoMessage " - fixing http: $siteUrl"
        if($siteUrl.StartsWith("http") -eq $false)  {   
            Log-M2DebugMessage " - Get-M2WebAppUrl..."
            $webAppUrl = Get-M2WebAppUrl $config

            Log-M2DebugMessage " - Updating url..."
            $siteUrl = $webAppUrl.TrimEnd('/') + $siteUrl
        }

        Log-M2DebugMessage " - final url: $siteUrl"
        Log-M2InfoMessage "Fetching default taxoomy store for site: [$siteUrl]" 
        
        $context = New-Object Microsoft.SharePoint.Client.ClientContext($siteUrl)

        $taxSession = [Microsoft.SharePoint.Client.Taxonomy.TaxonomySession]::GetTaxonomySession($context)
        $store = $taxSession.GetDefaultSiteCollectionTermStore();

        $context.Load($store)
        $context.ExecuteQuery()

        Log-M2DebugMessage "TermStoreName: $($store.Name)" 
        Log-M2DebugMessage "TermStoreId  : $($store.Id)" 

        $result = @{
            "TermStoreName" = $store.Name;
            "TermStoreId" = $store.Id
        }

    } else {

        $spConfig = $config.Configuration.SharePoint
        Check-NullOrEmpty $spConfig "Cannot find O365 condfig"

        $siteUrls = $spConfig.SiteCollectionUrls
        Check-NullOrEmpty $siteUrls "Cannot find O365 site colection urls"

        $siteUrl = $siteUrls

        Log-M2InfoMessage " - initial siteurl: $siteUrl"
        if( $siteUrl.Count -gt 0) {
            $siteUrl = $siteUrl[0]
        }

        Log-M2InfoMessage " - final url: $siteUrl"

        $o365_UserName = Get-O365UserName $config
        $secO365_UserPassword = ConvertTo-SecureString (Get-O365UserPassword $config) -AsPlainText -Force
        
        $credentials = New-Object Microsoft.SharePoint.Client.SharePointOnlineCredentials($o365_UserName, $secO365_UserPassword)
        
        $context = New-Object Microsoft.SharePoint.Client.ClientContext($siteUrl)
        $context.Credentials =  $credentials 

        $taxSession = [Microsoft.SharePoint.Client.Taxonomy.TaxonomySession]::GetTaxonomySession($context)
        $store = $taxSession.GetDefaultSiteCollectionTermStore();

        $context.Load($store)
        $context.ExecuteQuery()

        Log-M2InfoMessage "TermStoreName: $($store.Name)" 
        Log-M2InfoMessage "TermStoreId  : $($store.Id)" 

        $result = @{
            "TermStoreName" = $store.Name;
            "TermStoreId" = $store.Id
        }
    }

    return $result
}

function Ensure-M2SharePointServicesFix {
    Log-M2InfoMessage "Fixing up some SharePoint service settings..."

    . "$(Get-M2ScriptDirectory)\_spmeta2.sp.ps1"

    Log-M2InfoMessage "Ensuring PSSnapin: SharePoint..."
    Ensure-SPPSSnapin 

    Log-M2InfoMessage "Updating default site collection taxonomy..."
    Set-M2DefaultSiteCollectionTaxonomy

    Log-M2InfoMessage "Enabling SharePoint Sandboxed Code Service..."
    Enable-SPUserCodeService

    Log-M2InfoMessage "Ensuring accosiated groups for all sites..."
    Ensure-M2AssociatedGroupsAllSites
}

function Apply-M2RegressionSettings {

	Param(
        [Parameter(Mandatory=$True)]
        $config,

        [Parameter(Mandatory=$True)]
        $configName,

        [Parameter(Mandatory=$False)]
        [String]$api = "CSOM",

        [Parameter(Mandatory=$False)]
        [Boolean]$skipTestData = $false,

        [Parameter(Mandatory=$False)]
        [Boolean]$skipSPWebApp = $false,

        [Parameter(Mandatory=$False)]
        [Boolean]$skipSPPostCheck = $false,

        [Parameter(Mandatory=$False)]
        [Boolean]$skipEnvVariables = $false,

        [Parameter(Mandatory=$False)]
        [Boolean]$skipSPServicesFix = $false,

        [Parameter(Mandatory=$False)]
        [Boolean]$deleteSPWebApp = $false,
        
        [Parameter(Mandatory=$False)]
        [String]$o365UserName,

        [Parameter(Mandatory=$False)]
        [String]$o365UserPassword 
    )

  	Log-M2InfoMessage "Configuring SPMeta2 regression environment"
    Log-M2InfoMessage " - API: $api"
    Log-M2InfoMessage " - config: $configName"

    $config = New-Object PSObject -Property $config[$configName]

    if($skipTestData -eq $false)  {
        Ensure-M2TestData -api $api -config $config
    }

    if($skipSPWebApp -eq $false)  {
        Ensure-M2SharePointSettings -api $api `
                                    -config $config `
                                    -deleteSPWebApp $deleteSPWebApp
    }

    if($skipSPPostCheck -eq $false)  {
        Ensure-M2PostSharePointSettings  -api $api -config $config
    }

    if($skipSPServicesFix -eq $false)  {
        Ensure-M2SharePointServicesFix  -api $api -config $config
    }

    if($skipEnvVariables -eq $false) {
        Ensure-M2EnvironmentVariables  -api $api -config $config
    }

    Log-M2InfoMessage "Configuring SPMeta2 regression environment completed!"
}