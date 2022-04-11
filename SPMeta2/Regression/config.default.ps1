
$spMeta2Config["Default.SharePointServer"] = @{
  # regression testing works againt two environments
  # - SharePoint Server 2013/2016
  # - SharePoint Online
  # SharePoint 2013/2016 configuration
  Configuration = @{
    Accounts = @{
      # account used for DSC RunAs operations
      RunAsAccount = @{
        UserName     = "$env:USERDOMAIN\$env:USERNAME"
        UserPassword = "vagrant"
      }
      # account used SharePoint web app creation process    
      SPWebPoolManagedAccount = @{
        UserName     = "$env:USERDOMAIN\$env:USERNAME"
        UserPassword = "vagrant"
      }
      # account used SharePoint web app, site collection and web creation process      
      SPSetupAccount = @{
        UserName     = "$env:USERDOMAIN\$env:USERNAME"
        UserPassword = "vagrant"
      }
    }
  
    # SharePoint specific settings
    SharePoint = @{
      WebApplication = @{
        Port    = 31442
        IsHttps = $false
      }
    
      SiteCollectionUrls = @(
        "/"
        # "/sites/spmeta2"
      )

      WebUrls = @(
        "/"
        #"/sites/spmeta2"
      )
    }

    # domain specfic settings; test users and so on
    Domain = @{
      # test user logins to be used with security related tests
      # must not be same account under which tests are run
      DomainUsers = @(
        @{
          DomainName    = "$env:USERDOMAIN"
          UserName      = $env:USERDOMAIN + "\spmeta2-user1"
          EmailAddress  = "spmeta2-user1@" + $env:USERDNSDOMAIN
        }, 
        
        @{
          DomainName    = "$env:USERDOMAIN"
          UserName      = $env:USERDOMAIN + "\spmeta2-user2"
          EmailAddress  = "spmeta2-user2@" + $env:USERDNSDOMAIN          
        }
      )

      # test group user logins to be used with security related tests
      DomainGroups = @(
        @{
          GroupName = "spmeta2-group1"
          Category  = "Security"
        },

        @{
          GroupName = "spmeta2-group2"      
          Category  = "Security"      
        }
      )
    }
  }
}

$spMeta2Config["Default.SharePointOnline"] = @{
  # SharePoint Online environment configuration
  Configuration = @{
    Accounts = @{
      # account used to setup and execute tests against SharePoint online
      RunAsAccount = @{
        UserName      = "contoso@contoso.com"
        UserPassword  = "contoso"
      }
    }
    
    # site colection to be used with SharePoint online testing
    SharePoint = @{
      SiteCollectionUrls = @(
        "https://contoso.sharepoint.com/sites/contoso-121"
      )

      WebUrls = @(        
        "https://contoso.sharepoint.com/sites/contoso-121"
      )
    }
    
    # various settings, such as users and so on
    # named 'Domain' for a consistency with SharePoint 2013/2016 settings
    Domain = @{
      # online users for security relates tests
      Users = @(
        @{
          UserName      = "contoso"
          EmailAddress  = "contoso@contoso.com"
        }
      )
    }
  }
}
