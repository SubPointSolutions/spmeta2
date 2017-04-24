Adding content types to a list is enabled via a ContentTypeLinkDefinition object.

Both CSOM/SSOM object models are supported. 
Provision checks if a content type exists in a particular list by Name. 
If a content type cannot be found by Name, provision tries to find a list content type by comparing its parent content type Id. 
You can deploy either a single content type link or a set using AddContentTypeLinks() extension method as per following examples.