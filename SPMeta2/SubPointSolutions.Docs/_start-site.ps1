cls

$wyam = "Wyam"

cd  $PSScriptRoot

Start-Process "chrome.exe" http://localhost:5080
. "$PSScriptRoot\$wyam\Wyam.exe"  --input "$PSScriptRoot\Views" --output "$PSScriptRoot\Views-Output" --preview --watch --config "_site.wyam" 