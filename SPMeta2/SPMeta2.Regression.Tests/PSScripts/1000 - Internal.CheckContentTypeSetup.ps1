cls
 
Write-Host "Loading SharePoint API"
 
$ver = $host | select version
if ($ver.Version.Major -gt 1) {$host.Runspace.ThreadOptions = "ReuseThread"} 
if ((Get-PSSnapin "Microsoft.SharePoint.PowerShell" -ErrorAction SilentlyContinue) -eq $null) {
       Add-PSSnapin "Microsoft.SharePoint.PowerShell"
}

# checks that Pages and Docs are configured correctly

$rootWebUrl = "http://dev42:9299/sites/parliament-web-3"
$web = Get-SPWeb $rootWebUrl 

if($web -eq $null) {
    return
}

function isContentTypeHidden($list, $name) {

    $result = $true

    $cts = $list.RootFolder.ContentTypeOrder
    foreach($ct in $cts) {
        
        if($ct.Name -eq $name) {
            $result = $false
        }
    }

    return $result

}

function hasContentType($list, $name) {

    $result = $false

    $cts = $list.ContentTypes

    foreach($ct in $cts) {
        
        if($ct.Name -eq $name) {
            $result = $true
        }
    }

    return $result

}

function CheckPagesLibraryContentTypes($web) {

    $isValid = $true

    $pages = $web.Lists["Pages"]
    # should have 'Welcome Page' hidden

    
    $isValid = $true

    $isValid  = $isValid -and ((isContentTypeHidden  $pages "Welcome Page") -eq $true)
    $isValid  = $isValid -and ((hasContentType $pages "Welcome Page") -eq $true)
    $isValid  = $isValid -and ((hasContentType $pages "Article Page") -eq $false)
    $isValid  = $isValid -and ((hasContentType $pages "Page") -eq $false)
    

   
    if($isValid -eq $true) {
        Write-Host "`t`t[+] Page content types" -fore green 
    } else {
        Write-Host "`t`t[-] Page content types " -fore red 
    }
    #RETURN $isValid 
}


function CheckDocumentaLibraryContentTypes($web) {
    
    $isValid = $true

    $pages = $web.Lists["Pages"]
    # should have 'Welcome Page' hidden

    
    $isValid = $true

    $isValid  = $isValid -and ((hasContentType $pages "Document") -eq $false)
    

    if($isValid -eq $true) {
        Write-Host "`t`t[+] Document content types" -fore green
    } else {
        Write-Host "`t`t[-] Document content types" -fore red 
        $errors++
        #exit
    }

    #RETURN $isValid 
}

function ProcessWeb($web) {
   # Write-Host $web.ServerRelativeUrl

  
    CheckPagesLibraryContentTypes $web
    CheckDocumentaLibraryContentTypes $web
}

function ProcessSPWebRecursively([Microsoft.SharePoint.SPWeb] $web)
{
    Write-Host "Processing web: [$($web.Url)]" -fore Gray

    ProcessWeb($web)

    $subwebs = $web.GetSubwebsForCurrentUser()

    foreach($subweb in $subwebs)
    {
        ProcessSPWebRecursively($subweb)
        $subweb.Dispose()
    }   
}

Write-Host "Processing web: [$webUrl] with all subwebs" -fore Green
ProcessSPWebRecursively $web

Write-Host "Done"