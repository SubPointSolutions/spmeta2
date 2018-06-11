---
Title: Regression testing
FileName: regression-testing.html
Order: 700
---

# Regression testing
Regression testing is a cornerstone of the SPMeta2 library and its QA process. 
The sole purpose of the regression testing is to ensure that provision works exactly as it supposed to work under both CSOM/SSOM for both SharePoint 2013 and SharePoint online tenants.
Hence, we use a mixture of unit testing and integration testing with real SharePoint 2013 farms and SharePoint online tenants to ensure the outstanding quality of the SPMeta2 library. Altogether, we call that 'regression testing'.

Current regression testing implementation has around 900 tests, and amount of tests is increasing over the time.
Tests can be devided into different areas:

* Pure C# unit tests (they don't need SharePoint)
* Integration tests for CSOM/SSOM (they needs a real SharePoint farm or O365 tenant)

Most of the SPMeta2 core funtionality is covered by C# unit tests. 
Such tests are covered by "CI.Core" test category, they are run under CI every checkin. 
You can run them locally grouping Visual Studio test explorer by 'Traits' - you'll see "CI.Core" tests group.

All other tests are integration tests meaning that they require either SharePoint 2013 farm or SharePoint online tenant.
Such integration tests are meant to validate two areas:
* Basic provision of random definition (random definition tests)
* Various provision scenarios for giving definition (scenarios tests)

Every definition must have at least one 'random' regresison test, and several scenarios to ensure various provision cases.
For instance, FieldDefinition has the folloeing tests:
* CanDeployRandom_FieldDefinition (a random definition test)
* A lot, really a lot of other tests to ensure that field can be deployed

This is not the end of the testing story. All regression tests uses the following flow:
* Deploy model 
* Check definition properties (they should match the values in SharePoint)
* Deploy model again (change properties, ensure we can deploy over, update model)
* Check definition properties (they should match the values in SharePoint)
* Serialize model into JSON/XML
* Deserialize model from JSON/XML
* Deploy model again (change properties, ensure we can deploy over, update model)
* Check definition properties (they should match the values in SharePoint)

Simple saying, every test is run seevral times, simulating a real word usage. 
Such flow is meant to ensure model can be deployed, all properties match the actual values in SharePoint, model can be serialized/deserialized and deserialized model can be deployed.
Apart that, all tests also call .ToPrettyPrint() and .ToDot() methods on every model.

Next, all tests are run under a particular SharePoint API - either CSOM runtime or SSOM runtime. 
That meant that we have to run at least two full set of the regression testing against SharePoint 2013 farm - under CSOM provision and then under SSOM provision. 
Then, one more time, we run all these tests under CSOM runtime against SharePoint online tenants. Full regression testing before any public release to NuGet can be seen as following:

* All tests against SP2013 SSOM
* All tests against SP2013 CSOM
* All tests against O365 CSOM

As menationed early, every regression test has a complicated flow - it provisions the artifact, checks properties and so on.
The whole testing process isn't fast. It takes around 3-4 hours to run full SP2013 SSOM regression testing, then 3-4 hours for SP2013 CSOM, and then arounf 6-9 hours to get O365 regression completed.
Total regression time hits more that 10 hours which is absolutely unaffordable.

