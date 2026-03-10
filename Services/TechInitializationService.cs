using System.Linq;
using DSP_AP.Partials;

namespace DSP_AP.Services;

public static class TechInitializationService
{
    public static TechProtoPartial[] CreateTechProtos()
    {
        var sourceArray = LDB.techs.dataArray;
        var partials = new TechProtoPartial[sourceArray.Length];

        for (int i = 0; i < sourceArray.Length; i++)
        {
            if (sourceArray[i] != null)
                partials[i] = new TechProtoPartial(sourceArray[i]);
            else
                Plugin.BepinLogger.LogError($"source[i] was null??");
        }

        Plugin.BepinLogger.LogInfo($"Copied {partials.Count(x => x != null)} techs into TechProtoPartial array.");
        return partials;
    }
}
