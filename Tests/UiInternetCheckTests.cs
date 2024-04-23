using Common.Core.DataModels.CmdModels.Constants;
using FluentAssertions;
using Common.Core.Steps;
using Cmd = Common.Helpers.CmdCommandsHelper;

namespace Tests;

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

        SpeedtestPageSteps.GoToPage();
        var ipAdress = SpeedtestPageSteps.GetIpAdress();

        Cmd.SetInternetStatus(false);
        Cmd.GetCurrentInterfaceInfo(wlanInterfaceInfo.Name).Should().Contain(AdminState.Enabled);
        Cmd.Ping().Should().Contain(Cmd.ErrorMessageForNoInternet);
    }
}
