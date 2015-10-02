﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class InformationRightsManagementSettingsDefinitionGenerator : TypedDefinitionGeneratorServiceBase<InformationRightsManagementSettingsDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.AllowPrint = Rnd.Bool();
                def.AllowScript = Rnd.Bool();
                def.AllowWriteCopy = Rnd.Bool();

                def.DisableDocumentBrowserView = Rnd.Bool();
                def.DocumentAccessExpireDays = Rnd.Int(75);
                def.DocumentLibraryProtectionExpireDate = Rnd.Date();

                def.EnableDocumentAccessExpire = Rnd.Bool();
                def.EnableDocumentBrowserPublishingView = Rnd.Bool();
                def.EnableGroupProtection = Rnd.Bool();
                def.EnableLicenseCacheExpire = Rnd.Bool();

                def.GroupName = Rnd.UserEmail();
                def.LicenseCacheExpireDays = Rnd.Int(255);

                def.PolicyDescription = Rnd.String();
                def.PolicyTitle = Rnd.String();
            });
        }

        public override ModelNode GetCustomParenHost()
        {
            var listDefinitionGenerator = new ListDefinitionGenerator();
            var listDefinition = listDefinitionGenerator.GenerateRandomDefinition() as ListDefinition;

            listDefinition.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            
            listDefinition.IrmEnabled = true;
            listDefinition.IrmExpire = true;

            var node = new ListModelNode
            {
                Value = listDefinition,
            };

            return node;
        }
    }
}