Keeping full regression testing time low, we use [NCrunch](http://www.ncrunch.net/) and its distrubuting processing feature.
NCrunch allows not only tests execution parallelization but also allows running run tests on remote machines. 
Running tests on remote machines comes handy for SPMeta2 regression testing. We use either Hyper-V or Azure infrastructure having in avarage dozen SharePoint farms to run our regression testing against. 
NCruhc helps to parallelize regression testing execution amoung dozen SharePoint 2013 farms. Same strategy goes with SharePoint online tenants as well.
Such move cuts the total regression testing time to 45-60 minutes allowing us to ensure outstanding quality of SPMeta2 library.

### SPMeta2 tests solution structure
* /Tests/Impl/ folder housed all tests - unit and integration tests
* /Tests/Validators/ folder houses 'validator handler' for CSOM/SSOM
* /Tests/Containers/ folder has 'definition generators' and 'running hosts'

For unit tests you need to focus on /Tests/Impl/ folder and two solutions. Check the further paragraph in unit testing.
* SPMeta2.Regression.Impl.Tests
* SPMeta2.Regression.Tests

Regression testing is much more complicated, several solution and specific flow is required (more details later in this article)

### SPMeta2 unit testing
SPMeta2 unit tests are simple C# unit tests. All such tests must live under "CI.Core" category so that they will be run every checkin on github.
We use [AppVeyor](https://www.appveyor.com) continiuous integration service that hooks up with every checkin on github. 
Alternatively, you can run all tests under "CI.Core" category on your development envrionment.

There are two goals that we want to acoomplish with unit tests:
* Ensure all services of SPMeta2 work as expected
* Ensure and enforce integrity of SPMeta2 library

Ensuring that basic services of SPMeta2 library work well is all about writing classic C# unit tests.
For instance, we test that 'ConvertUtils' class works as expected so that we have 'ConvertUtilsTests'.
Same goes with serialization services (SerializationServicesTests), and other things.
Check out tests under "CI.Core" category to get more understanding. 

The other side of the unit testing is 'SPMeta2 integrity'. 
SPMeta2 introduces definitions, models, model syntax and other concepts. 
Such concepts, most of the time, follow the name convention and expected behaviour.

For instance:
* Definition has to be serializable
* Definition has .AddXXX() methods to work with model
* Definition has additional attributes to integrate wtih regression testing process
* Definition has to have 'random' regression test

Such contracts have to be forced, and we force them via unit tests as well.
For instance, we have 'AllSerializablesPublicPropsShouldBeMarkedAsDataMemberOrIgnoreDataMemberAttr' tests that ensures that all definition properties marked with 'DataMember' attribute.
That ensure that definition and its properties can be serialized.

We have 'Can_CreateDefinition' tests that creates all definitions dynamically. 
It reflects all definitions in SPMeta2, then creates and instance of definition.
That ensure that default constructor can be called without any errors.

We have 'RandomDefinitionTest_ShouldHave_Tests_ForAllDefinitions' test that ensure that 'RandomDefinitionSelfDiagnoosticTest' class has 'random' tests for every definition.
If you addd a new definition, you have to add a 'random' definition regression test (will be discussed later) and this test will fail indicating that you haven't added such regression test.

There are other unit tests to ensure default services, values, null-reference exception, ensure serialization and presence of correct attributes, and then some test to ensure that particular tests were written.
Altogether, at unit test level we ensure basic C# tests and some SPMeta2 name convention and integrity.

If you need to test something non-SharePoint related, a unit test is the right place.
Write a new test, ensure it's green and don't forget to make it with CI.Core" category.

For unit tests you need to focus on /Tests/Impl/ folder and two solutions. Place tests in the following solutions and appropriate folder / class:
* SPMeta2.Regression.Impl.Tests
* SPMeta2.Regression.Tests

### SPMeta2 regression testing
Regression testing is more complicated. 
As mentioned early, regression testing has a complicated execution flow and it required either SharePoint 2013 farm or SharePoint online tenant.

SSOM testing requires SharePoint 2013 farm.
CSOM testing goes either with SharePoint 2013 farm or SharePoint online tenant.

All regression tests tests uses the following flow:
* Deploy model 
* Check definition properties (they should match the values in SharePoint)
* Deploy model again (change properties, ensure we can deploy over, update model)
* Check definition properties (they should match the values in SharePoint)
* Serialize model into JSON/XML
* Deserialize model from JSON/XML
* Deploy model again (change properties, ensure we can deploy over, update model)
* Check definition properties (they should match the values in SharePoint)

There are two main categories for regression test:
* 'random' regression tests
* scenario regression tests

### Random regression tests
**Random regression** tests aim to cover very basic provision scenarios for an artifact.
They live under "RandomDefinitionSelfDiagnoosticTest" class and usually looks as this:

<a href="_samples/regression-testing-RegressiontestingClass.sample-ref"></a>

Every definition has to have a 'random' regression tests, and the presence of such test is checked by 'RandomDefinitionTest_ShouldHave_Tests_ForAllDefinitions' test.
If you have a new definition, you have to follow a name convention and add 'random' regression test.

A you can see, some magic is happening behind 'TestRandomDefinition()' function.
'TestRandomDefinition' function does the following:
* Generates a random definition of giving type
* Generates a random model for the giving definition
* Composes the final model
* Deploys and tests model as per the 'regression' testing flow

Forst of all, how you generate a random model? We use two projects for that with 'model generators':
* SPMeta2.Containers
* SPMeta2.Containers.Standard
     
In both project we have 'DefinitionGenerators' folder that houses 'random definition generators'.
Such classes generate a random, valid definition that can be deploye to SharePoint.
Check 'WebDefinitionGenerator', it looks as following:

<a href="_samples/regression-testing-WebDefinitionGeneratorClass.sample-ref"></a>

Follow the same style adding a new definition generator if you created a new definition.

Next, TestRandomDefinition() creates a random model for the giving definition. How come?
Every definition has two attributes that tell the immediate parent of the artifact, and then the root parent of the artifact.
For instance, web definition has the following attributes:
* [DefaultRootHost(typeof(SiteDefinition))]
* [DefaultParentHost(typeof(SiteDefinition))]

That means that the random model for WebDefinition will be a 'site model'.

ListViewDefinition has different values:
* [DefaultRootHost(typeof(WebDefinition))]
* [DefaultParentHost(typeof(ListDefinition))]

That means that the random model for ListViewDefinition will be a 'web model', and then the parent for ListViewDefinition will be a 'random list'.
    
Simple saying, 'DefaultRootHost' indicates a type of model - farm, web app, site or web model. 
'DefaultParentHost' attribute indicates a 'parent' for the giving definition. 
Both attributes organize the definitions into a 'tree', with parent-child relationship same way as it works in SharePoint.
Such parent-child relationships help SPMeta2 regression to generate not only random definitions, but also full-random and valid models at any level - farm, web app, site or web.

These two things are essentials to make 'random' regression testing work.
At this level, we generate fully random models, deploy them and ensure that 'basic' level of deployment can be accomplished.

All random tests live under "RandomDefinitionSelfDiagnoosticTest" class. 
Folow name convention to add a new tests, and then implement correct random definition generator.

### Scenarios regression tests
**Scenarios regression** tests is the next step to cover different provision scenarios.
Such tests reflect real word scenarios coming from community and real world projects.

All scenarios are written manually and aim to cover a particular scope.
For instance, with the fields:
* Can field be deployed under site?
* Can field be deployed under web?
* Can field be deployed under list?
* And so on...

With web parts it gets even trickier!
* Can web part be deployed on wiki page?
* Can web part be deployed on web part page page?
* Can web part be deployed on publishing page?
* Can web part be deployed on list view page in the list?
* Can web part be deployed on list view page in the library?
* Can web part be deployed on item details page in list?
* Can web part be deployed on item details page in library?
* And so on...

Same goes with module files:
* Can module file be deployed under library?
* Can module file be deployed under folder?
* Can module file be deployed under content type?
* And so on...

As you can see, we try to cover all possible cases that exist in real world projects.

Most of 'scenarios' exist in 'SPMeta2.Regression.Tests' projects under /Impl/Scenarios folder.
They are organized in 'DefinitionName-ScenarioTest' and work also as a reference point.

If a new scenario needs to be supported and tested, simple add a new test under correct file.
Follow name convention and check other tests as well. You would have to construct the model, and then use 'TestModel' methods to run the regression.

### Configuring environment for regression testing
Regression testing requires either SharePoint 2013 farm or SharePoint inline tenants.
Either way, some initial configuration is required.

During the regression tests, SPMeta2 deploy tons of random artifacts. Hence, we use a dedicated SharePoint web application to run regression tests again.
Once done, you may delete and create a new web application or use the same web app running tests again and again. That's fine.

There are some PowerShell scripts under SPMeta2.Regression.Tests/PSScripts that help to setup regression testing envrionment:
* _config.ps1 - use this one to configure your environment variables
* _sys.common.ps1 - don't change this one
* 100 - Ensure M2 Web Application.ps1 - use this one to create required web apps
* 200 - Configure M2 Test Environment.ps1 - use this one to setup CSOM/SSOM/O365 testing

_config.ps1 script defines all webapp/site/web URLs agains which SPMeta2 regression testing will be run.
Two globalPowerShell variables define all the parameters:
* $g_M2WebAppSettings
* $g_M2TestEnvironment

Reconfigure them as you need filling out URLs, logings and so on.
Once done, run '100 - Ensure M2 Web Application.ps1' - it will create local SharePoint 2013 web application.
Later, use '200 - Configure M2 Test Environment.ps1' with 'SSOM', 'CSOM' or 'O365' parameter.

How it all works?
Once you run regression test, SPMeta2 regression framework fetches environment settings defined by PowerShell scripts.

**First of all**, we identify if CSOM/SSOM/O365 runtime needs to be used. 
Once done, we load up once of the 'testing container' implemented the following projects:
* SPMeta2.Containers.CSOM
* SPMeta2.Containers.O365
* SPMeta2.Containers.SSOM

These 'test containers' are wrappers over the SPMeta2 provisioning. They deploy models using CSOM, SSOM or O365 SharePoint runtimes.
A new SPMeta2 model is generated by 'random' test (with model generators) or provided by 'scenarios' test. 
The model gets pushed to 'test container', the model gets provisioned. 
Once provisioning is done, 'test container' pulls 'model validators' that are defined under /Tests/Validators in the following projects:
* SPMeta2.Regression.CSOM
* SPMeta2.Regression.CSOM.Standard
* SPMeta2.Regression.SSOM
* SPMeta2.Regression.SSOM.Standard

Model validators are meant to compare the original SPMeta2 definition object with the provisioned SharePoint artifacts.
Every definition has a 'model validator'. Consider it to be sort of 'reversed model handler'. 
If model handler provisions artifact to SharePoint, then 'model validator' does the reverse things - it pulls artifact from SharePoint and then compares properties of artifact with properties defined in the definition.

Such flow is oversimplification of the actual things that happening behind. As mentioned, several rounds of provisioning is done, serialization is used, some other magic is involved.
At the end, it comes back to model validators and property-to-property comparison. To learn more, have a look how WebDefinitionValidator works. It is a great start to understand the regression testing.

**Why regression testing does not use default "Aseert" class?**
Now, you may notice that default unit tests have "Aseert" class to check statements. We don't use that in the regression testing.
The thing is that regression testing checks every property of the artifact producing a rich trace. 
It makes a report over all the properties showing:
* Which properties have not been validated by 'model validator'
* Which properties have been validated but were not equal

Default C# unit tests framework don't provide such functionality allowing only to fail on the first wrong situation.
SPMeta2 regression testing runs everything from start to end, allows to compare properties and renders the report over the comparison to the tests trace.

Such approach ensures that:
* We really test all properties
* Properties are tested
* Properties are equal
* We see which props fail

All that is made possible by the additional attributes on SPMEta2 definitions and enhanced assert utils we wrote.
Let's get deeper into regression testing attributes and assert utils with the next paragraph!

### Regression testing attributes

If you reading this, then you should know that SPMeta2 regression testing:
* Generates 'random models' based on definition attributes
* Uses models provided in scenarios tests
* Deploys stuff to real SharePoint farms
* Fetches stuff from SharePoint
* Makes property-to-property comparison 
* Checkes definition properties, deploys, tests, deploys.. tests again..

How all that automated? Most of the bits you should already know from the previous paragraphs.
Let now focus on regression testing attribues and assert utils. Welcome to the reality, Neo.

Open up [WebDefinition](https://github.com/SubPointSolutions/spmeta2/blob/master/SPMeta2/SPMeta2/Definitions/WebDefinition.cs) source code, let's talk about it.
WebDefinition has tons of attributes. Let's go one by one.

**[SPObjectType]**

This attribute defines what kind of SharePoint object is passed to OnProvisining/OnProvisioned events.
Regression testing checks if:
* OnProvisining/OnProvisioned events were fired
* Events have data passed
* Object type that is passed to the event matches object type in [SPObjectType] attributes

We are forcing outselves to ensure that SPMeta2 always raises OnProvisining/OnProvisioned events in the right way.

**[DefaultRootHost] / [DefaultParentHost]**

Mentioned early, these two attributes define parent-child relationships between definitions. 
Such relationships are used within regression testing while generating random models. 
DefaultRootHost suggest the type of the model to generate - farm, web app, site or web model. 
DefaultParentHost suggests the immediate parent of the artifact.

Be aware that these attributes do not cover all possible relationship combinations between artifacts.
For instance, field can be deployed under site, web and list. But FieldDefinition has only one 'default' parent host.
DefaultRootHost and DefaultParentHost attributes are used for random regression tests, the rest of the artifact combinations are handled by scenarios tests.

**[ExpectAddHostExtensionMethod]**

This attributes ensures that .AddHostXXX() methods exists. 
Sometimes, artifact already exists (such as style library), so that .AddHostList() method is used to build up a model.

If definition has [ExpectAddHostExtensionMethod] attribute, then regression checks if an appropriate extension method for SPMEta2 model syntax exists.

**[Serializable] / [DataContract]**

Default .NET attributes, Must have to ensure definition can be serialized.

**[ExpectWithExtensionMethod]**

Obsolete. Regression would check if .WithXXX() method exists at the model syntax level.

**[ExpectArrayExtensionMethod]**

Regression checks if .AddXXXs() method exists at the model syntax level, such as:
* AddFields(array)
* AddWebs(array)

**[ParentHostCapability]**

Similar to [DefaultParentHost] attribute, ParentHostCapability indicates possible parent of the current definitions.
There might be multiple [ParentHostCapability] attributes to indicate multiple parents. In this case, such attributes represents all potential combinations to create model tree of the giving definitions.

For instance, FieldDefinition has three [ParentHostCapability] with site, web and list values.
WebDefintion has two: for web, and site.

ParentHostCapability (and all other XXXCapability attributes)  are meant to indicate additional information of the defintion for 3rd part tools.
You can use ParentHostCapability attribute to figure out all possible parent for the current definition in your software or tool.

**[ExpectManyInstances]**

This is part of regression testing and 'random tests'.
Some definitions, such as lists, fields, webs, can be added into the model several times.
Some exist alone - such as 'BreakRoleInheritance'.

If [ExpectManyInstances] exists, then 'random' regression tests will create 1-3 instancies of the definition while constructing a random model for the random regression test.
That means that random regression tests create a random model with random amount of random definition of the giving type. Sounds cool right?

Okay, random model for BreakRoleInheritance will have only one instance of BreakRoleInheritance.
Most of the definitions have [ExpectManyInstances] attributes, so that a random model will have 1-3 instancies of the giving definition.
For instance, random tests for field definition will have 1-3 field definition instancies.

We need that to ensure that several definitions can be deployed in a row. We had some issues with one definition deployed well, and if you add two definitions in the model (two fields, two lists, two web parts), then provision fails.
Hence, we added [ExpectManyInstances] attributes and enhanced regression testing to generate several instancies of the giving definition within a random test.

Really, cool. Quality rocks, guys.
Get a coffe, we'll go next with the property attributes.

Every definition has set of properties, and every propertu has tons of custom attribues.
Open up [WebDefinition](https://github.com/SubPointSolutions/spmeta2/blob/master/SPMeta2/SPMeta2/Definitions/WebDefinition.cs) source code, let's talk WebDefinition properties and its attributes.

**[DataMember]**

Default .NET attribute to ensure that propety is serialized. Boring.

**[ExpectRequired]**

This attribute indicated that property must have a value. 
SPMeta2 has builtin validation. We prevent you from deploying incorrectly formed definitions.
For instance, WebDefinition must have Title and Url. Hence, both props have [ExpectRequired] attributes.

Sometimes we need to have one of the two or three properties being set.
With WebDefintion, we may have either WebTemplate or CustomWebTemplate. One of these must be set.
In such case, ExpectRequiest attribute has a 'group', as following:
[ExpectRequired(GroupName = "Web Template")]

Validation groups all ExpectRequired using the 'GroupName', and then ensures that one of the property with such attribute within a group is set.

**[IdentityKey]**

Identity key is something like a 'global unique identificator' for SPMeta2 definition.
Every definition must have one, unless the definition is 'single' such as BreakRoleInheritance.

We reserved the identity key attribute for the future to identify if two definitions are 'same' or 'different'.
Sure, other properties must be taken into account, but this 'identity key' helps us to understand if two defintions would be merged into 'update' or two definitions would be merged unto two definitions.
Such operations are needed once we perform diff/merge operation over two and more SPMeta2 models.

**[ExpectValidation]**

Used by regression testing. If this attribute exists at the property, then regression testing expect that you performed assert operation via 'model validation' while testing your definition.
Simply saying, all properties marked by [ExpectValidation] attributes are forces to be checked or irnored by regression testing assert utils.

**[ExpectNullable]**

This is part of the regresison testing. Random models gets deployed deveral times over the provisioning.
For the second and further provisioning, the model gets random updates: all definition properties get randomly updated.

ExpectNullable attriute suggests that this property can have NULL value. Regression testing consider that and sets NULL on random occasions over several rounds of provisioning.
That ensures that you canm deploy model and definitions with allowed NULL values in some properties (such as Description) and the provision would work well.

In the past we had some issues with description or other properties being NULL. 
We automated that via [ExpectNullable] attribute si that regression does several rounds of provisioning changing such properties to NULL.
Hence, we found all bugs and issues in the provisioning code that was not handling NULLs property. Shame, we know. But not anymore.

**[ExpectUpdate]**

This is cool attribute. Similar to [ExpectNullable], this attribute suggest that definition property can be updated.
For instance, field title, web title, web descriptions and so on.

If definition property has that attribute, then regression testing goes crazy over the random tests and multiple provisionings: it changes all properties marked with [ExpectUpdate] attributes.
So random model gates deployed, then deployed several times more, and all properties get updated, updated again, deployed, then changed to NULLs with [ExpectNullable] attributes and so on.

This is mess, we know. That's how SPMEtsa2 makes sure your provisioning works.

[ExpectUpdate] attributes isn't smart. Regression testing understands simple types, such as strings, number and so on, but sometimes you need to go smarter:
* Some properties have to be between 0 and 32000
* Some properties have to have specific array of values

In that case we have tons of attributes 'ExpectUpdateXXX' under 'SPMeta2.Attributes.Regression' namespace:
* ExpectUpdate
* ExpectUpdatAsToolbarType
* ExpectUpdateAsRichTextMode
* ExpectUpdateAsChoiceFieldEditFormat
* ExpectUpdateAsStandalone
* ExpectUpdateAsLCID
* ExpectUpdateAsCamlQuery
* ExpectUpdateAsUser
* ExpectUpdateAsFileName
* ExpectUpdateAsByte
* ExpectUpdateAsChromeType

and so on.. there are literally tons of them.

Now, every 'ExpectUpdateXXX' has a corresponsing 'update service' defined under /SPMeta2.Regression.Tests/Services/ExpectUpdateServices
Regresison testing gets all definition properties, updates all 'simple' props with [ExpectUpdate] attribute, and then delegates the update process for all 'ExpectUpdateXXX' attributes to the right service under /SPMeta2.Regression.Tests/Services/ExpectUpdateServices

Woohooo! Pretty much we done with the attributes. Altogether, these little trick and bits helps SPMeta2 prodice such outstanding quality.

### Regression testing asserts
Let's talk about 'model validators'. Open up [WebDefinitionValidator](https://github.com/SubPointSolutions/spmeta2/blob/master/SPMeta2/SPMeta2.Regression.SSOM/Validation/WebDefinitionValidator.cs).
Most of the model validators make the 'reverse' of the model handler - thet fetch the object from SharePoint to compare object properties with the definition properties.

Regresison testing deploys all models (model handlers are used), and then deplys model again with 'model validators'. Same pluggable infrastructure allows us to reuse a lot of code.
Nevertheless, most of the time you would inherit model handler overriding the DeployModel() method. There are two goals here:

* fetch the object
* make comparacing with the definition

Fetching the object, we tend to use the same codebased as model handlers. in case of WebDefinitionValidator, we call GetWeb() method from WebModelHandler.
Once we obtain the SharePoint object, we create an 'AsserPair' via AssertService. 

AsserPair has various methods to perform validation sich as:
* .ShouldBeEqual 
* .ShouldNotBeNull 
* .ShouldXXX
* so on...

The first object os always definition instance, the second one is the SharePoint object. Most of the time you can compare simple props such as string and numbers.
Sometimes you need to skip the property (if it's null, so you don't need to validate it), so that the following methd is to be used:
* assert.SkipProperty method

Finally, if the props are complex and can't be compard with out of the box methods, use the following ShouldBeEqual() override:
* assert.ShouldBeEqual

You must return PropertyValidationResult object, check [WebDefinitionValidator](https://github.com/SubPointSolutions/spmeta2/blob/master/SPMeta2/SPMeta2.Regression.SSOM/Validation/WebDefinitionValidator.cs) and .Url comparation to get into more details.

### New definition check list

On rare ocassions you may be interested to create a new definition or enhance existing one. Here is the standard flow and checklist on how to create a new definition and push it to the SPMeta2 regresison testing.
For all cases, refer to WebDefinition and its model handlers / validators for the fuether reference.

* Check [custom definition manual here](/spmeta2/extensibility/custom-definition.html)
* Create new definition
* Create new model handlers for CSOM/SSOM
* Create new model syntax 
* Create new definition generator
* Create new definition validators for CSOM/SSOM
* Update RandomDefinitionSelfDiagnoosticTest with new random test
* Add more scenarios for the created definition

Having said that, most of the definition are already created by the SPMeta2 team.

### Further reading
* [definitions concept](/spmeta2/reference/definitions)
* [models concept](/spmeta2/reference/models)
* [creating custom definition](/spmeta2/extensibility/custom-definition)
