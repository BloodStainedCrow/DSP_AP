using HarmonyLib;

namespace DSP_AP.Patches.DeathLinkReceive;

[HarmonyPatch(typeof(Mecha))]
public class MechaPatches
{
    [HarmonyPatch(nameof(Mecha.UpdateCombatStats))]
    [HarmonyPostfix]
    public static void UpdateCombatStatsPostfix(Mecha __instance)
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
        // Prevent respawn ui from showing when DeathLink is enabled.
        if (Plugin.ArchipelagoClient?.DeathLinkHandler?.deathLinkEnabled ?? true)
            __instance._Close();
    }
}

[HarmonyPatch(typeof(PlayerPackageUtility))]
public class PlayerPackageUtilityPatches
{
    [HarmonyPatch(nameof(PlayerPackageUtility.ThrowAllItemsInAllPackage))]
    [HarmonyPrefix]
    public static bool ThrowAllItemsInAllPackagePrefix()
    {
        // Don't drop inventory when DeathLink is enabled.
        return !Plugin.ArchipelagoClient?.DeathLinkHandler?.deathLinkEnabled ?? true;
    }
}
[HarmonyPatch(typeof(PlayerAction_Death))]
public class PlayerAction_DeathPatches
{
    [HarmonyPatch(nameof(PlayerAction_Death.SettleRespawnCost))]
    [HarmonyPrefix]
    public static bool SettleRespawnCostPrefix()
    {
        // Disable respawn costs when DeathLink is enabled.
        return !Plugin.ArchipelagoClient?.DeathLinkHandler?.deathLinkEnabled ?? true;
    }
}
