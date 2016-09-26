### Provision scenario
We should be able to provision a "Content Editor Web Part" in a nice, repeatable way.

### Scope
Should be deployed under wiki, web part or publishing page.

### Implementation
Content Editor Web Part provision is enabled via ContentEditorWebPartDefinition object.

There are two properties which are exposed by ContentEditorWebPartDefinition:

* **ContentLink**, URL of a target content
* **Content**, actual content

SPMeta2 adds the following tokens support for ContentLink property:

* ~sitecollection, replaced by site.ServerRelativeUrl
* ~site, replaced by web.ServerRelativeUrl

Both CSOM/SSOM object models are supported. 
You can deploy either single object or a set of the objects using AddContentEditorWebPart() extension method as per following examples.
