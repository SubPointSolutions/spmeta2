using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Containers.Assertion;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientListItemDefinitionValidator : ListItemModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<ListItemDefinition>("model", value => value.RequireNotNull());

            List list = null;
            Folder rootFolder = null;

            if (modelHost is ListModelHost)
            {
                list = (modelHost as ListModelHost).HostList;
                rootFolder = (modelHost as ListModelHost).HostList.RootFolder;

                if (!rootFolder.IsPropertyAvailable("ServerRelativeUrl"))
                {
                    rootFolder.Context.Load(rootFolder, f => f.ServerRelativeUrl);
                    rootFolder.Context.ExecuteQueryWithTrace();
                }
            }
            else if (modelHost is FolderModelHost)
            {
                list = (modelHost as FolderModelHost).CurrentList;
                rootFolder = (modelHost as FolderModelHost).CurrentListItem.Folder;

                if (!rootFolder.IsPropertyAvailable("ServerRelativeUrl"))
                {
                    rootFolder.Context.Load(rootFolder, f => f.ServerRelativeUrl);
                    rootFolder.Context.ExecuteQueryWithTrace();
                }
            }

            
            var spObject = FindListItem(list, rootFolder, definition);

            if (!spObject.IsPropertyAvailable(BuiltInInternalFieldNames.Title))
            {
                var context = spObject.Context;

                context.Load(spObject, o => o.DisplayName);
                context.ExecuteQueryWithTrace();
            }


            ValidateProperties(spObject, definition);
        }


        protected virtual void ValidateProperties(ListItem item, ListItemDefinition definition)
        {
            var stringCustomContentType = string.Empty;

            if (!string.IsNullOrEmpty(definition.ContentTypeName)
                || !string.IsNullOrEmpty(definition.ContentTypeId))
            {
                // TODO
                //stringCustomContentType = ResolveContentTypeId(folderHost, definition);
            }


            var assert = ServiceFactory.AssertService
                             .NewAssert(definition, item)
                                   .ShouldNotBeNull(item);

            assert
                .ShouldBeEqual(m => m.Title, o => o.DisplayName);

            if (!string.IsNullOrEmpty(definition.ContentTypeId))
            {
                // TODO
            }
            else
            {
                assert.SkipProperty(m => m.ContentTypeId, "ContentTypeId is null or empty. Skipping.");
            }

            if (!string.IsNullOrEmpty(definition.ContentTypeName))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.ContentTypeName);
                    var currentContentTypeName = d["ContentTypeId"].ToString();

                    var isValis = stringCustomContentType == currentContentTypeName;

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValis
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.ContentTypeName, "ContentTypeName is null or empty. Skipping.");
            }
        }

    }
}
