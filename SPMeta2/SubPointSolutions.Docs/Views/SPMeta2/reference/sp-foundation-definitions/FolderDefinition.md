Folders provision is enabled via FolderDefinition object.

Both CSOM/SSOM object models are supported. Provision checks if folder exists looking up it by Name property, then creates a new folder. You can deploy either single folder or a set of the folder using AddFolder() extension method as per following examples. Folder nesting is supported as well.

