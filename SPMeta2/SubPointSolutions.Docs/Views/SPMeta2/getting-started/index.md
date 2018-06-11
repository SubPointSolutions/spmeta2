<mermaid />

# Essentials

SPMeta2 is a hassle-free fluent API for code-based SharePoint artefact provisioning.
It offers a consistent provisioning API via SSOM/CSOM for SharePoin 2010, 2013 and O365.

The library provides an abstraction level over SharePoint API, so it is highly desired to understand a few concepts on which SPMeta2 library is built - definitions, models and provision services.

### A big picture
Here is a big pucture on how SPMeta2 library works.

SPMeta2 introduces a domain of c# POCO objects, then it maps every single POCO object on SharePoint artifacts.

```mermaid
graph LR;
    A[C# POCO objects]-->B[SPMeta2];
    B-->C[SharePoint];
```    
<a target="_blank" href="https://mermaidjs.github.io/mermaid-live-editor/#/edit/eyJjb2RlIjoiZ3JhcGggTFI7XG4gICAgQVtDIyBQT0NPIG9iamVjdHNdLS0-QltTUE1ldGEyXTtcbiAgICBCLS0-Q1tTaGFyZVBvaW50XTsiLCJtZXJtYWlkIjp7InRoZW1lIjoiZGVmYXVsdCJ9fQ">live editor for this diagram</a>

Have a look on the same idea expressed in a c# code. 
Don not focus on understanding every single bit of the code, it will be much clear to you as we refine ideas later in the article.

<a href="_samples/Basics-ABigPictureSample.sample-ref"></a>

### Zoom in, 1/3 scale - definitions

Let's zoom in and have a closer look.
SPMeta2 provides c# POCP objects, we call them **definitions** for every SharePoint artifact.
So you describe what you want to provision in definitions, and then SPMeta2 takes care about everything else.

```mermaid
graph LR;
    a1[Web definition]-->B[SPMeta2];
    a2[Field definition]-->B;
    a3[ContentType definition]-->B;
    a4[List definition]-->B;
    a6[... other definitions ...]-->B;
    B-->C[SharePoint];
```    
<a target="_blank" href="https://mermaidjs.github.io/mermaid-live-editor/#/edit/eyJjb2RlIjoiZ3JhcGggTFI7XG4gICAgYTFbV2ViIGRlZmluaXRpb25dLS0-QltTUE1ldGEyXTtcbiAgICBhMltGaWVsZCBkZWZpbml0aW9uXS0tPkI7XG4gICAgYTNbQ29udGVudFR5cGUgZGVmaW5pdGlvbl0tLT5CO1xuICAgIGE0W0xpc3QgZGVmaW5pdGlvbl0tLT5CO1xuICAgIGE2Wy4uLiBvdGhlciBkZWZpbml0aW9ucyAuLi5dLS0-QjtcbiAgICBCLS0-Q1tTaGFyZVBvaW50XTsiLCJtZXJtYWlkIjp7InRoZW1lIjoiZGVmYXVsdCJ9fQ">live editor for this diagram</a>

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

```mermaid
graph LR;
    a1[Field definition]-->site[Site model];
    a2[ContentType definition]-->site;
    a3[List definition]-->web[Web model];
    a4[List view definition]-->web;
    web-->s[SPMeta2]
    site-->s[SPMeta2]
    s-->C[SharePoint];
```

<a target="_blank" href="https://mermaidjs.github.io/mermaid-live-editor/#/edit/eyJjb2RlIjoiZ3JhcGggTFI7XG4gICAgYTFbRmllbGQgZGVmaW5pdGlvbl0tLT5zaXRlW1NpdGUgbW9kZWxdO1xuICAgIGEyW0NvbnRlbnRUeXBlIGRlZmluaXRpb25dLS0-c2l0ZTtcbiAgICBhM1tMaXN0IGRlZmluaXRpb25dLS0-d2ViW1dlYiBtb2RlbF07XG4gICAgYTRbTGlzdCB2aWV3IGRlZmluaXRpb25dLS0-d2ViO1xuICAgIHdlYi0tPnNbU1BNZXRhMl1cbiAgICBzaXRlLS0-c1tTUE1ldGEyXVxuICAgIHMtLT5DW1NoYXJlUG9pbnRdOyIsIm1lcm1haWQiOnsidGhlbWUiOiJkZWZhdWx0In19">live editor for this diagram</a>

And the same idea in the code. NewSiteModel(), AddField() and AddContentType() help to build a relationships between artifacts
Don't focus on understanding every single bit of the code, it will be much clear to you as we refine ideas later in the article.

<a href="_samples/Basics-ABigPictureSample.sample-ref"></a>

### Zoom in, 3/3 scale - provision services

We can have SharePoint 2010, SharePoint 2013 or SharePoint Online along with either SSOM or CSOM API.
SPMeta2 supports both SSOM and CSOM API abstracting them via 'provisioning services' implementations.

Here is an idea for SSOM or CSOM based provision:
```mermaid
graph LR;
    a1[Field definition]-->site[Site model];
    a2[ContentType definition]-->site;
    a3[List definition]-->web[Web model];
    a4[List view definition]-->web;
    web-->s[SPMeta2]
    site-->s[SPMeta2]
    s --> csom[CSOM service]
    s -->ssom[SSOM service]
    csom-->s2[SharePoint Online];
    csom-->s1[SharePoint 2010 / 2013 / 2016];
    ssom-->s3[SharePoint 2010 / 2013 / 2016];

```

<a target="_blank" href="https://mermaidjs.github.io/mermaid-live-editor/#/edit/eyJjb2RlIjoiZ3JhcGggTFI7XG4gICAgYTFbRmllbGQgZGVmaW5pdGlvbl0tLT5zaXRlW1NpdGUgbW9kZWxdO1xuICAgIGEyW0NvbnRlbnRUeXBlIGRlZmluaXRpb25dLS0-c2l0ZTtcbiAgICBhM1tMaXN0IGRlZmluaXRpb25dLS0-d2ViW1dlYiBtb2RlbF07XG4gICAgYTRbTGlzdCB2aWV3IGRlZmluaXRpb25dLS0-d2ViO1xuICAgIHdlYi0tPnNbU1BNZXRhMl1cbiAgICBzaXRlLS0-c1tTUE1ldGEyXVxuICAgIHMgLS0-IGNzb21bQ1NPTSBzZXJ2aWNlXVxuICAgIHMgLS0-c3NvbVtTU09NIHNlcnZpY2VdXG4gICAgY3NvbS0tPnMyW1NoYXJlUG9pbnQgT25saW5lXTtcbiAgICBjc29tLS0-czFbU2hhcmVQb2ludCAyMDEwIC8gMjAxMyAvIDIwMTZdO1xuICAgIHNzb20tLT5zM1tTaGFyZVBvaW50IDIwMTAgLyAyMDEzIC8gMjAxNl07XG4iLCJtZXJtYWlkIjp7InRoZW1lIjoiZGVmYXVsdCJ9fQ">live editor for this diagram</a>

Finally, the code should be absolutely clear to you. We create definitions, we build a logical model and then we use a provision service to push the model to the SharePoint site. Easy enough?

<a href="_samples/Basics-ABigPictureSample.sample-ref"></a>