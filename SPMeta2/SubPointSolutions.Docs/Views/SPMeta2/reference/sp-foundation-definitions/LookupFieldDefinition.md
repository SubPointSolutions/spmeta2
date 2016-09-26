Lookup field provision is enabled via LookupFieldDefinition object.

Both CSOM/SSOM object models are supported. 
You can deploy either single object or a set of the objects using AddLookupField() extension method as per following examples.

The following properties allow to adjust the desired field configuration:

One of the following properties are used to bind the field to the target list.
If none of them are defined, then lookup field is left unbinded.

* **LookupList**, GUID of the target list. Could also be "Self" or "UserInfo"
* **LookupListTitle**, Title of the target list
* **LookupListUrl**, web relative URL of the target list

* **LookupWebId**, Id od the target web if list is located on non-root web
* **LookupField**, refers to 'ShowField' property

**Provisioning on existing lists** could be done setting up the following properties:

* One of LookupList, LookupListTitle or LookupListUrl
* LookupWebId, if the list exists on non-root web
* LookupField, if it is different to 'Title'

Provision would create a lookup field and bind it to the target list.

**Provisioning on lists which don't exist yet** could be done the following way:

* Define lookup field without setting up a target list, then provision the field. 
* Provision all the list which you need
* Setup one of LookupList, LookupListTitle or LookupListUrl. LookupField and LookupWebId if needed
* Proivision lookup field again

That provision flow would create a lookup field without binding to the list, then you creare needed lists, and then re-provision lookup field again - that would bind field to the list.
