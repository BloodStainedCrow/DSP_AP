using DSP_AP.Archipelago;
using HarmonyLib;

namespace DSP_AP.Patches;

[HarmonyPatch(typeof(FactorySystem), "GameTickLabResearchMode")]
public class RecieveTechsPatch
{
    [HarmonyPostfix]
    public static void Postfix(FactorySystem __instance)
    {
        ArchipelagoClient.HandleQueue();
    }
}
