---
Title: Web model
FileName: webmodel.html
---

SPMeta2 introduces a domain model providing set of definitions for most of the SharePoint artifacts.

Before you begin, make sure you are familiar with the following concepts:

* [SPMeta2 basics](/spmeta2/basics/)
* [SPMeta2 definitions](/spmeta2/definitions/)
* [SPMeta2 models](/spmeta2/models/)

### Planning web models

Although it is possible to have only one web model with all the artifacts you have, there are better ways to get provisioning done. Here are a few hints to have a clean and manageable way to provision web models.

#### Tend to split up models into logical steps

Avoid 'monolithics' models preferring several small, manageable models. The following separation for web models can be recommended:

* Separate web features model
* Separate IA model (lists and list views)
* Separate pages model
* Separate web parts model
* Separate navigation settings model
* Separate master page and home page model
* The rest artifacts…

Such a separation would allow to have small, manageable models and full control over the provisioning flow. The other benefit is a partial provision - you would be able to deploy a particulal model to re-provision lists without affecting web part, or deploy webparts without re-deploying lists and features.

Here is code representation of mentioned approach:

[TEST.WebModelProvision]