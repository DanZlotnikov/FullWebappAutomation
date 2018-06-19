using OpenQA.Selenium.Remote;
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

            if (testsToRun["All Backoffice Menus"])
            {
                Backoffice.GeneralActions.SandboxLogin(backofficeDriver, username, password);

                Backoffice.CompanyProfile.App_Home_Screen(backofficeDriver);
                Backoffice.CompanyProfile.Branding(backofficeDriver);
                Backoffice.CompanyProfile.Company_Profile(backofficeDriver);
                Backoffice.CompanyProfile.Email_Settings(backofficeDriver);
                Backoffice.CompanyProfile.Home_Screen_Shortcut(backofficeDriver);
                Backoffice.CompanyProfile.Security(backofficeDriver);
                Backoffice.CompanyProfile.Sync_Settings(backofficeDriver);

                Backoffice.Catalogs.Manage_Catalogs(backofficeDriver);
                Backoffice.Catalogs.Edit_Form(backofficeDriver);
                Backoffice.Catalogs.Catalog_Views(backofficeDriver);
                Backoffice.Catalogs.Fields(backofficeDriver);

                Backoffice.Items.Order_Center_Thumbnail_Views(backofficeDriver);
                Backoffice.Items.Order_Center_Grid_View(backofficeDriver);
                Backoffice.Items.Order_Center_Matrix_View(backofficeDriver);
                Backoffice.Items.Order_Center_Flat_Matrix_View(backofficeDriver);
                Backoffice.Items.Order_Center_Item_Details_View(backofficeDriver);
                Backoffice.Items.Catalog_Item_View(backofficeDriver);
                Backoffice.Items.Item_Share_Email_Info(backofficeDriver);
                Backoffice.Items.Smart_Search(backofficeDriver);
                Backoffice.Items.Filters(backofficeDriver);
                Backoffice.Items.Automated_Image_Uploader(backofficeDriver);
                Backoffice.Items.Fields(backofficeDriver);

                Backoffice.Accounts.Views_And_Forms(backofficeDriver);
                Backoffice.Accounts.Accounts_Lists(backofficeDriver);
                Backoffice.Accounts.Accounts_Lists_New(backofficeDriver);
                Backoffice.Accounts.Map_View(backofficeDriver);
                Backoffice.Accounts.Card_Layout(backofficeDriver);
                Backoffice.Accounts.Account_Dashboard_Layout(backofficeDriver);
                Backoffice.Accounts.Search(backofficeDriver);
                Backoffice.Accounts.Smart_Search(backofficeDriver);
                Backoffice.Accounts.Fields(backofficeDriver);

                Backoffice.PricingPolicy.Pricing_Policy(backofficeDriver);
                Backoffice.PricingPolicy.Price_Level(backofficeDriver);
                Backoffice.PricingPolicy.Main_Category_Discount(backofficeDriver);
                Backoffice.PricingPolicy.Account_Special_Price_List(backofficeDriver);

                Backoffice.Users.Manage_Users(backofficeDriver);
                Backoffice.Users.Role_Heirarchy(backofficeDriver);
                Backoffice.Users.Profiles(backofficeDriver);
                Backoffice.Users.User_Lists(backofficeDriver);
                Backoffice.Users.Targets_Type(backofficeDriver);
                Backoffice.Users.Manage_Targets(backofficeDriver);
                Backoffice.Users.Rep_Dashboard_Add_Ons(backofficeDriver);

                Backoffice.Contacts.Contact_Lists(backofficeDriver);

                Backoffice.SalesActivities.Transaction_Types(backofficeDriver);
                Backoffice.SalesActivities.Activity_Types(backofficeDriver);
                Backoffice.SalesActivities.Sales_Activity_Lists(backofficeDriver);
                Backoffice.SalesActivities.Activity_List_Display_Options(backofficeDriver);
                Backoffice.SalesActivities.Activities_And_Menu_Setup(backofficeDriver);
                Backoffice.SalesActivities.Sales_Dashboard_Settings(backofficeDriver);

                Backoffice.ActivityPlanning.Account_Lists(backofficeDriver);
                Backoffice.ActivityPlanning.Activity_Planning_Display_Options(backofficeDriver);

                Backoffice.ERPIntegration.Plugin_Settings(backofficeDriver);
                Backoffice.ERPIntegration.Configuration(backofficeDriver);
                Backoffice.ERPIntegration.File_Uploads_And_Logs(backofficeDriver);

                Backoffice.ConfigurationFiles.Automated_Reports(backofficeDriver);
                Backoffice.ConfigurationFiles.Configuration_Files(backofficeDriver);
                Backoffice.ConfigurationFiles.Translation_Files(backofficeDriver);
                Backoffice.ConfigurationFiles.Online_Add_Ons(backofficeDriver);
                Backoffice.ConfigurationFiles.User_Defined_Tables(backofficeDriver);
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
