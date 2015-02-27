using System;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class ListDefinitionGenerator : TypedDefinitionGeneratorServiceBase<ListDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Title = Rnd.String();
                def.Description = Rnd.String();

                def.Url = Rnd.String(16);

                def.TemplateType = BuiltInListTemplateTypeId.GenericList;

                def.ContentTypesEnabled = Rnd.Bool();

                var draftOpt = Rnd.Int(10);

                if (draftOpt > 1)
                {
                    if (draftOpt <= 3)
                    {
                        def.DraftVersionVisibility = BuiltInDraftVisibilityTypes.Approver;

                        def.EnableVersioning = true;
                        //def.EnableMinorVersions = true;
                        def.EnableModeration = true;
                    }

                    if (draftOpt >= 3 && draftOpt <= 6)
                    {
                        def.DraftVersionVisibility = BuiltInDraftVisibilityTypes.Author;

                        def.EnableVersioning = true;
                        //def.EnableMinorVersions = true;
                        def.EnableModeration = true;
                    }

                    if (draftOpt > 6)
                        def.DraftVersionVisibility = BuiltInDraftVisibilityTypes.Reader;
                }
                else
                {
                    def.DraftVersionVisibility = string.Empty;
                }

                //def.NoCrawl = Rnd.NullableBool();
                //def.OnQuickLaunch = Rnd.NullableBool();
                //def.Hidden = Rnd.NullableBool();
                //def.EnableAttachments = Rnd.NullableBool();
                //def.EnableFolderCreation = Rnd.NullableBool();

                //def.EnableMinorVersions = Rnd.NullableBool();
                //def.EnableModeration = Rnd.NullableBool();
                //def.EnableVersioning = Rnd.NullableBool();
                //def.ForceCheckout = Rnd.NullableBool();

                //if (def.ForceCheckout.HasValue && def.ForceCheckout.Value)
                //{
                //    def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
                //}

                //if (def.Hidden.HasValue && def.Hidden.Value)
                //{
                //    def.OnQuickLaunch = false;
                //}
            });
        }
    }
}
