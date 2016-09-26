
Sandbox solution provision is enabled via SandboxSolutionDefinition object.

Both CSOM/SSOM object models are supported. 
Provision checks if object exists looking up it by SolutionId property, then deletes or upload a new object. 
You can deploy either single object or a set of the objects using AddSandboxSolution() extension method as per following examples.

Here are a few things you need to keep in mind:

* CSOM uses DesignPackage API
* **FileName** must not have "." - DesignPackage API fails to remove file with "."
* **SolutionId** must be set, it is used to lookup existing package
* **Content** is a byte array, so get it from whatver you wish - local folder or assembly resource
* **Activate** must be 'True' for CSOM - DesignPackage API limition
