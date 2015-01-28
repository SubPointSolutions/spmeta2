cls

function Get-ScriptDirectory
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

$currentPath =  Get-ScriptDirectory
$solutionRootPath =  "$currentPath\..\"

$msbuild_path = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"

# main solution, all, 45
#& $msbuild_path """$solutionRootPath\..\SPMeta2.sln"" /t:Clean,Rebuild /p:WarningLevel=0"

# partial, for NuGet, 40/45
& $msbuild_path """$solutionRootPath\SPMeta2\SPMeta2.csproj"" /t:Clean,Rebuild /p:Configuration=Debug40 /p:Platform=AnyCPU /p:WarningLevel=0"
& $msbuild_path """$solutionRootPath\SPMeta2\SPMeta2.csproj"" /t:Clean,Rebuild /p:Configuration=Debug45 /p:Platform=AnyCPU /p:WarningLevel=0"

& $msbuild_path """$solutionRootPath\SPMeta2.Syntax.Default\SPMeta2.Syntax.Default.csproj"" /t:Clean,Rebuild /p:Configuration=Debug40 /p:Platform=AnyCPU /p:WarningLevel=0"
& $msbuild_path """$solutionRootPath\SPMeta2.Syntax.Default\SPMeta2.Syntax.Default.csproj"" /t:Clean,Rebuild /p:Configuration=Debug45 /p:Platform=AnyCPU /p:WarningLevel=0"

& $msbuild_path """$solutionRootPath\SPMeta2.Standard\SPMeta2.Standard.csproj"" /t:Clean,Rebuild /p:Configuration=Debug40 /p:Platform=AnyCPU /p:WarningLevel=0"
& $msbuild_path """$solutionRootPath\SPMeta2.Standard\SPMeta2.Standard.csproj"" /t:Clean,Rebuild /p:Configuration=Debug45 /p:Platform=AnyCPU /p:WarningLevel=0"

# CSOM

& $msbuild_path """$solutionRootPath\SPMeta2.CSOM\SPMeta2.CSOM.csproj"" /t:Clean,Rebuild /p:Configuration=Debug40 /p:Platform=AnyCPU /p:WarningLevel=0"
& $msbuild_path """$solutionRootPath\SPMeta2.CSOM\SPMeta2.CSOM.csproj"" /t:Clean,Rebuild /p:Configuration=Debug45 /p:Platform=AnyCPU /p:WarningLevel=0"

& $msbuild_path """$solutionRootPath\SPMeta2.CSOM.Behaviours\SPMeta2.CSOM.Behaviours.csproj"" /t:Clean,Rebuild /p:Configuration=Debug40 /p:Platform=AnyCPU /p:WarningLevel=0"
& $msbuild_path """$solutionRootPath\SPMeta2.CSOM.Behaviours\SPMeta2.CSOM.Behaviours.csproj"" /t:Clean,Rebuild /p:Configuration=Debug45 /p:Platform=AnyCPU /p:WarningLevel=0"



& $msbuild_path """$solutionRootPath\SPMeta2.CSOM.Standard\SPMeta2.CSOM.Standard.csproj"" /t:Clean,Rebuild /p:Configuration=Debug40 /p:Platform=AnyCPU /p:WarningLevel=0"
& $msbuild_path """$solutionRootPath\SPMeta2.CSOM.Standard\SPMeta2.CSOM.Standard.csproj"" /t:Clean,Rebuild /p:Configuration=Debug45 /p:Platform=AnyCPU /p:WarningLevel=0"

& $msbuild_path """$solutionRootPath\SPMeta2.CSOM.Standard.Behaviours\SPMeta2.CSOM.Standard.Behaviours.csproj"" /t:Clean,Rebuild /p:Configuration=Debug40 /p:Platform=AnyCPU /p:WarningLevel=0"
& $msbuild_path """$solutionRootPath\SPMeta2.CSOM.Standard.Behaviours\SPMeta2.CSOM.Standard.Behaviours.csproj"" /t:Clean,Rebuild /p:Configuration=Debug45 /p:Platform=AnyCPU /p:WarningLevel=0"

# SSOM

& $msbuild_path """$solutionRootPath\SPMeta2.SSOM\SPMeta2.SSOM.csproj"" /t:Clean,Rebuild /p:Configuration=Debug40 /p:Platform=AnyCPU /p:WarningLevel=0"
& $msbuild_path """$solutionRootPath\SPMeta2.SSOM\SPMeta2.SSOM.csproj"" /t:Clean,Rebuild /p:Configuration=Debug45 /p:Platform=AnyCPU /p:WarningLevel=0"

& $msbuild_path """$solutionRootPath\SPMeta2.SSOM.Behaviours\SPMeta2.SSOM.Behaviours.csproj"" /t:Clean,Rebuild /p:Configuration=Debug40 /p:Platform=AnyCPU /p:WarningLevel=0"
& $msbuild_path """$solutionRootPath\SPMeta2.CSOM.Behaviours\SPMeta2.SSOM.Behaviours.csproj"" /t:Clean,Rebuild /p:Configuration=Debug45 /p:Platform=AnyCPU /p:WarningLevel=0"

& $msbuild_path """$solutionRootPath\SPMeta2.SSOM.Standard\SPMeta2.SSOM.Standard.csproj"" /t:Clean,Rebuild /p:Configuration=Debug40 /p:Platform=AnyCPU /p:WarningLevel=0"
& $msbuild_path """$solutionRootPath\SPMeta2.SSOM.Standard\SPMeta2.SSOM.Standard.csproj"" /t:Clean,Rebuild /p:Configuration=Debug45 /p:Platform=AnyCPU /p:WarningLevel=0"

# validation
& $msbuild_path """$solutionRootPath\SPMeta2.Validation\SPMeta2.Validation.csproj"" /t:Clean,Rebuild /p:Configuration=Debug40 /p:Platform=AnyCPU /p:WarningLevel=0"
& $msbuild_path """$solutionRootPath\SPMeta2.Validation\SPMeta2.Validation.csproj"" /t:Clean,Rebuild /p:Configuration=Debug45 /p:Platform=AnyCPU /p:WarningLevel=0"
