Module files provision is enabled via ModuleFileDefinition object.

Should be deployed under a document library or content type.
In case of content type, a resource folder is used.

Both CSOM/SSOM object models are supported. 
Provision checks if object exists looking up it by Name property, then creates a new object. 
You can deploy either single object or a set of the objects using AddModuleFile() extension method as per following examples.