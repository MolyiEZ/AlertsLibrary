using Molyi.Alerts.Enums;
using OpenMod.API.Ioc;
using OpenMod.Unturned.Players;

namespace Molyi.Alerts.Interfaces
{
	[Service]
	public interface IAlertsManager
	{
		void SendAlert(UnturnedPlayer uPlayer, string alertText, EAlertType alertType);
	}
}
