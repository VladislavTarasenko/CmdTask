using Common.Core.Pages;
using Common.Core.SeleniumWebDriver;

namespace Common.Core.Steps;

public class SpeedtestPageSteps
{
    public static void GoToPage()
    {
        DriverInstance.GetInstance().Navigate().GoToUrl("https://www.speedtest.net");
    }

    public static string GetIpAdress()
    {
        var speedtestPage = new SpeedtestPage();
        return speedtestPage.IpField.Text;
    }
}
