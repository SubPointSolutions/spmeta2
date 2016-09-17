
Web part page provision via WebPartPageDefinition object.

Both CSOM/SSOM object models are supported. Provision checks if artifact exists looking up it by FileName property, then creates a new site field. 
You can deploy either single page or a set of the pages using AddWebPartPage() extension method as per following examples.

We suggest to use BuiltInWebPartPageTemplates to address PageLayoutTemplate property. Use CustomPageLayout in case you need a custom web part page template.
