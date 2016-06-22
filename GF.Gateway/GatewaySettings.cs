// Copyright (c) Cragon. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cragon.Game.Gateway
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
