using DSP_AP.Services;
using HarmonyLib;

namespace DSP_AP.Patches.UITechTreeAdditions;

[HarmonyPatch(typeof(UITechTree))]
public class UITechTreePatches
{
    [HarmonyPatch(nameof(UITechTree.RefreshTranslate))]
    [HarmonyPostfix]
    public static void RefreshTranslatePostfix(UITechTree __instance)
    {
        TechUIService.RefreshUITechTree(__instance);
    }
}

[HarmonyPatch(typeof(UITechNode))]
public class UITechNodePatches
{
    [HarmonyPatch(nameof(UITechNode.UpdateInfoDynamic))]
    [HarmonyPostfix]
    public static void UpdateInfoDynamicPostfix(UITechNode __instance)
    {
        TechUIService.RefreshUITechNode(__instance, updateDescription: false);
    }

    [HarmonyPatch(nameof(UITechNode.UpdateInfoComplete))]
    [HarmonyPostfix]
    public static void UpdateInfoCompletePostFix(UITechNode __instance)
    {
        TechUIService.RefreshUITechNode(__instance, updateDescription: true);
    }
}
