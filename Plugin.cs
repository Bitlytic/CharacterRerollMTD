using BepInEx;
using BepInEx.Logging;

using HarmonyLib;

namespace CharacterRerollPlugin
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class CharacterRerollPlugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            try {
                Harmony.CreateAndPatchAll(typeof(Patch.RerollPatch));
            }
            catch {
                Logger.LogError($"[{PluginInfo.PLUGIN_GUID}]: Cannot patch methods!");
            }

        }

        private void OnDestroy() {
            Harmony.UnpatchAll();
        }
    }
}
