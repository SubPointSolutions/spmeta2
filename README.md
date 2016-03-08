# SPMeta2

The most comprehensive, enterprise-ready framework for provisioning SharePoint artifacts on SharePoint 2010, 2013 and O365. SPMeta2 is a code-first provision library supporting the provision of more than 120 artifacts in robust, highly repeatable and testable way while leveraging either one of the CSOM or SSOM SharePoint runtimes. Here is a few facts about it:

* Broad support of SharePoint 2010, 2013, O365, CSOM/SSOM
* No vendor lock-in: open source at GitHub
* Developer-friendly: fluent API, DSL and extensibility
* Visual Studio integration: project and item templates
* Documented: API samples, guidances and documentation
* Enterprise ready: regression tests, localization, SLA

### Build status
[![Build status](https://ci.appveyor.com/api/projects/status/0ym3fts7hmrdjvy1?svg=true)](https://ci.appveyor.com/project/SubPointSupport/spmeta2)

### SPMeta2 in details

#### Broad and unified provision API implemented for SharePoint 2013/SharePoint Online
SPMeta2 helps to deploy more that 120 SharePoint artifacts - fields, content types, lists, list views, webs, security groups, permission levels and the rest of the artifacts in testable, repeatable and upgradable manner. 

It supports both SSOM or CSOM API handling various provision scenarios with a consistent, unified, well-designer API. Built as .NET4/4.5 assemblies, packaged via NuGet, supports SP2013 Foundation, SP2013 Standard+, SharePoint Online and even SharePoint 2010.

#### Consistent fluent API, POCOs, DSL and extensibility
SPMeta2 provides POCO objects to define SharePoint artifacts and then a consistent fluent API and domain-specific language (DSL) helps to express your data model. It's like Entity Framework Code First, but for SharePoint. 

Extensible API helps to address specific project needs. You can write custom DSL syntax just with a few C# extension methods. As easy as that.

#### Built BY developers and FOR developers: supports wide set of development scenarios
SPMeta2 is an open source project under the MS-PL license hosted at GitHub. Like us there, pull something or just star and watch the show.

Compiled for .NET 4/4.5 and delivered by NuGet packages, SPMeta2 suits most of the custom development scenario.

The dark side of SharePoint's API is also handled by SPMeta2: inconsistency, bugs, "by design" behaviour. SPMeta2 fully replaces XML allowing you to focus on writing clean, unified, reliable and reusable codebase. 

Don't wait! Write amazing console applications, desktop application, full-trust *.wsp packages, implement remote provision via CSOM or modern SharePoint "apps" and "add-ons" - that's up to you.

#### Enterprise-ready: 600+ regression and unit tests, localization, Visual Studio integration
The outstanding quality of SPMeta2 is a result of 600+ regression tests being repeatedly run against a real SharePoint 2013 farm via SSOM, then via CSOM, and then it is also run against real O365 tenants via CSOM. Testing is done against REAL SharePoint 2013 SP1 instancies and O365 tenants - zero fakes or stubs are used.

SPMeta2 supports localization scenarios and has its Visual Studio project and items templates, snippets and debugger visualizer. Everything is covered to boost up SharePoint development process.

#### Production-ready: SLA and premium support
Hundreds of companies trust SPMeta2 to boost up they daily SharePoint related routines. Premium support and consulting is available to get the best value from SPMeta2, support production and mission critical applications offering in-depth, first-hand experience and SLA. 

### Resources

#### Documentation
* [Welcome to SPMeta2](http://docs.subpointsolutions.com/spmeta2/)
* [How it works](http://docs.subpointsolutions.com/spmeta2/basics/getting-started/) 
* [Getting started](http://docs.subpointsolutions.com/spmeta2/basics/getting-started/)
* [100+ samples & scenarios - NEW!](http://docs.subpointsolutions.com/spmeta2/scenarios/)
* [Useful utilities](http://docs.subpointsolutions.com/spmeta2/utils/)
* [Extensibility](http://docs.subpointsolutions.com/spmeta2/extensibility/)

#### Social
* [SPMeta2 @ Twitter](http://twitter.com/spmeta2)
* ['SPMeta2' tag @ stackoverflow](http://stackoverflow.com/search?q=spmeta2)

#### Yammer
* [Yammer Network](http://docs.subpointsolutions.com/spmeta2/)
* ['Tip of the day' Yammer group](https://www.yammer.com/spmeta2feedback/#/threads/inGroup?type=in_group&feedId=5963084)
* ['VS Extensions' Yammer group](https://www.yammer.com/spmeta2feedback/#/threads/inGroup?type=in_group&feedId=6192273)
* ['Ideas & Feedback' Yammer group](https://www.yammer.com/spmeta2feedback/#/threads/inGroup?type=in_group&feedId=4881224)

#### Support
* [Report bug](https://subpointsolutions.myjetbrains.com/youtrack/issues)
* [Feature request](https://subpointsolutions.myjetbrains.com/youtrack/issues) 
* [Premium support & SLA](http://localhost:48435/services)

#### Visual Studio integration

* [M2 Extensions @ VS Gallery - NEW!](https://visualstudiogallery.msdn.microsoft.com/364a867c-5b39-447b-88b8-afb093b75b93)
* [About extenstions, the experience](https://github.com/SubPointSolutions/spmeta2-vsixextensions/wiki)
* [General concepts](https://github.com/SubPointSolutions/spmeta2-vsixextensions/wiki/General-concepts)
* [M2 Intranet Model template](https://github.com/SubPointSolutions/spmeta2-vsixextensions/wiki/M2-Intranet-Model-project)
* [M2 Console provision template](https://github.com/SubPointSolutions/spmeta2-vsixextensions/wiki/M2-Console-Provision-project)
* [SPMeta2 snippets](https://github.com/SubPointSolutions/spmeta2-vsixextensions/wiki/M2-Snippets)
* [Download VSIX package](https://github.com/SubPointSolutions/spmeta2-vsixextensions/tree/master/Releases)

#### SPMeta2 @ NuGet
SPMeta2 core runtime
* [SPMeta2.Core](https://www.nuget.org/packages/SPMeta2.Core/)
* [SPMeta2.Core.Standard](https://www.nuget.org/packages/SPMeta2.Core.Standard/)

SharePoint Online
* [SPMeta2.CSOM.Foundation-v16](https://www.nuget.org/packages/SPMeta2.CSOM.Foundation-v16/)
* [SPMeta2.CSOM.Standard-v16](https://www.nuget.org/packages/SPMeta2.CSOM.Standard-v16/)

SharePoint 2013 CSOM
* [SPMeta2.CSOM.Foundation](https://www.nuget.org/packages/SPMeta2.CSOM.Foundation/)
* [SPMeta2.CSOM.Standard](https://www.nuget.org/packages/SPMeta2.CSOM.Standard/)

SharePoint 2013 SSOM
* [SPMeta2.SSOM.Foundation](https://www.nuget.org/packages/SPMeta2.SSOM.Foundation/)
* [SPMeta2.SSOM.Standard](https://www.nuget.org/packages/SPMeta2.SSOM.Standard/)

SharePoint 2010 SSOM
* [SPMeta2.SSOM.Foundation-v14](https://www.nuget.org/packages/SPMeta2.SSOM.Foundation-v14/)
* [SPMeta2.SSOM.Standard-v14](https://www.nuget.org/packages/SPMeta2.SSOM.Standard-v14/)

#### Blogposts
* [Office Dev PnP Web Cast – SharePoint Feature Framework vs Remote Provisioning](http://dev.office.com/blogs/feature-framework-vs-remote-provisioning)
* [Provisioning Google Maps JSLink with SPMeta2](http://chuvash.eu/2015/12/15/provisioning-google-maps-jslink-with-spmeta2/)
* [Provisioning: spmeta2 vs O365 PnP provisioning](http://blog.repsaj.nl/index.php/2015/05/o365-provisioning-spmeta2-vs-o365-pnp-provisioning/)
* [SharePoint Site Provisioning Engine](http://wp.sjkp.dk/sharepoint-site-provisioning-engine/)
* [Kom igång med SPMeta2](http://chuvash.eu/2015/09/17/kom-igang-med-spmeta2/)
* [Getting started with SPMeta2 by David Liong](https://davidliong.wordpress.com/2015/09/03/spmeta2/)
* [SPMeta2 - создание артeфактов SharePoint 2013](http://blog.cibpoint.ru/post/spmeta2-sharepoint2013-codebase-artifacts-provision/)
* [What is a SharePoint Application](http://chuvash.eu/2015/10/14/what-is-a-sharepoint-application/)

#### Presentations
* [SharePoint Saturday Oslo 2015 - NEW!](http://yuriburger.net/2015/10/17/slidedeck-sharepoint-saturday-oslo-2015-now-available/)
* [Slidedeck SharePoint Artifact Provisioning - VX Company](http://www.werkenbijvxcompany.nl/wp-content/uploads/2015/05/Provisioning-SharePoint-Artifacts-Blog.pdf)
* [SharePointCommunity.ch SharePoint Lösungen für die Zukunft](http://www.slideshare.net/fiddich1/sharepointcommunitych-sharepoint-lsungen-fr-die-zukunft)
