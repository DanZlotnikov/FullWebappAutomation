﻿using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.IO;
using System.Net;
using System.Threading;
using static FullWebappAutomation.Consts;
using static FullWebappAutomation.GlobalSettings;
using System.Diagnostics;

namespace FullWebappAutomation
{
    class HelperFunctions
    {
        /// <summary>
        /// Asserts a condition and throws AssertionException if false
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        public static void Assert(bool condition, string message)
        {
            if (condition)
                return;

            if (message != null)
            {
                throw new AssertionException(message);
            }

            else throw new AssertionException("Unknown assertion error");
        }

        /// <summary>
        /// Delegate a function into an object
        /// </summary>
        /// <param name="webappDriver"></param>
        /// <param name="backofficeDriver"></param>
        public delegate void Delegator(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver);

        /// <summary>
        /// Wraps tests with a try-catch-except logic that creates logs
        /// </summary>
        /// <param name="delegatedFunction"></param>
        /// <param name="remoteWebDriver"></param> 
        public static void BasicTestWrapper(Delegator delegatedFunction, RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            string functionName = delegatedFunction.Method.Name.ToString();

            Exception error = null;
            bool testSuccess = true;

            try
            {
                webappDriver.Navigate().GoToUrl(webappSandboxHomePageUrl);
                delegatedFunction(webappDriver, backofficeDriver);
            }
            catch (Exception e)
            {
                error = e;
                testSuccess = false;
            }
            finally
            {
                WriteToSuccessLog(functionName, testSuccess, error);
            }
        }

