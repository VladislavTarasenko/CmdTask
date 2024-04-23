using Common.Core.DataModels.CmdModels;

namespace Common.Helpers;

public class WlanInterfacesInfoParser
{
    public static WlanInterfacesInfo ParseWlanInterfacesInfo(string interfaceDetails)
    {
        WlanInterfacesInfo wlanInterfacesInfo = new WlanInterfacesInfo();

        var lines = interfaceDetails.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

        for (int i = 2; i < lines.Length; i++)
        {
            var line = lines[i];
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var keyAndValue = line.Split(new[] { ": " }, StringSplitOptions.None);
            if (keyAndValue.Length == 2)
            {
                var key = keyAndValue[0].Trim();
                var value = keyAndValue[1].Trim();

                switch (key)
                {
                    case "Name":
                        wlanInterfacesInfo.Name = value;
                        break;
                    case "State":
                        wlanInterfacesInfo.State = value;
                        break;
                    case "SSID":
                        wlanInterfacesInfo.SSID = value;
                        break;
                    case "Profile":
                        wlanInterfacesInfo.Profile = value;
                        break;
                    default:
                        break;
                }
            }
        }

        return wlanInterfacesInfo;
    }
}
