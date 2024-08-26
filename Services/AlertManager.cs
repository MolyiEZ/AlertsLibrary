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
	internal class AlertManager : IAlertManager
	{
		private readonly ushort m_MaxAlerts = 6;
		private readonly ushort m_AlertTime = 3500;
		private readonly Dictionary<CSteamID, Alert?[]> PlayerAlerts = [];
		private readonly Dictionary<CSteamID, Queue<Alert>> PlayerAlertsQueue = [];

		public void SendAlert(UnturnedPlayer uPlayer, string alertText, EAlertType alertType) => SendAlert(uPlayer.Player, alertText, alertType);

		public void SendAlert(Player player, string alertText, EAlertType alertType)
		{
			ITransportConnection connection = player.channel.GetOwnerTransportConnection();
			CSteamID steamId = player.channel.owner.playerID.steamID;
			if (!PlayerAlerts.TryGetValue(steamId, out Alert?[] Alerts))
			{
				PlayerAlerts.Add(steamId, Alerts = new Alert[m_MaxAlerts]);
				EffectManager.sendUIEffect(36002, 3602, connection, false);
			}

			int number = Array.FindIndex(Alerts, alert => alert == null || alert.IsExpired);

			string alertTypeStr = alertType.ToString();
			if (number == -1)
			{
				if (!PlayerAlertsQueue.TryGetValue(steamId, out Queue<Alert> alertQueue)) PlayerAlertsQueue.Add(steamId, alertQueue = new Queue<Alert>());
				alertQueue.Enqueue(new Alert(alertText, alertTypeStr));
			}
			else
			{
				Alerts[number]?.DisposeSyncOrAsync();
				Alerts[number] = new Alert(alertText, alertTypeStr);
				ShowAlert(player, number, alertText, alertTypeStr);
			}
		}

		public void CancelAlerts(CSteamID steamId)
		{
			PlayerAlerts.Remove(steamId);
			PlayerAlertsQueue.Remove(steamId);
		}

		private void ShowAlert(Player player, int number, string alertText, string alertType)
		{
			ITransportConnection connection = player.channel.GetOwnerTransportConnection();

			EffectManager.sendUIEffectVisibility(3602, connection, false, $"Canvas/Alert{alertType}{number}", false);
			EffectManager.sendUIEffectVisibility(3602, connection, false, $"Canvas/Alert{alertType}{number}", true);
			EffectManager.sendUIEffectText(3602, connection, false, $"Canvas/Alert{alertType}{number}/Text", alertText);

			AsyncHelper.Schedule("UpdateAlerts", () => UpdateAlerts(player, number));
		}

		private async Task UpdateAlerts(Player player, int number)
		{
			await Task.Delay(m_AlertTime);
			await UniTask.SwitchToMainThread();

			if (player == null || !PlayerAlerts.TryGetValue(player.channel.owner.playerID.steamID, out Alert?[] alertsList)) return;

			Alert? alert = alertsList?[number];
			if (alert == null || alert.CancelToken.Token.IsCancellationRequested) return;

			if (!PlayerAlertsQueue.TryGetValue(player.channel.owner.playerID.steamID, out Queue<Alert> alertsQueue) || alertsQueue.Count == 0) alertsList![number] = null;
			else
			{
				alertsList![number] = alertsQueue.Dequeue();
				ShowAlert(player, number, alertsList[number]!.Message, alertsList[number]!.AlertType);
			}
		}
	}
}
