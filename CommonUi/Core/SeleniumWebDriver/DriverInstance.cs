using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Common.Core.SeleniumWebDriver;

public class DriverInstance
{
    private static IWebDriver? _driver;

    public static IWebDriver GetInstance()
    {
        if (_driver == null)
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            _driver = new ChromeDriver(options);
            _driver.Manage().Timeouts().ImplicitWait.Add(TimeSpan.FromSeconds(10));
            _driver.Manage().Window.Maximize();
        }
        return _driver;
    }

    public static void CloseBrowser()
    {
        _driver?.Close();
        _driver?.Quit();
        _driver?.Dispose();
        _driver = null;
    }
}
