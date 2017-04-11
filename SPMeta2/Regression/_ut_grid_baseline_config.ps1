cls

$ScriptDirectory = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
. "$ScriptDirectory/_helpers.ps1"

foreach($server in $dsc_nodeNames) {

    Write-Host "Configuring server [$server]" -fore Green

    $isOnline = Test-Connection -Computername $server -BufferSize 16 -Count 1 -Quiet

    if($isOnline -eq $true)
    {
        Write-Host "Server [$server] online. Processing..." -fore Green

    Invoke-Command -ScriptBlock {

        
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

        function Ensure-AssociatedGroups() {

            Add-PSSnapin Microsoft.SharePoint.PowerShell


            $computerName = [environment]::MachineName

             # TODO

            $web = Get-SPWeb ("http://" + $computerName + ":31449")
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

        Ensure-AssociatedGroups

        Update-SharePoint-ULS
        Limit-SharePoint-Search

        Set-MSSQL-Ram (6 * 1024)
        
        Restart-SharePoint
        
        Ensure-ISS-AppPools
        Ensure-ISS-AppPools

    } -computer $server 
    }
    else
    {
        Write-Host "Server [$server] is OFFLINE. Skipping Processing..." -fore Yellow
    }
}