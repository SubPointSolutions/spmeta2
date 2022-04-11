﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class SharePointDesignerSettingsDefinitionGenerator : TypedDefinitionGeneratorServiceBase<SharePointDesignerSettingsDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.EnableCustomizingMasterPagesAndPageLayouts = Rnd.Bool();
                def.EnableDetachingPages = Rnd.Bool();
                def.EnableManagingWebSiteUrlStructure = Rnd.Bool();
                def.EnableSharePointDesigner = Rnd.Bool();
            });
        }
    }
}
