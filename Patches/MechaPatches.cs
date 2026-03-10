using HarmonyLib;

namespace DSP_AP.Patches;

[HarmonyPatch(typeof(Mecha))]
public class MechaPatches
{
    [HarmonyPatch("TakeDamage")]
    [HarmonyPostfix]
    public static void TakeDamagePostfix(Mecha __instance)
    {
        if (__instance.hp <= 0)
        {
            // The player has died
            if (Plugin.ArchipelagoClient.DeathLinkHandler != null)
            {
                Plugin.ArchipelagoClient.DeathLinkHandler.SendDeathLink("could not handle the Dark Fog.");
            }
        }
    }

    [HarmonyPatch("UpdateCombatStats")]
    [HarmonyPostfix]
    public static void UpdateCombatStatsPostfix(Mecha __instance)
    {
        Plugin.ArchipelagoClient?.DeathLinkHandler?.HandleQueue();
    }
}
