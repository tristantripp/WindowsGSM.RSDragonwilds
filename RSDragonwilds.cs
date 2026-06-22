using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using WindowsGSM.Functions;
using WindowsGSM.GameServer.Engine;
using WindowsGSM.GameServer.Query;

namespace WindowsGSM.Plugins
{
    public class RSDragonwilds : SteamCMDAgent
    {
        // Plugin metadata block - required by WindowsGSM
        public Plugin Plugin = new Plugin
        {
            name = "WindowsGSM.RSDragonwilds",
            author = "Community Developer",
            description = "🧩 WindowsGSM plugin for RuneScape: Dragonwilds Dedicated Server",
            version = "1.0.1",
            url = "https://github.com/WindowsGSM.RSDragonwilds",
            color = "#8B0000"
        };

        // SteamCMD installer settings - must be override properties, not fields
        public override bool loginAnonymous => true;
        public override string AppId => "4019830";

        // Standard constructor
        public RSDragonwilds(ServerConfig serverData) : base(serverData) =>
            base.serverData = _serverData = serverData;
        private readonly ServerConfig _serverData;

        public string Error, Notice;

        // Fixed variables - StartPath must be an override property
        public override string StartPath => @"RSDragonwilds\Binaries\Win64\RSDragonwildsServer-Win64-Shipping.exe";
        public string FullName = "RuneScape: Dragonwilds Dedicated Server";
        public bool AllowsEmbedConsole = false;
        public int PortIncrements = 1;
        public object QueryMethod = new A2S();

        // Default values
        public string Port = "7777";
        public string QueryPort = "27015";
        public string Defaultmap = "";
        public string Maxplayers = "6";
        public string Additional = "";

        public async void CreateServerCFG() { }

        public async Task<Process> Start()
        {
            string exePath = Functions.ServerPath.GetServersServerFiles(_serverData.ServerID, StartPath);
            if (!File.Exists(exePath))
            {
                Error = $"{Path.GetFileName(exePath)} not found ({exePath})";
                return null;
            }

            string param = $"-log -port={_serverData.ServerPort} -QueryPort={_serverData.ServerQueryPort} -MaxPlayers={_serverData.ServerMaxPlayer}";

            if (!string.IsNullOrWhiteSpace(_serverData.ServerParam))
                param += $" {_serverData.ServerParam}";

            var p = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = ServerPath.GetServersServerFiles(_serverData.ServerID),
                    FileName = exePath,
                    Arguments = param,
                    WindowStyle = ProcessWindowStyle.Minimized,
                    UseShellExecute = false
                },
                EnableRaisingEvents = true
            };

            if (AllowsEmbedConsole)
            {
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                var serverConsole = new ServerConsole(_serverData.ServerID);
                p.OutputDataReceived += serverConsole.AddOutput;
                p.ErrorDataReceived += serverConsole.AddOutput;

                try { p.Start(); }
                catch (Exception e) { Error = e.Message; return null; }

                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                return p;
            }

            try { p.Start(); return p; }
            catch (Exception e) { Error = e.Message; return null; }
        }

        public async Task Stop(Process p)
        {
            await Task.Run(() =>
            {
                Functions.ServerConsole.SetMainWindow(p.MainWindowHandle);
                Functions.ServerConsole.SendWaitToMainWindow("^c");
            });
            await Task.Delay(20000);
        }

        public async Task<Process> Install()
        {
            var steamCMD = new Installer.SteamCMD();
            Process p = await steamCMD.Install(_serverData.ServerID, string.Empty, AppId);
            Error = steamCMD.Error;
            return p;
        }

        public async Task<Process> Update(bool validate = false, string custom = null)
        {
            var (p, error) = await Installer.SteamCMD.UpdateEx(_serverData.ServerID, AppId, validate, custom: custom);
            Error = error;
            return p;
        }

        public bool IsInstallValid()
        {
            return File.Exists(Functions.ServerPath.GetServersServerFiles(_serverData.ServerID, StartPath));
        }

        public string GetLocalBuild()
        {
            var steamCMD = new Installer.SteamCMD();
            return steamCMD.GetLocalBuild(_serverData.ServerID, AppId);
        }

        public async Task<string> GetRemoteBuild()
        {
            var steamCMD = new Installer.SteamCMD();
            return await steamCMD.GetRemoteBuild(AppId);
        }
    }
}
