Web parts deletion is suppoerted by DeleteWebPartsDefinition.

DeleteWebPartsDefinition can be added to any page: wiki, publishing or web part page.
Use .AddDeleteWebParts() with new DeleteWebPartsDefinition.

DeleteWebPartsDefinition has .WebParts property that stores 'web part matches' to find web part for deletion operation.
WebPartMatch has three properties - Title, Id and WebpartType to lookup the targer web part.

Note that only WebPartMatch.Title is currently used to find the web part on the pages.
Id and WebpartType are reserverd for the future API enhancements.