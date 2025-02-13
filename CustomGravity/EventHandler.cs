using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Plugins;
using MEC;
using UnityEngine;

namespace CustomGravity
{
    internal class EventHandler
    {
        public static void Changed(PlayerChangedRoleEventArgs ev)
        {

            // Check the new role of the player if they are worth the effort B)
            if (ev.NewRole == PlayerRoles.RoleTypeId.Spectator ||
                ev.NewRole == PlayerRoles.RoleTypeId.Overwatch ||
                ev.NewRole == PlayerRoles.RoleTypeId.Filmmaker  ){
                return;
            }

            // Get the config for ease of use.
            Config config = CustomGravity.Instance.Config;

            // Check the config if the default gravity vector is set correctly.
            if (config.GravityVector.Count() != 3)
            {
                LabApi.Features.Console.Logger.Error("Please check your Config! Your Gravity Vector value is not set correctly!");
                return;
            }

            // Define the default gravity value.
            Vector3 gravity = new UnityEngine.Vector3(config.GravityVector[0], config.GravityVector[1], config.GravityVector[2]);

            // If a temporary gravity vector is being used. Override the default gravity.
            if (CustomGravity.Instance.UseTemporaryGravity)
            {
                gravity = CustomGravity.Instance.TemporaryGravity;
            }

            // If players gravity is modified, keep it like that.
            if (CustomGravity.Instance.AlteredGravityPlayers.Keys.Contains(ev.Player))
            {
                gravity = CustomGravity.Instance.AlteredGravityPlayers[ev.Player];
            }

            Debug.Log("Setting grav: " + gravity.ToString());
            // Set the gravity. Without delay gravity is not being applied, dont ask me. Its magic!
            Timing.CallDelayed(0.5f, () =>
            {
                ev.Player.Gravity = gravity;
            });
        }

        public static void DroppedItem(PlayerDroppedItemEventArgs ev)
        {
            PickupGravityHandler.CheckItemForGravity(ev.Pickup);
        }

        public static void DroppedAmmo(PlayerDroppedAmmoEventArgs ev)
        {
            PickupGravityHandler.CheckItemForGravity(ev.Pickup);
        }

        public static void PlayerDied(PlayerDeathEventArgs ev)
        {
            PickupGravityHandler.CheckAllItemsForGravity();
        }

        public static void Joined(PlayerJoinedEventArgs ev)
        {
            // For example if player quits and rejoins, clear the altered gravity cache.
            if (CustomGravity.Instance.AlteredGravityPlayers.Keys.Contains(ev.Player))
            {
                CustomGravity.Instance.AlteredGravityPlayers.Remove(ev.Player);
            }
        }

        public static void Restart()
        {
            // Clear all gravity cache for next round.
            CustomGravity.Instance.AlteredGravityPlayers.Clear();
            CustomGravity.Instance.UseTemporaryGravity = false;

            CustomGravity.Instance.AlteredItemTypes.Clear();
            CustomGravity.Instance.UseTemporaryPickupGravity = false;
        }
    }
}
