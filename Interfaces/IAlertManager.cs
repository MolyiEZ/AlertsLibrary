using Molyi.Alerts.Enums;
using OpenMod.API.Ioc;
using OpenMod.Unturned.Players;
using SDG.Unturned;
using Steamworks;

namespace Molyi.Alerts.Interfaces
{
	[Service]
	public interface IAlertManager
	{
		void SendAlert(UnturnedPlayer uPlayer, string alertText, EAlertType alertType);
		void SendAlert(Player player, string alertText, EAlertType alertType);
		void CancelAlerts(CSteamID steamId);
	}
}
