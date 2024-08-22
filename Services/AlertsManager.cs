using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Molyi.Alerts.Enums;
using Molyi.Alerts.Interfaces;
using Molyi.Alerts.Models;
using OpenMod.API.Ioc;
using OpenMod.Core.Helpers;
using OpenMod.Unturned.Players;
using SDG.NetTransport;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Molyi.Alerts.Services
{
	[ServiceImplementation(Lifetime = ServiceLifetime.Singleton)]
	public class AlertsManager : IAlertsManager
	{
		private readonly ushort m_MaxAlerts = 6;
		private readonly ushort m_AlertTime = 3500;
		private readonly Dictionary<CSteamID, Alert?[]> PlayerAlerts = [];
		private readonly Dictionary<CSteamID, Queue<Alert>> PlayerAlertsQueue = [];

		public void SendAlert(UnturnedPlayer uPlayer, string alertText, EAlertType alertType)
		{
			ITransportConnection connection = uPlayer.Player.channel.GetOwnerTransportConnection();
			if (!PlayerAlerts.TryGetValue(uPlayer.SteamId, out Alert?[] Alerts))
			{
				PlayerAlerts.Add(uPlayer.SteamId, Alerts = new Alert[m_MaxAlerts]);
				EffectManager.sendUIEffect(36002, 6001, connection, false);
			}

			int number = Array.FindIndex(Alerts, alert => alert == null || alert.IsExpired);

			string alertTypeStr = alertType.ToString();
			if (number == -1)
			{
				if (!PlayerAlertsQueue.ContainsKey(uPlayer.SteamId)) PlayerAlertsQueue.Add(uPlayer.SteamId, new Queue<Alert>());
				PlayerAlertsQueue[uPlayer.SteamId].Enqueue(new Alert(alertText, alertTypeStr));
			}
			else
			{
				Alerts[number] = new Alert(alertText, alertTypeStr);
				ShowAlert(uPlayer, number, alertText, alertTypeStr);
			}
		}

		private void ShowAlert(UnturnedPlayer uPlayer, int number, string alertText, string alertType)
		{
			ITransportConnection connection = uPlayer.Player.channel.GetOwnerTransportConnection();

			EffectManager.sendUIEffectVisibility(6001, connection, false, $"ZAlert{alertType}{number}", false);
			EffectManager.sendUIEffectVisibility(6001, connection, false, $"ZAlert{alertType}{number}", true);
			EffectManager.sendUIEffectText(6001, connection, false, $"text_alert{alertType}{number}", alertText);

			AsyncHelper.Schedule("UpdateAlerts", () => UpdateAlerts(uPlayer, number));
		}

		private async Task UpdateAlerts(UnturnedPlayer uPlayer, int number)
		{
			await Task.Delay(TimeSpan.FromSeconds(m_AlertTime));
			await UniTask.SwitchToMainThread();

			if (uPlayer == null || !PlayerAlerts.TryGetValue(uPlayer.SteamId, out Alert?[] Alerts) || Alerts[number] == null) return;

			if (!PlayerAlertsQueue.TryGetValue(uPlayer.SteamId, out Queue<Alert> alertQueue) || alertQueue.Count == 0) Alerts[number] = null;
			else
			{
				Alerts[number] = alertQueue.Dequeue();
				ShowAlert(uPlayer, number, Alerts[number]!.Message, Alerts[number]!.AlertType);
			}
		}
	}
}
