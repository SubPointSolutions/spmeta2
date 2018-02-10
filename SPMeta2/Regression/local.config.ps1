
# fail on errors and include metabox helpers
$ErrorActionPreference = "Stop"

$spMeta2Config = @{
    
}

$coreScript = "$($PSScriptRoot)\utils\_spmeta2.utils.ps1"
if(Test-Path $coreScript) { . $coreScript } else { throw "Cannot find core script: $coreScript"}

$dscFolder = "$($PSScriptRoot)\dsc\"
$files = Get-ChildItem $dscFolder
Log-M2InfoMessage "Loading [$($dscFolder.Count)] configurations: $dscFolder"

foreach($file in $files) {
    Log-M2DebugMessage "Loading DSC config: $($file.FullName)"
    . $file.FullName
}

$configScript = "$($PSScriptRoot)\config.default.ps1"
Log-M2InfoMessage "Loading default config file: $configScript"
if(!(Test-Path $configScript)) { throw "Cannot find core script: $configScript"}
. $configScript 

$configPath = "$($PSScriptRoot)\.configs\"
$files = Get-ChildItem $configPath
Log-M2InfoMessage "Loading [$($files.Count)] configs from: $configPath"

foreach($file in $files) {
    Log-M2DebugMessage "Loading config config: $($file.FullName)"
    . $file.FullName
}



