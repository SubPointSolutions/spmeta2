## What is SPMeta2?
SPMeta2 is a fluent API for code-based SharePoint artifact provisioning.

Struggling with SharePoint's API inconsistency, bugs, "by-design" behaviour, unaffordable amount of time to write, support and upgrade WSP packages and XML, a team of passionate SharePoint professionals decided to come up with robust, testable and repeatable way to deploy such artifacts like fields, content types, libraries, pages and many more.

As an outcome, we created SPMeta2 - a .NET 4.5 library to provide fluent API for SharePoint 2013 artifact with SSOM/CSOM or JSOM for both on premise and O365 instances. What's inside?

## SPMeta2 philosophy and mission
### Fluent API and syntax extensions
SPMeta2 API allows you to define SharePoint artifacts such as field, content type, list (and many more), define relationships between them and, finally, deploy them via SSOM/CSOM. You work with c# POCO objects defining your data model, we take care about the rest. 

SPMeta2 might be extended with custom syntax implementation to meet your project needs. Extension methods are used to adjust a specific behavior or property of SharePoint artifacts. Custom syntax or DSL can easily be created to address specific project needs.

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

### Regression and testing are implemented and supported
Having code based provision allows us to have full control over the provision and update flow. As there is no WSP or XML, not features need to be deployed or activated. 

This allows us to write integration tests within minutes, make sure deployment and upgrade work as expected. Most of the provision cases are covered with integration tests. We create a new site or web, deploy everything we need and check if everything has been deployed correctly.

### Documentation and support
<ul>
                    <li><a target="_blank" href="https://github.com/SubPointSolutions/spmeta2">SPMeta2 @ GitHub</a></li>
                    <li><a target="_blank" href="https://www.nuget.org/packages?q=spmeta2">SPMeta2 @ Nuget</a></li>
                    <li><a target="_blank" href="https://github.com/SubPointSolutions/spmeta2/wiki">SPMeta2 Documentation Wiki</a></li>
                    <li><a target="_blank" href="https://github.com/SubPointSolutions/spmeta2/issues">SPMeta2 Bugtracker</a></li>
                    <li><a target="_blank" href="http://subpointsolutions.github.io/spmeta2/Help">SPMeta2 API Documentation</a></li>
                </ul>
