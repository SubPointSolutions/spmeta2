Resetting role inheritance operations over securable SharePoint objects are implemented with ResetRoleInheritanceDefinition. 

ResetRoleInheritanceDefinition maps out SPSecurableObject.ResetRoleInheritance() method call. 

Note that .AddResetRoleInheritance() syntax passes the object on which the method was called.
For instance, for list, you would get the list wihtin AddResetRoleInheritance() as following:
list.AddResetRoleInheritance(list => {} )

For web, you would get the web wihtin AddResetRoleInheritance() as following:
web.AddResetRoleInheritance(web => {} )