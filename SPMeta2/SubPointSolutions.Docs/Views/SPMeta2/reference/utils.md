---
Title: Utils
FileName: utils.html
---
### SPMeta2 Utils

SPMeta2 has some useful utility methods, classes and constants which can be reused in your projects. 
This article provides an overview on hidden SPMeta2 gems.

### UrlUtility.CombineUrl() methods

UrlUtility.CombineUrl is a useful utility to turn a bunch of strings into URL.

```csharp
// fast on two params
var smQueryUrl = UrlUtility.CombineUrl("http://goole.com", "?q=spmeta2");
 
// a bigger one
var bgQueryUrl = UrlUtility.CombineUrl(new string[]{
    "http://goole.com",
    "?",
    "q=1",
    "&p1=3",
    "&p2=tmp"
});
```

### WebpartXmlExtensions class
WebpartXmlExtensions provides a low level API on web part XML v2 and v3. It works on both web part XML versions allowing to read/write properties as you go.

Here is how SPMeta2 uses WebpartXmlExtensions internally:

#### Setting up ClientWebPart properties
```csharp
var wpXml = WebpartXmlExtensions
               .LoadWebpartXmlDocument(BuiltInWebPartTemplates.ClientWebPart)
               .SetOrUpdateProperty("FeatureId", wpModel.FeatureId.ToString())
               .SetOrUpdateProperty("ProductId", wpModel.ProductId.ToString())
               .SetOrUpdateProperty("WebPartName", wpModel.WebPartName)
               .SetOrUpdateProperty("ProductWebId", webId)
               .ToString();
```

#### Setting up ContentEditorWebPart properties
```csharp
var wpXml = WebpartXmlExtensions
               .LoadWebpartXmlDocument(BuiltInWebPartTemplates.ContentEditorWebPart)
               .SetOrUpdateContentEditorWebPartProperty("Content", content, true)
               .SetOrUpdateContentEditorWebPartProperty("ContentLink", contentLink)
               .ToString();
```

#### Setting up XsltListViewWebPart properties
```csharp
var wpXml = WebpartXmlExtensions
                .LoadWebpartXmlDocument(BuiltInWebPartTemplates.XsltListViewWebPart)
                .SetListName(listName)
                .SetListId(listId)
                .SetTitleUrl(titleUrl)
                .SetOrUpdateProperty("JSLink", jsLink)
                .ToString();
```

### BuiltInXXX classes
SPMeta2 has tons of BuiltInXXX classes which described out of the box SharePoint constans - field names, fields IDs, list template, list template types and so on. Here is a full list of the classes used by SPMeta2 internally and which can be reused by you.

* TODO, https://github.com/SubPointSolutions/spmeta2-docs/issues/86
