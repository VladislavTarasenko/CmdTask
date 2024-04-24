using Common.Core.SeleniumWebDriver;

namespace Common.Core.Pages;

public class BasePage
{
    protected virtual string pageUrl { get; }

    public void GoToPage()
    {
        DriverInstance.GetInstance().Navigate().GoToUrl(pageUrl);
    }
}
