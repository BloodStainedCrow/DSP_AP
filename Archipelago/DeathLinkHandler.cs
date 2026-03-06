using System;
using System.Collections.Generic;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using DSP_AP.Utils;

namespace DSP_AP.Archipelago;

public class DeathLinkHandler
{
    public bool deathLinkEnabled;
    #region Private Fields
    private string slotName;
    private readonly DeathLinkService service;
    private readonly Queue<DeathLink> deathLinks = new();
    #endregion

    /// <summary>
    /// instantiates our death link handler, sets up the hook for receiving death links, and enables death link if needed
    /// </summary>
    /// <param name="deathLinkService">The new DeathLinkService that our handler will use to send and
    /// receive death links</param>
    /// <param name="enableDeathLink">Whether we should enable death link or not on startup</param>
    public DeathLinkHandler(DeathLinkService deathLinkService, string name, bool enableDeathLink = false)
    {
        service = deathLinkService;
        service.OnDeathLinkReceived += DeathLinkReceived;
        slotName = name;
        deathLinkEnabled = enableDeathLink;

        if (deathLinkEnabled)
        {
            service.EnableDeathLink();
        }
    }

    /// <summary>
    /// enables/disables death link
    /// </summary>
    public void ToggleDeathLink()
    {
        deathLinkEnabled = !deathLinkEnabled;

        if (deathLinkEnabled)
        {
            service.EnableDeathLink();
        }
        else
        {
            service.DisableDeathLink();
        }
    }

    private void DeathLinkReceived(DeathLink deathLink)
    {
        deathLinks.Enqueue(deathLink);

        Plugin.BepinLogger.LogDebug($"Received Death Link from: {deathLink.Source}, with cause {deathLink.Cause}");
    }

    /// <summary>
    /// can be called when in a valid state to kill the player, dequeueing and immediately killing the player with a
    /// message if we have a death link in the queue
    /// </summary>
    public void HandleQueue()
    {
        try
        {
            if (deathLinks.Count < 1)
                return;

            if (!GameMain.mainPlayer.isAlive)
            {
                // It seems the player is already dead or dying
                return;
            }

            var deathLink = deathLinks.Dequeue();

            //TODO kill the player
            string cause = deathLink.Cause == null ? "died." : deathLink.Cause;
            ArchipelagoConsole.LogMessage($"{deathLink.Source} {cause}");

            GameMain.mainPlayer.Kill();
        }
        catch (Exception e)
        {
            Plugin.BepinLogger.LogError(e);
        }
    }

    /// <summary>
    /// called to send a death link to the multiworld
    /// </summary>
    public void SendDeathLink(string cause = null)
    {
        try
        {
            if (!deathLinkEnabled)
                return;

            Plugin.BepinLogger.LogInfo("Sending Deathlink");
            ArchipelagoConsole.LogMessage("sharing your death...");

            var linkToSend = new DeathLink(slotName, cause);

            service.SendDeathLink(linkToSend);
        }
        catch (Exception e)
        {
            Plugin.BepinLogger.LogError(e);
        }
    }
}
