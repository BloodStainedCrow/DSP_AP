using DSP_AP.Services;
using HarmonyLib;

namespace DSP_AP.Patches;

[HarmonyPatch(typeof(UITechTree))]
public class UITechTreePatches
{
    [HarmonyPatch("RefreshTranslate")]
    [HarmonyPostfix]
    public static void RefreshTranslatePostfix(UITechTree __instance)
    {
        TechUIService.RefreshUITechTree(__instance);
    }
}
