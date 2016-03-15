﻿using Microsoft.SharePoint;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Containers.Assertion;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class JobDefinitionValidator : JobModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webAppModelHost = modelHost.WithAssertAndCast<WebApplicationModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<JobDefinition>("model", value => value.RequireNotNull());

            var spObject = FindWebApplicationJob(modelHost, webAppModelHost.HostWebApplication, definition);

            var assert = ServiceFactory.AssertService
                       .NewAssert(definition, spObject)
                       .ShouldNotBeNull(spObject)
                       .ShouldBeEqual(m => m.Name, o => o.Name)
                       .ShouldBeEqual(m => m.Title, o => o.Title);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(def => def.JobType);

                var jobType = ResolveJobType(definition);
                var spJobType = spObject.GetType();

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = null,
                    IsValid = jobType == spJobType
                };
            });

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(def => def.ScheduleString);
                var spSchedule = SPSchedule.FromString(definition.ScheduleString);

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = null,
                    IsValid = spSchedule.Equals(spObject.Schedule)
                };
            });

            if (definition.Properties.Any())
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.Properties);
                    var isValid = true;

                    foreach (var prop in s.Properties)
                    {
                        if (!d.Properties.ContainsKey(prop.Key))
                        {
                            isValid = false;
                            break;
                        }

                        var currentSrcProp = prop.Value;
                        var currentDstProp = d.Properties[prop.Key];

                        if (currentDstProp is string)
                        {
                            isValid = (string)currentDstProp == (string)currentSrcProp;
                        }
                        else if (currentDstProp is int)
                        {
                            isValid = (int)currentDstProp == (int)currentSrcProp;
                        }
                        else if (currentDstProp is double)
                        {
                            isValid = (double)currentDstProp == (double)currentSrcProp;
                        }
                        else
                        {
                            throw new Exception(string.Format("Type [{0}] is not supported yet", currentDstProp.GetType()));
                        }

                        if (!isValid)
                        {
                            break;
                        }
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
            else
            {
                assert.SkipProperty(p => p.Properties, "Properties are empty. Skiping.");
            }
        }

        #endregion
    }
}
