#region utils


function Get-TimeStamp() {
    return $(get-date -f "yyyy-MM-dd HH:mm:ss") 
}

function Write-BError($msg, $fore = 'red') {
    
    $stamp = Get-TimeStamp

    if([string]::IsNullOrEmpty($msg) -eq $false) {
        $msg = "[$stamp] [ERROR] $msg"
    } else {
        $msg = "[$stamp] [ERROR]"
    }

    Write-Host $msg -fore $fore
}

function Write-BVerbose($msg, $fore = 'gray') {
    
    $stamp = Get-TimeStamp

    if([string]::IsNullOrEmpty($msg) -eq $false) {
        $msg = "[$stamp] [Verbose] $msg"
    } else {
        $msg = "[$stamp] [Verbose]"
    }

    Write-Host $msg -fore $fore
}

function Write-BInfo($msg, $fore = 'green') {
    
    $stamp = Get-TimeStamp

    if([string]::IsNullOrEmpty($msg) -eq $false) {
        $msg = "[$stamp] [INFO] $msg"
    } else {
        $msg = "[$stamp] [INFO]"
    }

    Write-Host $msg -fore $fore
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

# https://www.appveyor.com/docs/build-phase
$g_isAppVeyor = ((Get-EnvironmentVariable "APPVEYOR") -ne $null)

#endregion