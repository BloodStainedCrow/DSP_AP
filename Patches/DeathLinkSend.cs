using HarmonyLib;
using UnityEngine;

namespace DSP_AP.Patches.DeathLinkSend;

[HarmonyPatch(typeof(Mecha))]
public class MechaPatches
{
    private const double sendDeathLinkCooldown = 10.0d;
    private static double lastDeathLinkSentTimestamp = 0.0d;

    [HarmonyPatch(nameof(Mecha.TakeDamage))]
    [HarmonyPostfix]
    public static void TakeDamagePostfix(Mecha __instance)
    {
        if (__instance.hp <= 0)
        {
            // The player is dead
            double now = Time.realtimeSinceStartupAsDouble;
            if (Plugin.ArchipelagoClient.DeathLinkHandler != null && now - lastDeathLinkSentTimestamp > sendDeathLinkCooldown)
            {
                lastDeathLinkSentTimestamp = now;
                Plugin.ArchipelagoClient.DeathLinkHandler.SendDeathLink("could not handle the Dark Fog.");
            }
        }
    }

    [HarmonyPatch(nameof(Mecha.UpdateCombatStats))]
    [HarmonyPostfix]
    public static void UpdateCombatStatsPostfix(Mecha __instance)
    {
        Plugin.ArchipelagoClient?.DeathLinkHandler?.HandleQueue();
    }
}
