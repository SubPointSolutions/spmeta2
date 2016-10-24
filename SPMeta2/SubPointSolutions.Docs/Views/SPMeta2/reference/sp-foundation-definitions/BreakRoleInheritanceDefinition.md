Break role inheritance operations over securable SharePoint objects are implemented with BreakRoleInheritanceDefinition. 

BreakRoleInheritanceDefinition maps out SPSecurableObject.BreakRoleInheritance() method call. 
Properties CopyRoleAssignments and ClearSubscopes get passed to CSOM/SSOM .BreakRoleInheritance() method.

Additional property ForceClearSubscopes is introduced by SPMeta2. The property forces SPMeta2 to clear .RoleAssignments collection every provision.

Note that .AddBreakRoleInheritance() syntax passes the object on which the method was called.
For instance, for list, you would get the list wihtin AddBreakRoleInheritance() as following:
list.AddBreakRoleInheritance(list => {} )

For web, you would get the web wihtin AddResetRoleInheritance() as following:
web.AddBreakRoleInheritance(web => {} )