
There are a few things around web part provision within SPMeta2 library:

* **ID** is used as a key for SSOM provision
* **Title** is used as a key for CSOM provision
* CSOM provision always deleted a web part and adds a new one

SPMeta2 provides three ways to provision a new web part. 
You use one of the following properties to define the provisino strategy:

* WebpartType prop (SSOM only) - provising AssemblyQualifiedName of the web part
* WebpartXmlTemplate prop - providing web part XML 
* WebpartFileName prop - providing file name from the web part gallery

**WebpartType** can be used only with SSOM. SPMeta2 would use the reflection to create an instance of the web part and either add or update it later.

**WebpartXmlTemplate** is supported by CSOM/SSOM. You need to provide XML template if the web part. Use either Sharepoint Designer to get one, or use web part export on the SharePoint page.

**WebpartFileName** needs to be a web part file name in the web part gallery. SPMeta2 would read this file, use this XML template and then push the web part on the page.

**ZoneId** and **ZoneIndex** are used to put the web part on the right place in the target page. That works well for web part pages along with publishing pages.

**AddToPageContent** flag is used to indicate that the web part needs to be added to the 'Content' are of the wiki/publishing page. Check more details on embedding web parts into wiki pages in the samples below.

Notice that WebPartDefinition provides a basic provision for generic web part. A much better provision could be archived either by using OnProvisioned events or "typed" web part definitions such as ContentEditorWebPartDefinition, ScriptEditorWebPartDefinition and the rest.
