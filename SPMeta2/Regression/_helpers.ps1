

function Get-RegressionConfig() {
    
    param (
        [Parameter(Mandatory=$true)] [ValidateNotNullorEmpty()] [string] $yamlFilePath
    )

    $yaml = (Get-Content $yamlFilePath -raw)
    $config = ConvertFrom-Yaml $yaml

    return $config
}

function Get-EnvironmentVariable($name) {
	
	$result = [System.Environment]::GetEnvironmentVariable($name) 
	if($result -ne $null) { return $result; }

	$result = [System.Environment]::GetEnvironmentVariable($name, "Machine") 
	if($result -ne $null) { return $result; }

	$result = [System.Environment]::GetEnvironmentVariable($name, "Process") 
	if($result -ne $null) { return $result; }

	$result = [System.Environment]::GetEnvironmentVariable($name, "User") 
	if($result -ne $null) { return $result; }

	return $null
}

if($dsc_configFilePath -eq $null) {
    
    $dsc_configFilePath = "config.yaml"

    # environment specific?
    $environmentSpecificConfigPath = Get-EnvironmentVariable "SPMeta2.Regression.Config.FilePath"

    if( [System.IO.File]::Exists($environmentSpecificConfigPath) -eq $true) {
        $dsc_configFilePath = $environmentSpecificConfigPath
        Write-Host "Using environment specific config file:[$dsc_configFilePath]" -ForegroundColor Green
    }
    else {
        $dsc_configFilePath = "config.yaml"
        Write-Host "Using default config file:[$dsc_configFilePath]" -ForegroundColor Green
        
    }
}

if( [System.IO.File]::Exists($dsc_configFilePath) -eq $false) {
    throw "Config file [$dsc_configFilePath] does not exist"
}


$dsc_config = Get-RegressionConfig($dsc_configFilePath)
$env_config = $dsc_config.Configuration.Environment
$regression_config = $dsc_config.Configuration.RegressionTests

$dsc_nodeNames = $dsc_config.Configuration.DSC.NodeNames

$dsc_RunAsUserName = $dsc_config.Configuration.DSC.RunAs.UserName
$dsc_RunAsUserPassword = $dsc_config.Configuration.DSC.RunAs.UserPassword

# from env variable?


$dsc_RunAsUserSecurePassword = ConvertTo-SecureString $dsc_RunAsUserPassword -AsPlainText -Force
$dsc_RunAsUserCredentials = New-Object System.Management.Automation.PSCredential ($dsc_RunAsUserName, $dsc_RunAsUserSecurePassword)

$dsc_WebPoolManagedAccountUserName = $dsc_config.Configuration.DSC.WebPoolManagedAccount.UserName
$dsc_WebPoolManagedAccountUserPassword = $dsc_config.Configuration.DSC.WebPoolManagedAccount.UserPassword
$dsc_WebPoolManagedAccountSecurePassword = ConvertTo-SecureString $dsc_WebPoolManagedAccountUserPassword -AsPlainText -Force
$dsc_WebPoolManagedCredentials = New-Object System.Management.Automation.PSCredential ($dsc_WebPoolManagedAccountUserName, $dsc_WebPoolManagedAccountSecurePassword)

$dsc_SPSetupAccountAccountUserName = $dsc_config.Configuration.DSC.SPSetupAccount.UserName
$dsc_SPSetupAccountAccountUserPassword = $dsc_config.Configuration.DSC.SPSetupAccount.UserPassword
$dsc_SPSetupAccountSecurePassword = ConvertTo-SecureString $dsc_SPSetupAccountAccountUserPassword -AsPlainText -Force
$dsc_SPSetupAccountCredentials = New-Object System.Management.Automation.PSCredential ($dsc_SPSetupAccountAccountUserName, $dsc_SPSetupAccountSecurePassword)

function Ensure-PowerShellModule {
    
    foreach($nodeName in $dsc_nodeNames) {
    
        $moduleName = "PowerShellModule"
        $moduleVersion = 0.3

        Write-Host "Ensuring module [$moduleName] on node:[$nodeName]" -fore green

        Invoke-Command -ScriptBlock {
        
            function InstallModule($name, $version) {

                $module = Get-Module -ListAvailable | where-object { $_.Name -eq "PowerShellModule" }

                if($module -ne $null -and $module.Version -eq $version) {

                    Write-Host "`tModule exists [$name] [$version]" -fore gray
                } else {

                    Write-Host "`tInstalling module [$name] [$version]" -fore gray
           
                    Find-Module -Name $name -RequiredVersion $version -Repository PSGallery | Install-Module
                    Get-DscResource -Module $name
                }
            }    
    
            InstallModule PowerShellModule 0.3

        } -ComputerName $nodeName
    }

}

function Apply-Dsc-Configuration {

     param (
        [Parameter(Mandatory=$true)] [ValidateNotNullorEmpty()] [string] $name,
        [Parameter(Mandatory=$true)] [ValidateNotNullorEmpty()] [string[]] $nodeNames,
        [Parameter(Mandatory=$false)][object] $config,
        [Parameter(Mandatory=$false)][boolean] $isVerbose = $true
    )

    if($config -eq $null) {
        $config = @{
             AllNodes = @()
        }
    }

    
    $config.AllNodes[0].RecurseValue = $true 

    $config.AllNodes[0].PsDscAllowPlainTextPassword = $true 
    $config.AllNodes[0].PSDscAllowDomainUser = $true 

    Write-Host "Cleaning up DSC config [$name]..." -fore green
    Remove-Item $name -Force -Recurse -Confirm:$false -ErrorAction:SilentlyContinue | out-null

    Write-Host "Compiling DSC config [$name] for [$($nodeNames.Count)] nodes..." -fore green

    foreach($nodeName in $nodeNames) {
    
        Write-Host "`tCompiling DSC config [$name] for node [$nodeName]" -fore Gray

        $config.AllNodes[0].NodeName = $nodeName 

        . $name -NodeName $nodeName `
                -RunAsCredential $dsc_RunAsUserCredentials `
                -ConfigurationData $config | out-null
    }

    Write-Host "Starting DSC config [$name]..." -fore Green

    if($isVerbose -eq $true)
    {
        Start-DscConfiguration $name  -force `
                                      -Wait  `
                                      -Verbose 
    } 
    else
    {
        Start-DscConfiguration $name  -force `
                                      -Wait       
    }

    Write-Host "Testing DSC config [$name]..." -fore Green
    $results = Test-DscConfiguration $name 

    foreach($result in $results) {

        $node = $result.PSComputerName
        $state = $result.InDesiredState

        $color = "Gray"

        if($result.InDesiredState) {
             $color = "Gray"
        } else {
            $color = "Red"    
        }

        Write-Host "`tNode[$node]`tInDesiredState:[$state]" -fore $color
    }
}