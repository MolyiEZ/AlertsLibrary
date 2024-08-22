using System;
using Cysharp.Threading.Tasks;
using OpenMod.Unturned.Plugins;
using OpenMod.API.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Molyi.Alerts.Interfaces;

// For more, visit https://openmod.github.io/openmod-docs/devdoc/guides/getting-started.html

[assembly: PluginMetadata("Molyi.AlertsLibrary", Author = "Molyi", DisplayName = "AlertsLibrary", Description = "Unturned OpenMod Library. Used to send Alerts through the UI.", Website = "https://github.com/MolyiEZ/AlertsLibrary")]
namespace Molyi.Alerts
{
	public class AlertsLibrary(
		IServiceProvider serviceProvider) : OpenModUnturnedPlugin(serviceProvider)
    {
		private readonly IServiceProvider m_ServiceProvider = serviceProvider;

		protected override UniTask OnLoadAsync()
        {
			m_ServiceProvider.GetRequiredService<IAlertsManager>();

			return UniTask.CompletedTask;
        }

        protected override UniTask OnUnloadAsync() => UniTask.CompletedTask;
	}
}
