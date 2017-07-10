Adding new field to site, web and lists is enabled via FieldDefinition object and .AddField() extension method.

Provision checks if field exists trying to find existing one by Id/Name properties.
If field cannot be found, a new one is created under selected scope - site, web or list.

It can also be suggested to use BuiltInFieldTypes to benefit OOTB SharePoint field types.

> Be aware: while adding field directly to a list FieldDefinition.AddFieldOption should be set to "AddFieldInternalNameHint" so that field would get the right internal name within a list.
By default SharePoint would generate InternalName for a list scoped fields.