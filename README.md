# WindowsGSM.RSDragonwilds
A WindowsGSM plugin to deploy and manage a RuneScape: Dragonwilds Dedicated Server.

## Installation
1. Download the `RSDragonwilds.cs` file from this repository.
2. In your WindowsGSM folder, navigate to `plugins/` and create a folder named `RSDragonwilds`.
3. Drop `RSDragonwilds.cs` into that folder.
4. Restart or click **Reload Plugins** inside WindowsGSM.

## Vital Setup Note
The server will crash on launch without an Admin ID. 
Before starting the server, go to the server settings inside WindowsGSM and add your player ID to the **Additional Launch Parameters** field:
`-OwnerId=YOUR_PLAYER_ID`
