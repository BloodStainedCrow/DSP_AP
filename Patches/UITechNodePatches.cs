using DSP_AP.Services;
using HarmonyLib;

namespace DSP_AP.Patches;

[HarmonyPatch(typeof(UITechNode))]
public class UITechNodePatches
{
    [HarmonyPatch("UpdateInfoDynamic")]
    [HarmonyPostfix]
    public static void UpdateInfoDynamicPostfix(UITechNode __instance)
    {
        TechUIService.RefreshUITechNode(__instance, updateDescription: false);
    }

    [HarmonyPatch("UpdateInfoComplete")]
    [HarmonyPostfix]
    public static void UpdateInfoCompletePostFix(UITechNode __instance)
    {
        TechUIService.RefreshUITechNode(__instance, updateDescription: true);
    }
}
