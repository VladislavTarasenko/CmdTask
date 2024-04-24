using System.Diagnostics;

namespace Common.Helpers;

public static partial class CmdCommandsHelper
{
    public const string ErrorMessageForNoInternet = "PING: transmit failed. General failure.";

    public const string InternetConnectionAvailableMessage = "Packets: Sent = 4, Received = 4, Lost = 0 (0% loss)";

    private const string NetworkInterfacesInfoCommand = "netsh interface show interface";

    private const string WlanInterfacesInfoCommand = "netsh wlan show interfaces";

    private const string PingCommand = "ping 8.8.8.8";

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
