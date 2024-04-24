using System.Diagnostics;

namespace Common.Helpers;

public static partial class CmdCommandsHelper
{
    public const string ErrorMessageForNoInternet = "General failure.";

    public const string InternetConnectionAvailableMessage = "Packets: Sent = 4, Received = 4, Lost = 0 (0% loss)";

    private const string NetworkInterfacesInfoCommand = "netsh interface show interface";

    private const string WlanInterfacesInfoCommand = "netsh wlan show interfaces";

    private const string PingCommand = "ping 8.8.8.8";

    private const string ShowProfilesCommand = "netsh wlan show profiles";

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

    private static string GetFirstProfile()
    {
        var output = ExecuteCommand(ShowProfilesCommand);
        var lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        foreach (var line in lines)
        {
            if (line.Contains("All User Profile"))
            {
                var parts = line.Split(':');
                if (parts.Length == 2)
                {
                    return parts[1].Trim();
                }
            }
        }

        return null;
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

    private static string GetConnectInterfaceCommand(string profileName, string interfaceName)
    {
        return $"netsh wlan connect name=\"{profileName}\" interface=\"{interfaceName}\"";
    }
}
