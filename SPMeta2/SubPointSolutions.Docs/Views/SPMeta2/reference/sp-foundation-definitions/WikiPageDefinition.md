
Wiki page provision is enabled via WikiPageDefinition object.

Both CSOM/SSOM object models are supported. Provision checks if wiki page exists looking up it by FileName property, then creates a new wiki page. You can deploy either single wiki page or a set of the pages using AddWikiPage() extension method as per following examples.

Content property gets copied to the wiki page via “SPBuiltInFieldId.WikiField” value.

Nesting under folders is supported as well.
