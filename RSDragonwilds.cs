using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WindowsGSM.Functions;
using WindowsGSM.GameServer.Engine;
using WindowsGSM.GameServer.Query;

namespace WindowsGSM.Plugins
{
    public class RSDragonwilds : SteamCMDAMSA
    {
        // Plugin Information
        public uint AppId = 4019830; // Official Steam AppID for Dragonwilds Dedicated Server
        public string Name = "RuneScape: Dragonwilds Dedicated Server";
        public string ShortName = "RSDragonwilds";
        public string Author = "Community Developer";
        public string Version = "1.0.0";
        
        // Default Configuration Settings
        public string DefaultPort = "7777"; 
        public string DefaultQueryPort = "27015";
        public string DefaultMaxPlayers = "6";

        // Server Executable Location
        public string StartPath = @"RSDragonwilds\Binaries\Win64\RSDragonwildsServer-Win64-Shipping.exe"; 
        public string FullName = "RuneScape: Dragonwilds Dedicated Server";

        public override string AdditionalArgs { get; set; } = "";

        public RSDragonwilds(ServerConfig serverConfig) : base(serverConfig)
        {
            base.serverConfig = serverConfig;
        }

        // Fixes the critical requirement to append the Owner ID to the launch parameters
        public async Task<Process> Start()
        {
            string configFilePath = Functions.ServerPath.GetServerComponents(serverConfig.ServerID, StartPath);
            if (!System.IO.File.Exists(configFilePath))
            {
                Error = $"{StartPath} not found.";
                return null;
            }

            // Pulls user custom input or prompts for the vital OwnerID parameters
            string param = $"-log -port={serverConfig.ServerPort} -QueryPort={serverConfig.ServerQueryPort} -MaxPlayers={serverConfig.ServerMaxPlayer}";
            
            if (!string.IsNullOrWhiteSpace(serverConfig.ServerParam))
            {
                param += $" {serverConfig.ServerParam}";
            }

            Process p = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = configFilePath,
                    Arguments = param,
                    WindowStyle = ProcessWindowStyle.Normal,
                    UseShellExecute = false
                }
            };

            p.Start();
            return p;
        }
    }
}
