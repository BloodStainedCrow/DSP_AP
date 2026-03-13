using HarmonyLib;

namespace DSP_AP.Patches.ZeroRespawnCost;

[HarmonyPatch(typeof(PlayerAction_Death))]
public class PlayerAction_DeathPatches
{
    [HarmonyPatch(nameof(PlayerAction_Death.SettleRespawnCost))]
    [HarmonyPrefix]
    public static bool SettleRespawnCostPrefix()
    {
        // Disable respawn costs when DeathLink is enabled.
        return !Plugin.ArchipelagoClient?.DeathLinkHandler.deathLinkEnabled ?? true;
    }
}
