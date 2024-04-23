using Common.Core.DataModels.Cmd;
using Common.Core.DataModels.CmdModels;
using Common.Core.DataModels.CmdModels.Constants;
using System.Diagnostics;

namespace Common.Helpers;

public static class CmdCommandsHelper
{
    public const string ErrorMessageForNoInternet = "PING: transmit failed. General failure.";

    public const string InternetConnectionAvailableMessage = "Packets: Sent = 4, Received = 4, Lost = 0 (0% loss)";

    private const string NetworkInterfacesInfoCommand = "netsh interface show interface";

    private const string WlanInterfacesInfoCommand = "netsh wlan show interfaces";

    private const string PingCommand = "ping 8.8.8.8";

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

        if(interfaceName == null)
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
                ExecuteCommand(GetConnectInterfaceCommand(wlanInterfaceInfo.SSID, wlanInterfaceInfo.Profile, wlanInterfaceInfo.Name));
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

    private static string ExecuteCommand(string command, bool runAsAdmin = false)
    {
        var processInfo = new ProcessStartInfo("cmd.exe", "/c " + command)
        {
            CreateNoWindow = true,
            UseShellExecute = runAsAdmin,
            RedirectStandardOutput = !runAsAdmin,
            RedirectStandardError = !runAsAdmin
        };

        if (runAsAdmin)
            processInfo.Verb = "runas";

        string output = null;
        using (var process = Process.Start(processInfo))
        {
            if (!runAsAdmin)
                output = process.StandardOutput.ReadToEnd();

            process.WaitForExit();
        }

        return output?.Trim();
    }

    private static string GetEnableInterfaceCommand(string interfaceName)
    {
        return $"netsh interface set interface \"{interfaceName}\" enable";
    }
    private static string GetDisableInterfaceCommand(string interfaceName)
    {
        return $"netsh interface set interface \"{interfaceName}\" disable";
    }
    private static string GetCheckInterfaceCommand(string interfaceName)
    {
        return $"{NetworkInterfacesInfoCommand} \"{interfaceName}\"";
    }

    private static string GetDisconnectInterfaceCommand(string interfaceName)
    {
        return $"netsh wlan disconnect interface=\"{interfaceName}\"";
    }

    private static string GetConnectInterfaceCommand(string ssid, string profileName, string interfaceName)
    {
        return $"netsh wlan connect ssid=\"{profileName}\" name=\"{profileName}\" interface=\"{interfaceName}\"";
    }
}
