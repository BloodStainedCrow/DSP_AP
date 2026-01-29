using HarmonyLib;
using DSP_AP.Archipelago;

namespace DSP_AP.Patches
{
    [HarmonyPatch(typeof(Mecha), "UpdateCombatStats")]
    public class HandleDeathLinkQueuePatch
    {
        [HarmonyPostfix]
        public static void Postfix(Mecha __instance)
        {
            if (Plugin.ArchipelagoClient.DeathLinkHandler != null)
            {
                Plugin.ArchipelagoClient.DeathLinkHandler.HandleQueue();
            }
        }
    }
}
