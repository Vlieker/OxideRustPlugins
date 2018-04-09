using System;
using Oxide.Core.Libraries.Covalence;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using Oxide.Core;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("PlayFX", "Vliek", "1.0.0")]
    [Description("Play a prefab effect on any player.")]

    class PlayFX : RustPlugin
    {
        string usePermission = "playfx.playeffect";

        void Init()
        {
            permission.RegisterPermission(usePermission, this);
        }


        [ConsoleCommand("playfx")]
        void CmdPlayFX(ConsoleSystem.Arg arg)
        {
                if (arg.Args.Length == 2)
                {
                    ulong effectTarget;
                    string argTarget = arg.Args[0];
                    string effect = arg.Args[1];
                    try
                    {
                        effectTarget = Convert.ToUInt64(argTarget);
                    }
                    catch (FormatException e)
                    {
                        Puts(e.Message);
                        Puts("Usage example: playfx 76561198238497190 assets/bundled/prefabs/fx/gestures/cameratakescreenshot.prefab");
                        return;
                    }
                    BasePlayer finaltarget = BasePlayer.FindByID(effectTarget);
                    if (finaltarget == null)
                    {
                        Puts("No users matching that SteamID");
                        return;
                    }
                    Effect.server.Run(effect, finaltarget.transform.position, Vector3.zero, null, false);
                    Puts("Ran on " + finaltarget.displayName);
                    return;
                }
                Puts("Usage example: playfx 76561198238497190 assets/bundled/prefabs/fx/gestures/cameratakescreenshot.prefab");
                return;
        }
    }
}