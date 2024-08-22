using System;
using Cysharp.Threading.Tasks;
using OpenMod.Unturned.Plugins;
using OpenMod.API.Plugins;

// For more, visit https://openmod.github.io/openmod-docs/devdoc/guides/getting-started.html

[assembly: PluginMetadata("Molyi.ZAlertsLibrary", DisplayName = "ZAlertsLibrary")]
namespace Molyi.Alerts
{
	public class AlertsLibrary(
		IServiceProvider serviceProvider) : OpenModUnturnedPlugin(serviceProvider)
    {
		protected override UniTask OnLoadAsync()
        {
            return UniTask.CompletedTask;
        }

        protected override UniTask OnUnloadAsync()
        {
			return UniTask.CompletedTask;
		}
    }
}
