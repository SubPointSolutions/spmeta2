﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Administration;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class DocumentParserDefinitionValidator : DocumentParserModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<DocumentParserDefinition>("model", value => value.RequireNotNull());

            var spObject = GetCurrentObject(farmModelHost.HostFarm, definition);

            // TODO
            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, spObject);

            assert.ShouldBeEqual(m => m.FileExtension, o => o.FileExtension);
            assert.ShouldBeEqual(m => m.ProgId, o => o.ProgId);
        }
    }
}
