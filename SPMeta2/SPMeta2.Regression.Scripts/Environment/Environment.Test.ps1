. .\Environment.ps1

TestFixture "SPMeta2.Regression" {

	TestSetup {
			
		$g_ssomWebAppUrl = ""
		$g_ssomSiteUrl = ""


		$g_o365SiteUrl = ""
		$g_o365UserName = ""
		$g_o365UserPassword	= ""
	}

	TestCase "Setup SSOM Environment" {
		
		$runnerAssemblies = @()
		$runnerAssemblies += "SPMeta2.Regression.Runners.SSOM.dll"

		SPM2_SetRegressionEnvironment $runnerAssemblies $g_ssomWebAppUrl $g_ssomSiteUrl $g_o365SiteUrl $g_o365UserName $g_o365UserPassword
	}

	TestCase "Setup O365 Environment" {
		
		$runnerAssemblies = @()
		$runnerAssemblies += "SPMeta2.Regression.Runners.O365.dll"

		SPM2_SetRegressionEnvironment 
	}

	TestCase "Setup SSOM/O365 Environment" {
		
		$runnerAssemblies = @()

		$runnerAssemblies += "SPMeta2.Regression.Runners.O365.dll"
		$runnerAssemblies += "SPMeta2.Regression.Runners.SSOM.dll"

		SPM2_SetRegressionEnvironment $runnerAssemblies $g_ssomWebAppUrl $g_ssomSiteUrl $g_o365SiteUrl $g_o365UserName $g_o365UserPassword
	}

}