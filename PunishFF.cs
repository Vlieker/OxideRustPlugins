using Oxide.Core.Plugins;
using System;
using System.Collections.Generic;

namespace Oxide.Plugins
{
    [Info("Punish Friendlyfire", "Vliek", "1.0.0")]
    [Description("Punish player by X% of the damage done at friends.")]

    class PunishFF : RustPlugin
    {
        [PluginReference]
        private Plugin Friends;

        private bool Changed;
        private float attackerAmount;

        protected override void LoadDefaultConfig()
        {
            Puts("Creating a new config file.");
            Config.Clear();
            LoadVariables();
        }

        private void LoadVariables()
        {
            attackerAmount = Convert.ToSingle(GetConfig("Settings", "% of punish damage given (Default 50%)", 0.5f));

            if (!Changed) return;
            SaveConfig();
            Changed = false;
        }

        object GetConfig(string menu, string datavalue, object defaultValue)
        {
            var data = Config[menu] as Dictionary<string, object>;
            if (data == null)
            {
                data = new Dictionary<string, object>();
                Config[menu] = data;
                Changed = true;
            }
            object value;
            if (!data.TryGetValue(datavalue, out value))
            {
                value = defaultValue;
                data[datavalue] = value;
                Changed = true;
            }
            return value;
        }

        private void Init()
        {
            LoadVariables();
            //permission.RegisterPermission(togglePerm, this);
        }

        private object OnAttackInternal(BasePlayer attacker, BasePlayer victim, HitInfo hit)
        {
            if (attacker == victim)
                return null;

            var victimId = victim.userID;
            var attackerId = attacker.userID;
            var hasFriend = (bool)(Friends?.CallHook("HasFriend", attackerId, victimId) ?? false);

            if (hasFriend)
            {
                float amount = hit.damageTypes.Get(hit.damageTypes.GetMajorityDamageType());
                //hit.damageTypes.ScaleAll(0);
                attacker.Hurt(amount * attackerAmount);
                return true;
            }
            return null;
        }

        void OnPlayerAttack(BasePlayer attacker, HitInfo hitInfo)
        {
            if (hitInfo?.HitEntity is BasePlayer)
                OnAttackInternal(attacker, (BasePlayer)hitInfo.HitEntity, hitInfo);
        }

        void OnEntityTakeDamage(BaseCombatEntity entity, HitInfo hitInfo)
        {
            if (entity is BasePlayer && hitInfo?.Initiator is BasePlayer)
                OnAttackInternal((BasePlayer)hitInfo.Initiator, (BasePlayer)entity, hitInfo);
        }
    }
}