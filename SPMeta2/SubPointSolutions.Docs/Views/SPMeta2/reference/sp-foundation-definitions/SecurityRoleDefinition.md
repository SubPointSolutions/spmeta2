
Security role provision is enabled via SecurityRoleDefinition object.

Both CSOM/SSOM object models are supported. 
Provision checks if object exists looking up it by Name property, then creates a new object. 
You can deploy either single object or a set of the objects using AddSecurityRole() extension method as per following examples.

