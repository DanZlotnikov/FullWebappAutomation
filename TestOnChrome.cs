﻿using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using static FullWebappAutomation.GlobalSettings;
using static FullWebappAutomation.HelperFunctions;
using static FullWebappAutomation.Tests;

namespace FullWebappAutomation
{
    class TestOnChrome
    {
        public static RemoteWebDriver webappDriver, backofficeDriver;

        public static void SetUp()
        {

            DesiredCapabilities capability = DesiredCapabilities.Chrome();
            webappDriver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), capability, TimeSpan.FromSeconds(600));
            backofficeDriver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), capability, TimeSpan.FromSeconds(600));

            GlobalSettings.InitLogFiles();
        }

        public static void TearDown()
        {
            webappDriver.Quit();
            backofficeDriver.Quit();
            WriteToFinalizedPerformanceLog();
            System.Diagnostics.Process.Start(successLogFilePath);
        }

        /// <summary>
        /// Runs test cases
        /// </summary>
        public static void TestSuite(string chosenUsername, Dictionary<string, bool> testsToRun)
        {
            username = chosenUsername;
            password = GetUserPassword(username);

            Webapp_Sandbox_Login(webappDriver, username, password);
            Backoffice.GeneralActions.SandboxLogin(backofficeDriver, username, password);

            if(testsToRun["Resync"])
            {
                Delegator delegatedFunction = Webapp_Sandbox_Resync;
                BasicTestWrapper(delegatedFunction, webappDriver, backofficeDriver);
            }

            if (testsToRun["Config Home Button"])
            {
                Delegator delegatedFunction = Webapp_Sandbox_Config_Home_Button;
                BasicTestWrapper(delegatedFunction, webappDriver, backofficeDriver);
            }

            if (testsToRun["Config App Buttons"])
            {
                Delegator delegatedFunction = Webapp_Sandbox_Config_App_Buttons;
                BasicTestWrapper(delegatedFunction, webappDriver, backofficeDriver);
            }

            if (testsToRun["Sales Order"])
            {
                Delegator delegatedFunction = Webapp_Sandbox_Sales_Order;
                BasicTestWrapper(delegatedFunction, webappDriver, backofficeDriver);
            }

            if (testsToRun["Item Search"])
            {
                Delegator delegatedFunction = Webapp_Sandbox_Item_Search;
                BasicTestWrapper(delegatedFunction, webappDriver, backofficeDriver);
            }

            if (testsToRun["Minimum Quantity"])
            {
                Delegator delegatedFunction = Webapp_Sandbox_Minimum_Quantity;
                BasicTestWrapper(delegatedFunction, webappDriver, backofficeDriver);
            }

            if (testsToRun["Delete Cart Item"])
            {
                Delegator delegatedFunction = Webapp_Sandbox_Delete_Cart_Item;
                BasicTestWrapper(delegatedFunction, webappDriver, backofficeDriver);
            }

            if (testsToRun["Unit Price Discount"])
            {
                Delegator delegatedFunction = Webapp_Sandbox_Unit_Price_Discount;
                BasicTestWrapper(delegatedFunction, webappDriver, backofficeDriver);
            }

            if (testsToRun["Continue Ordering"])
            {
                Delegator delegatedFunction = Webapp_Sandbox_Continue_Ordering;
                BasicTestWrapper(delegatedFunction, webappDriver, backofficeDriver);
            }

            if (testsToRun["Duplicate Line Item"])
            {
                Delegator delegatedFunction = Webapp_Sandbox_Duplicate_Line_Item;
                BasicTestWrapper(delegatedFunction, webappDriver, backofficeDriver);
            }

            if (testsToRun["Inventory Alert"])
            {
                Delegator delegatedFunction = Webapp_Sandbox_Inventory_Alert;
                BasicTestWrapper(delegatedFunction, webappDriver, backofficeDriver);
            }

            if (testsToRun["Search Activity"])
            {
                Delegator delegatedFunction = Webapp_Sandbox_Search_Activity;
                BasicTestWrapper(delegatedFunction, webappDriver, backofficeDriver);
            }

            if (testsToRun["Delete Activity"])
            {
                Delegator delegatedFunction = Webapp_Sandbox_Delete_Activity;
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
