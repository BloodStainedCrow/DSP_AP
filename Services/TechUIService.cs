using System.Collections.Generic;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using DSP_AP.Archipelago;

namespace DSP_AP.Services;

public static class TechUIService
{
    private static UnityEngine.Color[] OriginalBorderColors = null;
    private static UnityEngine.Color APBorderColor = new UnityEngine.Color(0.522f, 0.275f, 0.553f, 1.0f);
    private static string APHexColor = "#85468DCC";
    private static string DeliveredHexColor = "#80FF80CC";
    private static string NotDeliveredHexColor = "#FF8080CC";

    public static void RefreshUITechTree(UITechTree tree = null)
    {
        if (tree == null)
            tree = UIRoot.instance?.uiGame?.techTree;
        if (tree == null)
            return;

        Dictionary<int, UITechNode> nodes = tree.nodes;
        if (nodes == null)
            return;

        foreach (KeyValuePair<int, UITechNode> pair in nodes)
            RefreshUITechNode(pair.Value, updateDescription: true);
    }

    public static void RefreshUITechNode(UITechNode node, bool updateDescription)
    {
        if (node == null)
            return;

        GameHistoryData history = GameMain.history;
        if (history == null)
            return;

        TechProto techProto = node.techProto;
        if (techProto == null)
            return;

        bool unlocked = history.TechUnlocked(techProto.ID);
        bool researchable = history.PreTechUnlocked(techProto.ID);

        ItemInfo receivedTech;
        ArchipelagoClient.ReceivedTechs.TryGetValue(techProto.ID, out receivedTech);

        if (updateDescription && node.techDescText != null)
        {
            ScoutedItemInfo scoutedTech;
            ArchipelagoClient.ScoutedTechs.TryGetValue(techProto.ID, out scoutedTech);

            string text = techProto.description;
            text += "\n\n";

            text += "[MULTIWORLD INCOMING]\n";
            if (receivedTech != null)
            {
                text += $"  ORIGIN: <color=\"{APHexColor}\">{receivedTech.LocationDisplayName} - {receivedTech.Player} - {receivedTech.Player.Game}</color>\n";
                text += $"  STATUS: <color=\"{DeliveredHexColor}\">received</color>\n";
            }
            else
            {
                text += $"  ORIGIN: [REDACTED]\n";
                text += $"  STATUS: <color=\"{NotDeliveredHexColor}\">not received</color>\n";
            }
            text += "\n";

            text += "[MULTIWORLD OUTGOING]\n";
            if (scoutedTech != null)
            {
                ItemFlags flags = scoutedTech.Flags;
                int importance = 0;
                if ((flags & ItemFlags.Trap) != ItemFlags.None)
                    importance = 3;
                else if ((flags & ItemFlags.NeverExclude) != ItemFlags.None)
                    importance = 2;
                else if ((flags & ItemFlags.Advancement) != ItemFlags.None)
                    importance = 1;
                text += $"  ITEM: <color=\"{APHexColor}\">{scoutedTech.ItemDisplayName}</color>\n";
                text += $"  DESTINATION: <color=\"{APHexColor}\">{scoutedTech.Player} - {scoutedTech.Player.Game}</color>\n";
                text += $"  IMPORTANCE: {importance}\n";
            }
            else
            {
                text += "  ITEM: [REDACTED]\n";
                text += "  DESTINATION: [REDACTED]\n";
                text += "  IMPORTANCE: [REDACTED]\n";
            }
            if (unlocked)
                text += $"  STATUS: <color=\"{DeliveredHexColor}\">sent</color>";
            else
                text += $"  STATUS: <color=\"{NotDeliveredHexColor}\">not sent</color>";
            text += "\n";

            node.techDescText.text = text;
            node.descHeight = (int)(node.techDescText.preferredHeight * 0.95f);
        }

        if (OriginalBorderColors == null)
        {
            OriginalBorderColors = [
                node.bdColor1,
                node.bdColor2,
                node.bdColor3,
                node.bdColor4,
                node.bdColor5,
                node.bdColor6,
            ];
        }

        if (receivedTech == null)
        {
            node.bdColor1 = OriginalBorderColors[0];
            node.bdColor2 = OriginalBorderColors[1];
            node.bdColor3 = OriginalBorderColors[2];
            node.bdColor4 = OriginalBorderColors[3];
            node.bdColor5 = OriginalBorderColors[4];
            node.bdColor6 = OriginalBorderColors[5];
        }
        else
        {
            node.bdColor1 = APBorderColor;
            node.bdColor2 = APBorderColor;
            node.bdColor3 = APBorderColor;
            node.bdColor4 = APBorderColor;
            node.bdColor5 = APBorderColor;
            node.bdColor6 = APBorderColor;
        }

        if (node.bd1Image != null)
        {
            node.bd1Image.color = unlocked ? node.bdColor1 : node.bdColor2;
            if (receivedTech != null)
                node.bd1Image.enabled = true;
        }
        if (node.bd2Image != null)
        {
            node.bd2Image.color = unlocked ? node.bdColor3 : node.bdColor4;
            if (receivedTech != null)
                node.bd2Image.enabled = true;
        }
    }
}
