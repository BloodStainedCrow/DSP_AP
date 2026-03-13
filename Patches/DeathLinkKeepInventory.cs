using HarmonyLib;

namespace DSP_AP.Patches.DeathLinkKeepInventory;

[HarmonyPatch(typeof(PlayerPackageUtility))]
public class PlayerPackageUtilityPatches
{
    [HarmonyPatch(nameof(PlayerPackageUtility.ThrowAllItemsInAllPackage))]
    [HarmonyPrefix]
    public static bool ThrowAllItemsInAllPackagePrefix()
    {
        // Don't drop inventory when DeathLink is enabled.
        return !Plugin.ArchipelagoClient?.DeathLinkHandler.deathLinkEnabled ?? true;
    }
}
