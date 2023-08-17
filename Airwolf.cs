using Oxide.Core;
using Oxide.Game.Rust;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Oxide.Core.Libraries.Covalence;

namespace Oxide.Plugins
{
    [Info("Airwolf Vendor Spawner", "Chicken#1366", "1.0.3")]
    [Description("Moveable airwolf vendor plugin")]
    class Airwolf : RustPlugin
    {
        private BaseEntity airwolfVendor;
        private BaseEntity banditConversationalist;
        private Vector3 helicopterSpawnLocation;

        void Init()
        {
            permission.RegisterPermission("airwolf.admin", this);

            AddCovalenceCommand("airwolf", "CmdSpawnAirwolfVendor");
            AddCovalenceCommand("setspawnlocation", "CmdSetSpawnLocation");
        }

        [Command("airwolf")]
        void CmdSpawnAirwolfVendor(IPlayer player, string command, string[] args)
        {
            if (player.HasPermission("airwolf.admin"))
            {
                var basePlayer = player.Object as BasePlayer;
                if (basePlayer != null)
                {
                    Vector3 spawnPosition = basePlayer.transform.position;

                    // Delete the previous bandit_conversationalist NPC if it exists
                    DestroyBanditConversationalist();

                    // Delete the previous airwolfspawner entity if it exists
                    DestroyAirwolfVendor();

                    SpawnAdditionalNPC("assets/prefabs/npc/bandit/shopkeepers/bandit_conversationalist.prefab", spawnPosition);

                    player.Reply("Don't forget to set the spawn location using /setspawnlocation.");
                }
            }
        }

        [Command("setspawnlocation")]
        void CmdSetSpawnLocation(IPlayer player, string command, string[] args)
        {
            if (player.HasPermission("airwolf.admin"))
            {
                var basePlayer = player.Object as BasePlayer;
                if (basePlayer != null)
                {
                    helicopterSpawnLocation = basePlayer.transform.position;
                    player.Reply("Helicopter spawn location set successfully.");

                    // Spawn the Airwolf vendor
                    SpawnAirwolfVendor(helicopterSpawnLocation);
                }
            }
        }

        void SpawnAirwolfVendor(Vector3 position)
        {
            var prefabName = "assets/prefabs/npc/bandit/airwolfspawner.prefab";
            var entity = GameManager.server.CreateEntity(prefabName, position);
            entity.Spawn();

            airwolfVendor = entity;
        }

        void SpawnAdditionalNPC(string prefabName, Vector3 position)
        {
            var entity = GameManager.server.CreateEntity(prefabName, position);
            entity.Spawn();

            if (prefabName == "assets/prefabs/npc/bandit/shopkeepers/bandit_conversationalist.prefab")
            {
                banditConversationalist = entity;
            }
        }

        void MoveAirwolfVendor(Vector3 position)
        {
            if (airwolfVendor != null)
            {
                airwolfVendor.transform.position = position;
            }
        }

        void DestroyAirwolfVendor()
        {
            if (airwolfVendor != null)
            {
                airwolfVendor.Kill();
                airwolfVendor = null;
            }
        }

        void DestroyBanditConversationalist()
        {
            if (banditConversationalist != null)
            {
                banditConversationalist.Kill();
                banditConversationalist = null;
            }
        }
    }
}
