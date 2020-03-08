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

    public static bool UseSSL
    {
        get
        {
            switch (CurrentServer)
            {
                case Servers.LOCAL:
                    return false;
                case Servers.PRODUCTION:
                    return true;
                default:
                    throw new System.NotImplementedException("No ssl setting defined for server " + CurrentServer);
            }
        }
    }

    public static string ShortURL
    {
        get
        {
            switch (CurrentServer)
            {
                case Servers.LOCAL:
                    return "localhost:3000";
                case Servers.PRODUCTION:
                    return "bit.ly/2TytNCM";
                default:
                    throw new System.NotImplementedException("No short url defined for server " + CurrentServer);
            }
        }
    }

    public static string PlayerURL(string gameID = null)
    {
        string url;

        // choose the base url based on which server we're connecting to
        switch (CurrentServer)
        {
            case Servers.LOCAL:
                url = "http://localhost:3000";
                break;
            case Servers.PRODUCTION:
                url = "https://build-a-bot-royale.herokuapp.com";
                break;
            default:
                throw new System.NotImplementedException("No player url defined for server " + CurrentServer);
        }

        // append the game id query param if set
        if (gameID != null)
        {
            url += ("?id=" + gameID);
        }
        return url;
    }
}
