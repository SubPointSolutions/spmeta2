---
Title: Releases
Order: 1180
---

## Releases

SPMeta2 is an open source project driven and cured by [SubPoint Solutions](http://subpointsolutions.com) team.
Having said that, we appreciate all the contributions by the community from a small suggestion to the pull request at GITHUB. 

### Releases schedule 

The SPMeta2 library is being actively developed: new features, enhancements and fixes are constantly added. 
We tend to be agile and deliver new features and fixes as soon as possible:

* On weekly basis new features and minor fixes are published as -beta to NuGet
* On monthly basis a major release is published to [NuGet](https://www.nuget.org/profiles/SubPointSupport)
* Every commit to github [gets build with Appveyor](https://ci.appveyor.com/project/SubPointSupport/spmeta2) and the gets published to the [staging MyGet feed](https://www.myget.org/gallery/subpointsolutions-staging)

### Planning and feature requiests
We tend to use scrum and weekly iterations, aligning our roadmap with the comnunity using the following channels:

* [UserVoice](https://subpointsolutions.uservoice.com)
* [SPMeta2 Yammer network](https://www.yammer.com/spmeta2feedback) 
* [SPMeta2 Twitter](twitter.com/spmeta2) 

Once we deside on the features, we use [GITHUB issues](https://github.com/SubPointSolutions/spmeta2/issues) to plan and track iterations.
### Source code

We use GITHUB to host the source code - both dev/master branches could be found here:

* [SPMeta2 master branch](https://github.com/SubPointSolutions/spmeta2/tree/master)
* [SPMeta2 dev branch](https://github.com/SubPointSolutions/spmeta2/tree/dev)

## Releases notes
<hr/>

## What's new in SPMeta2 v1.2.60, Feb, 2016

Various fixes and enhancements, IndexedPropertyKeys support for web and list scopes, CSOM support for SP2010 (SPMeta2.CSOM.Foundation-v14  and SPMeta2.CSOM.Standard-v14 packages), build support for SP2016.

AssemblyFileVersion: 1.2.16068.0806


  
### Fixes
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/777'>#777</a>, WebDefinition provision must not reset title / description to NULL/Empty values
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/788'>#788</a>, Provisioning publishing page throws null reference exception
  

  
### Enhancements
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/774'>#774</a>, Enhance ListDefinition - add &quot;WriteSecurity&quot; prop
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/773'>#773</a>, Enhance JobDefinition to address job.Properties prop
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/375'>#375</a>, SP2010/.NET 3.5 support for CSOM
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/749'>#749</a>, Add Web.IndexedPropertyKeys provision support
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/641'>#641</a>, Add support for SP2016 - CSOM/SSOM
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/792'>#792</a>, Enhance BooleanFieldDefinition - add DefaultValue property validation
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/791'>#791</a>, Enhance PublishingPageDefinition - add PageLayoutFileName property validation
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/787'>#787</a>, Improve regression tests - definition must not be modified by handlers
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/778'>#778</a>, Add List.IndexedRootFolderPropertyKeys provision support
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/750'>#750</a>, Add detection of CSOM/SSOM support of the target model
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/737'>#737</a>, Enhance &#39;ContentTypeDefinition&#39; provision - add parent content type name prop
  

### Regression tests
* 750 regression tests 

### Support & Issue Resolution

In case you have unexpected issues please contact support on SPMeta2 Yammer Network:

* https://www.yammer.com/spmeta2feedback
<hr/>

## What's new in SPMeta2 v1.2.50, Jan, 2016

Various fixes and enhancements, added support for content types coming from "Content Type Hub", fixes on the correct HTML/Content for the publishing pages, enhances XsltListViewWebPartDefinition provision, enhancements on breaking role inheritance for publishing pages, default page layour CSOM, other API improvements and more regression tests.

AssemblyFileVersion: 1.2.16020.0016
  
### Fixes
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/745'>#745</a>, .AddContentTypeLink() should work well with the read-only content types (Content Type Hub)
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/744'>#744</a>, CSOM - Publishing page has broken Content/HTML values once model reprovisioned
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/743'>#743</a>, Enhance &#39;XsltListViewWebPartDefinition&#39; provision - ensure asset library &#39;Thumbnails&#39; view support
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/740'>#740</a>, CSOM - Fixed issue with setting default pagelayouts
 

  
### Enhancements
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/746'>#746</a>, Add regressions tests for .AddBreakRoleInheritance() on pages
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/742'>#742</a>, Support &#39;UniqueContentTypeFieldsOrderDefinition&#39; at list level content types
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/741'>#741</a>, ContentTypeLink definition should work correctly on Name without ID
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/739'>#739</a>, Troubleshoot &#39;HTMLFieldDefinition&#39; - is should render a proper markup for layout pages
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/430'>#430</a>, CSOM - Enhance &quot;ListDefinition&quot; with MajorWithMinorVersionsLimit/MajorVersionLimit 
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/754'>#754</a>, Enhance &#39;ContentTypeDefinition&#39; ParentContentType ID validation
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/567'>#567</a>, Implement fault tolerant provision for CSOM
  

### Regression tests
* 734 regression tests 

### Support & Issue Resolution

In case you have unexpected issues please contact support on SPMeta2 Yammer Network:

* https://www.yammer.com/spmeta2feedback

<hr/>

## What's new in SPMeta2 v1.2.45-beta-1, December 2015

Fixes, various enhancements, pull requests, new regression tests and tests hardening. 

AssemblyFileVersion: 1.2.15334.1243

### Fixes

* <a href='https://github.com/SubPointSolutions/spmeta2/issues/745'>#745</a>, .AddContentTypeLink() should work well with the read-only content types (Content Type Hub)
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/744'>#744</a>, CSOM - Publishing page has broken Content/HTML values once model reprovisioned
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/743'>#743</a>, Enhance 'XsltListViewWebPartDefinition' provision - ensure asset library 'Thumbnails' view support
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/740'>#740</a>, CSOM - Fixed issue with setting default pagelayouts



### Enhancements

* <a href='https://github.com/SubPointSolutions/spmeta2/issues/746'>#746</a>, Add regressions tests for .AddBreakRoleInheritance() on pages
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/742'>#742</a>, Support 'UniqueContentTypeFieldsOrderDefinition' at list level content types
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/741'>#741</a>, ContentTypeLink definition should work correctly on Name without ID
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/739'>#739</a>, Troubleshoot 'HTMLFieldDefinition' - is should render a proper markup for layout pages
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/430'>#430</a>, CSOM - Enhance "ListDefinition" with MajorWithMinorVersionsLimit/MajorVersionLimit 



### Pull Requests

* <a href='https://github.com/SubPointSolutions/spmeta2/issues/740'>#740</a>, CSOM - Fixed issue with setting default pagelayouts by @sjkp 
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/733'>#733</a>, CSOM - Taxonomy term lookup fix by @maratbakirov 


### Regression tests
* 732 regression tests

### Support & Issue Resolution

In case you have unexpected issues please contact support on SPMeta2 Yammer or YouTrack:

* https://www.yammer.com/spmeta2feedback
* https://subpointsolutions.myjetbrains.com/youtrack/issues

<hr />

## What's new in SPMeta2 v1.2.40, Nov, 2015

Preliminary implementation for 25 strong typed web parts. Fixes, various enhancements, new regression tests and tests hardening. 

AssemblyFileVersion: 1.2.15321.0933


### New definitions

* <a href='https://github.com/SubPointSolutions/spmeta2/issues/714'>#714</a>, Add 'CategoryWebPartDefinition'
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/713'>#713</a>, Add 'SocialCommentWebPartDefinition'
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/712'>#712</a>, Add 'MyMembershipWebPartDefinition'
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/711'>#711</a>, Add "PictureLibrarySlideshowWebPartDefinition"
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/710'>#710</a>, Add 'MembersWebPartDefinition'
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/709'>#709</a>, Add "TagCloudWebPartDefinition" 
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/708'>#708</a>, Add "CommunityJoinWebPartDefinition"
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/707'>#707</a>, Add "CommunityAdminWebPartDefinition"
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/706'>#706</a>, Add "UserDocsWebPartDefinition"
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/583'>#583</a>, Add 'DataFormWebPartDefinition' provision support
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/504'>#504</a>, Add 'SiteDocumentsDefinition'
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/503'>#503</a>, Add 'RSSAggregatorWebPartDefinition'
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/469'>#469</a>, Add "GettingStartedWebPartDefinition"
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/468'>#468</a>, Add "UserTasksWebPartDefinition"
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/467'>#467</a>, Add "SearchNavigationWebPartDefinition"
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/465'>#465</a>, Add "ImageWebPartDefinition"
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/463'>#463</a>, Add "DocumentSetPropertiesWebPartDefinition"
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/462'>#462</a>, Add "DocumentSetContentsWebPartDefinition"
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/461'>#461</a>, Add "XmlWebPartWebPartDefinition"
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/460'>#460</a>, Add "SPTimelineWebPartDefinition"
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/459'>#459</a>, Add "TableOfContentsWebPartDefinition"
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/458'>#458</a>, Add 'BlogAdminWebPartWebPartDefinition'
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/457'>#457</a>, Add 'BlogLinksWebPartDefinition'
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/456'>#456</a>, Add 'BlogMonthQuickLaunchDefinition' 
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/455'>#455</a>, Add "SearchBoxScriptWebPart" definition
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/454'>#454</a>, Add "SimpleFormWebPart" definition
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/264'>#264</a>, Add 'SearchSettings' provision support
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/330'>#330</a>, Add SSOM 'MetadataNavigationSettingsDefinition' provision support



### Fixes

* <a href='https://github.com/SubPointSolutions/spmeta2/issues/693'>#693</a>, Some web part provision on wiki page give empty markup 
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/725'>#725</a>, Enhance CSOM provision for 'XsltListViewWebPart' - all view props should be copied 
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/691'>#691</a>, Global navigation is not set as expected



### Enhancements

* <a href='https://github.com/SubPointSolutions/spmeta2/issues/718'>#718</a>, Enhance 'ChoiceFieldDefinition' - ensure EditFormat field updatibility
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/717'>#717</a>, Enhance 'UserFieldDefinition' - add 'ShowField' property
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/716'>#716</a>, Enhance 'XsltListViewWebPartDefinition' - add WebId/WebUrl properties
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/715'>#715</a>, Enhance 'ListViewDefinition' - add 'Scope' property
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/583'>#583</a>, Add 'DataFormWebPartDefinition' provision support
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/485'>#485</a>, Enhance 'UserCodeWebPartDefinition' - UserCodeProperty should support ~sitecollection/~site tokens
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/726'>#726</a>, BuiltinListDefinition.WorkflowTasks seems to be incorrect
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/721'>#721</a>, Enhance 'ModuleFileDefinition' - add 'Title' property
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/719'>#719</a>, Enhance 'PublishingPageDefinition' - add DefaultValues support
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/598'>#598</a>, Enhance 'PublishingPageLayoutDefinition' - add DefaultValues support
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/566'>#566</a>, Enhance 'ListItemDefinition' provision to support field values 
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/565'>#565</a>, Enhance 'ModuleFileDefinition' provision to support field values
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/402'>#402</a>, Enhance web part page provision - add DefaultValues prop support
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/401'>#401</a>, Enhance wiki page provision - add DefaultValues prop support
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/730'>#730</a>, Add built-in wiki pages into BuiltInWikiPages class
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/527'>#527</a>, Enhance ListViewWebPart/XsltListViewWebPart  provision - add 'toolbar' options support
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/734'>#734</a>, All validation services should have common base class and metadata



### Pull Requests

* <a href='https://github.com/SubPointSolutions/spmeta2/issues/728'>#728</a>, 2015.11.02 - page default values


### Regression tests
* 718 regression tests

### Support & Issue Resolution

In case you have unexpected issues please contact support on SPMeta2 Yammer or YouTrack:

* https://www.yammer.com/spmeta2feedback
* https://subpointsolutions.myjetbrains.com/youtrack/issues

<hr />

## What's new in SPMeta2 v1.2.35-beta2, Nov, 2015

ContentTypeName,  ContentTypeId and default values for pages, list items and modules. A few major fixes for web navigation settings and XsltListViewWebPart provision. New regression tests and tests hardening. 

AssemblyFileVersion: 1.2.15306.0935




### Fixes

* <a href='https://github.com/SubPointSolutions/spmeta2/issues/725'>#725</a>, Enhance CSOM provision for 'XsltListViewWebPart' - all view props should be copied 
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/691'>#691</a>, Global navigation is not set as expected



### Enhancements

* <a href='https://github.com/SubPointSolutions/spmeta2/issues/726'>#726</a>, BuiltinListDefinition.WorkflowTasks seems to be incorrect
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/721'>#721</a>, Enhance 'ModuleFileDefinition' - add 'Title' property
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/719'>#719</a>, Enhance 'PublishingPageDefinition' - add DefaultValues support
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/598'>#598</a>, Enhance 'PublishingPageLayoutDefinition' - add DefaultValues support
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/566'>#566</a>, Enhance 'ListItemDefinition' provision to support field values 
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/565'>#565</a>, Enhance 'ModuleFileDefinition' provision to support field values
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/402'>#402</a>, Enhance web part page provision - add DefaultValues prop support
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/401'>#401</a>, Enhance wiki page provision - add DefaultValues prop support


### Regression tests
* 706 regression tests

### Support & Issue Resolution

In case you have unexpected issues please contact support on SPMeta2 Yammer or YouTrack:

* https://www.yammer.com/spmeta2feedback
* https://subpointsolutions.myjetbrains.com/youtrack/issues

<hr />

## What's new in SPMeta2 v1.2.35-beta1, Oct, 2015

Preliminary implementation for 25 strong typed web parts, further improvements will be made prior November 2015 release. Fixes, various enhancements, new regression tests and tests hardening. 

AssemblyFileVersion: 1.2.15301.0536

### New definitions

* <a href='https://github.com/SubPointSolutions/spmeta2/issues/714'>#714</a>, Add 'CategoryWebPartDefinition'
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/712'>#712</a>, Add 'MyMembershipWebPartDefinition'
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/711'>#711</a>, Add "PictureLibrarySlideshowWebPartDefinition"
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/710'>#710</a>, Add 'MembersWebPartDefinition'
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/709'>#709</a>, Add "TagCloudWebPartDefinition" 
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/708'>#708</a>, Add "CommunityJoinWebPartDefinition"
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/707'>#707</a>, Add "CommunityAdminWebPartDefinition"
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/706'>#706</a>, Add "UserDocsWebPartDefinition"
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/583'>#583</a>, Add 'DataFormWebPartDefinition' provision support
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/504'>#504</a>, Add 'SiteDocumentsDefinition'
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/503'>#503</a>, Add 'RSSAggregatorWebPartDefinition'
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/469'>#469</a>, Add "GettingStartedWebPartDefinition"
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/468'>#468</a>, Add "UserTasksWebPartDefinition"
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/467'>#467</a>, Add "SearchNavigationWebPartDefinition"
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/465'>#465</a>, Add "ImageWebPartDefinition"
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/463'>#463</a>, Add "DocumentSetPropertiesWebPartDefinition"
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/462'>#462</a>, Add "DocumentSetContentsWebPartDefinition"
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/461'>#461</a>, Add "XmlWebPartWebPartDefinition"
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/460'>#460</a>, Add "SPTimelineWebPartDefinition"
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/459'>#459</a>, Add "TableOfContentsWebPartDefinition"
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/458'>#458</a>, Add 'BlogAdminWebPartWebPartDefinition'
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/457'>#457</a>, Add 'BlogLinksWebPartDefinition'
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/456'>#456</a>, Add 'BlogMonthQuickLaunchDefinition' 
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/455'>#455</a>, Add "SearchBoxScriptWebPart" definition
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/454'>#454</a>, Add "SimpleFormWebPart" definition



### Fixes

* <a href='https://github.com/SubPointSolutions/spmeta2/issues/693'>#693</a>, Some web part provision on wiki page give empty markup 



### Enhancements

* <a href='https://github.com/SubPointSolutions/spmeta2/issues/718'>#718</a>, Enhance 'ChoiceFieldDefinition' - ensure EditFormat field updatibility
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/717'>#717</a>, Enhance 'UserFieldDefinition' - add 'ShowField' property
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/716'>#716</a>, Enhance 'XsltListViewWebPartDefinition' - add WebId/WebUrl properties
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/715'>#715</a>, Enhance 'ListViewDefinition' - add 'Scope' property
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/583'>#583</a>, Add 'DataFormWebPartDefinition' provision support
* <a href='https://github.com/SubPointSolutions/spmeta2/issues/485'>#485</a>, Enhance 'UserCodeWebPartDefinition' - UserCodeProperty should support ~sitecollection/~site tokens


### Regression tests
* 690 regression tests

### Support & Issue Resolution

In case you have unexpected issues please contact support on SPMeta2 Yammer or YouTrack:

* https://www.yammer.com/spmeta2feedback
* https://subpointsolutions.myjetbrains.com/youtrack/issues

<hr />

## What's new in SPMeta2 v1.2.30, October 2015

General assembly clean up to comply with "Microsoft Managed Recommended Rules", improved built-in validation on fields and content type, improved model serialization, various enhancements and new regression tests.

AssemblyFileVersion: 1.2.15294.1219

### New definitions
* AnonymousAccessSettingsDefinition - SSOM only, should be deployed under web
* ClearRecycleBinDefinition preview, SSOM + limited CSOM

### Fixes
* Incorrect deserialization in particular cases  - a model could not be provisioned correctly once deserialized, ModelNodeOptions.RequireSelfProcessing is always false
* Null reference exception while deserializing model
* Incorrect web part type with CSOM ContentBySearchWebPartModelHandler
* Correct BuiltInFieldTypes.LookupMulti type
* Folders support .AddProperty() syntax
* All fields can be deployed to site, web and list scopes #677 
* Title props is excluded from the validation for wiki pages
* Fixed NullReferenceException in SSOM ManagedPropertyModelHandler  
* Correct FieldType for LookupFieldDefinition/UserFieldDefinition while using AllowMultipleValues flag

### Enhancements
* HTMLFieldDefinition has RichText and RichTextMode properties
* Updated RegionalSettings provision for CSOM
* Corrected built-in web part page templates for SP2010/2013
* SSOM only - ListViewDefinition supports 'view style' prop #530
* Added built-in validation for InternalField name - no more than 32 chars
* Added built-in validation for ContentTypeId - must start with '0x'
* WebPartDefinition has generic property collection for web part props
* Enhanced 'WebDefinition' provision - AlternateCssUrl/SiteLogoUr props
* New props for SP2013Workflow/SP2013WorkflowSubscription definitions
* TaxonomyTermSet / Term enhanced with CustomProperties, LocalCustomProperties, CustomSortOrder, Contact, "Available for tagging" props
* UserCustomAction validates absolute URL in ScriptSrc prop
* NoteFieldDefinition updates RichTextMode prop for CSOM
* Builtin class for ManagedDataType

### General assembly clean up
A big clean up on the warning and recommended coding practices for the following assemblies:

* SPMeta2
* SPMeta2.Standard
* SPMeta2.CSOM
* SPMeta2.CSOM.Standard
* SPMeta2.SSOM
* SPMeta2.SSOM.Standard

The following settings are used:

* Microsoft Managed Recommended Rules
* Disabled warnings on ListDefinition.GetUrl() and .ListDefinition.Url in the assemblies only
* Warning Level 4
* Suppressed warning 1591 at the build level
* XML documentation file checked
* All warnings threaded as errors for mentioned assemblies
* These settings should be considered as a baseline for the following implementation.

### Regression tests
* 630 regression tests, improved coverage, new tests on provision, serialization and model validation

<hr />

## What's new in v1.2.25-beta3

[Fixed] Incorrect deserialization in particular cases  - a model could not be provisioned correctly once deserialized, ModelNodeOptions.RequireSelfProcessing is always false.

Related to:
* Deserialized models cannot be deployed #697

AssemblyFileVersion: 1.2.15286.1214

No other changes were introduced.

### What's new in v1.2.25-beta2

Several critical bug-fixes (deserialization in particular cases, CSOM ContentBySearchWebPart fix) plus a big clean up on the warning and recommended coding practices on the M2 assemblies.

AssemblyFileVersion: 1.2.15285.1337

### New definitions
* No new definitions were added

### Fixes
* Null reference exception while deserializing model
* Incorrect web part type with CSOM ContentBySearchWebPartModelHandler

### Enhancements
* SSOM only - ListViewDefinition supports 'view style' prop #530
* Added built-in validation for InternalField name - no more than 32 chars
* Added built-in validation for ContentTypeId - must start with '0x'
* WebPartDefinition has generic property collection for web part props

### General assembly clean up
A big clean up on the warning and recommended coding practices for the following assemblies:

* SPMeta2
* SPMeta2.Standard
* SPMeta2.CSOM
* SPMeta2.CSOM.Standard
* SPMeta2.SSOM
* SPMeta2.SSOM.Standard

The following settings are used:

* Microsoft Managed Recommended Rules
* Disabled warnings on ListDefinition.GetUrl() and .ListDefinition.Url in the assemblies only
* Warning Level 4
* Suppressed warning 1591 at the build level
* XML documentation file checked
* All warnings threaded as errors for mentioned assemblies
* These settings should be considered as a baseline for the following implementation.

### Regression tests
* 625 regression tests (+9 since last beta release)

<hr />

## What's new in v1.2.25-beta1

### New definitions
* AnonymousAccessSettingsDefinition - SSOM only, should be deployed under web

### Fixes
* Correct BuiltInFieldTypes.LookupMulti type
* Folders support .AddProperty() syntax
* All fields can be deployed to site, web and list scopes #677 
* Title props is excluded from the validation for wiki pages
* Fixed NullReferenceException in SSOM ManagedPropertyModelHandler  
* Correct FieldType for LookupFieldDefinition/UserFieldDefinition while using AllowMultipleValues flag

### Enhancements
* Enhanced 'WebDefinition' provision - AlternateCssUrl/SiteLogoUr props
* New props for SP2013Workflow/SP2013WorkflowSubscription definitions
* TaxonomyTermSet / Term enhanced with CustomProperties, LocalCustomProperties, CustomSortOrder, Contact, "Available for tagging" props
* UserCustomAction validates absolute URL in ScriptSrc prop
* NoteFieldDefinition updates RichTextMode prop for CSOM
* Builtin class for ManagedDataType

### Regression tests
* 616 regression tests.

<hr />

### SPMeta2 v1.2.2 - September 2015 -  release notes

If you have 1.1.XXX and moving to 1.2.XXX please review the following release notes carefully.

The following information aims to provide an overview on assembly related changes, refactorings, API changes, new features and API to prepare and simplify migration from v1.1.XXX to v1.2.XXX. 
 https://github.com/SubPointSolutions/spmeta2/releases/tag/1.2.0-beta1

Please review it carefully, consider that depending on your solution and API usage additional effort might be required to get updated to v1.2.XXX. Once updated, make sure your solution can be compiled without errors, then give a few rounds of provision.

In case you have unexpected issues please contact support on SPMeta2 Yammer or YouTrack:
* https://www.yammer.com/spmeta2feedback
* https://subpointsolutions.myjetbrains.com/youtrack/issues

AssemblyFileVersion - 1.2.15265.2146

### What's new in v1.2.2

### New definitions
- SupportedUICultureDefinition, should be deployed under web

### Fixes
+ AddProperty() method on list nodes
+ Fixed security provision for SSOM on wiki, web part and publishing pages

### Enhancements
+ Localization support (TitleResource / DescriptionResource) for web, field, content type, list, list view, navigation nodes and user custom actions
+ Enhance 'ModuleFileDefinition' provision - enable security operations #655
+ Correct BuiltInListDefinitions.CustomUrl props, regression tests
+ Version property validation on AppDefinition

### Regression tests
- Improved regression testing

Improved and hardened regression tests coverage - 600+ tests run against the following environments
* SP2013 SP1+ SSOM 
* SP2013 SP1+ CSOM 
* O365 tenants

Current test coverage is split into three major categories:
* Classic unit tests for pure c# based API
* Random generated definition provision - tests deploy several randomly generated artefacts multiple times, then fetch provisioned SharePoint artefacts, compare properties with original definitions, then randomly update original definition properties and make the second round of the provision, fetch and property comparing
* Scenario based definition provision - manually written provision to cover various real word provision scenarios. Validation process is the same - deploy several times, check, changes, deploy and check again.

### Support & Issue Resolution
In case you have unexpected issues please contact support on SPMeta2 Yammer or YouTrack:
* https://www.yammer.com/spmeta2feedback
* https://subpointsolutions.myjetbrains.com/youtrack/issues

<hr />

### SPMeta2 v1.2.1 - August 2015 -  release notes

If you have 1.1.XXX and moving to 1.2.XXX please review the following release notes carefully.

The following information aims to provide an overview on assembly related changes, refactorings, API changes, new features and API to prepare and simplify migration from v1.1.XXX to v1.2.XXX. 
 https://github.com/SubPointSolutions/spmeta2/releases/tag/1.2.0-beta1

Please review it carefully, consider that depending on your solution and API usage additional effort might be required to get updated to v1.2.XXX. Once updated, make sure your solution can be compiled without errors, then give a few rounds of provision.

In case you have unexpected issues please contact support on SPMeta2 Yammer network or sent a ticket here at github:
* https://www.yammer.com/spmeta2feedback
* https://github.com/SubPointSolutions/spmeta2/issues

### What's new in v1.2.1

Current assembly file version: 1.2.15236.1334

### New definitions
- No new definition were added

### Fixes
- AddTaxonomyTermLabel() is added under TaxonomyTermDefinition 
- AddSecurityRoleLink() is added on SecurityGroupDefinition
- Correct XML validation on XML based properties
- Site provision fixes for SSOM
- ClientWebPart provision on wiki pages (was failing on ID prop presence)
- Correct order for content type related operations in the list 
- Correct order for app provision on the web

### Enhancements
- AssemblyFileVersion is updated during the build
- TaxonomyTermDefinition has CustomProperties property to address custom prop provision
- SiteDefinition enhanced with required attributes
- M2 handles starting '/' in the web urls avoiding fails
- SecurityGroupDefinition allows nesting security groups (for on-prem AD groups and global O365 groups)
- PublishingPageLayoutDefinition.Title is not required any more, mimic SharePoint
- ListDefinition.DocumentTemplateUrl is introduced
- ListViewDefinition has new props - Type/ViewData to support Gantt, Calendar and Datasheet view provision
- ClientWebPart.ProductWebId is nullable and not required. Current web id will be used if empty
- WebDefinition supports custom web templates via CustomWebTemplate props
- TaxonomyFieldDefinition has IsSiteCollectionGroup and GroupName/GroupId props to enhance provision
- BuiltInSiteFeatures.EnableAppSideLoading props is introduced
- BuiltInWikiPages and BuiltInPublishingPages classes are introduced\
- AppDefinition provision supports updates, automatically detect if any is needed
- ModuleFileUtils.LoadModuleFilesFromLocalFolder() has overload with 'shouldInclude' callback
- LookupFieldDefinition.RelationshipDeleteBehavior is introduced
- PublishingPageLayoutDefinition.PreviewImage is introduced
- Syntax.Extended is added, more details here - https://github.com/SubPointSolutions/spmeta2/issues/582
- LookupFieldDefinition.LookupWebUrl is added, ~site and ~sitecolleciton token support is implemented
- DeleteWebPartDefinition supports DeleteWebPartDefinition.WebParts props and 'Title' match 

### Regression tests
- Better web part provision tests - all web parts are deployed to wiki, web part, publishing, list view, new, edit and view list forms
- New tests on apps / client web part provision on web and subwebs
- New tests on app upgrades provision
- New tests on CSOMTokenReplacementService / SSOMTokenReplacementService 

Improved and hardened regression tests coverage - 580 tests run again the following three environments
* SP2013 SP1+ SSOM 
* SP2013 SP1+ CSOM 
* O365 tenants

Current test coverage is split into three major categories:
* Classic unit tests for pure c# based API
* Random generated definition provision - tests deploy several randomly generated artefacts multiple times, then fetch provisioned SharePoint artefacts, compare properties with original definitions, then randomly update original definition properties and make the second round of the provision, fetch and property comparing
* Scenario based definition provision - manually written provision to cover various real word provision scenarios. Validation process is the same - deploy several times, check, changes, deploy and check again.

### Support & Issue Resolution
In case you have unexpected issues please contact support on SPMeta2 Yammer network or sent a ticket here at github:
* https://www.yammer.com/spmeta2feedback
* https://github.com/SubPointSolutions/spmeta2/issues

### New definitions
- DependentLookupFieldDefinition (site columns and content type refs)
- RefinementScriptWebPartDefinition
- ComposedLookItemLinkDefinition
- SilverlightWebPartDefinition
- MasterPagePreviewDefinition
- FilterDisplayTemplateDefinition
- ManagedPropertyDefinition (SP2013 SSOM impl only)

### Fixes
- Fixed WebNavigationSettings provision for SSOM - correct ShowSubSites/Pages flags setup

### Enhancements
- Module files definition can be deployed under lists
- RootWebDefinition can be deployed under web
- Taxonomy terms provision checks special characters
- Incorrectly deleted taxonomy field is handled during re-provisioning
- WebPartDefinition has ParameterBndings prop
- ModuleFileUtils.LoadModuleFilesFromLocalFolder(..) method to map "local folder" intto model tree
- WebpartXmlExtensions.LoadDefinitionFromWebpartFile(..) method to load webpart definition from *.webpart/*.dwp files
- ContentTypeFieldLinkDefinition has InternalFieldName prop
- ListFieldLinkDefinition has InternalFieldName prop
- Webpart provision handles both "ChromeType" and "FrameType" prop values
- XsltListViewWebPartDefinition has new props - Xsl, XslLink, XmlDefinition, XmlDefinitionLink, DisableXXX,InplaceSearchEnabled
 
### Obsolete API
- SPMeta2.SSOM.DefaultSyntax.ListDefinitionSyntax.GetListUrl() marked as obsoleted
- SPMeta2.CSOM.DefaultSyntax.ListDefinitionSyntax.GetListUrl() marked as obsoleted
- ListDefinition.Url  marked as obsolete - use ListDefinition.CustomUrl with specifying "/List/mylist" or "mylibrary" URL instead

The challenge is that GetListUrl() method was supposed to calculate a correct list URL (with/without 'List' prefix) based on the list type. That works well for known lists, but works incorrectly on list templates, custom list template and lists which types are unknown to M2 library. We suggest to use "CustomUrl" property instead of "Url" and specify web-related list URL - with or without "/List" prefix depending on your case. 

### Regression tests
Improved regression test covered - 480 tests to cover more provision scenarios and cases.

### New definitions
- RefinementScriptWebPartDefinition
- ComposedLookItemLinkDefinition
- SilverlightWebPartDefinition
- MasterPagePreviewDefinition
- FilterDisplayTemplateDefinition
- ManagedPropertyDefinition (SP2013 SSOM impl only)

### Fixes
- Fixed WebNavigationSettings provision for SSOM - correct ShowSubSites/Pages flags setup
Enhancements
- Module files definition can be deployed under lists
- RootWebDefinition can be deployed under web
- Taxonomy terms provision checks special characters
- Incorrectly deleted taxonomy field is handled during re-provisioning
- WebPartDefinition has ParameterBndings prop
- ModuleFileUtils.LoadModuleFilesFromLocalFolder(..) method to map "local folder" intto model tree
- WebpartXmlExtensions.LoadDefinitionFromWebpartFile(..) method to load webpart definition from *.webpart/*.dwp files

### Obsolete API
- SPMeta2.SSOM.DefaultSyntax.ListDefinitionSyntax.GetListUrl() marked as obsoleted
- SPMeta2.CSOM.DefaultSyntax.ListDefinitionSyntax.GetListUrl() marked as obsoleted
- ListDefinition.Url  marked as obsolete - use ListDefinition.CustomUrl with specifying "/List/mylist" or "mylibrary" URL instead

The challenge is that GetListUrl() method was supposed to calculate a correct list URL (with/without 'List' prefix) based on the list type. That works well for known lists, but works incorrectly on list templates, custom list template and lists which types are unknown to M2 library. We suggest to use "CustomUrl" property instead of "Url" and specify web-related list URL - with or without "/List" prefix depending on your case. 

### Regression tests
Improved regression test covered - 460+ tests to cover more provision scenarios and cases.

<hr />

The main focus for the June 2015 release is secondary lookup field implementation and better regression test coverage.

New definitions to improve web part provision, improved existing definitions, multiple enhancements and reaching 430+ regression tests.

### New definitions
* ReusableHTMLItemDefinition 
* ReusableTextItemDefinition
* ContentBySearchWebPartDefinition 
* ResultScriptWebPartDefinition 
* ProjectSummaryWebPartDefinition 
* ContentByQueryWebPartDefinition 
* PageViewerWebPartDefinition 
* ComposedLookItemDefinition 

### Fixes
* Improved XsltListViewWebPart provision (failed on EnsureView() method)
* TaxonomyTermSetDefinition.IsAvailableForTagging chaned to bool? type
* Fixed display template provision for SSOM (invalid type case error for boolean values on TemplateHidden prop)
* BuiltInMasterPageDefinitions.Oslo refers to "Oslo.master", not to 'Seattle.master'
* SSOM provision handles SecurityGroupDefinition.Description = NULL value

### Enhancements
* BuiltInContentClass to address OOTB seatch enumerations
* Improved taxonomy navigation (CanDeploy_WebNavigationSettings_As_TaxonomyProvider)
* ContactFieldControlDefinition has new properties 
* "Nullable" properties are covered by the regression testing 
* WebPartDefinition has more basic props - w/h, ChromeType/State, ExportMode, etc.
* Check on WebPartDefinition.AssemblyQualifiedName - should be AssemblyQualifiedName string
* BuiltInContentTypeNames and enhanced BuiltInPublishingContentTypeNames classes
* Top/QuickLaunchNodeDefinition URLs support ~sitecollection/~site tokens
* ScriptEditorWebPartDefinition.Id check - must be more than 32 chars (SharePoint issue)
* CSOM regression testing on XsltListViewWebPart, ListViewWebPartDefinition, SummaryLinksWebPart, ContentEditorWebpart
* XsltListViweWebPartDefinition has new props - ShowTimelineIfAvailable, xslt-related props for timeout and caching
*  Enhanced BuiltInListDefinitinions class  - CacheProfiles, ComposedLooks, SiteCollectionDocuments, SiteCollectionImages, ReusableContent, ets.
* ModuleFileUtils.LoadModuleFilesFromLocalFolder() method to build up a mode tree based on the local folder/fles - easy uploading local folders/files to SharePoint
* Fixed typo with BuiltInListDefinitions.Calalogs class
* SSOMProvisionServiceExtensions.DeployFarmModel() and SSOMProvisionServiceExtensions.DeployWebApplicationModel() extensions to provide typed deployment for farm/webapp models
* Managed navigation provision support

### Documentation
Updated documentation is available here - http://docs.subpointsolutions.com/spmeta2

Get familiar with the [basics concepts](http://docs.subpointsolutions.com/spmeta2/), [get started](http://docs.subpointsolutions.com/spmeta2/basics/getting-started/) and check out [scenarios page](http://docs.subpointsolutions.com/spmeta2/scenarios/). That would save the day. We continuously updating the content, but let us know if you miss something right now.

<hr />

Publishing fields support, XML/JSON serialization support for models, improving lookup/taxonomy fields provision, adding other enhancements and reaching 400+ regression tests. [Updated documentation](http://docs.subpointsolutions.com/spmeta2/) and new [scenarios page](http://docs.subpointsolutions.com/spmeta2/scenarios/). 

### New definitions
* WebPartGalleryFileDefinition
* ControlDisplayTemplateDefinition
* ItemDisplayTemplateDefinition
* JavaScriptDisplayTemplateDefinition
* LinkFieldDefinition
* ImageFieldDefinition
* HTMLFieldDefinition
* SummaryLinkFieldDefinition
* MediaFieldDefinition
* OutcomeChoiceFieldDefinition
* GeolocationFieldDefinition
* SummaryLinkWebPartDefinition
* UserCodeWebPartDefinition (sandbox web parts, only SSOM)
* DeleteWebPartDefinition 

### Fixes
* Correct parent-child content type sorting during provision
* Correct DateTimeFieldDefinition provision (CalendarType/DisplayFormat/FriendlyDisplayFormat props issue)
* ListViewDefinition fixes for top folder/content type bindings

### Enhancements
* Lookup/Taxonomy fields can be deployed with empty bindings
* BuiltInFolderDefinitions contans 'Master page gallery' folders
* OnCreatingContext<TObjectType, TDefinitionType> contains ModelHost object
* PublishingPageDefinition has DefaultValues prop to enable provision with required properties
* Fields can be provisioned under web (web scoped fields)
* Content types can be provisioned under web (web scoped content types)
* TaxonomyTermSetDefinition has IsOpenForTermCreation prop
* TaxonomyFieldDefinition has IsPathRendered prop
* ContentTypeDefinition has better document template provision
* ModuleFileDefinition has DefaultValues/ContentTypeName/ContentTypeId props
* spmeta2.csom.utils class included into the main library
* WelcomePageDefinition handles starting '/' (amazing SharePoint API) 
* ListDefinition has MajorWithMinorVersionsLimit/MajorVersionLimit props (SSOM only)
* FieldDefinition has EnforceUniqueValues prop
* FieldDefinition has ShowInEditForm/ShowInNewForm/ShowInListForm props
* JSOM/XML serialization support for models (based on DataContractSerializer)
* Model validation on required properties 
* VS IntelliSense support - XML comments are included into NuGet packages
* TaxonomyTermSetDefinition.IsAvailableForTagging chaned to bool?
* Improved taxonomy navigation (CanDeploy_WebNavigationSettings_As_TaxonomyProvider)

### Documentation
Updated documentation is available here - http://docs.subpointsolutions.com/spmeta2

Get familiar with the [basics concepts](http://docs.subpointsolutions.com/spmeta2/), [get started](http://docs.subpointsolutions.com/spmeta2/basics/getting-started/) and check out [scenarios page](http://docs.subpointsolutions.com/spmeta2/scenarios/). That would save the day. We continuously updating the content, but let us know if you miss something right now.

<hr />

### Upcoming release - November 2014 CSOM runtime upgrade
The upcoming May 2015 release will shift CSOM assemblies from SP2013 SP1 to November 2014 CU due to support ListDefinition MajorWithMinorVersionsLimit/MajorVersionLimit props for CSOM. 

This release was heavily influenced by the community feedback and suggestions in both [Yammer](https://www.yammer.com/spmeta2feedback) and [UserVoice](https://subpointsolutions.uservoice.com). We constantly listen to the community enhancing SPMeta2 library with new features and enhancements.

Mainly, improved updatebility for lots of definitions so that property changes are reflected in SharePoint artefacts for the second and following provisions. New definitions and tests been added as well.

#### More definitions
- MasterPageDefinition (ASP NET Master Page)
- ResetRoleInheritanceDefinition
- TaxonomyTermLabelDefinition

#### Improvements & enhancements
- 'Array' overload for AddXXX() methods, for instance 'AddFields(IEnumerable<>)' and 'AddLists(IEnumerable<>)'
- Strong typed extensions for CSOM/SSOM provision services - DeploySiteModel()/DeployWebModel()
- Fixed WebDefinition provision (does not drop Title/Description to String.Empty)
- Token support (~site/~sitecollection) for ContentEditorWebPartDefinition.ContentLink 
- New enumeration - BuiltInPublishingContentTypeNames
- Enhanced 'ListDefinition' - draft option props
- Enhanced 'TaxonomyTermGroupDefinition' - IsSiteCollectionGroup prop
- Enhances 'TaxonomyTermDefinition' provision - supports nested 'TaxonomyTermLabelDefinition' 
- Enhanced feature definition provision (sandbox features support)
- Enhanced 'UserFieldDefinition' provision (SelectionGroup/SelectionGroupName props)
- Enhanced 'XsltListViewWebPart' provision - CSOM supports list views (!!)
- Covered by regression tests updatability on the following artifacts - ListDefinition, ContentTypeDefinition, ListViewDefinition, RootWebDefinition, SecurityGroupDefinition, SecurityRoleDefinition, TreeViewSettingsDefinition, TaxonomyTermGroupDefinition, TaxonomyTermDefinition, PublishingPageDefinition,  WikiPageDefinition, WebPartPageDefinition, PublishingPageLayoutDefinition

#### Incredible regression test coverage - 340+ tests
Test coverage is constantly improved. We have 340+ regression tests is run under CSOM/SSOM/O365 environments to ensure stellar quality of the provisioning flow. That's 30+ more tests on top of February 2015 release.

#### More definitions
- 'AuditingSettings' definition

#### Improvements & enhancements
- Better O365 support: new v16 NuGet packages for O365 CSOM v16 runtime
- Experimental SP2010 SSOM support: new v14 NuGet packages for SP2010 SSOM runtime

- PropertyDefinition provision on farm/webapp leves
- Security group provision enhancements - AllowMembersEditMembership, AllowRequestToJoinLeave, AutoAcceptRequestToJoinLeave properties
- Added 'BasicWebParts' feature definitions for BuiltInSiteFeatures enumeration
- UserCustomActionDefinition definition enhancement - CommandUIExtension property
- Better field provision, correct processing for ValidationMessage/ValidationFormula properties
- ListDefinition enhancements - add 'CustomUrl' property 
- LookupFieldDefinition enhancements - LookupListTitle, LookupListUrl, LookupList properties
- FieldDefinition/ListFieldLinkDefinition enhancements, AddFieldOptions option
- XsltListViewWebPartDefinition enhancements for SSOM

#### Incredible regression test coverage - 310+ tests
Test coverage was significantly improved. It is becoming absolutely insane - more than 310+ regression tests is run under CSOM/SSOM/O365 environments to ensure stellar quality of the provisioning flow. That's 40+ more tests on top of January 2015 release.

#### More definitions
- 'URL' field definition
- 'RegionalSettings' definition
- 'SearchSchema' definition
- 'ContentDatabase' definition
- 'WebConfigModification' definition
- 'InformationRightsManagementSettings' definition
- 'SecureStore/TargetAplication' definitions
- 'AlternateUrl' definition
- 'TreeViewSettings' definition
- 'PublishingPageLayout' definition
- 'RootWeb' definition

#### Improvements & enhancements
- StandardCSOMProvisionService/StandardSSOMProvisionService classes for "standard" artifacts provision (SharePoint Standard+)
- Enhanced sandbox solution provision (SiteAsset library is not used anhymore)
- Multi-target build for NuGet - .NET4 and .NET 4.5 are included in the NuGet pckages
- 'BuiltInPublishingFieldTypes' enumeration
- 'RawXml'/'AdditionalAttributes' properties for FieldDefinition
- Correct 'AllowMultipleValues' property provision for Lookup field
- ContentTypeFieldLinkDefinition enhancements with Hidden/Required/DisplayName props
- Correct 'IsMult' property provision for TaxonomyFieldDefinition
- Multiple fixed for TermGroup, TermSet and Term definitions
- Strong names for CSOM based assemblies
- 'IsAvailableForTagging/Description for taxonomy definitions
- Correct nested terms privisoin
- Correct module file provision under content type
- JSLink property for XsltListViewWebpartDefinition
- Navigation node provision enhancements - respect node order during provision
- Multiple level provision for navigation nodes
- Enhanced pages/files provision while page/file is unpublished/checkout

#### Incredible regression test coverage - 270+ tests
Test coverage was significantly improved. It is becoming absolutely insane - more than 270 regression tests is run under CSOM/SSOM/O365 environments to ensure stellar quality of the provisioning flow. That's hundred tests on top of December 2014 release.
