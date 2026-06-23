# WindowsGSM.RSDragonwilds
A WindowsGSM plugin to deploy and manage a RuneScape: Dragonwilds Dedicated Server.

## Installation
1. Download the `RSDragonwilds.cs` file from this repository.
2. In your WindowsGSM folder, navigate to `plugins/` and create a folder named `RSDragonwilds`.
3. Drop `RSDragonwilds.cs` into that folder.
4. Restart or click **Reload Plugins** inside WindowsGSM.

## Vital Setup Note
**The server will fail to launch without a valid Owner ID.**

Before playing, you must add your in-game Player ID to the server's configuration file. 

1. Launch RuneScape: Dragonwilds and go to the **Settings** menu.
2. Look at the bottom-left corner of the screen and copy your **Player ID**.
3. Start the server once in WindowsGSM. *(It will fail to fully launch, which is normal, but this generates the necessary configuration folders).*
4. Navigate to your server files: `serverfiles\RSDragonwilds\Saved\Config\WindowsServer\DedicatedServer.ini`
5. Open the file and add your Player ID under the settings block:
```ini
   [/Script/Dominion.DedicatedServerSettings]
   OwnerId=YOUR_PLAYER_ID
