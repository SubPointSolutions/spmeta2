
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

. "$currentDir\SPMeta2.Core.ps1"


cls

$p = @()

$p += "spmeta2.dll"
$p += "spmeta2.common.dll"
$p += "spmeta2.default.syntax.dll"

$p += "spmeta2.csom.dll"
$p += "SPMeta2.CSOM.Behaviours.dll"

$p += "SPMeta2.CSOM.Standard.dll"
$p += "SPMeta2.CSOM.Standard.Behaviours.dll"

$p += "SPMeta2.SSOM.dll"
$p += "SPMeta2.SSOM.Behaviours.dll"

$p += "SPMeta2.Regression.dll"
$p += "SPMeta2.Regression.Common.dll"

$p += "SPMeta2.Regression.SSOM.dll"
$p += "SPMeta2.Regression.CSOM.dll"

$p += "SPMeta2.Validation.dll"

CreatePackages $p
