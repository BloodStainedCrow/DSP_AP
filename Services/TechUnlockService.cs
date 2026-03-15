using System.Collections.Generic;

namespace DSP_AP.Services;

public static class TechUnlockService
{
    public static bool DoTrueUnlock = false;

    public static List<long> GetUnlockedTechIds()
    {
        var list = new List<long>();
        foreach (KeyValuePair<int, TechState> techStateKVP in GameMain.history.techStates)
        {
            int techId = techStateKVP.Key;
            TechState techState = techStateKVP.Value;
            bool unlocked = techState.unlocked;
            if (unlocked)
                list.Add(techId);
        }
        return list;
    }

    public static List<long> GetUnlockedOrResearchableTechIds()
    {
        List<long> list = new();
        foreach (KeyValuePair<int, TechState> techStateKVP in GameMain.history.techStates)
        {
            int techId = techStateKVP.Key;
            TechState techState = techStateKVP.Value;
            if (techState.unlocked || GameMain.history.PreTechUnlocked(techId))
                list.Add(techId);
        }
        return list;
    }

    public static void ApplyTechRewards(int techId)
    {
        Plugin.BepinLogger.LogInfo($"Unlocking rewards for tech id: {techId}");

        GameHistoryData history = GameMain.history;

        if (!history.techStates.ContainsKey(techId))
        {
            Plugin.BepinLogger.LogWarning($"No Techstate found for id {techId}");
            return;
        }
        TechState techState = history.techStates[techId];

        TechProto techProto = LDB.techs.Select(techId);
        if (techProto == null)
        {
            Plugin.BepinLogger.LogWarning($"No TechProto found for id {techId}");
            return;
        }

        try
        {
            DoTrueUnlock = true;
            for (int i = 0; i < techProto.UnlockRecipes.Length; i++)
                history.UnlockRecipe(techProto.UnlockRecipes[i]);
            for (int i = 0; i < techProto.UnlockFunctions.Length; i++)
                history.UnlockTechFunction(techProto.UnlockFunctions[i], techProto.UnlockValues[i], techState.maxLevel);
            for (int i = 0; i < techProto.AddItems.Length; i++)
                history.GainTechAwards(techProto.AddItems[i], techProto.AddItemCounts[i]);
            history.NotifyTechUnlock(techId, techState.maxLevel);
        }
        finally
        {
            DoTrueUnlock = false;
        }

        Plugin.BepinLogger.LogInfo($"Unlocked research rewards for tech id: {techId}");
    }
}
