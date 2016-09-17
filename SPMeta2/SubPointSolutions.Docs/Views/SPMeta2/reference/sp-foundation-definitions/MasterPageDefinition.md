Master page provision is enabled via MasterPageDefinition object.

Both CSOM/SSOM object models are supported.
Provision checks if a master page exists looking up it by FileName property, then creates a new one. 
You can deploy either single object or a set of the object using AddMasterPage() extension method as per following examples.

We suggest to use BuiltInListDefinitions.Calalogs.MasterPage to resolve built-in master page gallery list.
