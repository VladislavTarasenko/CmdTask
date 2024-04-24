using Common.Core.DataModels.CmdModels.Constants;
using FluentAssertions;
using Common.Core.Steps;
using Cmd = Common.Helpers.CmdCommandsHelper;
using Common.Core.Pages;

namespace Tests;
/*
** IMPORTANT **
Before running the tests, please ensure that the "Connect automatically" checkbox
is unchecked in your Wi-Fi connection settings, as it may affect the test results.
*/
public class UiInternetCheckTests : BaseUiTest
{
    [Test]
    public void TestInternetConnection()
    {
        Cmd.SetInternetStatus(true);
        var wlanInterfaceInfo = Cmd.GetWlanInterfaceInfo();
        Cmd.EnableWiFiInterface(wlanInterfaceInfo.Name);
        Cmd.GetCurrentInterfaceInfo(wlanInterfaceInfo.Name).Should().Contain(AdminState.Enabled);
        Cmd.Ping().Should().Contain(Cmd.InternetConnectionAvailableMessage);

        var speedtestPage = new SpeedtestPage();
        speedtestPage.GoToPage();
        var ipAdress = SpeedtestPageSteps.GetIpAdress();

        Cmd.SetInternetStatus(false);
        Cmd.GetCurrentInterfaceInfo(wlanInterfaceInfo.Name).Should().Contain(AdminState.Enabled);
        Cmd.Ping().Should().Contain(Cmd.ErrorMessageForNoInternet);
    }
}
