﻿using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client.WebParts;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using SPMeta2.Containers.Assertion;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientDeleteWebPartsDefinitionValidator : DeleteWebPartsModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listItemModelHost = modelHost.WithAssertAndCast<ListItemModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<DeleteWebPartsDefinition>("model", value => value.RequireNotNull());

            //var listItem = listItemModelHost.HostListItem;
            var list = listItemModelHost.HostList;

            var context = list.Context;
            var currentPageFile = GetCurrentPageFile(listItemModelHost);
            var spObject = GetCurrentPageFile(listItemModelHost);

            var webPartManager = spObject.GetLimitedWebPartManager(PersonalizationScope.Shared);

            // web part on the page
            var webpartOnPage = webPartManager.WebParts.Include(wp => wp.Id, wp => wp.WebPart);
            var webPartDefenitions = context.LoadQuery(webpartOnPage);
            context.ExecuteQueryWithTrace();

            var assert = ServiceFactory.AssertService.NewAssert(definition, spObject);

            assert
                 .ShouldNotBeNull(webPartDefenitions);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.WebParts);
                var webPartMatches = s.WebParts;

                var isValid = true;

                if (webPartMatches.Count == 0)
                {
                    isValid = webPartDefenitions.Count() == 0;
                }
                else
                {
                    // title only yet
                    // should not be any by mentioned title
                    var wpTitles = webPartMatches.Select(wpMatch => wpMatch.Title);
                    isValid = webPartDefenitions.Count(w => wpTitles.Contains(w.WebPart.Title)) == 0;
                }

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = null,
                    IsValid = isValid
                };
            });

        }
    }
}
