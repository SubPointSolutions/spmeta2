## SPMeta2 is now part of <a href="http://subpointsolutions.com">SubPoint Solutions</a>

<a href="http://subpointsolutions.com">SubPoint Solutions</a> is an inovative company that helps SharePoint professionals and consultancy companies be efficient. 

We look after <a href="http://subpointsolutions.com/spmeta2/about">SPMeta2 library</a>, <a href="http://subpointsolutions.com/spcafcontrib/about">SPCAFContrib</a> and a few more projects aim to provide a powerful foundation and experience for SharePoint professionals. 

With growing demaind of effective SharePoint artifact provision for SP2013 and O365, we are taking <a href="http://subpointsolutions.com/spmeta2/about">SPMeta2 library</a> to the next level, offering <a href='http://subpointsolutions.com/spmeta2/sdk'>SPMeta2 SDK</a> and <a hre='http://subpointsolutions.com/services/support'>additional support</a> for our clients.

Learn more about <a href="http://subpointsolutions.com/spmeta2/about">SPMeta2 library</a>, stay tuned with <a href='https://www.yammer.com/spmeta2feedback/'>SPMeta2 Yammer Group</a> and <a href='https://subpointsolutions.uservoice.com/'>let us know how it works for you</a>.

## Build status with appveyor
[![Build status](https://ci.appveyor.com/api/projects/status/0ym3fts7hmrdjvy1?svg=true)](https://ci.appveyor.com/project/SubPointSupport/spmeta2)

## What is SPMeta2?
SPMeta2 is a fluent API for code-based SharePoint artifact provisioning.

Struggling with SharePoint's API inconsistency, bugs, "by-design" behaviour, unaffordable amount of time to write, support and upgrade WSP packages and XML, a team of passionate SharePoint professionals decided to come up with robust, testable and repeatable way to deploy such artifacts like fields, content types, libraries, pages and many more.

As an outcome, we created SPMeta2 - a .NET 4.5 library to provide fluent API for SharePoint 2013 artifact with SSOM/CSOM or JSOM for both on premise and O365 instances. Have a look around, check out get started links and more details about SPMeta. 

## Get started!
<table style="color: #222222; height: 125px;" width="483">
<tbody>
<tr>
<td valign="top" width="50%">
<ul>
<li><a style="color: #006adf;" href="http://docs.subpointsolutions.com/spmeta2">About</a></li>
<li><a style="color: #006adf;" href="http://docs.subpointsolutions.com/spmeta2/features">Features</a></li>
<li><a style="color: #006adf;" href="http://docs.subpointsolutions.com/spmeta2/releases">Releases and roadmap</a></li>
<li><a style="color: #006adf;" href="http://docs.subpointsolutions.com/spmeta2/provision">How-to artefact provision</a></li>
<li><a style="color: #006adf;" href="https://www.nuget.org/profiles/SubPointSupport">SPMeta2 @ Nuget</a></li>
<li><a style="color: #006adf;" href="https://github.com/SubPointSolutions/spmeta2.contoso">SPMeta2.Contoso - sample projects</li>
</ul>
</td>
<td valign="top" width="50%">
<ul>

<li><a style="color: #006adf;" href="http://docs.subpointsolutions.com/spmeta2/license">License</a></li>
<li><a style="color: #006adf;" href="http://subpointsolutions.com/services/support">Support</a></li>
<li><a style="color: #006adf;" href="https://subpointsolutions.uservoice.com/">Feature requests</a></li>
<li><a style="color: #006adf;" href='https://www.yammer.com/spmeta2feedback/'>SPMeta2 Yammer Group</a></li>
</ul>
</td>
</tr>
</tbody>
</table>

## SPMeta2 philosophy and mission
### Fluent API and syntax extensions
SPMeta2 API allows you to define SharePoint artifacts such as field, content type, list (and many more), define relationships between them and, finally, deploy them via SSOM/CSOM. You work with c# POCO objects defining your data model, we take care about the rest. 

SPMeta2 might be extended with custom syntax implementation to meet your project needs. Extension methods are used to adjust a specific behavior or property of SharePoint artifacts. Custom syntax or DSL can easily be created to address specific project needs.

Learn more here - <a href="http://docs.subpointsolutions.com/spmeta2/how-it-works/">How SPMeta2 Works</a>.

### Model tree build-in validation
The model tree might be optionally validated with build in rules. It guarantees nobody adds a field to the web or list to the site collection. Custom validators can be implemented to address your project needs as well. 

Builtin validators ensure SharePoint limitations and boundaries. SPMeta2 checks that internal names of all your fields don not exceed 32 chars. There is more magic than you can imagine.

### SharePoint 2013 Foundation, Standard, Enterprise and O365 are supported
SPMeta2 supports all SharePoint editions. It is splitted up into several packages to reflect SharePoint editions: Foundation, Standart and Enterprise. O365 support is implemented with CSOM.

### SSOM and CSOM are supported. JSOM is coming up.
SPMeta2 supports SSOM and CSOM. We are working on JSOM support implemeted with TypeScript and SPTypeScript.

### No XML inside - only code
SPMeta2 is code based provision library for SharePoint 2013.
* You DO NOT write XML
* You DO NOT write WSP
* You DO write code instead

Learn more here - <a href="http://docs.subpointsolutions.com/spmeta2/how-it-works/">How SPMeta2 Works</a>.

### Regression and testing are implemented and supported
Having code based provision allows us to have full control over the provision and update flow. As there is no WSP or XML, not features need to be deployed or activated. 

This allows us to write integration tests within minutes, make sure deployment and upgrade work as expected. Most of the provision cases are covered with integration tests. We create a new site or web, deploy everything we need and check if everything has been deployed correctly.
