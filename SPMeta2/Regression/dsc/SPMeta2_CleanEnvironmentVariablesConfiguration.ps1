Configuration SPMeta2_CleanEnvironmentVariablesConfiguration {
    
    Node localhost
    {
        $variables = $Node.Variables

        foreach($variable in $variables.Keys) {

            $variableName = $variable 
            $variableValue =  $variables[$variable]

            $resourceName = $variableName.Replace(".", "_")

            Environment $resourceName {
                Name = $variableName
                Ensure = 'Absent'
            }
        }
    }
}
