using HarmonyLib;

namespace DSP_AP.Patches;

[HarmonyPatch(typeof(Mecha), "UpdateCombatStats")]
public class HandleDeathLinkQueuePatch
{
    [HarmonyPostfix]
    public static void Postfix(Mecha __instance)
    {
        Plugin.ArchipelagoClient?.DeathLinkHandler?.HandleQueue();
    }
}

[HarmonyPatch(typeof(UIDeathPanel))]
public class UIDeathPanelPatches
{
    [HarmonyPatch(nameof(UIDeathPanel.Determine))]
    [HarmonyPostfix]
    public static void DeterminePostfix(UIDeathPanel __instance)
    {
        __instance._Close();
    }
}
