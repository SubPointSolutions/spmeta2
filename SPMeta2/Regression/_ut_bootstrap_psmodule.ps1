cls

$ScriptDirectory = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
. "$ScriptDirectory/_helpers.ps1"

Ensure-PowerShellModule