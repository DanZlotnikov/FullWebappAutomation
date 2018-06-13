﻿using OpenQA.Selenium.Remote;
using System;
using static FullWebappAutomation.HelperFunctions;
using static FullWebappAutomation.Consts;

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
            GetToOrderCenter(webappDriver);
        }
    }
}
