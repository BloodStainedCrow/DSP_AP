// Due to an issue in the Abnormality Detector, it will throw an index out of bound exceptions if technologies 2401 through 2407 do not have the `UnlockFunctions` set.
// TODO: Ideally this would not be needed
using HarmonyLib;
using DSP_AP.Archipelago;

namespace DSP_AP.Patches
{
    [HarmonyPatch(typeof(ABN_CommunicationControl), "CheckValue")]
    public class DisableABNCommunicationsControlPatch
    {
        [HarmonyPostfix]
        public static bool Prefix()
        {
            return false;
        }
    }
}
