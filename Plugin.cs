using System.IO;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using DSP_AP.Archipelago;
using DSP_AP.Partials;
using DSP_AP.Services;
using DSP_AP.Utils;
using HarmonyLib;

namespace DSP_AP
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        #region Constants
        public const string PluginGUID = "BloodStainedCrow.DSP.DSP_AP";
        public const string PluginName = "DSP_AP";
        public const string PluginVersion = "0.2.1";
        public const string ModDisplayInfo = $"{PluginName} v{PluginVersion}";
        // This *must* match the constant in the apworld, and should match the constant in TechProto.kMaxProtoId
        public const int GoalItemIDOffset = 12000 * 2;
        // This *must* match the constant in the apworld, and should match the constant in TechProto.kMaxProtoId
        public const int ProgressiveItemOffset = 12000;
        #endregion

        #region Static Fields
        public static ManualLogSource BepinLogger;
        public static ArchipelagoClient ArchipelagoClient;
        public static string PluginPath;
        public static bool IsOnLinux;
        public static ConfigEntry<string> ConfigDefaultsHost;
        public static ConfigEntry<string> ConfigDefaultsSlot;
        public static ConfigEntry<string> ConfigDefaultsPassword;
        public static ConfigEntry<bool> ConfigDefaultsDeathLinkEnabled;
        public static TechProtoPartial[] APTechProtos;
        public static bool RefreshTechTree;
        public static Plugin Instance;
        #endregion

        private void Awake()
        {
            BepinLogger = base.Logger;
            Plugin.BepinLogger.LogInfo($"Paths.PluginPath: {Paths.PluginPath}");
            PluginPath = Path.Combine(Paths.PluginPath, PluginName);

            if (PluginPath.StartsWith("Z:\\home"))
            {
                Plugin.BepinLogger.LogWarning($"Found \\home at the start of the plugin path. Assuming the player is on linux!");
                PluginPath = PluginPath.Substring("Z:".Length);
                Plugin.BepinLogger.LogWarning($"Adjusted the plugin path to {PluginPath}");
                IsOnLinux = true;
            }
            else
            {
                IsOnLinux = false;
            }

            Plugin.ConfigDefaultsHost = Config.Bind("Defaults", "Host", "localhost", "The URI of the Archipelago server");
            Plugin.ConfigDefaultsSlot = Config.Bind("Defaults", "Slot", "Player1", "The player name used for connecting");
            Plugin.ConfigDefaultsPassword = Config.Bind("Defaults", "Password", "", "The password used for connecting");
            Plugin.ConfigDefaultsDeathLinkEnabled = Config.Bind("Defaults", "DeathLinkEnabled", false, "Whether to enable DeathLink by default");

            Harmony harmony = new Harmony(PluginGUID + ".Harmony");
            harmony.PatchAll();

            ArchipelagoClient = new ArchipelagoClient();
            ArchipelagoConsole.Awake();
            APTechProtos = TechInitializationService.CreateTechProtos();

            RefreshTechTree = true;

            ArchipelagoConsole.LogMessage($"{ModDisplayInfo} loaded!");
        }

        private void OnGUI()
        {
            PluginUI.DrawModLabel();
            PluginUI.DrawStatusUI();
            PluginUI.DrawDebugButtons();

            if (RefreshTechTree)
            {
                RefreshTechTree = false;
                TechUIService.RefreshUITechTree();
            }
        }
    }
}
