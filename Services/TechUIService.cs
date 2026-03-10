using System.Collections.Generic;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using DSP_AP.Archipelago;
using UnityEngine.UI;

namespace DSP_AP.Services;

public static class TechUIService
{
    public static void RefreshTechUI()
    {
        GameHistoryData history = GameMain.history;
        if (history == null)
            return;

        Dictionary<int, UITechNode> nodes = UIRoot.instance?.uiGame?.techTree?.nodes;
        if (nodes == null)
            return;

        foreach (KeyValuePair<int, UITechNode> pair in nodes)
        {
            UITechNode node = pair.Value;
            if (node == null)
                continue;

            Text description = node.techDescText;
            if (description == null)
                continue;

            description.text = "[REDACTED]";

            TechProto techProto = node.techProto;
            if (techProto == null)
                continue;

            bool unlocked = history.TechUnlocked(techProto.ID);
            bool researchable = history.PreTechUnlocked(techProto.ID);

            if (unlocked || researchable)
            {
                Dictionary<long, ScoutedItemInfo> scoutedTechs = ArchipelagoClient.scoutedTechs;
                if (scoutedTechs == null)
                    continue;

                if (!scoutedTechs.ContainsKey(techProto.ID))
                    continue;

                ScoutedItemInfo scoutedTech = scoutedTechs[techProto.ID];
                if (scoutedTech == null)
                    continue;

                string text = $"{scoutedTech.Player}'s {scoutedTech.ItemDisplayName} ({scoutedTech.ItemGame})\n";
                switch (scoutedTech.Flags)
                {
                    case ItemFlags.None:
                        text += "[IMPORTANCE 0]";
                        break;
                    case ItemFlags.Advancement:
                        text += "[IMPORTANCE 1]";
                        break;
                    case ItemFlags.NeverExclude:
                        text += "[IMPORTANCE 2]";
                        break;
                    case ItemFlags.Trap:
                        text += "[IMPORTANCE 3]";
                        break;
                }

                description.text = text;
            }
        }
    }
}
