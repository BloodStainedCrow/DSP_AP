using DSP_AP.Services;
using HarmonyLib;

namespace DSP_AP.Patches.LocationSend;

[HarmonyPatch(typeof(GameHistoryData))]
public class GameHistoryDataPatches
{
    [HarmonyPatch(nameof(GameHistoryData.NotifyTechUnlock))]
    [HarmonyPrefix]
    public static bool NotifyTechUnlockPrefix(int _techId)
    {
        TechProto techProto = LDB.techs.Select(_techId);
        if (TechUnlockService.DoTrueUnlock)
        {
            Plugin.BepinLogger.LogInfo($"Tech received: {(Localization.CanTranslate(techProto.Name) ? techProto.Name.Translate() : techProto.Name)} ({_techId})");
            return true;
        }
        else
        {
            Plugin.BepinLogger.LogInfo($"Tech location researched: {(Localization.CanTranslate(techProto.Name) ? techProto.Name.Translate() : techProto.Name)} ({_techId})");
            Plugin.ArchipelagoClient.CheckLocationsAsync();
            Plugin.ArchipelagoClient.ScoutLocationsAsync();
            return false;
        }
    }
}
