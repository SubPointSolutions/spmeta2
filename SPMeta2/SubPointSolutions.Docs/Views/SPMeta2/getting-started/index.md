----
Title: Getting started
Order: 100
TileLink: true
TileLinkOrder: 10
----
### SPMeta2 essentials

SPMeta2 is a hassle-free fluent API for code-based SharePoint artefact provisioning.
It offers a consistent provisioning API via SSOM/CSOM for SharePoin 2010, 2013 and O365.

The library provides an abstraction level over SharePoint API, so it is highly desired to understand a few concepts on which SPMeta2 library is built - definitions, models and provision services.

### A big picture
Here is a big pucture on how SPMeta2 library works.

SPMeta2 introduces a domain of c# POCO objects, then it maps every single POCO object on SharePoint artifacts.

<img src='http://g.gravizo.com/g?
 digraph G {
   rankdir="LR";
   "CSharp POCO objects" -> "SPMeta2" -> SharePoint;    } '></img>

Have a look on the same idea expressed in a c# code. 
Don not focus on understanding every single bit of the code, it will be much clear to you as we refine ideas later in the article.

<a href="_samples/Basics-ABigPictureSample.sample-ref"></a>

### Zoom in, 1/3 scale - definitions

Let's zoom in and have a closer look.
SPMeta2 provides c# POCP objects, we call them **definitions** for every SharePoint artifact.
So you describe what you want to provision in definitions, and then SPMeta2 takes care about everything else.

<img src='http://g.gravizo.com/g? digraph G {
   rankdir="LR"; "Web definition" -> "SPMeta2";  
   "Field definition" -> "SPMeta2";  
   "Content type definition" -> "SPMeta2";  
   "List definition" -> "SPMeta2";  
   "List view definition" -> "SPMeta2";  
   "Web part page definition" -> "SPMeta2";  
   "Web part definition" -> "SPMeta2"; 
   "... other definitions ..." -> "SPMeta2";  "SPMeta2" -> SharePoint;  }' ></img>

And the same idea in the code. Now field definition and content type definition make sense, don't they?
Don't focus on understanding every single bit of the code, it will be much clear to you as we refine ideas later in the article.

<a href="_samples/Basics-ABigPictureSample.sample-ref"></a>

### Zoom in, 2/3 scale - models

All SharePoint artifacts have relationships between each other.
For intance, here a few to mentions:

* fields belong to site, web or list
* content types belong to site or web
* lists belong to web
* list views belong to list
* etc..

Having definitions of not enought, we need to describe relationshop between them, so that SPMeta2 would know how to provision them correctly. 
Here is a refined view on how SPMeta2 works for site and web level provision:

<img src='http://g.gravizo.com/g?
 digraph G {
   rankdir="LR";
   "SPMeta2" -> SharePoint;   
   "Site model" -> "SPMeta2";
   "Web model" -> "SPMeta2";
   "Field definition"  -> "Site model";  
   "Content type definition" -> "Site model";  
   "User Custom Action" -> "Site model";  
   "List definition" -> "Web model";  
   "List view definition" -> "Web model";  
   "Web partpage" -> "Web model"; }' ></img>

And the same idea in the code. NewSiteModel(), AddField() and AddContentType() help to build a relationships between artifacts
Don't focus on understanding every single bit of the code, it will be much clear to you as we refine ideas later in the article.

<a href="_samples/Basics-ABigPictureSample.sample-ref"></a>

### Zoom in, 3/3 scale - provision services

We can have SharePoint 2010, SharePoint 2013 or SharePoint Online along with either SSOM or CSOM API.
SPMeta2 supports both SSOM and CSOM API abstracting them via 'provisioning services' implementations.

Here is an idea for SSOM or CSOM based provision:

<img src='http://g.gravizo.com/g?
 digraph G {
   "Site model" -> "Choose your provision strategy";
   "Web model" -> "Choose your provision strategy";
   "Field definition" -> "Site model";  
   "Content type definition" -> "Site model";  
   "User Custom Action" -> "Site model";  
   "List definition" -> "Web model";  
   "List view definition" -> "Web model";  
   "Web partpage" -> "Web model";  
    "Choose your provision strategy" -> "CSOM provision service";   
    "Choose your provision strategy" -> "SSOM provision service";   
    "CSOM provision service" -> "SharePoint 2013";
    "CSOM provision service" -> "SharePoint Online";
    "SSOM provision service" -> "SharePoint 2010";
    "SSOM provision service" -> "SharePoint 2013; }' ></img>

Finally, the code should be absolutely clear to you. We create definitions, we build a logical model and then we use a provision service to push the model to the SharePoint site. Easy enough?

<a href="_samples/Basics-ABigPictureSample.sample-ref"></a>