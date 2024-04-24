using Common.Core.Pages;

namespace Common.Core.Steps;

public class SpeedtestPageSteps
{
    public static string GetIpAdress()
    {
        var speedtestPage = new SpeedtestPage();
        return speedtestPage.IpField.Text;
    }
}
