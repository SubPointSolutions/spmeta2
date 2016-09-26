List view provision is enabled via ListViewDefinition object.

Both CSOM/SSOM object models are supported. Provision checks if list view exists looking up it by Url property, then by Title, and the creates a new list definition. You can deploy either single list view or a set of the list views using AddListView() extension method as per following examples.
