using Common.Core.DataModels.CmdModels.Constants;
using FluentAssertions;
using Type = Common.Core.DataModels.CmdModels.Constants.Type;
using Cmd = Common.Helpers.CmdCommandsHelper;

namespace Tests;

/*
** IMPORTANT **
Before running the tests, please ensure that the "Connect automatically" checkbox
is unchecked in your Wi-Fi connection settings, as it may affect the test results.
*/
public class InternetCheckTests
{
    [Test]
    public void DisableEnableWiFiAdapter()
    {
        //Arrange
        Cmd.SetInternetStatus(false);
        var networkInterfacesInfo = Cmd.GetNetworkInterfacesInfo();
        var networkInterfacesInfoList = Cmd.ParseNetworkInterfacesInfo(networkInterfacesInfo);
        var interfaceName = Cmd.GetInterfaceNameByAdminStateAndType(networkInterfacesInfoList, AdminState.Enabled, Type.Dedicated);
        interfaceName.Should().NotBeNull();

        //Action
        Cmd.DisableWiFiInterface(interfaceName);
        Cmd.Ping().Should().Contain(Cmd.ErrorMessageForNoInternet);
        Cmd.EnableWiFiInterface(interfaceName);

        //Assert
        Cmd.GetCurrentInterfaceInfo(interfaceName).Should().Contain(AdminState.Enabled);
        Cmd.Ping().Should().Contain(Cmd.ErrorMessageForNoInternet);
    }
}