using System;
using Cysharp.Threading.Tasks;
using OpenMod.Unturned.Plugins;
using OpenMod.API.Plugins;

[assembly: PluginMetadata("Molyi.AlertsLibrary", Author = "Molyi", DisplayName = "AlertsLibrary", Description = "Unturned OpenMod Library. Used to send Alerts through the UI.", Website = "https://github.com/MolyiEZ/AlertsLibrary")]
namespace Molyi.Alerts
{
	internal class AlertsLibrary(IServiceProvider serviceProvider) : OpenModUnturnedPlugin(serviceProvider)
	{ 
		protected override UniTask OnLoadAsync() => UniTask.CompletedTask;

		protected override UniTask OnUnloadAsync() => UniTask.CompletedTask;
	}
}
