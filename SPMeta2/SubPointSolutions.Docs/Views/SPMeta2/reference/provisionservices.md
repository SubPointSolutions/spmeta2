---
Title: Provision services
FileName: provisionservices.html
---
### SPMeta2 Provision Services

Provision services is the last concept you need to know about SPMeta2.

Before you begin, make sure you are familiar with the following concepts:

* [Definitions concept](/spmeta2/reference/definitions)
* [Models concept](/spmeta2/reference/models)

Once you get your definitions and model, so-called "provision services" make all the heavy lifting to get model deployed to SharePoint.
SPMeta2 provides several provision services aiming to support CSOM and SSOM APIs as well as SharePoint Foundation and SharePoint Standard editions.

### Default provision services
All versions of SPMeta2 comes with the following provision services:

#### CSOM based provision:
* CSOMProvisionService
* StandardCSOMProvisionService

#### SSOM based provision:
* SSOMProvisionService
* StandardSSOMProvisionService

CSOM and SSOM corresponds to the API which would be used for provisioning. 
Normally, you should use either StandardCSOMProvisionService or StandardCSOMProvisionService provision services unless you use SharePoint foundation - that's there you better stay on CSOMProvisionService or SSOMProvisionService.

Such separation and seevral provision services helpds to avoid assembly referencies to a particular API (CSOM/SSOM) and dependency on a particular SharePoint edition. 
We still can develop with SharePoint Standard using StandardXXXProvisionService or downgrade to SharePoint Foundation, more of a legacy scenario, using  CSOMProvisionService or SSOMProvisionService.

While adding referencies to SPMeta2 in your project use the following NuGet pckages:
#### SharePoint 2010 CSOM
* SPMeta2.CSOM.Foundation-v14 (CSOMProvisionService)
* SPMeta2.CSOM.Standard-v14 (StandardCSOMProvisionService)

#### SharePoint 2010 SSOM
* SPMeta2.SSOM.Foundation-v14 (SSOMProvisionService)
* SPMeta2.SSOM.Standard-v14 (StandardSSOMProvisionService)

#### SharePoint 2013 CSOM / SharePoint Online:
* SPMeta2.CSOM.Foundation (CSOMProvisionService)
* SPMeta2.CSOM.Standard  (StandardCSOMProvisionService)

#### SharePoint 2013 SSOM
* SPMeta2.SSOM.Foundation (CSOMProvisionService)
* SPMeta2.SSOM.Standard  (StandardCSOMProvisionService)

#### SharePoint Online:
* SPMeta2.CSOM.Foundation-v16 (CSOMProvisionService)
* SPMeta2.CSOM.Standard-v16  (StandardCSOMProvisionService)

Once you add correct referencies, it is really easy to get your model deployed to SharePoint. 
Use the following snippets or check [Writing a simple console app](/spmeta2/getting-started/writing-console-app.html) example.


#### CSOM provision, site models
<a href="_samples/ProvisionServices-Deploy_SiteModel_CSOM.sample-ref"></a>

#### CSOM provision, web models
<a href="_samples/ProvisionServices-Deploy_WebModel_CSOM.sample-ref"></a>

#### SSOM provision, site models
<a href="_samples/ProvisionServices-Deploy_SiteModel_SSOM.sample-ref"></a>

#### SSOM provision, web models
<a href="_samples/ProvisionServices-Deploy_WebModel_SSOM.sample-ref"></a>

SSOM provision has more capabilitied over CSOM, so additional methods such as DeployFarmModel(..) and DeployWebApplicationModel(..) enable farm level and web application level provision.

#### SSOM provision, farm models
<a href="_samples/ProvisionServices-Deploy_FarmModel_SSOM.sample-ref"></a>

#### SSOM provision, web application models
<a href="_samples/ProvisionServices-Deploy_WebApplicationModel_SSOM.sample-ref"></a>
