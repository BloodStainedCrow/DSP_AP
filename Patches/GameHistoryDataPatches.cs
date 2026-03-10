using DSP_AP.Services;
using HarmonyLib;

namespace DSP_AP.Patches;

[HarmonyPatch(typeof(GameHistoryData))]
public class GameHistoryDataPatches
{
    [HarmonyPatch("UnlockRecipe")]
    [HarmonyPrefix]
    public static bool UnlockRecipePrefix()
    {
        return TechUnlockService.DoTrueUnlock;
    }

    [HarmonyPatch("UnlockTechFunction")]
    [HarmonyPrefix]
    public static bool UnlockTechFunctionPrefix()
    {
        return TechUnlockService.DoTrueUnlock;
    }

    [HarmonyPatch("GainTechAwards")]
    [HarmonyPrefix]
    public static bool GainTechAwardsPrefix()
    {
        return TechUnlockService.DoTrueUnlock;
    }

    [HarmonyPatch("NotifyTechUnlock")]
    [HarmonyPrefix]
    public static bool NotifyTechUnlockPrefix(int _techId)
    {
        if (TechUnlockService.DoTrueUnlock)
            return true;
        else
        {
            TechProto techProto = LDB.techs.Select(_techId);
            Plugin.BepinLogger.LogInfo($"Tech location researched: {(Localization.CanTranslate(techProto.Name) ? techProto.Name.Translate() : techProto.Name)} ({_techId})");

            Plugin.ArchipelagoClient.CheckLocationsAsync();
            Plugin.ArchipelagoClient.ScoutLocationsAsync();
            return false;
        }
    }
}
