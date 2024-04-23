using Common.Core.SeleniumWebDriver;
using OpenQA.Selenium;

namespace Common.Core.Pages;

public class SpeedtestPage
{
    public IWebElement IpField => DriverInstance.GetInstance().FindElement(By.XPath("//div[@title='{{ipType}}']"));
}
