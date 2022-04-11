Configuration SPMeta2_DomainConfiguration {

    Import-DscResource -ModuleName xActiveDirectory
   
    $userPass       = ConvertTo-SecureString "spmeta2-123!@#" -AsPlainText -Force
    $userPassCreds  = New-Object System.Management.Automation.PSCredential("vagrant", $userPass)
    
    Node localhost
    {
        $users  = $Node.Users
        $groups = $Node.Groups

        $runAsAccount = $Node.RunAsAccount

        foreach($user in $users) {
            xADUser $user.UserName
            {
                Ensure = "Present"  

                DomainName   = $user.DomainName
                UserName     = $user.UserName
                EmailAddress = $user.EmailAddress
                Password     = $userPassCreds

                DomainAdministratorCredential  = $runAsAccount
            }
        }

        foreach($group in $groups) {
            xADGroup $group.GroupName
            {
                Ensure = "Present"

                GroupName = $group.GroupName
                Category  = $group.Category

                Credential = $runAsAccount
            }
        }
    }        
}
