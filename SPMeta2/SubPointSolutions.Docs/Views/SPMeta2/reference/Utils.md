---
Title: Utils
FileName: utils.html
---
### SPMeta2 Utils

SPMeta2 has some useful utility methods, classes and constants which can be reused in your projects. 
This article provides an overview on hidden SPMeta2 gems.

### UrlUtility.CombineUrl() methods

UrlUtility.CombineUrl is a useful utility to turn a bunch of strings into URL.

<a href="_samples/Utils-UrlConcatenation.sample-ref"></a>

### WebpartXmlExtensions class
WebpartXmlExtensions provides a low level API on web part XML v2 and v3. It works on both web part XML versions allowing to read/write properties as you go.

Here is how SPMeta2 uses WebpartXmlExtensions internally:

#### Setting up ClientWebPart properties
<a href="_samples/Utils-ClientWebPartSetup.sample-ref"></a>

#### Setting up ContentEditorWebPart properties
<a href="_samples/Utils-ContentEditorWebPartSetup.sample-ref"></a>

#### Setting up XsltListViewWebPart properties
<a href="_samples/Utils-XsltListViewWebPartSetup.sample-ref"></a>

### BuiltInXXX classes
SPMeta2 has tons of BuiltInXXX classes which described out of the box SharePoint constans - field names, fields IDs, list template, list template types and so on. Here is a full list of the classes used by SPMeta2 internally and which can be reused by you.

* TODO, https://github.com/SubPointSolutions/spmeta2-docs/issues/86