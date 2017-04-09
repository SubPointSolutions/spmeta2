cls

$ScriptDirectory = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
. "$ScriptDirectory/_helpers.ps1"

foreach($server in $dsc_nodeNames) {

    Write-Host "Configuring server [$server]" -fore Green

    Invoke-Command -ScriptBlock {

        
        Write-Host "Updating SharePoint ULS settings..."

        add-pssnapin microsoft.sharepoint.powershell
        Set-SPDiagnosticConfig -LogMaxDiskSpaceUsageEnabled
        Set-SPDiagnosticConfig -LogDiskSpaceUsageGB 1

        Write-Host "Updating SQL settings..."

        [System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SMO") | out-null
        $s = New-Object ("Microsoft.SqlServer.Management.Smo.Server") $server

        #$s.Configuration.MinServerMemory.ConfigValue = 3 * 1024
        $s.Configuration.MaxServerMemory.ConfigValue = 3 * 1024
        $s.Configuration.Alter()

        Write-Host "Restarting services..."

        Restart-Service MSSQLSERVER -Force
        Restart-Service sptimerv4 -Force

        iisreset

    } -computer $server 
}