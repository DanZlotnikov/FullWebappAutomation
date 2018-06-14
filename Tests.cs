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
        public static void WebappSandboxLogin(RemoteWebDriver webappDriver, string username, string password)
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
                WriteToSuccessLog("WebappSandboxLogin", testSuccess, error);
            }

        }

        public static void WebappSandboxSalesOrder(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
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
            orderInfo["orderRemark"] = "Automation" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

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

            Webapp_Sandbox_Check_Sales_Order(webappDriver, orderInfo);
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
    }
}
