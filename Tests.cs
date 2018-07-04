using OpenQA.Selenium.Remote;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using static FullWebappAutomation.GlobalSettings;
using static FullWebappAutomation.HelperFunctions;
using static FullWebappAutomation.Consts;
using System.Threading;


namespace FullWebappAutomation
{
    class Tests
    {
        public static void All_Backoffice_Menus(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
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

        public static void Webapp_Sandbox_Login(RemoteWebDriver webappDriver, string username, string password)
        {
            Exception error = null;
            bool testSuccess = true;

            try
            {
                // Login page
                webappDriver.Navigate().GoToUrl(webappSandboxLoginPageUrl);
                webappDriver.Manage().Window.Maximize();

                // Input credentials
                SafeSendKeys(webappDriver, "//input[@type='email']", username);
                SafeSendKeys(webappDriver, "//input[@type='password']", password);

                // Login button
                SafeClick(webappDriver, "//button[@type='submit']");
            }
            catch (Exception e)
            {
                error = e;
                testSuccess = false;
            }
            finally
            {
                WriteToSuccessLog("Webapp_Sandbox_Login", testSuccess, error);
            }
        }

        public static void Webapp_Sandbox_Resync(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            webappDriver.Navigate().GoToUrl(webappSandboxHomePageUrl);

            // Add "/rec" to url
            if (!(webappDriver.Url.Contains("/rec")))
                webappDriver.Navigate().GoToUrl(webappSandboxHomePageUrl + "/rec");

            // Click "Resync"
            SafeClick(webappDriver, "/html/body/app-root/div/app-home-page/app-user-helper/div/nav/div/div/ul/li[2]/a");

            // Start Resyncing
            SafeClick(webappDriver, "/html/body/app-root/div/app-home-page/app-user-helper/div/nav/div/div/ul/li[2]/ul/li/a");
            Thread.Sleep(3000);
        }

        // Checked with "Sales Order"
        public static void Webapp_Sandbox_Sales_Order(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            // Dictionary to store order info to assert later
            Dictionary<string, string> orderInfo = new Dictionary<string, string>();

            GetToOrderCenter_SalesOrder(webappDriver);

            // Small view
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/span");
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/ul/li");
            Thread.Sleep(bufferTime);

            // Item qty plus
            SafeClick(webappDriver,
               "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/mat-grid-list/div/mat-grid-tile[8]/figure/app-custom-field-generator/app-custom-quantity-selector/div/span[2]/i");

            Thread.Sleep(bufferTime);

            // Get units qty from qty selector
            orderInfo["unitsQty"] = SafeGetValue(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/mat-grid-list/div/mat-grid-tile[8]/figure/app-custom-field-generator/app-custom-quantity-selector/div/input", "innerHTML");

            // Cart
            SafeClick(webappDriver, "//button[@id='goToCartBtn']/span");

            // Small view
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/span");
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/ul/li[2]");
            Thread.Sleep(bufferTime);

            // Transaction Menu
            SafeClick(webappDriver, "//div[@id='containerActions']/ul/li/a/i");

            // Order details
            SafeClick(webappDriver, "//div[@id='containerActions']/ul/li/ul/li/span");

            // Create remark and store it
            orderInfo["orderRemark"] = "Automation " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            // Click remark field
            SafeClick(webappDriver, "//div[@id='orderDetailsContainer']/app-custom-form/fieldset/div[15]/div/app-custom-field-generator/app-custom-textbox/div/input");

            // Add remark
            SafeSendKeys(webappDriver, "//div[@id='orderDetailsContainer']/app-custom-form/fieldset/div[15]/div/app-custom-field-generator/app-custom-textbox/div/input", orderInfo["orderRemark"]);

            // Save button
            SafeClick(webappDriver, "//body/app-root/div/app-order-details/app-bread-crumbs/div/div/div/div[3]/div[2]");

            // Submit
            SafeClick(webappDriver, "//button[@id='btnTransition']/span");

            // Home button
            SafeClick(webappDriver, "//a[@id='btnMenuHome']/span");

            try
            {
                SafeClick(webappDriver, "//div[@class='btn allButtons btnOk grnbtn ng-star-inserted']");
                Webapp_Sandbox_Resync(webappDriver, backofficeDriver);
            }
            catch { }

            Webapp_Sandbox_Check_Sales_Order(webappDriver, orderInfo);
            Backoffice_Sandbox_Check_Sales_Order(backofficeDriver, orderInfo);
        }

        /// <summary>
        /// Check if sales order appears correctly in activities list
        /// </summary>
        /// <param name="webappDriver"></param>
        /// <param name="orderInfo"></param>
        public static void Webapp_Sandbox_Check_Sales_Order(RemoteWebDriver webappDriver, Dictionary<string, string> orderInfo)
        {
            // Activities
            SafeClick(webappDriver, "//div[@id='mainCont']/app-home-page/footer/div/div[2]/div[2]/div");

            // Get remark and assert correctly - first 10 activities
            bool remarkMatch = false;
            int orderIndexInList = 0;
            string actualRemark;

            for (int i = 1; i < 12; i++)
            {
                actualRemark = SafeGetValue(webappDriver, string.Format("(//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/div/app-custom-field-generator/app-custom-button/a/span)[{0}]", i), "innerHTML").ToString();

                if (actualRemark == orderInfo["orderRemark"])
                {
                    remarkMatch = true;
                    orderIndexInList = i;
                }
            }

            Assert(remarkMatch, "No sales order with matching remark in activity list");

            // Get actual activity ID from web page and compare with API data
            string actualActivityID = SafeGetValue(webappDriver, string.Format("(//label[@id='WrntyID'])[{0}]", orderIndexInList), "innerHTML").ToString();

            // Get Activity ID from API
            var apiData = GetApiData(username, password, "transactions", "Remark", orderInfo["orderRemark"]);
            string apiActivityID = apiData[0].InternalID.ToString();

            Assert(actualActivityID == apiActivityID, "Activity ID doesn't match API data");

            // Check correct Units Qty 
            SafeClick(webappDriver, string.Format("(//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/div/app-custom-field-generator/app-custom-button/a/span)[{0}]", orderIndexInList));
            string actualUnitsQty = SafeGetValue(webappDriver, "//input[@id='UnitsQuantity']", "innerHTML");

            Assert(actualUnitsQty == orderInfo["unitsQty"], "Actual units Qty doesn't match ordered Qty");
        }

        /// <summary>
        /// Check if sales order arrives correctly to Backoffice
        /// </summary>
        /// <param name="webappDriver"></param>
        /// <param name="orderInfo"></param>
        public static void Backoffice_Sandbox_Check_Sales_Order(RemoteWebDriver backofficeDriver, Dictionary<string, string> orderInfo)
        {
            backofficeDriver.Navigate().GoToUrl(backofficeSandboxHomePageUrl);
            SafeClick(backofficeDriver, "(//a[contains(text(),'Activities')])[2]");

            bool remarkMatch = false;
            int orderIndexInList = 0;
            string actualRemark;

            for (int i = 0; i < 30; i++)
            {
                try
                {
                    actualRemark = SafeGetValue(backofficeDriver, string.Format("//table[@class='ll_tbl']/tbody/tr[{0}]/td[2]/a", i), "title");
                    if (actualRemark == orderInfo["orderRemark"])
                    {
                        remarkMatch = true;
                        orderIndexInList = i;
                        break;
                    }
                }

                catch { }
            }

            Assert(remarkMatch, "No sales order with matching remark in activity list");

            // Get activity ID from BO
            string actualActivityID = SafeGetValue(backofficeDriver, string.Format("//table[@class='ll_tbl']/tbody/tr[{0}]/td[3]/a", orderIndexInList), "title");

            // Get activity ID from API
            var apiData = GetApiData(username, password, "transactions", "Remark", orderInfo["orderRemark"]);
            string apiActivityID = apiData[0].InternalID.ToString();

            Assert(apiActivityID == actualActivityID, "BO activity id doesn't match sql data");
        }

        public static void Webapp_Sandbox_Config_Home_Button(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            bool homeButtonMatch = false;
            try
            {
                // Change home button in Backoffice
                Backoffice_Sandbox_Config_Home_Button(backofficeDriver);

                Webapp_Sandbox_Resync(webappDriver, backofficeDriver);
                string actualHomeButton = SafeGetValue(webappDriver, "//div[@id='mainButton']", "innerHTML").ToString();
                homeButtonMatch = actualHomeButton == "Visit";

            }
            catch { }

            finally
            {
                // Revert Home button to sales order
                Backoffice_Sandbox_Revert_Config_Home_Button(backofficeDriver);

                Assert(homeButtonMatch, "Home button not matching Backoffice configuration");
            }
        }

        /// <summary>
        /// Change home button to visit
        /// </summary>
        /// <param name="backofficeDriver"></param>
        public static void Backoffice_Sandbox_Config_Home_Button(RemoteWebDriver backofficeDriver)
        {
            backofficeDriver.Navigate().GoToUrl(backofficeSandboxHomePageUrl);

            Backoffice.CompanyProfile.Home_Screen_Shortcut(backofficeDriver);

            // Edit Rep
            SafeClick(backofficeDriver, "//div[@id='formContTemplate']/div/div[2]/div/span[2]");

            // Delete "Sales Order" button
            SafeClick(backofficeDriver, "//body/form/div[6]/div/div[4]/div[2]/div/div/div[3]/div[4]/div/ul/li/div/div/span[4]");

            // Click "Activities" basket
            SafeClick(backofficeDriver, "//body/form/div[6]/div/div[4]/div[2]/div/div/div[3]/div[2]/ul/div/div/div");

            for (int i = 1; i < 10; i++)
            {
                try
                {
                    // Find the "Visit" button and click it
                    if (SafeGetValue(backofficeDriver, string.Format("//body/form/div[6]/div/div[4]/div[2]/div/div/div[3]/div[2]/ul/div/ul/li[{0}]/div/span[2]/span", i), "innerHTML").ToString().Trim() == "(Visit)")
                    {
                        SafeClick(backofficeDriver, string.Format("//body/form/div[6]/div/div[4]/div[2]/div/div/div[3]/div[2]/ul/div/ul/li[{0}]/div/div", i));
                        break;
                    }
                }

                catch { }
            }

            // Save button
            SafeClick(backofficeDriver, "//div[@id='formContTemplate']/div[4]/div/div");
        }

        /// <summary>
        /// Revert home button to sales order
        /// </summary>
        /// <param name="backofficeDriver"></param>
        public static void Backoffice_Sandbox_Revert_Config_Home_Button(RemoteWebDriver backofficeDriver)
        {
            backofficeDriver.Navigate().GoToUrl(backofficeSandboxHomePageUrl);
            Backoffice.CompanyProfile.Home_Screen_Shortcut(backofficeDriver);

            // Edit Rep
            SafeClick(backofficeDriver, "//div[@id='formContTemplate']/div/div[2]/div/span[2]");

            // Delete "Visit" button
            SafeClick(backofficeDriver, "//body/form/div[6]/div/div[4]/div[2]/div/div/div[3]/div[4]/div/ul/li/div/div/span[4]");

            // Click "Activities" basket
            SafeClick(backofficeDriver, "//body/form/div[6]/div/div[4]/div[2]/div/div/div[3]/div[2]/ul/div/div/span");

            for (int i = 1; i < 10; i++)
            {
                try
                {
                    // Find the "Sales Order" button and click it
                    if (SafeGetValue(backofficeDriver, string.Format("//body/form/div[6]/div/div[4]/div[2]/div/div/div[3]/div[2]/ul/div/ul/li[{0}]/div/span[2]/span", i), "innerHTML").ToString().Trim() == "(Sales Order)")
                    {
                        SafeClick(backofficeDriver, string.Format("//body/form/div[6]/div/div[4]/div[2]/div/div/div[3]/div[2]/ul/div/ul/li[{0}]/div/div", i));
                        break;
                    }
                }
                catch { continue; }
            }

            // Save button
            SafeClick(backofficeDriver, "//div[@id='formContTemplate']/div[4]/div/div");
        }

        public static void Webapp_Sandbox_Config_App_Buttons(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            bool visitButtonFlag = false, photoButtonFlag = false;
            string currentButton;
            try
            {
                // Config app buttons in Backoffice
                Backoffice_Sandbox_Config_App_Buttons(backofficeDriver);

                Webapp_Sandbox_Resync(webappDriver, backofficeDriver);

                // Search for visit and photo buttons in home screen
                for (int i = 1; i < 10; i++)
                {
                    if (photoButtonFlag && visitButtonFlag)
                        break;

                    currentButton = SafeGetValue(webappDriver, string.Format("//div[@id='mainCont']/app-home-page/footer/div/div[2]/div[{0}]/div", i), "title");
                    if (currentButton == "Visit")
                        visitButtonFlag = true;

                    if (currentButton == "Photo")
                        photoButtonFlag = true;
                }
            }
            catch { }

            finally
            {
                Backoffice_Sandbox_Revert_Config_App_Buttons(backofficeDriver);
                Assert(visitButtonFlag && photoButtonFlag, "App button configuration not working properly");
            }
        }

        /// <summary>
        /// Configure app buttons
        /// </summary>
        /// <param name="backofficeDriver"></param>
        public static void Backoffice_Sandbox_Config_App_Buttons(RemoteWebDriver backofficeDriver)
        {
            backofficeDriver.Navigate().GoToUrl(backofficeSandboxHomePageUrl);
            Backoffice.CompanyProfile.App_Home_Screen(backofficeDriver);

            // Edit Rep
            SafeClick(backofficeDriver, "//div[@id='formContTemplate']/div/div[2]/div/span[2]");

            // Click "Activities" basket
            SafeClick(backofficeDriver, "//body/form/div[6]/div/div[4]/div[2]/div/div/div[3]/div[2]/ul/div/div/div");

            bool visitAdded = false, photoAdded = false;

            for (int i = 1; i < 10; i++)
            {
                if (visitAdded && photoAdded)
                    break;
                try
                {
                    // Find the "Visit" button and click it
                    if (SafeGetValue(backofficeDriver, string.Format("//body/form/div[6]/div/div[4]/div[2]/div/div/div[3]/div[2]/ul/div/ul/li[{0}]/div/span[2]/span", i), "innerHTML").ToString().Trim() == "(Visit)")
                    {
                        visitAdded = true;
                        SafeClick(backofficeDriver, string.Format("//body/form/div[6]/div/div[4]/div[2]/div/div/div[3]/div[2]/ul/div/ul/li[{0}]/div/div", i));
                    }

                    // Find the "Photo" button and click it
                    if (SafeGetValue(backofficeDriver, string.Format("//body/form/div[6]/div/div[4]/div[2]/div/div/div[3]/div[2]/ul/div/ul/li[{0}]/div/span[2]/span", i), "innerHTML").ToString().Trim() == "(Photo)")
                    {
                        photoAdded = true;
                        SafeClick(backofficeDriver, string.Format("//body/form/div[6]/div/div[4]/div[2]/div/div/div[3]/div[2]/ul/div/ul/li[{0}]/div/div", i));
                    }
                }

                catch { }
            }

            // Save button
            SafeClick(backofficeDriver, "//div[@id='formContTemplate']/div[4]/div/div");
        }

        /// <summary>
        /// Revert app buttons back to original
        /// </summary>
        /// <param name="backofficeDriver"></param>
        public static void Backoffice_Sandbox_Revert_Config_App_Buttons(RemoteWebDriver backofficeDriver)
        {
            bool visitDeleted = false, photoDeleted = false;
            
            backofficeDriver.Navigate().GoToUrl(backofficeSandboxHomePageUrl);
            Backoffice.CompanyProfile.App_Home_Screen(backofficeDriver);

            // Edit Rep
            SafeClick(backofficeDriver, "//div[@id='formContTemplate']/div/div[2]/div/span[2]");

            for (int i = 1; i < 10; i++)
            {
                if (visitDeleted && photoDeleted)
                    break;
                try
                {
                    // Find the "Visit" button and delete it
                    if (SafeGetValue(backofficeDriver, string.Format("//body/form/div[6]/div/div[4]/div[2]/div/div/div[3]/div[4]/div/ul/li[{0}]/div/div/div", i), "title").ToString().Trim() == "Visit")
                    {
                        SafeClick(backofficeDriver, string.Format("//body/form/div[6]/div/div[4]/div[2]/div/div/div[3]/div[4]/div/ul/li[{0}]/div/div/span[4]", i));
                        visitDeleted = true;
                    }

                    // Find the "Photo" button and delete it
                    if (SafeGetValue(backofficeDriver, string.Format("//body/form/div[6]/div/div[4]/div[2]/div/div/div[3]/div[4]/div/ul/li[{0}]/div/div/div", i), "title").ToString().Trim() == "Photo")
                    {
                        SafeClick(backofficeDriver, string.Format("//body/form/div[6]/div/div[4]/div[2]/div/div/div[3]/div[4]/div/ul/li[{0}]/div/div/span[4]", i));
                        photoDeleted = true;
                    }
                }
                catch { }
            }

            // Save button
            SafeClick(backofficeDriver, "//div[@id='formContTemplate']/div[4]/div/div");
        }

        // Checked with "Sales Order"
        public static void Webapp_Sandbox_Item_Search(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            GetToOrderCenter_SalesOrder(webappDriver);

            // Small view
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/span");
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/ul/li");
            Thread.Sleep(bufferTime);

            // Get item name from webpage
            string itemName = SafeGetValue(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/mat-grid-list/div/mat-grid-tile[6]/figure/app-custom-field-generator/app-custom-textbox/label/span", "innerHTML");

            // Get item ID from API
            var apiData = GetApiData(username, password, "items", "Name", itemName);
            string apiItemCode = apiData[0].ExternalID.ToString();

            // Click search icon
            SafeClick(webappDriver, "//body/app-root/div/app-order-center/div/app-bread-crumbs/div/div/div/span/i");

            // Input search parameter
            SafeSendKeys(webappDriver, "//body/app-root/div/app-order-center/div/app-bread-crumbs/div/div/div/div[5]/div/div/input", apiItemCode);

            // Click search button
            SafeClick(webappDriver, "//body/app-root/div/app-order-center/div/app-bread-crumbs/div/div/div/div[5]/div/div/span[2]");

            Thread.Sleep(bufferTime);

            // Get item code from webpage
            string actualItemName = SafeGetValue(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/mat-grid-list/div/mat-grid-tile[6]/figure/app-custom-field-generator/app-custom-textbox/label/span", "innerHTML");

            Assert(itemName == actualItemName, "Actual item doesn't match expected");
        }

        // Checked with "Sales Order"
        public static void Webapp_Sandbox_Minimum_Quantity(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            GetToOrderCenter_SalesOrder(webappDriver);

            // Small view
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/span");
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/ul/li");
            Thread.Sleep(bufferTime);

            // Get item ID from webpage
            string itemID = SafeGetValue(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/mat-grid-list/div/mat-grid-tile[7]/figure/app-custom-field-generator/app-custom-textbox/label/span", "innerHTML");

            // Get item data from api
            var apiData = GetApiData(username, password, "items", "ExternalID", itemID);

            // Parse the data to integer and store it in variable - min qty
            Int32.TryParse(apiData[0].MinimumQuantity.ToString(), out int apiItemMinQty);

            Assert(apiItemMinQty > 1, "Item Qty is 1, unable to perform test");

            // Item Qty selector field
            SafeClick(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/mat-grid-list/div/mat-grid-tile[8]/figure/app-custom-field-generator/app-custom-quantity-selector/div/input");

            // Insert less than min qty into qty selector
            SafeSendKeys(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/mat-grid-list/div/mat-grid-tile[8]/figure/app-custom-field-generator/app-custom-quantity-selector/div/input", (apiItemMinQty - 1).ToString());

            // Click outside the box
            SafeClick(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]");

            Thread.Sleep(bufferTime);

            // Check qty selector style - supposed to be red (alerted)
            string qtySelectorStyle = SafeGetValue(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/mat-grid-list/div/mat-grid-tile[8]/figure/app-custom-field-generator/app-custom-quantity-selector/div/input", "style");

            Assert(qtySelectorStyle.Contains("color: rgb(255, 0, 0);"), "Min qty doesn't mark in red");
        }

        // Checked with "Sales Order"
        public static void Webapp_Sandbox_Delete_Cart_Item(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            GetToOrderCenter_SalesOrder(webappDriver);

            // Small view
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/span");
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/ul/li");
            Thread.Sleep(bufferTime);

            // First item qty plus
            SafeClick(webappDriver,
                "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/mat-grid-list/div/mat-grid-tile[8]/figure/app-custom-field-generator/app-custom-quantity-selector/div/span[2]/i");

            // Second item qty plus
            SafeClick(webappDriver,
                "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div[2]/app-custom-form/fieldset/mat-grid-list/div/mat-grid-tile[8]/figure/app-custom-field-generator/app-custom-quantity-selector/div/span[2]/i");

            // Cart
            SafeClick(webappDriver, "//button[@id='goToCartBtn']/span");

            // Small view
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/span");
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/ul/li");
            Thread.Sleep(bufferTime);

            // Click minus button on first item
            SafeClick(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/mat-grid-list/div/mat-grid-tile[5]/figure/app-custom-field-generator/app-custom-quantity-selector/div/span/i");

            // Click "Delete" alert
            SafeClick(webappDriver, "//div[@type='button'][2]");

            // Click qty selector on remaining item
            SafeClick(webappDriver, "//input[@id='UnitsQuantity']");

            // Insert 0 into qty selector
            SafeSendKeys(webappDriver, "//input[@id='UnitsQuantity']", Keys.Backspace);
            SafeSendKeys(webappDriver, "//input[@id='UnitsQuantity']", "0");

            // Press enter
            SafeSendKeys(webappDriver, "//input[@id='UnitsQuantity']", Keys.Enter);

            // Click "Delete" alert
            SafeClick(webappDriver, "//div[@type='button'][2]");

            // Find the "Items not found" message
            string notFoundMessage = SafeGetValue(webappDriver, "//div[@class='no-data ng-star-inserted']", "innerHTML");

            Assert(notFoundMessage == "Items not found", "Delete action didn't work properly");
        }

        // Checked with "Sales Order 2"
        public static void Webapp_Sandbox_Unit_Price_Discount(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            GetToOrderCenter_SalesOrder2(webappDriver);

            // Small view
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/span");
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/ul/li");
            Thread.Sleep(bufferTime);

            // First item qty plus
            SafeClick(webappDriver,
                "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/mat-grid-list/div/mat-grid-tile[5]/figure/app-custom-field-generator/app-custom-quantity-selector/div/span[2]/i");

            // Cart
            SafeClick(webappDriver, "//button[@id='goToCartBtn']/span");

            // Small view
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/span");
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/ul/li");
            Thread.Sleep(bufferTime);

            // Get unit price
            string unitPriceStr = SafeGetValue(webappDriver, "//label[@id='UnitPrice']", "innerHTML");
            unitPriceStr = unitPriceStr.Trim(new Char[] { ' ', '$' });
            double.TryParse(unitPriceStr, out double unitPrice);

            // Click on unit discount
            SafeClick(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/div[7]/app-custom-field-generator/app-custom-textbox/label");

            // Input 50% discount
            SafeClear(webappDriver, "//input[@id='UnitDiscountPercentage']");
            SafeSendKeys(webappDriver, "//input[@id='UnitDiscountPercentage']", "50");
            SafeSendKeys(webappDriver, "//input[@id='UnitDiscountPercentage']", Keys.Enter);
            Thread.Sleep(bufferTime);

            // Get price after discount
            string priceAfterDiscountStr = SafeGetValue(webappDriver, "//input[@id='UnitPriceAfterDiscount']", "title");
            priceAfterDiscountStr = priceAfterDiscountStr.Trim(new Char[] { '$' });
            double.TryParse(priceAfterDiscountStr, out double priceAfterDiscount);

            Assert(priceAfterDiscount == unitPrice / 2, "Price after discount miscalculated");

            double unitPriceAfterDiscount = 3;

            // Click on price after discount
            SafeClick(webappDriver, "//input[@id='UnitPriceAfterDiscount']");

            // Input price after discount
            SafeClear(webappDriver, "//input[@id='UnitPriceAfterDiscount']");
            SafeSendKeys(webappDriver, "//input[@id='UnitPriceAfterDiscount']", unitPriceAfterDiscount.ToString());
            SafeSendKeys(webappDriver, "//input[@id='UnitPriceAfterDiscount']", Keys.Enter);
            Thread.Sleep(bufferTime);

            // Click on unit discount
            SafeClick(webappDriver, "//input[@id='UnitDiscountPercentage']");

            // Get unit discount
            string discountStr = SafeGetValue(webappDriver, "//input[@id='UnitDiscountPercentage']", "title");
            discountStr = discountStr.Trim(new char[] { '%' });
            double.TryParse(discountStr, out double discount);

            Assert(discount == (((unitPrice - unitPriceAfterDiscount) / unitPrice) * 100), "Discount miscalculated");
        }

        // Checked with "Sales Order" 
        public static void Webapp_Sandbox_Continue_Ordering(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            // Dictionary to store order info to assert later
            Dictionary<string, string> orderInfo = new Dictionary<string, string>();

            GetToOrderCenter_SalesOrder(webappDriver);

            // Small view
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/span");
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/ul/li");
            Thread.Sleep(bufferTime);

            // Item qty plus
            SafeClick(webappDriver,
               "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/mat-grid-list/div/mat-grid-tile[8]/figure/app-custom-field-generator/app-custom-quantity-selector/div/span[2]/i");

            Thread.Sleep(bufferTime);

            // Get units qty from qty selector
            orderInfo["unitsQty"] = SafeGetValue(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/mat-grid-list/div/mat-grid-tile[8]/figure/app-custom-field-generator/app-custom-quantity-selector/div/input", "innerHTML");

            // Cart
            SafeClick(webappDriver, "//button[@id='goToCartBtn']/span");

            // Small view
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/span");
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/ul/li");
            Thread.Sleep(bufferTime);

            // Transaction Menu
            SafeClick(webappDriver, "//div[@id='containerActions']/ul/li/a/i");

            // Order details
            SafeClick(webappDriver, "//div[@id='containerActions']/ul/li/ul/li/span");

            // Create remark and store it
            orderInfo["orderRemark"] = "Automation " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            // Click remark field
            SafeClick(webappDriver, "//div[@id='orderDetailsContainer']/app-custom-form/fieldset/div[15]/div/app-custom-field-generator/app-custom-textbox/div/input");

            // Add remark
            SafeSendKeys(webappDriver, "//div[@id='orderDetailsContainer']/app-custom-form/fieldset/div[15]/div/app-custom-field-generator/app-custom-textbox/div/input", orderInfo["orderRemark"]);

            // Save button
            SafeClick(webappDriver, "//body/app-root/div/app-order-details/app-bread-crumbs/div/div/div/div[3]/div[2]");

            // Home button
            SafeClick(webappDriver, "//a[@id='btnMenuHome']/span");

            webappDriver.Navigate().GoToUrl(webappSandboxHomePageUrl);

            // Activities
            SafeClick(webappDriver, "//div[@id='mainCont']/app-home-page/footer/div/div[2]/div[2]/div");

            //  Find the sales order in activity list
            string actualRemark;
            int orderIndexInList = 0;

            for (int i = 1; i < 12; i++)
            {
                actualRemark = SafeGetValue(webappDriver, string.Format("(//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/div/app-custom-field-generator/app-custom-button/a/span)[{0}]", i), "innerHTML").ToString();

                if (actualRemark == orderInfo["orderRemark"])
                {
                    orderIndexInList = i;
                }
            }

            // Drill down into the sales order 
            SafeClick(webappDriver, string.Format("(//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/div/app-custom-field-generator/app-custom-button/a/span)[{0}]", orderIndexInList));

            // Continue ordering
            SafeClick(webappDriver, "(//button[@type='button'])[2]");

            // Item qty plus
            SafeClick(webappDriver,
               "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/mat-grid-list/div/mat-grid-tile[8]/figure/app-custom-field-generator/app-custom-quantity-selector/div/span[2]/i");

            // Cart
            SafeClick(webappDriver, "//button[@id='goToCartBtn']/span");

            // Submit
            SafeClick(webappDriver, "//button[@id='btnTransition']/span");
        }

        // Checked with "Sales Order"
        public static void Webapp_Sandbox_Duplicate_Line_Item(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            GetToOrderCenter_SalesOrder(webappDriver);

            // Small view
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/span");
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/ul/li");
            Thread.Sleep(bufferTime);

            // First item qty plus
            SafeClick(webappDriver,
                "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/mat-grid-list/div/mat-grid-tile[8]/figure/app-custom-field-generator/app-custom-quantity-selector/div/span[2]/i");

            // Cart
            SafeClick(webappDriver, "//button[@id='goToCartBtn']/span");

            // GridLine view
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/span");
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/ul/li[2]");
            Thread.Sleep(bufferTime);

            // Select item
            SafeClick(webappDriver, "(//input[@type='checkbox'])[2]");

            // Edit
            SafeClick(webappDriver, "//li[@id='editButton']/a/i");

            // Duplicate
            SafeClick(webappDriver, "//li[@id='dropdownActionsDuplicate']/span");

            // Duplicate popup button
            SafeClick(webappDriver, "(//div[@type='button'])[2]");

            string item1 = SafeGetValue(webappDriver, "//body/app-root/div/app-cart/div/div/div/div/app-custom-list/virtual-scroll/div/div/app-custom-form/fieldset/div[4]/app-custom-field-generator/app-custom-textbox/label", "title");
            string item2 = SafeGetValue(webappDriver, "//body/app-root/div/app-cart/div/div/div/div/app-custom-list/virtual-scroll/div/div[2]/app-custom-form/fieldset/div[4]/app-custom-field-generator/app-custom-textbox/label", "title");

            Assert(item1 == item2, "Duplicate action failed");
        }

        // Checked with "Sales Order 2"
        public static void Webapp_Sandbox_Inventory_Alert(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            GetToOrderCenter_SalesOrder2(webappDriver);

            // Small view
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/span");
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/ul/li");
            Thread.Sleep(bufferTime);

            // Get item Name from webpage
            string itemID = SafeGetValue(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/mat-grid-list/div/mat-grid-tile[3]/figure/app-custom-field-generator/app-custom-textbox/label/span", "innerHTML");

            // Get item api data
            var apiData = GetApiData(username, password, "items", "Name", itemID);

            // Parse out InternalID
            string InternalID = apiData[0].InternalID.ToString();

            // Get item inverntory data from api
            apiData = GetApiData(username, password, "inventory", "ItemInternalID", InternalID);

            // Parse out InStockQuantity
            Int32.TryParse(apiData[0].InStockQuantity.ToString(), out int inStockQuantity);

            // Item Qty selector field
            SafeClick(webappDriver, "//input[@id='UnitsQuantity']");

            // Insert less than min qty into qty selector
            SafeSendKeys(webappDriver, "//input[@id='UnitsQuantity']", (inStockQuantity + 20).ToString());

            // Click outside the box
            SafeClick(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]");

            Thread.Sleep(bufferTime);

            string quantityStr = SafeGetValue(webappDriver, "//input[@id='UnitsQuantity']", "title");

            double quantity = Double.Parse(quantityStr);

            Assert(quantity == (double)inStockQuantity, "Inverntory auto-set failed");
        }

        public static void Webapp_Sandbox_Search_Activity(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            // Activities
            SafeClick(webappDriver, "//div[@id='mainCont']/app-home-page/footer/div/div[2]/div[2]/div");

            // Get first activity ID and remark
            string id = SafeGetValue(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/div[2]/app-custom-field-generator/app-custom-textbox/label", "title").ToString();
            string remark = SafeGetValue(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/div/app-custom-field-generator/app-custom-button/a/span", "title").ToString();

            // Click search icon, then search box
            SafeClick(webappDriver, "//li[@id='btnSearch']/a/span");
            SafeClick(webappDriver, "//li[@id='btnSearch']/div/input");

            // Insert ID into search bar and click search
            SafeSendKeys(webappDriver, "//li[@id='btnSearch']/div/input", id);
            SafeClick(webappDriver, "//li[@id='btnSearch']/a/span");

            Thread.Sleep(3000);

            // Get found item's ID and remark
            string foundID = SafeGetValue(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/div[2]/app-custom-field-generator/app-custom-textbox/label", "title").ToString();
            string foundRemark = SafeGetValue(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/div/app-custom-field-generator/app-custom-button/a/span", "title").ToString();

            Assert((foundID == id) && (foundRemark == remark), "Activity search failed");
        }

        public static void Webapp_Sandbox_Delete_Activity(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            // Activities
            SafeClick(webappDriver, "//div[@id='mainCont']/app-home-page/footer/div/div[2]/div[2]/div");

            // Get first activity ID and remark
            string id = SafeGetValue(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/div[2]/app-custom-field-generator/app-custom-textbox/label", "title").ToString();

            // Select first activity
            SafeClick(webappDriver, "(//input[@type='checkbox'])[2]");

            // Click edit pencil 
            SafeClick(webappDriver, "//span[@id='pencilId']");

            // Click "delete"
            SafeClick(webappDriver, "//li[@id='dropdownActionsDelete']/span");

            // "Continue" on popup
            SafeClick(webappDriver, "(//div[@type='button'])[2]");
            Thread.Sleep(7000);

            string foundID = SafeGetValue(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/div[2]/app-custom-field-generator/app-custom-textbox/label", "title").ToString();

            Assert(id != foundID, "Delete activity action failed");
        }

        public static void Webapp_Sandbox_Account_Search_Activity(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            // Accounts
            SafeClick(webappDriver, "//div[@id='mainCont']/app-home-page/footer/div/div[2]/div/div");

            // First account
            SafeClick(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div[1]/app-custom-form/fieldset/div/app-custom-field-generator/app-custom-button/a/span");

            // Get first activity id
            string id = SafeGetValue(webappDriver, " //body/app-root/div/app-accounts-home-page/div/div[2]/div/div/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/div[2]/app-custom-field-generator/app-custom-textbox/label", "title");

            // Get remark
            string remark = SafeGetValue(webappDriver, " //body/app-root/div/app-accounts-home-page/div/div[2]/div/div/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/div/app-custom-field-generator/app-custom-button/a/span", "title");

            // Click search button
            SafeClick(webappDriver, "//li[@id='btnSearch']/a/span");

            // Input id
            SafeClick(webappDriver, "//li[@id='btnSearch']/div/input");
            SafeSendKeys(webappDriver, "//li[@id='btnSearch']/div/input", id);

            // Click search button
            SafeClick(webappDriver, "//li[@id='btnSearch']/a/span");
            Thread.Sleep(bufferTime);

            // Assert activity found (same remark and id)
            string foundId = SafeGetValue(webappDriver, "//body/app-root/div/app-accounts-home-page/div/div[2]/div/div/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/div[2]/app-custom-field-generator/app-custom-textbox/label", "title");
            string foundRemark = SafeGetValue(webappDriver, "//body/app-root/div/app-accounts-home-page/div/div[2]/div/div/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/div/app-custom-field-generator/app-custom-button/a/span", "title");

            Assert(id == foundId && remark == foundRemark, "Account activity search failed");
        }

        public static void Webapp_Sandbox_Account_Drill_Down(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            // Accounts
            SafeClick(webappDriver, "//div[@id='mainCont']/app-home-page/footer/div/div[2]/div/div");

            // Get first account's name and drill down into it
            string name = SafeGetValue(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div[1]/app-custom-form/fieldset/div/app-custom-field-generator/app-custom-button/a/span", "title");
            SafeClick(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div[1]/app-custom-form/fieldset/div/app-custom-field-generator/app-custom-button/a/span");

            // Get account name from top of page and assert
            string nameInside = SafeGetValue(webappDriver, "//div[@id='accountsHomePageCont']/acc-details/div/div/div/label", "innerHTML");

            Assert(name == nameInside, "Account drill down failed");
        }

        public static void Webapp_Sandbox_Enter_To_Activity(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            // Activities
            SafeClick(webappDriver, "//div[@id='mainCont']/app-home-page/footer/div/div[2]/div[2]/div");

            string remark = "";
            string type = "";
            string id = "";
            int i = 0;

            // Get activity IDs and remarks until you find one with a remark 
            while (remark == "" || (type != "Sales Order" && type != "Sales Order  2"))
            {
                try
                {
                    i++;
                    remark = SafeGetValue(webappDriver, string.Format("//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div[{0}]/app-custom-form/fieldset/div/app-custom-field-generator/app-custom-button/a/span", i), "title", safeWait: 100).ToString();
                    id = SafeGetValue(webappDriver, string.Format("//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div[{0}]/app-custom-form/fieldset/div[2]/app-custom-field-generator/app-custom-textbox/label", i), "title", safeWait: 100).ToString();
                    type = SafeGetValue(webappDriver, string.Format("(//label[@id='Type'])[{0}]", i), "title", safeWait: 50);
                }
                catch { break; }
            }

            // Drill down to the chosen activity
            SafeClick(webappDriver, string.Format("//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div[{0}]/app-custom-form/fieldset/div/app-custom-field-generator/app-custom-button/a/span", i));

            Thread.Sleep(bufferTime);
            string continueOrderingButton = SafeGetValue(webappDriver, "(//button[@type='button'])[2]", "innerHTML");

            Assert(continueOrderingButton == "Continue ordering", "Enter to activity failed (couldn't find continue ordering button)");
        }

        public static void Webapp_Sandbox_Account_Activity_Drilldown(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            // Accounts
            SafeClick(webappDriver, "//div[@id='mainCont']/app-home-page/footer/div/div[2]/div/div");

            // Drill down into first account
            SafeClick(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div[1]/app-custom-form/fieldset/div/app-custom-field-generator/app-custom-button/a/span");

            string remark = "";
            string type = "";
            string id = "";
            int i = 0;

            // Get activity IDs and remarks until you find one with a remark 
            while (remark == "" || (type != "Sales Order" && type != "Sales Order  2"))
            {
                try
                {
                    i++;
                    remark = SafeGetValue(webappDriver, string.Format("//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div[{0}]/app-custom-form/fieldset/div/app-custom-field-generator/app-custom-button/a/span", i), "title").ToString();
                    id = SafeGetValue(webappDriver, string.Format("//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div[{0}]/app-custom-form/fieldset/div[2]/app-custom-field-generator/app-custom-textbox/label", i), "title").ToString();
                    type = SafeGetValue(webappDriver, string.Format("(//label[@id='Type'])[{0}]", i), "title");
                }
                catch { break; }
            }

            // Drill down to the chosen activity
            SafeClick(webappDriver, string.Format("//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div[{0}]/app-custom-form/fieldset/div/app-custom-field-generator/app-custom-button/a/span", i));

            Thread.Sleep(bufferTime);
            string continueOrderingButton = SafeGetValue(webappDriver, "(//button[@type='button'])[2]", "innerHTML");

            Assert(continueOrderingButton == "Continue ordering", "Account activity drilldown failed (couldn't find continue ordering button)");
        }

        public static void Webapp_Sandbox_Breadcrumbs_Navigation(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            GetToOrderCenter_SalesOrder(webappDriver);

            // Item info
            SafeClick(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/mat-grid-list/div/mat-grid-tile/figure/app-custom-field-generator/app-custom-image/span/i");

            // App back button
            SafeClick(webappDriver, "//div[@id='header']/div/div");
        
            // Get item name from webpage
            string itemName = SafeGetValue(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/mat-grid-list/div/mat-grid-tile[6]/figure/app-custom-field-generator/app-custom-textbox/label/span", "innerHTML");
        }

        public static void Webapp_Sandbox_Duplicate_Transaction(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            GetToOrderCenter_SalesOrder2(webappDriver);

            // Small view
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/span");
            SafeClick(webappDriver, "//div[@id='header']/div/div[4]/ul/li/ul/li");
            Thread.Sleep(bufferTime);

            // First item qty plus
            SafeClick(webappDriver,
                "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/mat-grid-list/div/mat-grid-tile[5]/figure/app-custom-field-generator/app-custom-quantity-selector/div/span[2]/i");
            Thread.Sleep(bufferTime);

            // Get units qty from qty selector
            string originalQty = SafeGetValue(webappDriver, "//input[@id='UnitsQuantity']", "title");
            Console.WriteLine(originalQty);

            // Cart
            SafeClick(webappDriver, "//button[@id='goToCartBtn']/span");

            // Transaction Menu
            SafeClick(webappDriver, "//div[@id='containerActions']/ul/li/a/i");

            // Order details
            SafeClick(webappDriver, "//div[@id='containerActions']/ul/li/ul/li/span");

            // Create remark and store it
            string originalRemark = "Automation " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            // Click remark field
            SafeClick(webappDriver, "//div[@id='orderDetailsContainer']/app-custom-form/fieldset/div[15]/div/app-custom-field-generator/app-custom-textbox/div/input");

            // Add remark
            SafeSendKeys(webappDriver, "//div[@id='orderDetailsContainer']/app-custom-form/fieldset/div[15]/div/app-custom-field-generator/app-custom-textbox/div/input", originalRemark);

            // Save button
            SafeClick(webappDriver, "//body/app-root/div/app-order-details/app-bread-crumbs/div/div/div/div[3]/div[2]");

            // Transaction Menu
            SafeClick(webappDriver, "//div[@id='containerActions']/ul/li/a/i");

            // Duplicate transaction
            SafeClick(webappDriver, "//div[@id='containerActions']/ul/li/ul/li[3]/span");
            Thread.Sleep(bufferTime);

            // Get units qty from qty selector
            string duplicatedQty = SafeGetValue(webappDriver, "//input[@id='UnitsQuantity']", "title");
            Console.WriteLine(duplicatedQty);
            // Transaction Menu
            SafeClick(webappDriver, "//div[@id='containerActions']/ul/li/a/i");

            // Order details
            SafeClick(webappDriver, "//div[@id='containerActions']/ul/li/ul/li/span");

            // Get remark from form
            string duplicatedRemark = SafeGetValue(webappDriver, "//div[@id='orderDetailsContainer']/app-custom-form/fieldset/div[15]/div/app-custom-field-generator/app-custom-textbox/div/input", "title");
            
            Assert(Double.TryParse(duplicatedQty, out double result) == Double.TryParse(originalQty, out double result2) && duplicatedRemark == originalRemark, "Duplicated data doesn't match original");
        }

        public static void Webapp_Sandbox_Search_Account(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            // Accounts
            SafeClick(webappDriver, "//div[@id='mainCont']/app-home-page/footer/div/div[2]/div/div");

            // Get name of last account
            string name = SafeGetValue(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div[4]/app-custom-form/fieldset/div/app-custom-field-generator/app-custom-button/a/span", "title").ToString();

            // Click search button
            SafeClick(webappDriver, "//li[@id='btnSearch']/a/span");

            // Input name
            SafeClick(webappDriver, "//li[@id='btnSearch']/div/input");
            SafeSendKeys(webappDriver, "//li[@id='btnSearch']/div/input", name);

            // Click search button
            SafeClick(webappDriver, "//li[@id='btnSearch']/a/span");
            Thread.Sleep(bufferTime);

            string foundName = SafeGetValue(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div[1]/app-custom-form/fieldset/div/app-custom-field-generator/app-custom-button/a/span", "title").ToString();

            Assert(name == foundName, "Account search failed (found name doesn't match expected)");
        }
    }
}
