---
Title: Custom definition
FileName: custom-definition.html
Order: 500
---
### Writing custom definition

SPMeta2 can be extended with custom definition and model handler, so that you can plug in your own provision logic.
This article provides basics on creating a custom definion and model handler for SPMeta2 library.

Before you begin, make sure you are familiar with the following concepts:

* [Get started with SPMeta2](/spmeta2/getting-started)
* [Definitions concept](/spmeta2/reference/definitions)
* [Models concept](/spmeta2/reference/models)
* [Provisioning services concept](/spmeta2/reference/provisionservices)


Here is a big puctire on how SPMeta2 provision walks through the web model with lists and list views.

<img src='http://g.gravizo.com/g?@startuml;
"web definition" -> "web definition": lookup handler;
"web definition" -> "web definition": DeployModel();
"web definition" -> "web definition": lookup children models;
"web definition" -> "list definition": WithResolvingModelHost(), get web instance;
"list definition" -> "list definition": lookup handler;
"list definition" -> "list definition": DeployModel();
"list definition" -> "list definition": lookup children models;
"list definition" -> "list view definition": WithResolvingModelHost(), get list instance;
"list view definition" -> "list view definition": lookup handler;
"list view definition" -> "list view definition": DeployModel();
"list view definition" -> "list definition": back;
"list definition" -> "web definition": back;@enduml' ></img>

At the end, SPMeta2 provision service walks through the model tree resolving correct model handler and calling a pair of DeployModel() and WithResolvingModelHost() methods.

All model handlers must inherit ModelHandlerBase class and implement the following methods:

* DeployModel(object modelHost, DefinitionBase model) method
* TargetType property
* Optionally, WithResolvingModelHost(ModelHostResolveContext context) method

DeployModel() methods must also raise a pair of OnProvisioning / OnProvisioned event, so that it would be possible to get access to the raw SharePoint object during the provision.

Let's have a closer look and create a simple custom definition with model handler.

### Scenario
We need to have a custom defintion to change existing web title and description.

#### Creating definition
All definition should meet the following criterias:

* Inherit DefinitionBase class
* Have [Serializable] attribute

Here is how a custom ChangeWebTitleAndDescriptionDefinition might look like:

<a href="_samples/writing-custom-definition-ChangeWebTitleAndDescriptionDefinitionClass.sample-ref"></a>

#### Creating model handlers
The next step would be creating a custom model handler:

<a href="_samples/writing-custom-definition-ChangeWebTitleAndDescriptionModelHandlerClass.sample-ref"></a>

#### Registering model handler
One you created a custom model handler, we need to let SPMeta2 know about it.

Provision service have the following methods to address this:

* RegisterModelHandler(ModelHandlerBase modelHandlerType)
* RegisterModelHandlers(Assembly assembly)

Let's use the first one and register our handler:

<a href="_samples/writing-custom-definition-RegisterCustomModelHandler.sample-ref"></a>

#### Custom syntax
There is a separate article on how to create a [custom syntax extensions](/spmeta2/extensibility/writing-custom-syntax/), so let's just improve our provision and add custom syntax for ChangeWebTitleAndDescriptionDefinition:

<a href="_samples/writing-custom-definition-ChangeWebTitleAndDescriptionDefinitionSyntaxClass.sample-ref"></a>

Now we can re-write provision with a better syntax:
<a href="_samples/writing-custom-definition-RegisterCustomModelHandlerWithSyntax.sample-ref"></a>

#### Handling events
We expect that our model handler would raise OnProvisioning / OnProvisioned while pushing definition to SharePoint. Let's attache to these events and see how it goes.
<a href="_samples/writing-custom-definition-RegisterCustomModelHandlerWithEvents.sample-ref"></a>
