cls

# fail on errors and include metabox helpers
$ErrorActionPreference = "Stop"

$coreScript = "$($PSScriptRoot)/utils/_spmeta2.utils.ps1"
if(Test-Path $coreScript) { . $coreScript } else { throw "Cannot find core script: $coreScript"}

Ensure-M2PSModules