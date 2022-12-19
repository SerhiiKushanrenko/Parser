using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace BLL.Helpers
{
    public static class WebDriverHelper
    {
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }
            return driver.FindElement(by);
        }

        public static ReadOnlyCollection<IWebElement> FindElements(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElements(by));
            }
            return driver.FindElements(by);
        }

        public static IWebElement FindElementIfExists(this IWebDriver driver, By by)
        {
            driver.WaitUntil(driver => driver.FindElementsIfExists(by).Count() != 0);
            var elms = driver.FindElementsIfExists(by);

            var result = elms?.FirstOrDefault();
            return result;
        }

        public static void WaitUntil<TResult>(this IWebDriver driver, Func<IWebDriver, TResult> condition, bool silent = true, TimeSpan? timeout = null)
        {
            var wait = new WebDriverWait(driver, timeout ?? new TimeSpan(0, 10, 120));

            try
            {
                wait.Until(condition);
            }
            catch
            {
                if (!silent)
                    throw;
            }
        }

        public static IEnumerable<IWebElement> FindElementsIfExists(this IWebDriver driver, By by)
        {
            using var _ = driver.UseZeroImplicitWait();

            var elms = driver.FindElements(by);

            var result = (elms?.Where(e =>
            {
                try
                {
                    return e.Displayed;
                }
                catch (StaleElementReferenceException) { }
                return false;
            }) ?? Enumerable.Empty<IWebElement>()).ToArray();
            return result;
        }

        public static IDisposable UseImplicitWait(this IWebDriver webDriver, TimeSpan timeout)
        {
            return new WebDriverImplicitWaiter(webDriver, timeout);
        }

        public static IDisposable UseZeroImplicitWait(this IWebDriver webDriver) => webDriver.UseImplicitWait(TimeSpan.Zero);

        private class WebDriverImplicitWaiter : IDisposable
        {
            private readonly IWebDriver _webDriver;
            private readonly TimeSpan _oldImplicitWait;
            public WebDriverImplicitWaiter(IWebDriver webDriver, TimeSpan timeout)
            {
                _webDriver = webDriver;
                _oldImplicitWait = webDriver.Manage().Timeouts().ImplicitWait;
                webDriver.Manage().Timeouts().ImplicitWait = timeout;
            }

            public void Dispose()
            {
                _webDriver.Manage().Timeouts().ImplicitWait = _oldImplicitWait;
            }
        }
    }
}
