---
Title: Regression testing
FileName: regression-testing.html
Order: 700
---

#### Overview
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

#### SPMeta2 tests solution overview
* /Tests/Impl/ folder housed all tests - unit and integration tests
* /Tests/Validators/ folder houses 'validator handler' for CSOM/SSOM
* /Tests/Containers/ folder has 'definition generators' and 'running hosts'

For unit tests you need to focus on /Tests/Impl/ folder and two solutions. Check the further paragraph in unit testing.
* SPMeta2.Regression.Impl.Tests
* SPMeta2.Regression.Tests

Regression testing is much more complicated, several solution and specific flow is required (more details later in this article)

#### SPMeta2 unit testing in details
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
If you addd a new definition, you have to add a 'random' definitio regresison test (will be discussed later) and this test will fail indicating that you haven't added such regression test.

There are other unit tests to ensure defaukt services, values, null-reference exception, ensure serialization and presence of correct attributes, and then some test to ensure that particular tests were written.
Altogether, at unit test level we ensure basic C# tests and some SPMeta2 name convention and integrity.

If you need to test something non-SharePoint related, a unit test is the right place.
Write a new test, ensure it's green and don't forget to make it with CI.Core" category.

For unit tests you need to focus on /Tests/Impl/ folder and two solutions. Place tests in the following solutions and appropriate folder / class:
* SPMeta2.Regression.Impl.Tests
* SPMeta2.Regression.Tests

#### SPMeta2 regression testing in details