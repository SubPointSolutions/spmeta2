
function GetPackagesDirectory
{
	return "SPMeta2";
}

function GetSolutionDirectory
{
	$dir = GetCurrentDirectory

	$dir += "\..\"


	return $dir;
}

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

function CreatePackages($asm) {
	
	$solutionDirectory = GetSolutionDirectory
	
	$currentDir = GetCurrentDirectory
	$packagesDirName = GetPackagesDirectory

	$targetFolder = [System.IO.Path]::Combine($currentDir, $packagesDirName)
	
	foreach($fileName in $asm) {
		
		$files = Get-ChildItem $solutionDirectory -Recurse -Include "$fileName"

		if($files.Length -gt 0) {
			$targetFile = $files[0];
			
			Write-Host "Copying... [$targetFile]"		
			Copy-Item $targetFile $targetFolder -Force 
		}
	}
}