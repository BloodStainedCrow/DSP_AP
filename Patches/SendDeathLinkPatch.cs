using HarmonyLib;
using DSP_AP.Archipelago;
using DSP_AP.Utils;

namespace DSP_AP.Patches
{
    [HarmonyPatch(typeof(Mecha), "TakeDamage")]
    public class SendDeathLinkPatch
    {
        [HarmonyPostfix]
        public static void Postfix(Mecha __instance)
        {
            if (__instance.hp <= 0) {
                // The player has died
                if (Plugin.ArchipelagoClient.DeathLinkHandler != null)
                {
                    Plugin.ArchipelagoClient.DeathLinkHandler.SendDeathLink("could not handle the Dark Fog.");
                }
            }
        }
    }
}
