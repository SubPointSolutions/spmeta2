﻿using System;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Utils;
using FeatureDefinitionScope = SPMeta2.Definitions.FeatureDefinitionScope;
using SPMeta2.Exceptions;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientFeatureDefinitionValidator : FeatureModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<FeatureDefinition>("model", value => value.RequireNotNull());

            if (!IsValidHost(modelHost))
                throw new Exception("model host needs to be SPFarm, SPWebApplication, SPSit or SPWeb instance");


            FeatureCollection features = null;
            Feature spObject = null;

            var assert = ServiceFactory.AssertService
                   .NewAssert(definition, spObject);

            switch (definition.Scope)
            {
                case FeatureDefinitionScope.Farm:
                    throw new SPMeta2NotImplementedException("Farm features are not supported in CSOM.");

                case FeatureDefinitionScope.WebApplication:

                    throw new SPMeta2NotImplementedException("WebApplication features are not supported in CSOM.");

                case FeatureDefinitionScope.Site:

                    assert.SkipProperty(m => m.Scope, "Correct site scope");

                    var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
                    features = siteModelHost.HostSite.Features;

                    var siteContext = siteModelHost.HostSite.Context;
                    siteContext.Load(features);
                    siteContext.ExecuteQueryWithTrace();


                    break;

                case FeatureDefinitionScope.Web:

                    assert.SkipProperty(m => m.Scope, "Correct web scope");

                    var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
                    features = webModelHost.HostWeb.Features;

                    var webContext = webModelHost.HostWeb.Context;
                    webContext.Load(features);
                    webContext.ExecuteQueryWithTrace();

                    break;
            }

            var featureId = definition.Id;

            spObject = features.GetById(featureId);
            features.Context.Load(spObject, o => o.DefinitionId);
            features.Context.ExecuteQueryWithTrace();

            assert.Dst = spObject;

            if (definition.Enable == false)
            {
                // check is null, skill all props

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.Enable);

                    var isValid = d.ServerObjectIsNull == true;

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });

                assert
                    .SkipProperty(m => m.Id, "Enable = false. Skipping.")
                    .SkipProperty(m => m.ForceActivate, "Enable = false. Skipping.")
                    .SkipProperty(m => m.Enable, "Enable = false. Skipping.")
                    .SkipProperty(m => m.Title, "Enable = false. Skipping.");
            }
            else
            {
                assert.ShouldBeEqual(m => m.Id, o => o.DefinitionId);

                if (definition.ForceActivate)
                {
                    assert
                        .SkipProperty(m => m.ForceActivate, "ForceActivate = true. Expect not null feature instance.")
                        .ShouldNotBeNull(spObject);
                }
                else
                {
                    assert
                        .SkipProperty(m => m.ForceActivate, "ForceActivate = false. Skipping.");
                }


                if (definition.Enable)
                {
                    assert
                        .SkipProperty(m => m.Enable, "Enable = true. Expect not null feature instance.")
                        .ShouldNotBeNull(spObject);
                }
                else
                {
                    assert
                        .SkipProperty(m => m.Enable, "Enable = false. Expect null feature instance.")
                        .ShouldBeNull(spObject);
                }
            }

        }


    }

    //internal static class SPFeatureExtensions
    //{
    //    public static Guid GetFeatureId(this Feature feature)
    //    {
    //        return feature.DefinitionId;
    //    }
    //}
}
