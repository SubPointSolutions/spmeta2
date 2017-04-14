cls

$ScriptDirectory = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
. "$ScriptDirectory/_helpers.ps1"

Configuration SPMeta2_WebApp
{
    param (
        [Parameter(Mandatory=$true)] [ValidateNotNullorEmpty()] [string] $NodeName,
        [Parameter(Mandatory=$true)] [ValidateNotNullorEmpty()] [PSCredential] $RunAsCredential
    )

    Import-DscResource -ModuleName PSDesiredStateConfiguration
    Import-DscResource -ModuleName SharePointDsc -ModuleVersion 1.6.0.0
 
    $config = $ConfigurationData.AllNodes

    $webApp_Port = $config.WebAppPort
    $webApp_isHttps = $config.IsHttps

    $machineName = $NodeName
    
    if($webApp_isHttps  -eq $true) {
        $webApp_Url = "https://" + $machineName 
    } else {
        $webApp_Url = "http://" + $machineName 
    }
    
    $ensure = 'Present'

    $clean = $config.DeleteWebApplication

    if($clean -eq $true) {
        $ensure = 'Absent'
    }

    Node $NodeName {

        SPManagedAccount WebAppPoolManagedAccount  
        {
            AccountName          = $config.WebPoolManagedAccount.UserName
            Account              = $config.WebPoolManagedAccount
            PsDscRunAsCredential = $config.SPSetupAccount
            Ensure = 'Present'
        }

        
        SPWebApplication WebApp
        {
            Name                   = "SPMeta2 Regression Web App - $webApp_Port"
            ApplicationPool        = "SPMeta2 Regression Web App"
            ApplicationPoolAccount = $config.WebPoolManagedAccount.UserName
            AllowAnonymous         = $false
            AuthenticationMethod   = "NTLM"
            DatabaseName           = "SPMeta2_Regression_Content_$webApp_Port"
            Url                    = $webApp_Url
            #HostHeader             = "spmeta2.contoso.com"
            Port                   = $webApp_Port
            PsDscRunAsCredential   = $config.SPSetupAccount
            DependsOn              = "[SPManagedAccount]WebAppPoolManagedAccount"
            Ensure = $ensure
        }

        if($ensure -eq 'Present') {
            
            SPSite RootSite
            {
                Url                      = $webApp_Url + ":" + $webApp_Port
                OwnerAlias               = $config.SPSetupAccount.UserName
                Name                     = "SPMeta2 Regression Root Site"
                Template                 = "STS#0"
                PsDscRunAsCredential     = $config.SPSetupAccount
                DependsOn                = "[SPWebApplication]WebApp"
            }

            # other site collections

            foreach($url in $config.SiteCollectionUrls) {
                
                if($url -eq "/") {
                    continue;
                }

                SPSite "SubSite$url"
                {
                    Url                      = $webApp_Url + ":" + $webApp_Port + $url
                    OwnerAlias               = $config.SPSetupAccount.UserName
                    Name                     = "SPMeta2 Regression $url"
                    Template                 = "STS#0"
                    PsDscRunAsCredential     = $config.SPSetupAccount
                    DependsOn                = "[SPWebApplication]WebApp"
                }
            }
        }
    }   
}

$config = @{
        AllNodes = @(
                @{
                    WebPoolManagedAccount = $dsc_WebPoolManagedCredentials
                    SPSetupAccount = $dsc_SPSetupAccountCredentials

                    DeleteWebApplication = $false
                    
                    WebAppPort = $env_config.SharePoint.WebApp.Port

                    SiteCollectionUrls = $env_config.SharePoint.SiteCollection.Urls
                    WebUrls = $env_config.SharePoint.Web.Urls
                }
            )
    }

Apply-Dsc-Configuration -name SPMeta2_WebApp `
                        -nodeNames $dsc_nodeNames `
                        -isVerbose $true `
                        -config $config
