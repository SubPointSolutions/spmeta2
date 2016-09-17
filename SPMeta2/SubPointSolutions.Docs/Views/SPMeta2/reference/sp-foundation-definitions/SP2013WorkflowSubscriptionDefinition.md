
SharePoint 2013 workflow binding to a web or list is enabled via SP2013WorkflowSubscriptionDefinition object.

Both CSOM/SSOM object models are supported. 
Provision checks if object exists looking up it by Name property, then creates a new object. 
You can deploy either single object or a set of the objects using AddSP2013WorkflowSubscription() extension method as per following examples

