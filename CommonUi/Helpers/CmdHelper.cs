using Common.Core.DataModels.Cmd;
using Common.Core.DataModels.CmdModels.Constants;
using Common.Core.DataModels.CmdModels;

namespace Common.Helpers;

public static partial class CmdCommandsHelper
{
    public static void EnableWiFiInterface(string interfaceName)
    {
        if (GetCurrentInterfaceInfo(interfaceName).Contains(AdminState.Disabled))
        {
            ExecuteCommand(GetEnableInterfaceCommand(interfaceName), true);
        }
    }

    public static void DisableWiFiInterface(string interfaceName)
    {
        if (GetCurrentInterfaceInfo(interfaceName).Contains(AdminState.Enabled))
        {
            ExecuteCommand(GetDisableInterfaceCommand(interfaceName), true);
        }
    }

    public static string Ping()
    {
        return ExecuteCommand(PingCommand);
    }

    public static string GetCurrentInterfaceInfo(string interfaceName)
    {
        return ExecuteCommand(GetCheckInterfaceCommand(interfaceName));
    }

    public static string GetNetworkInterfacesInfo()
    {
        return ExecuteCommand(NetworkInterfacesInfoCommand);
    }

    public static List<NetworkInterfacesInfo> ParseNetworkInterfacesInfo(string networkInterfacesInfo)
    {
        return NetworkInterfacesInfoParser.ParseNetworkInterfaces(networkInterfacesInfo);
    }

    public static WlanInterfacesInfo ParseWlanInterfacesInfo(string wlanInterfacesInfo)
    {
        return WlanInterfacesInfoParser.ParseWlanInterfacesInfo(wlanInterfacesInfo);
    }

    public static string GetInterfaceNameByAdminStateAndType(List<NetworkInterfacesInfo> interfacesInfoList, string adminState, string type)
    {
        var interfaceName = interfacesInfoList.FirstOrDefault(i => i.AdminState == adminState && i.Type == type)?.InterfaceName;

        if (interfaceName == null)
        {
            throw new Exception($"There are no interfaces found with AdminState = {adminState} and Type = {type}");
        }
        return interfaceName;
    }

    public static void SetInternetStatus(bool internetShouldBeOn)
    {
        var wlanInterfaceInfo = GetWlanInterfaceInfo();

        if (internetShouldBeOn)
        {
            if (!Ping().Contains(InternetConnectionAvailableMessage))
            {
                var command = GetConnectInterfaceCommand(wlanInterfaceInfo.Profile ?? GetFirstProfile(), wlanInterfaceInfo.Name);
                ExecuteCommand(command);
            }
        }
        else
        {
            if (Ping().Contains(InternetConnectionAvailableMessage))
                ExecuteCommand(GetDisconnectInterfaceCommand(wlanInterfaceInfo.Name));
        }
    }

    public static WlanInterfacesInfo GetWlanInterfaceInfo()
    {
        var wlanInterfacesInfo = ExecuteCommand(WlanInterfacesInfoCommand);
        return WlanInterfacesInfoParser.ParseWlanInterfacesInfo(wlanInterfacesInfo);
    }
}
