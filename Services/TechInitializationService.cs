using System.Linq;
using BepInEx.Logging;
using DSP_AP.Partials;

namespace DSP_AP.Services;

public static class TechInitializationService
{
    public static TechProtoPartial[] CreateTechProtos(ManualLogSource logger)
    {
        var sourceArray = LDB.techs.dataArray;
        var partials = new TechProtoPartial[sourceArray.Length];

        for (int i = 0; i < sourceArray.Length; i++)
        {
            if (sourceArray[i] != null)
            {
                partials[i] = new TechProtoPartial(sourceArray[i]);

                // Strip original unlock results
                sourceArray[i].AddItems = [];
                sourceArray[i].UnlockFunctions = [];
                sourceArray[i].UnlockRecipes = [];
            }
            else
            {
                logger.LogError($"source[i] was null??");
            }
        }

        logger.LogInfo($"Copied {partials.Count(x => x != null)} techs into TechProtoPartial array.");
        return partials;
    }
}
