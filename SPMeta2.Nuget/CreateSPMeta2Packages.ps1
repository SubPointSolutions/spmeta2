
function GetCurrentDirectory
{
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value;
    if($Invocation.PSScriptRoot)
    {
        $Invocation.PSScriptRoot;
    }
    Elseif($Invocation.MyCommand.Path)
    {
        Split-Path $Invocation.MyCommand.Path
    }
    else
    {
        $Invocation.InvocationName.Substring(0,$Invocation.InvocationName.LastIndexOf("\"));
    }
}

$currentDir = GetCurrentDirectory

. "$currentDir\SPMeta2.Core.Config.ps1"
. "$currentDir\SPMeta2.Core.ps1"


cls
CreateSPMeta2Packages $shouldPublish $useDayVersion
