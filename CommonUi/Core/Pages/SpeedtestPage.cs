using Common.Core.SeleniumWebDriver;
using OpenQA.Selenium;

namespace Common.Core.Pages;

public class SpeedtestPage : BasePage
{
    protected override string pageUrl => "https://www.speedtest.net";

    public IWebElement IpField => DriverInstance.GetInstance().FindElement(By.XPath("//div[@title='{{ipType}}']"));
}
