
Taxonomy field provision is enabled via TaxonomyFieldDefinition object.

Both CSOM/SSOM object models are supported. 
You can deploy either single object or a set of the objects using AddTaxonomyField() extension method as per following examples.

The following properties allow to adjust the desired field configuration:

* **UseDefaultSiteCollectionTermStore**, indicates that the default site taxonomy store needs to be used
* **SspId**, ID of the target taxonomy store
* **SspName**, Name of the target taxonomy store
* **IsMulti**, allows multiple choices option for the field
* **TermSetName**, term set name to be bind to the field
* **TermSetId**, term set id to be bind to the field
* **TermName**, term name to be bind to the field
* **TermId**, term id to be bind to the field
* **IsPathRendered**, indicated path rendering,
* **CreateValuesInEditForm**, allows to create new values in the edit form,
* **Open**, indicates that the term is open

