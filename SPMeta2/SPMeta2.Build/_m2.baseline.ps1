
# Build baseline is a quality gate for both build and NuGet packaging process
# https://github.com/SubPointSolutions/spmeta2/issues/832

# both scripts check assemblies while building and packaging to meet the baseline criterias
# the nees is due to potential mistakes in the build-packaging workflow which passes incorrect assemblies to the NuGet

$g_buildBaseline = @{

	Assemblies = @(
		
        #region SPMeta2.dll
		@{
            AssemblyFileName = "SPMeta2.dll";
			Runtime = "14";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

		@{
            AssemblyFileName = "SPMeta2.dll";
			Runtime = "15";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

		@{
            AssemblyFileName = "SPMeta2.dll";
			Runtime = "16";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

		@{
            AssemblyFileName = "SPMeta2.dll";
			Runtime = "365";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

        #end region

        # SPMeta2.Standard
        @{
            AssemblyFileName = "SPMeta2.Standard.dll";
			Runtime = "14";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

        @{
            AssemblyFileName = "SPMeta2.Standard.dll";
			Runtime = "15";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

        @{
            AssemblyFileName = "SPMeta2.Standard.dll";
			Runtime = "16";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},
        
        @{
            AssemblyFileName = "SPMeta2.Standard.dll";
			Runtime = "365";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

        # SPMeta2.SSOM.dll
		@{
            AssemblyFileName = "SPMeta2.SSOM.dll";
			Runtime = "14";
            
			ExcludedHandlers = @(
			    "SPMeta2.SSOM.ModelHandlers.ComposedLookItemLinkModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a",
                
                "SPMeta2.SSOM.ModelHandlers.AppModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.ModelHandlers.AppPrincipalModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                
                "SPMeta2.SSOM.ModelHandlers.Fields.GeolocationFieldModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.ModelHandlers.Fields.OutcomeChoiceFieldModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                
                "SPMeta2.SSOM.ModelHandlers.InformationRightsManagementSettingsModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.ModelHandlers.ComposedLookItemModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                
                "SPMeta2.SSOM.ModelHandlers.SP2013WorkflowDefinitionHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.ModelHandlers.SP2013WorkflowSubscriptionDefinitionModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                
                "SPMeta2.SSOM.ModelHandlers.Webparts.BlogLinksWebPartModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.ModelHandlers.Webparts.ClientWebPartModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.ModelHandlers.Webparts.GettingStartedWebPartModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.ModelHandlers.Webparts.PictureLibrarySlideshowWebPartModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.ModelHandlers.Webparts.SPTimelineWebPartModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.ModelHandlers.Webparts.ScriptEditorWebPartModelHandler, SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
			);

			ExcludedDefinitions =  @(
			
			);
		},
        
        @{
            AssemblyFileName = "SPMeta2.SSOM.dll";
			Runtime = "15";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

        @{
            AssemblyFileName = "SPMeta2.SSOM.dll";
			Runtime = "16";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

        # SPMeta2.SSOM.Standard.dll
		@{
            AssemblyFileName = "SPMeta2.SSOM.Standard.dll";
			Runtime = "14";
            
			ExcludedHandlers = @(
                "SPMeta2.SSOM.Standard.ModelHandlers.ManagedPropertyModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.Standard.ModelHandlers.ImageRenditionModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                
                "SPMeta2.SSOM.Standard.ModelHandlers.SearchResultModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.Standard.ModelHandlers.SearchConfigurationModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.SSOM.Standard.ModelHandlers.WebNavigationSettingsModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                
                "SPMeta2.SSOM.Standard.ModelHandlers.Webparts.CommunityAdminWebPartModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.Standard.ModelHandlers.Webparts.CommunityJoinWebPartModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.Standard.ModelHandlers.Webparts.ContentBySearchWebPartModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.SSOM.Standard.ModelHandlers.Webparts.MyMembershipWebPartModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.Standard.ModelHandlers.Webparts.RefinementScriptWebPartModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.Standard.ModelHandlers.Webparts.ResultScriptWebPartModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.SSOM.Standard.ModelHandlers.Webparts.SearchBoxScriptWebPartModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.SSOM.Standard.ModelHandlers.Webparts.SearchNavigationWebPartModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                
                "SPMeta2.SSOM.Standard.ModelHandlers.Webparts.SiteFeedWebPartModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.SSOM.Standard.ModelHandlers.Webparts.ProjectSummaryWebPartModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
				"SPMeta2.SSOM.Standard.ModelHandlers.DesignPackageModelHandler, SPMeta2.SSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
			);

			ExcludedDefinitions =  @(
			
			);
		},

        @{
            AssemblyFileName = "SPMeta2.SSOM.Standard.dll";
			Runtime = "15";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

        @{
            AssemblyFileName = "SPMeta2.SSOM.Standard.dll";
			Runtime = "16";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

        # SPMeta2.CSOM.dll
        @{
            AssemblyFileName = "SPMeta2.CSOM.dll";
			Runtime = "14";
            
			ExcludedHandlers = @(
			    "SPMeta2.CSOM.ModelHandlers.AppModelHandler, SPMeta2.CSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.CSOM.ModelHandlers.AppPrincipalModelHandler, SPMeta2.CSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.CSOM.ModelHandlers.ClearRecycleBinModelHandler, SPMeta2.CSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.CSOM.ModelHandlers.ComposedLookItemLinkModelHandler, SPMeta2.CSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.CSOM.ModelHandlers.EventReceiverModelHandler, SPMeta2.CSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.CSOM.ModelHandlers.Fields.OutcomeChoiceFieldModelHandler, SPMeta2.CSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.CSOM.ModelHandlers.Fields.GeolocationFieldModelHandler, SPMeta2.CSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.CSOM.ModelHandlers.InformationRightsManagementSettingsModelHandler, SPMeta2.CSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.CSOM.ModelHandlers.RegionalSettingsModelHandler, SPMeta2.CSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.CSOM.ModelHandlers.SP2013WorkflowSubscriptionDefinitionModelHandler, SPMeta2.CSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.CSOM.ModelHandlers.SupportedUICultureModelHandler, SPMeta2.CSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.CSOM.ModelHandlers.SP2013WorkflowDefinitionHandler, SPMeta2.CSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
			);

			ExcludedDefinitions =  @(
			
			);
		},

        @{
            AssemblyFileName = "SPMeta2.CSOM.dll";
			Runtime = "15";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

        
        @{
            AssemblyFileName = "SPMeta2.CSOM.dll";
			Runtime = "16";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

        
        @{
            AssemblyFileName = "SPMeta2.CSOM.dll";
			Runtime = "365";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

        # SPMeta2.CSOM.Standard.dll
        @{
            AssemblyFileName = "SPMeta2.CSOM.Standard.dll";
			Runtime = "14";
            
			ExcludedHandlers = @(
			    "SPMeta2.CSOM.Standard.ModelHandlers.Fields.TaxonomyFieldModelHandler, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                
                "SPMeta2.CSOM.Standard.ModelHandlers.ImageRenditionModelHandler, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.CSOM.Standard.ModelHandlers.PageLayoutAndSiteTemplateSettingsModelHandler, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.CSOM.Standard.ModelHandlers.SearchConfigurationModelHandler, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                
                "SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy.TaxonomyGroupModelHandler, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy.TaxonomyTermLabelModelHandler, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy.TaxonomyTermModelHandler, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy.TaxonomyTermSetModelHandler, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy.TaxonomyTermStoreModelHandler, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.CSOM.Standard.ModelHandlers.WebNavigationSettingsModelHandler, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.CSOM.Standard.ModelHandlers.TermGroupModelHost, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.CSOM.Standard.ModelHandlers.TermModelHost, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.CSOM.Standard.ModelHandlers.TermSetModelHost, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
                "SPMeta2.CSOM.Standard.ModelHandlers.TermStoreModelHost, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.CSOM.Standard.ModelHandlers.SandboxSolutionModelHandler, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"
				"SPMeta2.CSOM.Standard.ModelHandlers.DocumentSetModelHandler, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

                "SPMeta2.CSOM.Standard.ModelHandlers.DesignPackageModelHandler, SPMeta2.CSOM.Standard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a"

			);

			ExcludedDefinitions =  @(
			
			);
		},

         @{
            AssemblyFileName = "SPMeta2.CSOM.Standard.dll";
			Runtime = "15";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

        
        @{
            AssemblyFileName = "SPMeta2.CSOM.Standard.dll";
			Runtime = "16";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		},

        
        @{
            AssemblyFileName = "SPMeta2.CSOM.Standard.dll";
			Runtime = "365";
            
			ExcludedHandlers = @(
			
			);

			ExcludedDefinitions =  @(
			
			);
		}
	)
}