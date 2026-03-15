using DSP_AP.Archipelago;
using DSP_AP.Services;
using HarmonyLib;

namespace DSP_AP.Patches.ItemReceive;

[HarmonyPatch(typeof(FactorySystem))]
public class FactorySystemPatches
{
    [HarmonyPatch(nameof(FactorySystem.GameTickLabResearchMode))]
    [HarmonyPostfix]
    public static void GameTickLabResearchModePostfix(FactorySystem __instance)
    {
        ArchipelagoClient.HandleQueue();
    }
}

[HarmonyPatch(typeof(GameHistoryData))]
public class GameHistoryDataPatches
{
    [HarmonyPatch(nameof(GameHistoryData.UnlockRecipe))]
    [HarmonyPrefix]
    public static bool UnlockRecipePrefix()
    {
        return TechUnlockService.DoTrueUnlock;
    }

    [HarmonyPatch(nameof(GameHistoryData.UnlockTechFunction))]
    [HarmonyPrefix]
    public static bool UnlockTechFunctionPrefix()
    {
        return TechUnlockService.DoTrueUnlock;
    }

    [HarmonyPatch(nameof(GameHistoryData.GainTechAwards))]
    [HarmonyPrefix]
    public static bool GainTechAwardsPrefix()
    {
        return TechUnlockService.DoTrueUnlock;
    }

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
