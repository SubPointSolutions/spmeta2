using System;
namespace SPMeta2.SSOM.DefaultSyntax
{
    [Obsolete("Obsolete. Will be removed from the SPMeta2 API. Use ModernSyntax.OnProvisioning/OnProvisioned events.")]
    public static class ContentTypeDefinitionSyntax
    {
        #region methods

        //public static DefinitionBase OnCreating(this DefinitionBase model, Action<ContentTypeDefinition, SPContentType> action)
        //{
        //    model.RegisterModelUpdatingEvent(action);

        //    return model;
        //}

        //public static DefinitionBase OnCreated(this DefinitionBase model, Action<ContentTypeDefinition, SPContentType> action)
        //{
        //    model.RegisterModelUpdatedEvent(action);

        //    return model;
        //}

        #endregion
    }
}
