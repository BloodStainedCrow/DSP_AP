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
