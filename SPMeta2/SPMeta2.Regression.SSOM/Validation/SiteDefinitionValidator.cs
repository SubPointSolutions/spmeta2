﻿using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using SPMeta2.Regression.SSOM;
using SPMeta2.Containers.Assertion;

namespace SPMeta2.Regression.Validation.ServerModelHandlers
{
    public class SiteDefinitionValidator : SiteModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webAppModelHost = modelHost.WithAssertAndCast<WebApplicationModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<SiteDefinition>("model", value => value.RequireNotNull());

            var spObject = GetExistingSite(webAppModelHost.HostWebApplication, definition);

            //  [FALSE] - [Name]
            //[FALSE] - [Description]
            //[FALSE] - [Url]
            //[FALSE] - [PrefixName]
            //[FALSE] - [SiteTemplate]
            //[FALSE] - [LCID]
            //[FALSE] - [OwnerLogin]
            //[FALSE] - [OwnerName]
            //[FALSE] - [OwnerEmail]
            //[FALSE] - [SecondaryContactName]
            //[FALSE] - [SecondaryContactEmail]
            //[FALSE] - [DatabaseName]

            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldNotBeNull(spObject);

            assert
                .ShouldBeEqual(m => m.Name, o => o.GetSiteName())
                .ShouldBeEqual(m => m.Description, o => o.GetSiteDescription())
                .ShouldBeEqual(m => m.SiteTemplate, o => o.GetSiteTemplate())
                //.ShouldBeEndOf(m => m.Url, o => o.Url)
                //.ShouldBePartOf(m => m.PrefixName, o => o.Url)

                .ShouldBePartOf(m => m.OwnerLogin, o => o.GetOwnerLogin())
                .ShouldBePartOf(m => m.OwnerName, o => o.GetOwnerName())
                .SkipProperty(m => m.OwnerEmail, "Skipping OwnerEmail validation.")

                //.ShouldBePartOf(m => m.SecondaryContactLogin, o => o.GetSecondOwnerLogin())
                //.ShouldBePartOf(m => m.SecondaryContactName, o => o.GeSecondtOwnerName())
                .SkipProperty(m => m.SecondaryContactEmail, "Skipping SecondaryContactEmail validation.")

                .ShouldBeEqual(m => m.LCID, o => o.GetSiteLCID());

            if (string.IsNullOrEmpty(definition.Url) || definition.Url == "/")
            {
                // Enhance SiteDefinition provision - enable provision under the managed path  #853 
                // the URL would end with the managed path
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.Url);
                    var isValid = true;

                    isValid = d.Url.EndsWith(s.PrefixName);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });
            }
            else
            {
                // should be an end if set
                assert.ShouldBeEndOf(m => m.Url, o => o.Url);
            }

            if (!string.IsNullOrEmpty(definition.SecondaryContactLogin))
                assert.ShouldBePartOf(m => m.SecondaryContactLogin, o => o.GetSecondOwnerLogin());
            else
                assert.SkipProperty(m => m.SecondaryContactLogin, "SecondaryContactLogin is null or empty. Skipping.");

            if (!string.IsNullOrEmpty(definition.SecondaryContactName))
                assert.ShouldBePartOf(m => m.SecondaryContactName, o => o.GeSecondtOwnerName());
            else
                assert.SkipProperty(m => m.SecondaryContactName, "SecondaryContactName is null or empty. Skipping.");

            if (!string.IsNullOrEmpty(definition.PrefixName))
            {
                assert.ShouldBePartOf(m => m.PrefixName, o => o.Url);
            }
            else
            {
                assert.SkipProperty(m => m.PrefixName, "PrefixName is null or empty. Skipping.");
            }

            if (!string.IsNullOrEmpty(definition.DatabaseName))
            {
                assert.ShouldBeEqual(m => m.DatabaseName, o => o.GetContentDbName());
            }
            else
            {
                assert.SkipProperty(m => m.DatabaseName, "DatabaseName is null or empty. Skipping.");
            }
        }
    }

    internal static class SPSiteExtensions
    {
        public static string GetContentDbName(this SPSite site)
        {
            return site.ContentDatabase.Name;
        }

        public static string GetSiteTemplate(this SPSite site)
        {
            return string.Format("{0}#{1}", site.RootWeb.WebTemplate, site.RootWeb.Configuration);
        }

        public static string GetOwnerLogin(this SPSite site)
        {
            return site.Owner.LoginName.ToLower();
        }

        public static string GetOwnerName(this SPSite site)
        {
            return site.Owner.Name;
        }

        public static string GetSecondOwnerLogin(this SPSite site)
        {
            return site.SecondaryContact.LoginName.ToLower();
        }

        public static string GeSecondtOwnerName(this SPSite site)
        {
            return site.SecondaryContact.Name;
        }

        public static string GetSiteName(this SPSite site)
        {
            return site.RootWeb.Title;
        }

        public static string GetSiteDescription(this SPSite site)
        {
            return site.RootWeb.Description;
        }

        public static uint GetSiteLCID(this SPSite site)
        {
            return (uint)site.RootWeb.Locale.LCID;
        }
    }
}
