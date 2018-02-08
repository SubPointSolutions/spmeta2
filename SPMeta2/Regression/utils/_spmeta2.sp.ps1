
function Ensure-SPPSSnapin 
{
    $ver = $host | select version
    if ($ver.Version.Major -gt 1) {$host.Runspace.ThreadOptions = "ReuseThread"} 
    if ((Get-PSSnapin "Microsoft.SharePoint.PowerShell" -ErrorAction SilentlyContinue) -eq $null) {
        Add-PSSnapin "Microsoft.SharePoint.PowerShell" -ErrorAction SilentlyContinue
    }
}

function Get-M2SPServiceAppProxy($name) {
    $all = Get-SPServiceApplicationProxy
    $result = $all | Where-Object { $_.Name -eq $name }
    
    if($result.Count -eq $null)  {
        foreach($proxy in $all) {
             Log-M2InfoMessage "$proxy"
        }

        throw "Cannot finde any proxy of type: $name"
    }

    $result
}


function Get-M2SPServiceInstance($name) {
    $all = Get-SPServiceInstance
    $result = $all | Where-Object { $_.TypeName -eq $name }
    
    if($result.Count -eq $null)  {
        foreach($proxy in $all) {
             Log-M2InfoMessage "$proxy"
        }

        throw "Cannot finde any service instance of type: $name"
    }

    $result
}

function Set-M2DefaultSiteCollectionTaxonomy {

    $apps = Get-M2SPServiceAppProxy "Managed Metadata Service Application Proxy"

    foreach($app in $apps) {
        Log-M2InfoMessage "Updating: $app"
        Log-M2InfoMessage " IsDefaultSiteCollectionTaxonomy: true"

        $app.Properties["IsDefaultSiteCollectionTaxonomy"] = $true
        $app.Update() 
    } 
}

function Enable-SPUserCodeService {

    $services = Get-M2SPServiceInstance "Microsoft SharePoint Foundation Sandboxed Code Service"

    foreach($service in $services) {
        Log-M2InfoMessage "starting service: $service"

        $service | Start-SPServiceInstance -confirm:$false 
    } 
}

function Ensure-SPAssemblies {

    Param(
        [Parameter(Mandatory=$True)]
        [String]$path
    )      

    Log-M2InfoMessage "Loading SharePoint CSOM API" 

    $files = [System.IO.Directory]::GetFiles($path, "*.dll")

    if($files.Count -eq 0) {
        throw "Cannot find any assemblies in path: $path"
    } else {
        Log-M2InfoMessage "`tfound file: $($files.Count)"
    }

    foreach($filePath in $files) {
        Log-M2InfoMessage "`tloading assembly: [$filePath]"
        $a = [System.Reflection.Assembly]::LoadFile($filePath)
    }

}

function Update-SharePoint-ULS() {
    Log-M2InfoMessage  "Updating SharePoint ULS settings..."
    
    Set-SPDiagnosticConfig -LogMaxDiskSpaceUsageEnabled
    Set-SPDiagnosticConfig -LogDiskSpaceUsageGB 1
}

  function Ensure-M2AssociatedGroupsAllSites {
    
    $sites = Get-SPSite

    foreach($site in $sites)
    {
        Log-M2InfoMessage  "Ensuring associated security group on site:[$($site.Url)]"
        Ensure-M2AssociatedGroups $site.Url
    }
}

 function Ensure-M2AssociatedGroups($url) {

    $computerName = [environment]::MachineName

    $web = Get-SPWeb ($url)
    if ($web.AssociatedVisitorGroup -eq $null) {
        Log-M2InfoMessage  "`tthe Visitor Group does not exist. It will be created..."
        $currentLogin = $web.CurrentUser.LoginName

        if ($web.CurrentUser.IsSiteAdmin -eq $false){
            Log-M2InfoMessage  ("`tthe user '+$currentLogin+' needs to be a SiteCollection administrator, to create the default groups.") 
            return
        }

        $web.CreateDefaultAssociatedGroups($currentLogin, $currentLogin, [System.String].Empty)
        Log-M2InfoMessage "`tthe default Groups have been created."
    } else {
       Log-M2InfoMessage "`tthe Visitor Group already exists."
    }

}