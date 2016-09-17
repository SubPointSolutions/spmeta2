---
Title: Site model
FileName: sitemodel.html
---

SPMeta2 introduces a domain model providing set of definitions for most of the SharePoint artifacts.

Before you begin, make sure you are familiar with the following concepts:

* [SPMeta2 basics](/spmeta2/basics/)
* [SPMeta2 definitions](/spmeta2/definitions/)
* [SPMeta2 models](/spmeta2/models/)

### Planning site models

Although it is possible to have only one site model with all the artifacts you have, there are better ways to get provisioning done. Here are a few hints to have a clean and manageable way to provision site models.

#### Tend to split up models into logical steps

Avoid 'monolithics' models preferring several small, manageable models. The following separation for site models can be recommended:

* Separate taxonomy model
* Separate sandbox solutions model
* Separate site features model
* Separate IA model (fields and content types)
* Separate custom user actions model
* The rest artifacts...

Such a separation would allow to have small, manageable models and full control over the provisioning flow. The other benefit is a partial provision - you would be able to deploy a particulal model to re-provision sandbox solution without affecting fields, or deploy fields without re-deploying sandbox solution.

Here is code representation of mentioned approach:

[TEST.SiteModelProvision]