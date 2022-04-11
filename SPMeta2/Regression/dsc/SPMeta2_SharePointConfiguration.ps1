Configuration SPMeta2_SharePointConfiguration {
   
    Import-DscResource -ModuleName PSDesiredStateConfiguration
    Import-DscResource -ModuleName SharePointDsc -ModuleVersion "1.9.0.0"
 
    Node localhost {

        $config = $Node

        $ensure = 'Present'

        $machineName = $env:computerName

        $webAppPort = $config.WebAppPort
        $webAppIsHttps = $config.IsHttps
        
        $deleteWebApp = $config.DeleteWebApplication

        if($deleteWebApp -eq $true) {
            $ensure = 'Absent'
        }

        if($webAppIsHttps  -eq $true) {
            $webAppUrl = "https://" + $machineName 
        } else {
            $webAppUrl = "http://" + $machineName 
        }

        SPManagedAccount WebAppPoolManagedAccount  
        {
            AccountName          = $config.WebPoolManagedAccount.UserName
            Account              = $config.WebPoolManagedAccount
            PsDscRunAsCredential = $config.SPSetupAccount
            Ensure = 'Present'
        }

        SPWebApplication WebApp
        {
            Name                   = "SPMeta2 Regression Web App - $webAppPort"
            ApplicationPool        = "SPMeta2 Regression Web App"
            ApplicationPoolAccount = $config.WebPoolManagedAccount.UserName
            AllowAnonymous         = $false

            # https://github.com/PowerShell/SharePointDsc/issues/707
            AuthenticationMethod   = "NTLM"
            DatabaseName           = "SPMeta2_Regression_Content_$webAppPort"
            Url                    = $webAppUrl
            #HostHeader             = "spmeta2.contoso.com"
            Port                   = $webAppPort
            PsDscRunAsCredential   = $config.SPSetupAccount
            DependsOn              = "[SPManagedAccount]WebAppPoolManagedAccount"
            Ensure = $ensure
        }

        # issue with the rence SharePointDSC release
        # https://github.com/PowerShell/SharePointDsc/issues/707
        # SPWebAppAuthentication WebAppDefaultAuth
        # {
        #     WebAppUrl  = $webAppUrl
        #     Default = @(
        #         MSFT_SPWebAppAuthenticationMode {
        #             AuthenticationMethod = "NTLM"
        #         }
        #     )
        #     DependsOn = "[SPWebApplication]WebApp"
        # }

        if($ensure -eq 'Present') {
            
            # root site collection
            SPSite RootSite
            {
                Url                      = $webAppUrl + ":" + $webAppPort
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
                    Url                      = $webAppUrl + ":" + $webAppPort + $url
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
