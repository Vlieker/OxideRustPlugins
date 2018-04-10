using System;
using Oxide.Core.Libraries.Covalence;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using Oxide.Core;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("PlayFX", "Vliek", "1.0.1", ResourceId = 2810)]
    [Description("Play any Rust fx/effect on any specified player.")]

    class PlayFX : RustPlugin
    {
        string usagePerm = "playfx.use";

        string GetLang(string msg, string userID) => lang.GetMessage(msg, this, userID);

        protected override void LoadDefaultMessages()
        {
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["usageExample"] = "Usage example: /playfx 76561198238497190 assets/bundled/prefabs/fx/gestures/drink_vomit.prefab",
                ["noPlayerFound"] = "No player matching this SteamID",
                ["noPermission"] = "You don't have the required permission to play effects on players."
            }, this);
        }

        void Init()
        {
            permission.RegisterPermission(usagePerm, this);
        }


        [Command("playfx")]
        void CmdPlayFX(BasePlayer player, string command, string[] args)
        {
            if (permission.UserHasPermission(usagePerm, player.UserIDString))
            {
                if (args.Length == 2)
                {
                    ulong effectTarget;
                    string argTarget = args[0];
                    string effect = args[1];
                    try
                    {
                        effectTarget = Convert.ToUInt64(argTarget);
                    }
                    catch (FormatException e)
                    {
                        Puts(e.Message);
                        Player.Reply(player, GetLang("usageExample", player.UserIDString));
                        return;
                    }
                    BasePlayer finaltarget = BasePlayer.FindByID(effectTarget);
                    if (finaltarget == null)
                    {
                        Player.Reply(player, GetLang("noPlayerFound", player.UserIDString));
                        return;
                    }
                    Effect.server.Run(effect, finaltarget.transform.position, Vector3.zero, null, false);
                    Puts("Ran on " + finaltarget.displayName);
                    return;
                }
                Player.Reply(player, GetLang("usageExample", player.UserIDString));
                return;
            }
            Player.Reply(player, GetLang("noPermission", player.UserIDString));
        }
    }
}