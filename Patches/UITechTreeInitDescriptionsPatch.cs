using DSP_AP.Services;
using HarmonyLib;

namespace DSP_AP.Patches;

[HarmonyPatch(typeof(UITechTree), "RefreshTranslate")]
public class UITechTreeInitDescriptionsPatch
{
    [HarmonyPostfix]
    public static void Postfix(UITechTree __instance)
    {
        TechUIService.RefreshTechUI();
    }
}
