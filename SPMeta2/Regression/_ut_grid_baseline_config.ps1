cls

$ScriptDirectory = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
. "$ScriptDirectory/_helpers.ps1"

$isParallerRun = $true

$configScript = {
        
        function Ensure-ISS-AppPools() {

            Import-Module WebAdministration

            $pools = Get-ChildItem –Path IIS:\AppPools

            foreach($pool in $pools) {

                $name = $pool.Name
                $state = $pool.State

                if($state -ne "Started") {
                    Write-Host "`tStarting [$name] - [$state]" -fore Yellow
                    Start-WebAppPool -Name $name
                } else {
                    Write-Host "`tStarted [$name] - [$state]" -fore Gray
                }
            }

        }

        function Restart-SharePoint {
            
            Write-Host "Restarting services..."

            Restart-Service MSSQLSERVER -Force
            Restart-Service sptimerv4 -Force

            iisreset
        }

        function Update-SharePoint-ULS() {
             Write-Host "Updating SharePoint ULS settings..."

            add-pssnapin microsoft.sharepoint.powershell
            Set-SPDiagnosticConfig -LogMaxDiskSpaceUsageEnabled
            Set-SPDiagnosticConfig -LogDiskSpaceUsageGB 1
        }

        function Set-MSSQL-Ram($ramInMb) {
             Write-Host "Updating SQL settings..."

            [System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SMO") | out-null
            $s = New-Object ("Microsoft.SqlServer.Management.Smo.Server") $server

            #$s.Configuration.MinServerMemory.ConfigValue = 3 * 1024
            $s.Configuration.MaxServerMemory.ConfigValue = $ramInMb
            $s.Configuration.Alter()
        }

         function Ensure-AssociatedGroups-ForAllSites() {

            Add-PSSnapin Microsoft.SharePoint.PowerShell
            $sites = Get-SPSite

            foreach($site in $sites)
            {
                Write-Host "Ensuring associated security group on site:[$($site.Url)]"
                Ensure-AssociatedGroups $site.Url
            }
         }

        function Ensure-AssociatedGroups($url) {

            Add-PSSnapin Microsoft.SharePoint.PowerShell


            $computerName = [environment]::MachineName

             # TODO

            $web = Get-SPWeb ($url)
            if ($web.AssociatedVisitorGroup -eq $null) {
                Write-Host 'The Visitor Group does not exist. It will be created...' -ForegroundColor DarkYellow
                $currentLogin = $web.CurrentUser.LoginName
 
                if ($web.CurrentUser.IsSiteAdmin -eq $false){
                    Write-Host ('The user '+$currentLogin+' needs to be a SiteCollection administrator, to create the default groups.') -ForegroundColor Red
                    return
                }
 
                $web.CreateDefaultAssociatedGroups($currentLogin, $currentLogin, [System.String].Empty)
                Write-Host 'The default Groups have been created.' -ForegroundColor Green
            } else {
                Write-Host 'The Visitor Group already exists.' -ForegroundColor Green
            }

        }

        function Limit-SharePoint-Search() {
             Add-PSSnapin Microsoft.SharePoint.PowerShell

            set-SPEnterpriseSearchService -PerformanceLevel Reduced
        }
        
        function Ensure-SecureStoreApplicationKey()
        {
            param (
                [Parameter(Mandatory=$true)] [ValidateNotNullorEmpty()] [string] $passPhrase,
                [Parameter(Mandatory=$false)] [ValidateNotNullorEmpty()] [boolean] $force
            )

            Write-Host "Loading SharePoint API..."
            Add-PSSnapin Microsoft.SharePoint.PowerShell

            $service = Get-SPserviceApplicationProxy  | Where-Object { $_.TypeName -eq "Secure Store Service Application Proxy" }
    

            if(($service.Properties["spmeta2.regression"] -eq $null) -or ($force -eq $true) )
            {
                Write-Host "Creating new master key with password:[$passPhrase]" -fore Green

                Update-SPSecureStoreMasterKey -ServiceApplicationProxy $service -Passphrase $passPhrase > $null
                Start-Sleep 5

                Update-SPSecureStoreApplicationServerKey -ServiceApplicationProxy $service -Passphrase $passPhrase > $null
                Start-Sleep 5

                $service.Properties["spmeta2.regression"]  = 1;
                $service.Update()

                Write-Host "Master ket was created." -fore Green
            }
            else
            {
                Write-Host "Master key exists." -fore Green
            }
        }

        $computerName = [environment]::MachineName

        Write-Host "Runing on [$computerName]"

        Ensure-AssociatedGroups-ForAllSites

        Update-SharePoint-ULS
        Limit-SharePoint-Search

        Set-MSSQL-Ram (6 * 1024)
        
        Ensure-SecureStoreApplicationKey "pass@word1" 

        Restart-SharePoint
        
        Ensure-ISS-AppPools
        Ensure-ISS-AppPools

}

$invokeParallelCmd = get-command -Name "Invoke-Parallel" -ErrorAction SilentlyContinue

if($isParallerRun -eq $false) {
    $invokeParallelCmd = $null
}

if($invokeParallelCmd -eq $null) {

    Write-Host "Configuring servers [$dsc_nodeNames] sequentially" -fore Green

    foreach($server in $dsc_nodeNames) {

    Write-Host "Configuring server [$server]" -fore Green

    $isOnline = Test-Connection -Computername $server -BufferSize 16 -Count 1 -Quiet

    if($isOnline -eq $true)
    {
        Write-Host "Server [$server] online. Processing..." -fore Green

        Invoke-Command -ScriptBlock $configScript -computer $server 
    }
    else
    {
        Write-Host "Server [$server] is OFFLINE. Skipping Processing..." -fore Yellow
    }
}

} else {

    Write-Host "Configuring servers [$dsc_nodeNames] in parallel" -fore Green

    $dsc_nodeNames | Invoke-Parallel -ScriptBlock {
        
        $server = $_

        Write-Host "Configuring server [$server]" -fore Green

        $isOnline = Test-Connection -Computername $server -BufferSize 16 -Count 1 -Quiet

        if($isOnline -eq $true)
        {
            Write-Host "Server [$server] online. Processing..." -fore Green

            Invoke-Command -ScriptBlock $configScript -computer $server 
        }
        else
        {
            Write-Host "Server [$server] is OFFLINE. Skipping Processing..." -fore Yellow
        }

    } 
}