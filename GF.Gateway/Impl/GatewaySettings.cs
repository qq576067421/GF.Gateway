// Copyright (c) Cragon. All rights reserved.

namespace GF.Gateway
{
    using System.Configuration;

    public static class GatewaySettings
    {
        public static bool IsSsl
        {
            get
            {
                string ssl = ConfigurationManager.AppSettings["Ssl"];
                return !string.IsNullOrEmpty(ssl) && bool.Parse(ssl);
            }
        }

        public static int Port => int.Parse(ConfigurationManager.AppSettings["Port"]);
    }
}
