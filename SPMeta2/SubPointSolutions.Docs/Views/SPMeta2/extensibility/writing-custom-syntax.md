---
Title: Custom syntax
FileName: custom-syntax.html
Order: 400
---

### Writing custom syntax extension
SPMeta2 provides a low level syntax API to compose definitions into a model. Sometimes a model can get quite big, clunky and hard to maintain, so a custom syntax extension can be written to fill the gap.
This article provides basics on creating a custom definion and model handler for SPMeta2 library.

Before you begin, make sure you are familiar with the following concepts:

* [Get started with SPMeta2](/spmeta2/getting-started)
* [Definitions concept](/spmeta2/reference/definitions)
* [Models concept](/spmeta2/reference/models)
* [Provisioning services concept](/spmeta2/reference/provisionservices)

There is a naming convention which is used to provide base syntax API. All SPMeta2 definitions meet the following criterias where XXX is a name of the definition:

#### Required criterias
* AddXXX(definition) method must exist. It provides a basic API to add a definition to the model.
* AddXXX(definition, action) method must exist. Adds a definition to the model and provides a call back to resolve current definition's model node.
* AddXXXs(IEnumerable<>) method must exist. It provides an array overlow for adding a bunch of the definitions to the model.

#### Optional criterias
* AddHostXXXs(action) method should optionally exist
AddHostXXXs method is used to set ModelNode.Options.RequireSelfProcessing to 'FALSE', so that a definition won't be processes. It assumes that artifact already exists, and definition is used only to lookup artifact to continue provision flow.

### Writing custom syntax 
Most of the time you would need a custom definition to address the following scenarios:

* Make the model more readable and maintainable
* Introduce project specific domain

The following method prototype should be used to create a custom syntax extension:

<a href="_samples/writing-custom-syntax-SyntaxExtensionPrototype.sample-ref"></a>

Let's have a close look and see how it goes. Here is a scenarios - we need to hide some fields in the target content type, and here is how it can be done with SPMeta2:

<a href="_samples/writing-custom-syntax-HideContentTypeFieldsAsOOTB.sample-ref"></a>

This does the job hiding fields within target content type, but looks to hard to understand and maintain.
Let's improve this and write a custom syntax extension, so that our model would be express in a much better way.

The following extension method should be created:

<a href="_samples/writing-custom-syntax-HideContentTypeFieldsByIds.sample-ref"></a>

Now we can re-write hiding fields model with a better syntax:

<a href="_samples/writing-custom-syntax-HideContentTypeFieldsAsExtension.sample-ref"></a>