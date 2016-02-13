using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Services;
using SPMeta2.Exceptions;
using SPMeta2.Syntax.Default;


namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class ListViewScenariousTest : SPMeta2RegresionScenarioTestBase
    {
        #region internal

        [ClassInitializeAttribute]
        public static void Init(TestContext context)
        {
            InternalInit();
        }

        [ClassCleanupAttribute]
        public static void Cleanup()
        {
            InternalCleanup();
        }

        #endregion

        #region types

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListsViews.Types")]
        public void CanDeploy_ListView_AsHtml()
        {
            TestRandomDefinition<ListViewDefinition>(def =>
            {
                def.Hidden = false;
                def.Type = BuiltInViewType.Html;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListsViews.Types")]
        public void CanDeploy_ListView_AsCalendar()
        {

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddRandomList(list =>
                        {
                            var listDef = list.Value as ListDefinition;

                            listDef.TemplateType = BuiltInListTemplateTypeId.Events;

                            list.AddRandomListView(view =>
                            {
                                var viewDef = view.Value as ListViewDefinition;

                                viewDef.Hidden = false;
                                viewDef.Type = BuiltInViewType.Calendar;

                                viewDef.Query = @"<Where>
<DateRangesOverlap>
<FieldRef Name=""EventDate"" />
<FieldRef Name=""EndDate"" />
<FieldRef Name=""RecurrenceID"" />
<Value Type=""DateTime""><Month />
</Value>
</DateRangesOverlap>
</Where>";


                                viewDef.ViewData = @"<FieldRef Name=""Title"" Type=""CalendarMonthTitle""/>
<FieldRef Name=""Title"" Type=""CalendarWeekTitle""/>
<FieldRef Name=""Created"" Type=""CalendarWeekLocation""/>
<FieldRef Name=""Title"" Type=""CalendarDayTitle""/>
<FieldRef Name=""Created"" Type=""CalendarDayLocation""/>";
                            });
                        });
                });

            TestModel(model);
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.ListsViews.Types")]
        public void CanDeploy_ListView_AsChart()
        {
            TestRandomDefinition<ListViewDefinition>(def =>
            {
                def.Hidden = false;
                def.Type = BuiltInViewType.Chart;
            });
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.ListsViews.Types")]
        public void CanDeploy_ListView_AsGantt()
        {

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddRandomList(list =>
                        {
                            var listDef = list.Value as ListDefinition;

                            listDef.TemplateType = BuiltInListTemplateTypeId.TasksWithTimelineAndHierarchy;

                            list.AddRandomListView(view =>
                            {
                                var viewDef = view.Value as ListViewDefinition;

                                viewDef.Hidden = false;

                                viewDef.Type = BuiltInViewType.Gantt;
                                viewDef.Query = string.Empty;

                                viewDef.Fields = new Collection<string>
                                {
                                    "LinkTitle",
                                    "StartDate",
                                    "DueDate",
                                    "PercentComplete",
                                    "Predecessors",
                                    "AssignedTo",
                                    "GUID"
                                };

                                viewDef.ViewData = @"<FieldRef Name=""Title"" Type=""GanttTitle"" />
<FieldRef Name=""StartDate"" Type=""GanttStartDate"" />
<FieldRef Name=""DueDate"" Type=""GanttEndDate"" />
<FieldRef Name=""PercentComplete"" Type=""GanttPercentComplete"" />
<FieldRef Name=""Predecessors"" Type=""GanttPredecessors"" />
<FieldRef Name=""ParentID"" Type=""HierarchyParentID"" />
<FieldRef Name=""DueDate"" Type=""TimelineDueDate"" />";
                            });
                        });
                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListsViews.Types")]
        public void CanDeploy_ListView_AsDatasheetView()
        {
            TestRandomDefinition<ListViewDefinition>(def =>
            {
                def.Fields = new Collection<string>
                {
                    BuiltInInternalFieldNames.ID,
                    BuiltInInternalFieldNames.Title
                };

                def.Hidden = false;
                def.Type = BuiltInViewType.Grid;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListsViews.Types")]
        public void CanDeploy_ListView_AsRecurrence()
        {
            throw new SPMeta2NotImplementedException();

            //TestRandomDefinition<ListViewDefinition>(def =>
            //{
            //    def.Hidden = false;
            //    def.Type = BuiltInViewType.Recurrence;
            //});
        }

        #endregion

        #region scopes

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListsViews.Scope")]
        public void CanDeploy_ListView_Scope_As_Default()
        {
            TestRandomDefinition<ListViewDefinition>(def =>
            {
                def.Scope = BuiltInViewScope.Default;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListsViews.Scope")]
        public void CanDeploy_ListView_Scope_As_FilesOnly()
        {
            TestRandomDefinition<ListViewDefinition>(def =>
            {
                def.Scope = BuiltInViewScope.FilesOnly;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListsViews.Scope")]
        public void CanDeploy_ListView_Scope_As_Recursive()
        {
            TestRandomDefinition<ListViewDefinition>(def =>
            {
                def.Scope = BuiltInViewScope.Recursive;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListsViews.Scope")]
        public void CanDeploy_ListView_Scope_As_RecursiveAll()
        {
            TestRandomDefinition<ListViewDefinition>(def =>
            {
                def.Scope = BuiltInViewScope.RecursiveAll;
            });
        }

        #endregion

        #region default

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListsViews.Folders")]
        public void CanDeploy_ListView_InFolderOfContentType()
        {
            TestRandomDefinition<ListViewDefinition>(def =>
            {
                def.ContentTypeId = BuiltInContentTypeId.Folder;
                def.DefaultViewForContentType = false;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListsViews.Folders")]
        public void CanDeploy_ListView_InTopLevelFolder()
        {
            TestRandomDefinition<ListViewDefinition>(def =>
            {
                def.ContentTypeId = BuiltInContentTypeId.RootOfList;
                def.DefaultViewForContentType = true;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListsViews")]
        public void CanDeploy_ListView_AsDefaultForContentType()
        {
            TestRandomDefinition<ListViewDefinition>(def =>
            {
                def.DefaultViewForContentType = true;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListsViews")]
        public void CanDeploy_ListView_AsDefaultForContentType_ById()
        {
            TestRandomDefinition<ListViewDefinition>(def =>
            {
                def.DefaultViewForContentType = true;
                def.ContentTypeId = BuiltInContentTypeId.Item;
            });
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.ListsViews")]
        public void CanDeploy_ListView_AsDefaultForContentType_ByName()
        {
            TestRandomDefinition<ListViewDefinition>(def =>
            {
                def.DefaultViewForContentType = true;
                def.ContentTypeName = "Item";
            });
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.ListsViews")]
        public void CanDeploy_ListView_WithCustomUrl()
        {
            TestRandomDefinition<ListViewDefinition>(def =>
            {
                def.Title = Rnd.String();
                def.Url = string.Format("{0}.aspx", Rnd.String());
            });
        }

        #endregion

        #region localization

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListsViews.Localization")]
        public void CanDeploy_Localized_ListView()
        {
            var definition = GetLocalizedDefinition();
            var subWebDefinition = GetLocalizedDefinition();

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomList(list =>
                {
                    list.AddListView(definition);
                });

                web.AddRandomWeb(subWeb =>
                {
                    subWeb.AddRandomList(list =>
                    {
                        list.AddListView(subWebDefinition);
                    });
                });
            });

            TestModel(model);
        }

        #endregion

        #region utils

        protected ListViewDefinition GetLocalizedDefinition()
        {
            var definition = ModelGeneratorService.GetRandomDefinition<ListViewDefinition>();
            var localeIds = Rnd.LocaleIds();

            foreach (var localeId in localeIds)
            {
                definition.TitleResource.Add(new ValueForUICulture
                {
                    CultureId = localeId,
                    Value = string.Format("LocalizedTitle_{0}", localeId)
                });
            }

            return definition;
        }

        #endregion
    }
}
