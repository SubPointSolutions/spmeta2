SPMeta2 - Fluent API for code-based SharePoint artifact provisioning
=======

SPMeta2 is a research project by spdevlab.com team aims to provide fluent API for SharePoint objects provision, deployment and retraction.

SPMeta2 is aimed to support provision of such artifacts like fields, content types, lists, lists views, web parts, security groups, web part pages and publishing page, etc. It will also allow to setup web app, site collection, sites, some service application and farm settings.

Where did it come from?
-------------

The first version of SPMeta library has been built and used in our production projects more than for a year. We decided that it is a good time to put all the best practices together and share our experience which was recently learnt. So, what is inside?

Fluent API and syntax extensions
-------------
The fluent API allows you to create a model tree from any SharePoint object you may want to in a familiar to most of the developer manner. Setup c# POCO, build the model tree linking sites, webs, fields, content types, pages and so on together and call our API to put it on your SharePoint. It's easy, logical and familiar.

The Fluent API might be extended by custom syntax implementation to meet your project needs as well as lambdas might be utilized to adjust the specific behavior or property of SharePoint artifacts in your scenario. You may build your own syntax or domain to address specific project needs. We will show how.

Model tree build-in validation and extensions
-------------
The model tree might be optionally validated with build in rules. It guarantees nobody can try to add a field to the web or a list to the site collection. Custom validators might be implemented to address your project needs.

Also, there are some support for a SharePoint limitations and boundaries. SPMeta2 can make sure that internal names of all your fields don't exceed 32 chars. Otherwise, SharePoint would trim it, and you might get unpredictable behavior and very untraceable errors. Some of these checks are already included. 

All SharePoint versions are welcomed
-------------
The design of SPMeta2 allows to use it in SharePoint foundation as well as add additional modules to get benefits of SharePoint Standard or Enterprise edition. No unsuspected refineries to "Microsoft.SharePoint.Portal.dll" in your foundation projects. We promise.

SSOM/CSOM support (JSOM is coming up)
-------------
SPMeta2 supports server side object model, client object model (c#) ans we are working on js support to allow you create you "new apps" for SharePoint 2013 in a smooth and effective way.

"NoXML paradigm" inside
-------------
Having a lot of struggling with XML for a simple and not provisioning, we just given up and build SPMeta. SPMeta2 is a better version. It is here and being created right now. We know that XML va code-based provision is a long story. We will cover it here soon or later.
