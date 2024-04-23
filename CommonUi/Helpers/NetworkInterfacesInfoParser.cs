using Common.Core.DataModels.Cmd;

namespace Common.Helpers;

public class NetworkInterfacesInfoParser
{
    public static List<NetworkInterfacesInfo> ParseNetworkInterfaces(string output)
    {
        var lines = output.Split('\n');
        var interfaces = new List<NetworkInterfacesInfo>();

        for (int i = 2; i < lines.Length; i++)
        {
            var parts = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length >= 4)
            {
                var info = new NetworkInterfacesInfo
                {
                    AdminState = parts[0],
                    State = parts[1],
                    Type = parts[2],
                    InterfaceName = string.Join(' ', parts.Skip(3)).TrimEnd()
                };

                interfaces.Add(info);
            }
        }

        return interfaces;
    }
}
