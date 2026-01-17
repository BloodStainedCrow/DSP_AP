using BepInEx.Logging;
using DSP_AP.Partials;
using System.Linq;

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

                // Strip original unlock data
                sourceArray[i].UnlockRecipes = [];
                sourceArray[i].AddItems = [];

                if (sourceArray[i].ID >= 2401 && sourceArray[i].ID <= 2407)
                {
                    // This is Communication Control
                    // Due to an issue in the Abnormality Detector, it will throw an index out of bound exceptions if for these technologies do not have the `UnlockFunctions` set
                    // TODO: Ideally this would not be needed
                    Assert.Equals(sourceArray[i].UnlockValues.Length, 1);
                    sourceArray[i].UnlockValues[0] = 0.0;
                } else
                {
                    sourceArray[i].UnlockFunctions = [];
                }
            } else
            {
                logger.LogError($"source[i] was null??");
            }
        }

        logger.LogInfo($"Copied {partials.Count(x => x != null)} techs into TechProtoPartial array.");
        return partials;
    }
}
