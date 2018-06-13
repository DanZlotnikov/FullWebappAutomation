using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using static FullWebappAutomation.HelperFunctions;
using static FullWebappAutomation.Tests;

namespace FullWebappAutomation
{
    class TestOnChrome
    {
        public static RemoteWebDriver webappDriver, backofficeDriver;

        /// <summary>
        ///  Sets up testing requirements
        /// </summary>
        public static void SetUp()
        {

            DesiredCapabilities capability = DesiredCapabilities.Chrome();
            webappDriver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), capability, TimeSpan.FromSeconds(600));
            backofficeDriver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), capability, TimeSpan.FromSeconds(600));

            GlobalSettings.InitLogFiles();
        }

        /// <summary>
        /// Tears down testing environment
        /// </summary>
        public static void TearDown()
        {
            webappDriver.Quit();
            backofficeDriver.Quit();
            WriteToFinalizedPerformanceLog();
        }

        /// <summary>
        /// Runs test cases
        /// </summary>
        public static void TestSuite(string username, Dictionary<string, bool> testsToRun)
        {
            if (testsToRun["Login"])
                WebappSandboxLogin(webappDriver, username, GetUserPassword(username));

            if (testsToRun["Sales Order"])
            {
                Delegator delegatedFunction = WebappSandboxSalesOrder;
                BasicTestWrapper(delegatedFunction, webappDriver, backofficeDriver);
            }
        }

        public static void RunTests(string chosenUsername, Dictionary<string, bool> testsToRun)
        {
            SetUp();
            TestSuite(chosenUsername, testsToRun);
            TearDown();
        }

    }
}
