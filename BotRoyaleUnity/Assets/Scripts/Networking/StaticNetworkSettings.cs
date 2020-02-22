using UnityEngine;

public static class StaticNetworkSettings
{
    public static Servers CurrentServer = Servers.PRODUCTION;

    static StaticNetworkSettings()
    {
        // by default, use local server in the editor, and deployed server in builds
        if (Application.isEditor)
        {
            CurrentServer = Servers.LOCAL;
        }
        else
        {
            CurrentServer = Servers.PRODUCTION;
        }
    }

    public static string ServerURL {
        get
        {
            switch (CurrentServer)
            {
                case Servers.LOCAL:
                    return "localhost";
                case Servers.PRODUCTION:
                    return "build-a-bot-royale.herokuapp.com";
                default:
                    throw new System.NotImplementedException("No server URL defined for server " + CurrentServer);
            }
        }
    }

    public static int ServerPort
    {
        get
        {
            switch (CurrentServer)
            {
                case Servers.LOCAL:
                    return 9000;
                case Servers.PRODUCTION:
                    return 80;
                default:
                    throw new System.NotImplementedException("No server port defined for server " + CurrentServer);
            }
        }
    }
}
