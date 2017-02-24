
Write-Host "Running pester regression..."
$results = Invoke-Pester -Script Pester/regression.nuget.ps1 -PassThru -Quiet

$failedCount = 0;
$passedCount = 0;

foreach($r in $results.TestResult) {

  Write-Host "- Test [$($r.Name)] with result:[$($r.Result)]"

  switch($r.Result) {
      "Failed"  {
          $failedCount++
      }; 
       "Passed"  {
          $passedCount++
      }; 
  }
}

Write-Host "- Failed count:[$failedCount]"
Write-Host "- Passed count:[$passedCount]"

if($failedCount -gt 0) {
    throw "[FAIL] Didn't pass Pester regression. Failed tests count:[$failedCount]"
}
else{
    if($passedCount -le 0) {
        throw "[FAIL] Weird. No tests were passed. Check it, please?"
    }
} 