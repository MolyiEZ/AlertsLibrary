using Microsoft.Extensions.DependencyInjection;
using Molyi.Alerts.Interfaces;
using OpenMod.API.Eventing;
using OpenMod.Core.Eventing;
using OpenMod.Unturned.Players.Connections.Events;
using System.Threading.Tasks;

namespace Molyi.Alerts.Events.Players
{
	[EventListenerLifetime(ServiceLifetime.Singleton)]
	internal class PlayerDisconnected(IAlertManager alertManager) : IEventListener<UnturnedPlayerDisconnectedEvent>
	{
		private readonly IAlertManager m_AlertManager = alertManager;

		[EventListener(Priority = EventListenerPriority.Highest)]
		public Task HandleEventAsync(object? sender, UnturnedPlayerDisconnectedEvent @event)
		{
			m_AlertManager.CancelAlerts(@event.Player.SteamId);
			return Task.CompletedTask;
		}
	}
}