        /// <summary>
        /// Gets from home page to cart(Sales Order > Store1 > Default Catalog)
        /// </summary>
        /// <param name="webappDriver"></param>
        public static void GetToOrderCenter_SalesOrder(RemoteWebDriver webappDriver)
        {
            webappDriver.Navigate().GoToUrl(webappSandboxHomePageUrl);

            // Accounts
            SafeClick(webappDriver, "//div[@id='mainCont']/app-home-page/footer/div/div[2]/div/div");

            // First account
            SafeClick(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div[1]/app-custom-form/fieldset/div/app-custom-field-generator/app-custom-button/a/span");


            Thread.Sleep(5000);

            // Plus button
            SafeClick(webappDriver, "//div[@id='actionBar']/div/ul[3]/li/a/span");

            // Sales Order
            SafeClick(webappDriver, "//div[@id='actionBar']/div/ul[3]/li/ul/li/span");

            /*      
            // Origin account
            SafeClick(webappDriver, "//body/app-root/div/app-accounts-home-page/object-chooser-modal/div/div/div/div/div/div/app-custom-list/virtual-scroll/div/div[2]/app-custom-form/fieldset/div");

            //Done
            SafeClick(driver, "//div[@id='mainCont']/app-accounts-home-page/object-chooser-modal/div/div/div/div[3]/div[2]");
            */

            // Default Catalog
            SafeClick(webappDriver, "//div[@id='container']/div[2]");
        }

        public static void GetToOrderCenter_SalesOrder2(RemoteWebDriver webappDriver)
        {
            webappDriver.Navigate().GoToUrl(webappSandboxHomePageUrl);

            // Accounts
            SafeClick(webappDriver, "//div[@id='mainCont']/app-home-page/footer/div/div[2]/div/div");

            // First account
            SafeClick(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div[1]/app-custom-form/fieldset/div/app-custom-field-generator/app-custom-button/a/span");


            Thread.Sleep(5000);

            // Plus button
            SafeClick(webappDriver, "//div[@id='actionBar']/div/ul[3]/li/a/span");

            // Sales Order 2
            SafeClick(webappDriver, "//div[@id='actionBar']/div/ul[3]/li/ul/li[2]/span");

            /*      
            // Origin account
            SafeClick(webappDriver, "//body/app-root/div/app-accounts-home-page/object-chooser-modal/div/div/div/div/div/div/app-custom-list/virtual-scroll/div/div[2]/app-custom-form/fieldset/div");

            //Done
            SafeClick(driver, "//div[@id='mainCont']/app-accounts-home-page/object-chooser-modal/div/div/div/div[3]/div[2]");
            */

            // Default Catalog
            SafeClick(webappDriver, "//div[@id='container']/div[2]");
        }

        /// <summary>
        /// Clicks an element in the web page. Uses retry logic for a specified amount of tries. One second buffer between tries.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="elementXPath"></param>
        public static void SafeClick(RemoteWebDriver driver, string elementXPath)
        {
            IWebElement element;

            int retryCount = 1;
            while (retryCount < maxRetryCount)
            {
                try
                {
                    element = driver.FindElementByXPath(elementXPath);
                    Highlight(driver, element);
                    element.Click();

                    break;
                }
                catch (Exception e)
                {
                    Thread.Sleep(1000);
                    retryCount++;
                    continue;
                }
            }

            // If succeeded, write to performance log
            if (retryCount < maxRetryCount)
            {
                // Get caller test function name
                StackTrace stackTrace = new StackTrace();
                string testName = stackTrace.GetFrame(1).GetMethod().Name;

                WriteToPerformanceLog(testName, "SafeClick", retryCount);
                return;
            }

            // Otherwise throw execption
            else
            {
                string errorMessage = string.Format("Click action failed for element at XPath: {0}", elementXPath);
                RetryException error = new RetryException(errorMessage);
                throw error;

            }
        }

        /// <summary>
        /// Sends keys to an element in the web page. Uses retry logic for a specified amount of tries. One second buffer between tries.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="elementXPath"></param>
        /// <param name="KeysToSend"></param>
        public static void SafeSendKeys(RemoteWebDriver driver, string elementXPath, string KeysToSend)
        {
            IWebElement element;

            int retryCount = 1;
            while (retryCount < maxRetryCount)
            {
                try
                {
                    element = driver.FindElementByXPath(elementXPath);
                    Highlight(driver, element);
                    element.SendKeys(KeysToSend);

                    return;
                }
                catch (Exception e)
                {
                    Thread.Sleep(1000);
                    retryCount++;
                    continue;
                }
            }

            string errorMessage = string.Format("SendKeys action failed for element at XPath: {0}", elementXPath);
            RetryException error = new RetryException(errorMessage);
            throw error;
        }

        /// <summary>
        /// Clears an element in the web page. Uses retry logic.
        /// </summary>
        /// <param name="driver"></param>
        public static void SafeClear(RemoteWebDriver driver, string elementXPath)
        {
            IWebElement element;

            int retryCount = 1;
            while (retryCount < maxRetryCount)
            {
                try
                {
                    element = driver.FindElementByXPath(elementXPath);
                    Highlight(driver, element);
                    element.Clear();
                    return;
                }
                catch (Exception e)
                {
                    Thread.Sleep(1000);
                    retryCount++;
                    continue;
                }
            }

            string errorMessage = string.Format("SendKeys action failed for element at XPath: {0}", elementXPath);
            RetryException error = new RetryException(errorMessage);
            throw error;
        }

        /// <summary>
        /// Gets value from an element in the web page. Uses retry logic for a specified amount of tries. One second buffer between tries.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="elementXPath"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static dynamic SafeGetValue(RemoteWebDriver driver, string elementXPath, string attribute)
        {
            IWebElement element;

            int retryCount = 1;
            while (retryCount < maxRetryCount)
            {
                try
                {
                    element = driver.FindElementByXPath(elementXPath);
                    var value = element.GetAttribute(attribute);
                    Highlight(driver, element);
                    return value;
                }
                catch (Exception e)
                {
                    Thread.Sleep(1000);
                    retryCount++;
                    continue;
                }
            }

            string errorMessage = string.Format("GetValue action failed for element at XPath: {0}", elementXPath);
            RetryException error = new RetryException(errorMessage);
            throw error;
        }

        /// <summary>
        /// Highlights an element in the webpage
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="element"></param>
        public static void Highlight(IWebDriver driver, IWebElement element)
        {
            // Highlight element
            var jsDriver = (IJavaScriptExecutor)driver;
            string highlightJavascript = @"arguments[0].style.cssText = ""background: yellow; border-width: 2px; border-style: solid; border-color: red"";";
            jsDriver.ExecuteScript(highlightJavascript, new object[] { element });

            // Restore element css to original 
            System.Threading.Thread.Sleep(500);
            string originalStyleJavascript = @"arguments[0].style.cssText = ""background: none; border-width: 1px; border-style: solid; border-color: transparent"";";
            jsDriver.ExecuteScript(originalStyleJavascript, new object[] { element });
        }

        /// <summary>
        /// Creates a new log file to the grid_logs folder
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="browserName"></param>
        /// <param name="dateTime"></param>
        /// <returns> Log file path </returns>. 
        public static string CreateNewLog(string logType, string browserName, DateTime dateTime)
        {
            string strDateTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff").Replace('\\', '-').Replace(' ', '_').Replace(':', '-');
            string path = string.Format("C:\\Users\\Dan.Z\\Desktop\\grid_logs\\CS\\{0}_{1}_{2}.json", logType, browserName, strDateTime);
            using (StreamWriter streamWriter = File.AppendText(path))
            {
                streamWriter.Write("");
            }
            return path;
        }

        /// <summary>
        /// Creates and returns a success json string for a test
        /// </summary>
        public static string CreateSuccessJson(string testName, bool success, Exception error)
        {
            // Create an object to dump - dynamic means it can be manipulated as I wish (like python)
            dynamic successObject = new
            {
                testName,
                success,
                error = error?.Message
            };

            var json = JsonConvert.SerializeObject(successObject);
            return json;
        }

        /// <summary>
        /// Writes a new test success string to the success log file
        /// </summary>
        /// <param name="testName"></param>
        /// <param name="success"></param>
        /// <param name="error"></param>
        public static void WriteToSuccessLog(string testName, bool success, Exception error)
        {
            string jsonSuccessString = CreateSuccessJson(testName, success, error);
            using (StreamWriter streamWriter = File.AppendText(successLogFilePath))
            {
                streamWriter.WriteLine(jsonSuccessString);
            }
        }

        /// <summary>
        /// Creates and returns a success json string for a test
        /// </summary>
        /// <param name="testName"></param>
        /// <param name="action"></param>
        /// <param name="tryCounter"></param>
        /// <returns></returns>
        public static string CreatePerformanceJson(string testName, string action, int tryCount)
        {
            // Create an object to dump - dynamic means it can be manipulated as I wish (like python)
            dynamic performanceObject = new
            {
                testName,
                action,
                tryCount
            };

            var json = JsonConvert.SerializeObject(performanceObject);
            return json;
        }

        /// <summary>
        /// Writes a new actions performance string to the performance log file
        /// </summary>
        /// <param name="testName"></param>
        /// <param name="action"></param>
        /// <param name="tryCount"></param>
        public static void WriteToPerformanceLog(string testName, string action, int tryCount)
        {
            string jsonPerformanceString = CreatePerformanceJson(testName, action, tryCount);

            using (StreamWriter streamWriter = File.AppendText(performanceLogFilePath))
            {
                streamWriter.WriteLine(jsonPerformanceString);
            }
        }

        /// <summary>
        /// Creates and return a line for a bad performance action
        /// </summary>
        /// <param name="action"></param>
        /// <param name="testName"></param>
        /// <param name="actionIndexInTest"></param>
        /// <param name="tryCount"></param>
        /// <returns></returns>
        public static string CreateFinalizedPerformanceJson(string action, string testName, int actionIndexInTest, int tryCount)
        {
            // Create an object to dump - dynamic means it can be manipulated as I wish (like python)
            dynamic finalizedPerformanceObject = new
            {
                action,
                testName,
                actionIndexInTest,
                tryCount
            };

            var json = JsonConvert.SerializeObject(finalizedPerformanceObject);
            return json;
        }

        /// <summary>
        /// Analyzes the performance log and writes the laggy actions to the finalized performance log
        /// </summary>
        public static void WriteToFinalizedPerformanceLog()
        {
            string[] logLines = File.ReadAllLines(performanceLogFilePath);

            string previousTest = "";
            int actionIndexInTest = 1;

            foreach (string line in logLines)
            {
                dynamic lineObject = JsonConvert.DeserializeObject(line);
                string currentTest = lineObject.testName.ToString();

                // Count index of operation in function (set to 1 if changed function)
                actionIndexInTest = (currentTest.Equals(previousTest)) ? actionIndexInTest + 1 : 1;

                var tryCount = Int32.Parse(lineObject.tryCount.ToString());

                // Check if function exceeded try limit
                if (tryCount > actionPerformanceLimit)
                {
                    var action = lineObject.action.ToString();
                    string finalizedString = CreateFinalizedPerformanceJson(action, currentTest, actionIndexInTest, tryCount);

                    using (StreamWriter streamWriter = File.AppendText(finalizedPerformanceLogFilePath))
                    {
                        streamWriter.WriteLine(finalizedString);
                    }
                }
                previousTest = currentTest;
            }
        }

        /// <summary>
        /// Gets API data from the server. Returns an array of objects representing the query data fetched (every object is a db row).
        /// </summary>
        /// <param name="AuthorizationPassword"> API authorization </param>
        /// <param name="AuthorizationUsername"> API authorization </param>
        /// <param name="objectType"> Requested object type (transaction, user, item, etc.) </param>
        /// <param name="property"> Object attribute to be evaluated in request (id, name, email) </param>
        /// <param name="value"> Expected value of requested attribute </param>
        /// 
        /// <returns></returns>
        public static dynamic GetApiData(string AuthorizationUsername, string AuthorizationPassword, string objectType, string property, string value)
        {
            var data = string.Empty;

            // API request url, with inserted data from method params
            string url = string.Format(@"https://apint.sandbox.pepperi.com/restapi/PepperiAPInt.Data.svc/V1.0/{0}?where={1}='{2}'", objectType, property, value);

            // Create the request object
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            // Create encoding (authorization) string
            String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(AuthorizationUsername + ":" + AuthorizationPassword));

            // Add authorization to request header
            request.Headers.Add("Authorization", encoded);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())

            // Data stream from request        
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                data = reader.ReadToEnd();
            }

            // Convert json API data to c# object
            dynamic DataObject = JsonConvert.DeserializeObject(data);

            return DataObject;
        }

        /// <summary>
        /// Gets user password from JSON file
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static string GetUserPassword(string username)
        {
            string[] users = File.ReadAllLines(UserCredentialsFilePath);
            foreach (string user in users)
            {
                dynamic userObject = JsonConvert.DeserializeObject(user);
                if (userObject.username == username)
                    return userObject.password;
            }

            return null;
        }
    }
}
