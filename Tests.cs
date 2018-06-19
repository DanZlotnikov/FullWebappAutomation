using OpenQA.Selenium.Remote;
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

            // Resync
            SafeClick(webappDriver, "/html/body/app-root/div/app-home-page/app-user-helper/div/nav/div/div/ul/li[2]/a");

            // Start Resyncing
            SafeClick(webappDriver, "/html/body/app-root/div/app-home-page/app-user-helper/div/nav/div/div/ul/li[2]/ul/li/a");
            Thread.Sleep(3000);
        }


        public static void Webapp_Sandbox_Sales_Order(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            // Dictionary to store order info to assert later
            Dictionary<string, string> orderInfo = new Dictionary<string, string>();

            GetToOrderCenter(webappDriver);

            // Item qty plus
            SafeClick(webappDriver,
               "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/mat-grid-list/div/mat-grid-tile[8]/figure/app-custom-field-generator/app-custom-quantity-selector/div/span[2]/i");

            Thread.Sleep(bufferTime);

            // Get units qty from qty selector
            orderInfo["unitsQty"] = SafeGetValue(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/mat-grid-list/div/mat-grid-tile[8]/figure/app-custom-field-generator/app-custom-quantity-selector/div/input", "innerHTML");

            // Cart
            SafeClick(webappDriver, "//button[@id='goToCartBtn']/span");

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
            webappDriver.Navigate().GoToUrl(webappSandboxHomePageUrl);

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


        public static void Webapp_Sandbox_Item_Search(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            GetToOrderCenter(webappDriver);

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


        public static void Webapp_Sandbox_Minimum_Quantity(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            GetToOrderCenter(webappDriver);

            // Get item ID from webpage
            string itemID = SafeGetValue(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div/app-custom-form/fieldset/mat-grid-list/div/mat-grid-tile[7]/figure/app-custom-field-generator/app-custom-textbox/label/span", "innerHTML");

            // Get item's min qty
            var apiData = GetApiData(username, password, "items", "ExternalID", itemID);

            // Parse the data to integer and store it in variable
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
    }
}
